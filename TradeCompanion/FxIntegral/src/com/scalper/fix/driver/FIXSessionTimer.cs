/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd. ("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd..
/// 
/// ****************************************************
/// </summary>
using System;
using FixException = FxIntegral.com.scalper.fix.FixException;
using Message = FxIntegral.com.scalper.fix.Message;
using log4net;
/// <summary> <p>Description: Session timer </p></summary>
namespace  FxIntegral.com.scalper.fix.driver
{
	
	class FIXSessionTimer
	{
        private void  InitBlock()
		{
			scheduler = new TaskScheduler(this);
		}
		/// <summary> Gets the heart beat interval of the current session.</summary>
		/// <returns> Returns the hBeatInterval.
		/// </returns>
		/// <summary> Sets the heartbeat interval.</summary>
		/// <returns>
		/// </returns>
		virtual public int HBeatInterval
		{
			get
			{
				return heartBeatInt;
			}
			
			set
			{
				if (value < 0)
				{
					throw new System.ArgumentException("Interval cannot be negative: " + value);
				}
				this.heartBeatInt = value;
				lock (heartbeatLock)
				{
					if (heartbeatTask != null)
					{
						scheduler.setDelay(heartbeatTask, heartBeatInt);
					}
				}
			}
			
		}
		/// <summary> Gets the heart beat interval of the current session.</summary>
		/// <returns> Returns the hBeatInterval.
		/// </returns>
		virtual public int TestRequestInterval
		{
			get
			{
				return testRequestInt;
			}
			
			set
			{
				if (value < 0)
				{
					throw new System.ArgumentException("Interval cannot be negative.");
				}
				this.testRequestInt = value;
				lock (testRequestLock)
				{
					if (testRequestTask != null)
					{
						scheduler.setDelay(testRequestTask, testRequestInt);
					}
				}
			}
			
		}
		virtual public System.String TimeStamp
		{
			get
			{
				// there are some complications here because nothing is synchronized.
				// there's a race condition where the timer task might null out the reference
				// at any point during this loop, which is why we use a copy and why the loop is necessary.
				
				// another race condition where two threads could call this method at the same time.  They will
				// both calculate the timestamp (which is a couple of extra milliseconds) but the results are
				// correct so it is not worth thinking about too hard.
				
				// if the timer is not running, we must always do the work ourselves.
				if (scheduler == null)
				{
					timestamp = null;
					return buildTimestamp();
				}
                ///VM_ Fix Time not chnaged for heartbeat message, now i m changeing the time
                System.String timeStampCopy;// = timestamp; 
                //if (timeStampCopy != null)
                //{
                //    return timeStampCopy;
                //}
				timeStampCopy = buildTimestamp();
				timestamp = timeStampCopy;
				return timeStampCopy;
			}
			
		}
        private static readonly ILog log = LogManager.GetLogger(typeof(FIXSessionTimer));
		/// <summary> Default granularity of timer thread.</summary>
		/// <todo>  would like to make this a full second but when heartbeats get reset it happens mid-interval, </todo>
		/// <summary> so could be off by as much as a full second.
		/// </summary>
		private const int TIMER_GRANULARITY = 250;
		private const int TIMER_ALLOWED_OFFSET = 100;
		
		/// <summary> drop connection if this counterparty doesn't respond within this test request intervals
		/// (N-1 test request messages are actually sent)
		/// </summary>
		private int TEST_REQUEST_BAILOUT;

        private ScalperFixSession session;
		
		private System.Object logonLock = new System.Object();
		private System.Object heartbeatLock = new System.Object();
		private System.Object testRequestLock = new System.Object();
		private System.Object waitForResendRequestLock = new System.Object();
		
		//private Timer timer;
		//private TimerTask logonTask;
		// repeating tasks that will be called once every second to check if they need to do something.
		private HeartbeatTask heartbeatTask;
		private TestRequestTask testRequestTask;
		private WaitForResendRequestTask waitForResendRequestTask;
		private ScheduledTask logonTask;
		
		private TaskScheduler scheduler;
		// a cached value of the timeStamp (for performance reasons.)
		// starts out null.
		// when a thread wants it, if it's null, the thread generates it on its own time.
		// a TimerTask runs on a periodic basis that invalidates this reference by setting
		// it to null.
		private System.String timestamp;
		
		//number of milliseconds of inactivity before a heartbeat is sent out
		private int heartBeatInt = - 1;
		//number of milliseconds of inactivity before a test request is sent out
		private int testRequestInt = - 1;
		
		private long loginTimeout = 1000000;
		private long logoutTimeout = 1000000;
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			FIXSessionTimer timer = null; //new FIXSessionTimer();
			timer.startHeartbeatTimer();
			timer.startTestRequestTimer();
			try
			{
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 30000));
				timer.stopHeartbeatTimer();
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 20000));
				timer.resetTestRequestTimer();
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 10000));
				timer.stopTestRequestTimer();
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 10000));
				timer.stop();
			}
			catch (System.Exception e)
			{
                log.Error(e);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}

        public FIXSessionTimer(ScalperFixSession session, SessionProperties properties)
		{
			InitBlock();
			this.session = session;
			TEST_REQUEST_BAILOUT = properties.getIntegerProperty(SessionProperties.Prop_TestRequestBailout);
			if (TEST_REQUEST_BAILOUT <= 1)
			{
				throw new System.ArgumentException("Test request bailout must be > 1");
			}
			resetHeartbeatTimer();
			//FIX sepc says use transmission time(delta) as 20% of the heartbeat interval.
			int loginTime = properties.getIntegerProperty(SessionProperties.Prop_LoginTimeout);
			if (loginTime > 0)
				loginTimeout = loginTime;
			int logoutTime = properties.getIntegerProperty(SessionProperties.Prop_LogoutTimeout);
			if (logoutTime > 0)
				logoutTimeout = logoutTime;
			scheduler.Start();
		}
		
		/// <summary>starts the scheduler.  Does not start any other tasks. </summary>
		public virtual void  start()
		{
		}
		
		public virtual void  stop()
		{
            log.Info((System.DateTime.Now.Ticks - 621355968000000000) / 10000 + ":Stopping the scheduler..");
			scheduler.cancel();
		}
		
		private Message createHeartbeatMsg()
		{
			try
			{
				return session.buildHeartBeatMessage(false);
			}
			catch (FixException fe)
			{
				// just going to get a NullPointerException if try to return null
				throw new System.SystemException(fe.Message);
			}
		}
		
		private Message createTestRequestMsg()
		{
			try
			{
				// the QuickFix tests assume the text "TEST", should probably be configurable.
				return session.buildTestRequestMessage("TEST");
			}
			catch (FixException fe)
			{
				// just going to get a NullPointerException if try to return null
				throw new System.SystemException(fe.Message);
			}
		}
		
		// getters/setters ___________________________________________________________________________________________
		
		public virtual void  startHeartbeatTimer()
		{
			lock (heartbeatLock)
			{
				if (heartbeatTask == null)
				{
					int delay = HBeatInterval;
					heartbeatTask = new HeartbeatTask(this);
					scheduler.schedule(heartbeatTask, delay, delay, "HeartBeatTimer");
				}
			}
		}
		
		abstract internal class ScheduledTask
		{
			public abstract void  takeAction();
		}
		
		private class ScheduleItem
		{
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			virtual public long Delay
			{
				get
				{
					return delay;
				}
				
				set
				{
					this.delay = value;
				}
				
			}
			virtual public System.String Name
			{
				get
				{
					return name;
				}
				
				set
				{
					this.name = value;
				}
				
			}
			virtual public long Elapsed
			{
				get
				{
					return elapsed_Renamed_Field;
				}
				
				set
				{
					this.elapsed_Renamed_Field = value;
				}
				
			}
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal long delay;
			internal long startTime;
			internal long elapsed_Renamed_Field;
			internal ScheduledTask task;
			internal System.String name;
			
			public ScheduleItem(FIXSessionTimer enclosingInstance, ScheduledTask task, System.String name, long delay)
			{
				InitBlock(enclosingInstance);
				this.task = task;
				this.delay = delay;
				this.name = name;
				elapsed_Renamed_Field = 0;
				startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			}
			
			public virtual void  run()
			{
				task.takeAction();
			}
			
			public virtual void  reset()
			{
				Elapsed = 0;
				startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
			}
			
			public virtual long elapsed()
			{
				elapsed_Renamed_Field = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime;
				return elapsed_Renamed_Field;
			}
			
			public virtual bool shouldRun()
			{
                return ((delay > 0) && (elapsed_Renamed_Field + FxIntegral.com.scalper.fix.driver.FIXSessionTimer.TIMER_ALLOWED_OFFSET) >= delay);
			}
		}
		
		public class TaskScheduler:SupportClass.ThreadClass
		{
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.Collections.Hashtable tasks = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			internal bool keepRunning = true;
			public TaskScheduler(FIXSessionTimer enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			
			public virtual void  cancel()
			{
				keepRunning = false;
			}
			
			override public void  Run()
			{
				while (keepRunning)
				{
					System.Collections.IEnumerator sTasks = tasks.Keys.GetEnumerator();
					try //added temporarily to come out of the error - 
                    {
                        while (sTasks.MoveNext())
                        {
                            System.Object taskKey = sTasks.Current;
                            ScheduleItem schInfo = (ScheduleItem)tasks[taskKey];
                            schInfo.elapsed();
                            if (schInfo.shouldRun())
                            {
                                schInfo.run();
                                schInfo.reset();
                            }
                        }
                    }
                    catch
                    { 

                    }
					try
					{
                        System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64)10000 * FxIntegral.com.scalper.fix.driver.FIXSessionTimer.TIMER_GRANULARITY));
					}
					catch (System.Exception e)
					{
                        log.Error(e);
						SupportClass.WriteStackTrace(e, Console.Error);
					}
				}
			}
			
			public virtual void  cancelTask(ScheduledTask task)
			{
				if (tasks.ContainsKey(task))
				{
					tasks.Remove(task);
				}
			}
			
			public virtual void  setDelay(ScheduledTask task, long delay)
			{
				if (tasks.ContainsKey(task))
				{
					ScheduleItem sItem = (ScheduleItem) tasks[task];
					sItem.Delay = delay;
				}
			}
			
			public virtual void  reset(ScheduledTask task)
			{
				if (tasks.ContainsKey(task))
				{
					ScheduleItem sItem = (ScheduleItem) tasks[task];
					sItem.reset();
				}
			}
			
			public virtual void  schedule(ScheduledTask task, long iniDelay, long delay, System.String name)
			{
				ScheduleItem sItem = null;
				if (tasks.ContainsKey(task))
				{
					sItem = (ScheduleItem) tasks[task];
					sItem.Delay = delay;
					sItem.Elapsed = 0;
				}
				else
				{
					sItem = new ScheduleItem(enclosingInstance, task, name, delay);
					tasks[task] = sItem;
				}
			}
		}
		
		public virtual void  stopHeartbeatTimer()
		{
			lock (heartbeatLock)
			{
				if (heartbeatTask == null)
				{
					// shouldn't happen
					//Logging.warn("Heartbeat task wasn't registered.");
                    log.Warn("Heartbeat task wasn't registered.");
				}
				else
				{
					scheduler.cancelTask(heartbeatTask);
					heartbeatTask = null;
				}
			}
		}
		
		public virtual void  resetHeartbeatTimer()
		{
			// @todo will probably need to be optimized - this will get called for each message received
			lock (heartbeatLock)
			{
				if (heartbeatTask == null)
				{
					// not an error - could be client sending the initial login message
					// (heartbeat timer isn't started until after the server sends back the login response.)
					
				}
				else
				{
					scheduler.reset(heartbeatTask);
				}
			}
		}
		
		public virtual void  startTestRequestTimer()
		{
			lock (testRequestLock)
			{
				
				if (testRequestTask == null)
				{
					int delay = TestRequestInterval;
					testRequestTask = new TestRequestTask(this);
					scheduler.schedule(testRequestTask, delay, delay, "TestRequestTimer");
				}
			}
		}
		
		public virtual void  stopTestRequestTimer()
		{
			lock (testRequestLock)
			{
				if (testRequestTask == null)
				{
					// shouldn't happen
					//Logging.warn("TestRequest task wasn't registered.");
                    log.Warn("TestRequest task wasn't registered.");
				}
				else
				{
					scheduler.cancelTask(testRequestTask);
					testRequestTask = null;
				}
			}
		}
		
		public virtual void  resetTestRequestTimer()
		{
			// @todo will probably need to be optimized - this will get called for each message sent
			lock (testRequestLock)
			{
				if (testRequestTask == null)
				{
					// not an error - could be client receiving the initial login message
					// (test request timer isn't started until after that login message is processed.)
					
				}
				else
				{
					scheduler.reset(testRequestTask);
				}
			}
		}
		
		public virtual void  markWaitForResendTimerForReset()
		{
			if (waitForResendRequestTask == null)
			{
				// not an error - could be client receiving the initial login message
				// (test request timer isn't started until after that login message is processed.)
				
			}
			else
			{
				try
				{
					scheduler.reset(waitForResendRequestTask);
					waitForResendRequestTask.markTestRequestTimerForReset();
				}
				catch //(System.Exception e)
				{
				}
			}
		}
		
		/// <summary> sets the flag indicating logon response is received.</summary>
		public virtual void  receivedLogonResp()
		{
			lock (logonLock)
			{
				if (logonTask == null)
				{
					// shouldn't happen
					//Logging.warn("Timer wasn't expecting logon response.");
                    log.Warn("Timer wasn't expecting logon response.");
				}
				else
				{
					scheduler.cancelTask(logonTask);
					logonTask = null;
				}
			}
		}
		
		/// <summary> starts a timer that waits for a message from the other side.
		/// 
		/// </summary>
		/// <param name="delay">the amount of time to wait in ms.
		/// </param>
		public virtual void  startWaitingForResend(long delay)
		{
			lock (waitForResendRequestLock)
			{
				if (waitForResendRequestTask != null)
				{
					//Logging.warn("Already waiting for resend.");
                    log.Warn("Already waiting for resend.");
					waitForResendRequestTask = null;
				}
				
				waitForResendRequestTask = new WaitForResendRequestTask(this, delay);
				scheduler.schedule(waitForResendRequestTask, delay, delay, "WaitForResendRequest");
			}
		}
		
		/// <summary>do NOT restart if already stopped. </summary>
		public virtual void  resetWaitingForResend()
		{
			
			lock (waitForResendRequestLock)
			{
				if (waitForResendRequestTask == null)
				{
                    log.Debug("Task to wait for resend was stopped, not starting...");
				}
				else
				{
					
					long i = waitForResendRequestTask.delay;
					scheduler.cancelTask(waitForResendRequestTask);
					waitForResendRequestTask = null;
					startWaitingForResend(i);
				}
			}
		}
		
		public virtual void  stopWaitingForResend()
		{
			lock (waitForResendRequestLock)
			{
				if (waitForResendRequestTask != null)
				{
					scheduler.cancelTask(waitForResendRequestTask);
					waitForResendRequestTask = null;
				}
			}
		}
		
		/// <summary> starts the timer that does an automatic logout after 10 seconds if the other side hasn't dropped connection first.</summary>
		public virtual void  receivedLogout()
		{
			
			//FIX spec says use 10 seconds for logout response or disconnect.
			
			// note: I'm not fully clear on this, but it seems like there is no way to reset the timer,
			// i.e. one way or another, after 10 seconds you're going to disconnect whether you're the logon
			// acceptor or logon initiator.
			ScheduledTask logoutTask = new DropConnectionTask(this, "FFillFixSession: Other side initiated logout and didn't drop connection after " + (loginTimeout / 1000) + " seconds.  Disconnecting...");
			
			scheduler.schedule(logoutTask, loginTimeout, loginTimeout, "LoginTimeout");
		}
		
		/// <summary> starts the timer that does an automatic logout after 10 seconds if the other side doesn't respond with a logout
		/// message.  Note that the logout acceptor should also log out after 10 seconds if the logon initiator hasn't
		/// dropped connection first.
		/// </summary>
		public virtual void  sentLogout()
		{
			
			//FIX spec says use 10 seconds for logout response or disconnect.
			
			// note: I'm not fully clear on this, but it seems like there is no way to reset the timer,
			// i.e. one way or another, after 10 seconds you're going to disconnect whether you're the logon
			// acceptor or logon initiator.
			ScheduledTask logoutTask = new DropConnectionTask(this, "FFillFixSession: Couldn't receive logout response within " + (logoutTimeout / 1000) + " seconds, disconnecting...");
			//timer.schedule(logoutTask, loginTimeout);    // changed to use logoutTimeout instead of loginTimeout
			scheduler.schedule(logoutTask, logoutTimeout, logoutTimeout, "LogoutTimeOut");
		}
		
		public virtual void  sentLogon()
		{
			lock (logonLock)
			{
				
				if (logonTask == null)
				{
					//FIX spec says use 10 seconds for logon response.
					logonTask = new DropConnectionTask(this, "FFillFixSession: Couldn't receive logon response within " + (loginTimeout / 1000) + " seconds, disconnecting...");
					scheduler.schedule(logonTask, loginTimeout, loginTimeout, "LoginTimeOut");
				}
			}
		}
		
		private System.String buildTimestamp()
		{
			System.DateTime tempAux = System.DateTime.Now.ToUniversalTime();
			return Message.buildDateString(ref tempAux, false, session.UseMillis);
		}
		
		/// <summary>Normally transmits the message immediately.
		/// If the other side has requested a resend, this message will be queued until all relevant messages have
		/// been resent.
		/// </summary>
		private void  transmit(Message msg)
		{
			session.transmit(msg);
			
		}
		
		private class WaitForResendRequestTask:ScheduledTask
		{
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			virtual public long Delay
			{
				get
				{
					return delay;
				}
				
			}
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private bool resetFlag = false;
			public long delay;
			
			public WaitForResendRequestTask(FIXSessionTimer enclosingInstance, long delay)
			{
				InitBlock(enclosingInstance);
				this.delay = delay;
			}
			
			public override void  takeAction()
			{
				if (resetFlag)
				{
					resetFlag = false;
					return ;
				}
				Enclosing_Instance.session.waitForResendTimerExpired();
			}
			
			public virtual void  markTestRequestTimerForReset()
			{
				resetFlag = true;
			}
		}
		
		/// <summary>waits for a resend request from the other side and if none is received, declares the application
		/// ready to send application messages
		/// </summary>
		private class DropConnectionTask:ScheduledTask
		{
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private System.String logString;
			public DropConnectionTask(FIXSessionTimer enclosingInstance, System.String logString)
			{
				InitBlock(enclosingInstance);
				this.logString = logString;
			}
			
			public override void  takeAction()
			{
				// "FFillFixSession: Couldn't receive logout response within 10 seconds, disconnecting.."
				//Logging.warn(logString);
                log.Warn(logString);
				//Shutdown the session - should drop dead
				Enclosing_Instance.session.stopImmediately();
			}
		}
		
		/// <summary> sends out heartbeats on a regular basis</summary>
		private class HeartbeatTask:ScheduledTask
		{
			public HeartbeatTask(FIXSessionTimer enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public override void  takeAction()
			{
				try
				{
					SupportClass.ThreadClass.Current().Priority = (System.Threading.ThreadPriority) System.Threading.ThreadPriority.Highest;
					Enclosing_Instance.transmit(Enclosing_Instance.createHeartbeatMsg());
				}
				catch (FixException e)
				{
					//Logging.error("FIXSessionTimer failed transmitting heartbeat.");
                    log.Error("FIXSessionTimer failed transmitting heartbeat.");
				}
				try
				{
					Enclosing_Instance.session.flushAdminQueue();
				}
				catch //(System.Exception e)
				{
                    log.Error("FIXSessionTimer failed draining admin processor.");
					//Logging.error("FIXSessionTimer failed draining admin processor.");
				}
			}
		}
		
		/// <summary> sends out test requests on a regular basis</summary>
		private class TestRequestTask:ScheduledTask
		{
			public TestRequestTask(FIXSessionTimer enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(FIXSessionTimer enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionTimer enclosingInstance;
			public FIXSessionTimer Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//Count that keeps track of how many test request has been sent without any response from counter-party,
			//party will be assumed dead if we dont receive response for consecutive 3 test request message.
			private int testRequestCount = 0;
			internal bool resetFlag;
			
			public override void  takeAction()
			{
				if (testRequestCount < Enclosing_Instance.TEST_REQUEST_BAILOUT - 1)
				{
					try
					{
						Enclosing_Instance.transmit(Enclosing_Instance.createTestRequestMsg());
						testRequestCount = 0;
					}
					catch //(System.Exception e)
					{
                        log.Debug("FIXSessionTimer.takeTick: Failed transmitting test request");
					}
					testRequestCount++;
					//Reset test request ticker.
					resetTestRequestTimer(false);
				}
				else
				{
					//Means you've not got response for the last test request that you've sent.
					//If this is consecutive 'nth' time that we have not got response from the
					//counter-party, we should consider counter party as dead.
					//Send shutdown notice.
					Enclosing_Instance.session.possiblyNotifyListeners(this, FIXNotice.FIX_TESTREQUEST_BAILOUT, "Counter-party is dead");
					
					//Logging.error("Counterparty is dead, session will be shut down.");
                    log.Error("Counterparty is dead, session will be shut down.");
                    // technically it shouldn't be necessary to explicitly call stop() here as it will be called
					// indirectly from session.stopImmediately, but running into problems where it wasn't getting
					// executed soon enough and causing lots of extra log messages.
					Enclosing_Instance.stop();
					Enclosing_Instance.session.stopImmediately();
				}
			}
			
			public virtual void  resetTestRequestTimer()
			{
				resetTestRequestTimer(true);
			}
			
			public virtual void  markTestRequestTimerForReset()
			{
				resetFlag = true;
			}
			
			private void  resetTestRequestTimer(bool resetTestRequestBailout)
			{
				if (resetTestRequestBailout)
				{
					testRequestCount = 0;
				}
			}
		} // End TestRequestTask inner class
	}
}