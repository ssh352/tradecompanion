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
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
using Dukascopy.com.scalper.fix;
using Dukascopy.com.scalper.fix.driver;
using BasicUtilities = Dukascopy.com.scalper.util.BasicUtilities;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

using log4net;
using log4net.Config;

namespace Dukascopy.com.scalper.fix.driver.client
{

    //External hook for handling responses from the Ffastfill FIX server
    public delegate void FIXResponseEventHandler(object sender, FIXResponseEventArgs args);

	/// <summary> Description: Simple FIX client implementation that knows how to log in to server and notifies listeners
	/// when it receives messages
	/// </summary>
	public abstract class AbstractClient : Client
	{
        // registryFlag is set to true. Registry is written only if this variable is 'true'.
        public static Boolean registryFlag = true;
		private class AnonymousClassFIXNoticeListener : FIXNoticeListener
		{
			public AnonymousClassFIXNoticeListener(AbstractClient enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AbstractClient enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AbstractClient enclosingInstance;
			public AbstractClient Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual void  takeNote(FIXNotice notice)
			{
                FIXResponseEventArgs ev = new FIXResponseEventArgs(notice);
                if (enclosingInstance.FIXResponseEvent != null) enclosingInstance.FIXResponseEvent(this, ev);
			} 
		}
		virtual public bool Connected
		{			
			get
			{
				return session.Active;
			}
			
		}
        virtual public ScalperFixSession Session
		{
			get
			{
				return session;
			}
			
		}
		virtual public int SendMsgSeqNum
		{
			set
			{
				// need to tell the persister manually that it's sending a message whose sequence number is too high.
				// Normally persister insists on messages being sent in the right order.
				
				int expectedSeqNum = session.FIXPersister.LastOutSeqNo + 1;
				if (value > expectedSeqNum)
				{
					try
					{
						for (int i = expectedSeqNum; i < value; i++)
						{
							session.FIXPersister.storeOutboundAdminMessage(createFakeAdminMessage(i));
						}
						session.test_syncMsgSeqNumWithPersister();
					}
					catch (FixException fe)
					{
                        log.Error(fe);
						SupportClass.WriteStackTrace(fe, Console.Error);
					}
				}
			}
			
		}
        private ScalperFixSession session;
		private FIXPersister persister;
		private MyMessageListener listener;
		private FIXMessageListener otherListener;

        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractClient));

        //This is the event that is raised whenever a response is received from Ffastfill
        public event FIXResponseEventHandler FIXResponseEvent;
     
		// @todo remove
		private bool setInitialValues;
		
		public AbstractClient()
		{
			 listener = new MyMessageListener();
		}
		
		public virtual void  connect(SessionProperties properties)
		{
			connect(properties, false);
		}
		
		public virtual void  connect(SessionProperties properties, bool sendNewOrderAfterConnect)
		{
			connect(properties, sendNewOrderAfterConnect, new FIXVolatilePersister());
		}
		public virtual void  connect(SessionProperties properties, bool sendNewOrderAfterConnect, FIXPersister persister)
		{
            this.persister = persister;
			// need to set fields on the session the first time we send a message
			setInitialValues = false;
			// new TestClientSessionCustomizer());
			session = createSession(properties, persister);
			
			session.setTestMode(true);
			session.addMessageListener(listener);
			if (otherListener != null)
			{
				session.addMessageListener(otherListener);
			}
			
			session.registerNoticeListener(new AnonymousClassFIXNoticeListener(this), true);
			session.start();
		}

        protected internal abstract ScalperFixSession createSession(SessionProperties properties, FIXPersister persister);
		
		public virtual bool waitUntilDisconnected(long timeout)
		{
			if (session != null)
				return session.waitUntilDisconnected(timeout);
			else
				return true;
		}
		
		public virtual void  addMessageListener(FIXMessageListener l)
		{
			otherListener = l;
		}
				
		public virtual void  sendMessage(Message msg)
		{
			if (session == null)
			{
				throw new FixException("Cannot send message, haven't established FIX session yet.");
			}
			
			if (!setInitialValues)
			{
				System.String senderCompID = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i);
				if (senderCompID != null)
				{
					session.SenderCompID = senderCompID;
				}
				
				System.String targetCompID = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i);
				if (targetCompID != null)
				{
					session.TargetCompID = targetCompID;
				}
				int msgSeqNum = msg.MsgSeqNum;
				if (msgSeqNum > - 1 && (persister is FIXVolatilePersister))
				{
					((FIXVolatilePersister) persister).test_setLastOutSeqNo(msgSeqNum - 1);
					session.test_syncMsgSeqNumWithPersister();
				}
				
				setInitialValues = true;
			}
						
			session.transmit(msg);
		}
		
		public virtual void  sendMessages(Message[] msgs)
		{
			if (session == null)
			{
				throw new FixException("Cannot send message, haven't established FIX session yet.");
			}
			
			session.transmit(msgs, 0, msgs.Length);
		}
		
		protected internal virtual Message createFakeAdminMessage(int msgSeqNum)
		{
			Message msg = new Message(Dukascopy.com.scalper.fix.Constants_Fields.MSGHeartbeat);
			msg.MsgSeqNum = msgSeqNum;
			return msg;
		}
		
		public virtual void  clearMessagesReceived()
		{
			listener.clearMessagesReceived();
		}
		
		public virtual Message accept(long timeout)
		{
			Message message = listener.takeMessage(timeout);
			// this should really be in a separate thread like the server does, but having all kinds of problems with that.
			// @todo
			if (message != null && !BasicFixUtilities.isAdmin(message) && message.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag) != 'Y')
				session.complete(message);
			return message;
		}
		
		// removes all messages with any of the given message types from the top of the queue.
		// does not block.  It is not an error if the queue is empty.
		// @todo remove?
		public virtual void  removeMessages(System.String[] msgTypeArr)
		{
			Message[] msgArr = listener.removeMessages(msgTypeArr);
			for (int i = 0; i < msgArr.Length; i++)
			{
				Message message = msgArr[i];
				// this should really be in a separate thread like the server does, but having all kinds of problems with that.
				// @todo
				if (message != null && !BasicFixUtilities.isAdmin(message) && message.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag) != 'Y')
				{
					try
					{
						session.complete(message);
					}
					catch (FixException fe)
					{
                        log.Error(fe);
						SupportClass.WriteStackTrace(fe, Console.Error);
					}
				}
			}
		}
		
		/// <summary>for use in testing only, to enforce a certain message sequence </summary>
		public virtual void  readerThreadSleep(int waitTime)
		{
			session.readerThreadSleep(waitTime);
		}
		
		protected internal static bool isSequenceReset_Reset(Message msg)
		{
			System.String msgType = msg.MsgType;
			if (Dukascopy.com.scalper.fix.Constants_Fields.MSGSequenceReset.Equals(msgType))
			{
				char gapFillFlag = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGGapFillFlag_i);
				return gapFillFlag != 'Y';
			}
			return false;
		}
		
		private class MyMessageListener : FIXMessageListener
		{
			private System.Collections.IList messageList;
			
			public MyMessageListener()
			{
				messageList = new System.Collections.ArrayList();
			}
			
			/// <summary>notification that a message has been sent by the driver </summary>
			public virtual void  messageSent(Message msg)
			{
				// no-op
			}

            /// <summary>notification that a message has been received by the driver </summary>
			public virtual void  messageReceived(Message msg)
			{
				/*lock (messageList)
				{
					messageList.Add(msg);
					System.Threading.Monitor.PulseAll(messageList);
				}*/
			}
			
			public virtual Message takeMessage(long timeout)
			{
				lock (messageList)
				{
					if ((messageList.Count == 0))
					{
						try
						{
							System.Threading.Monitor.Wait(messageList, TimeSpan.FromMilliseconds(timeout));
						}
						catch (System.Threading.ThreadInterruptedException ie)
						{
                            log.Error(" ", ie);
						}
					}
					if ((messageList.Count == 0))
					{
						return null;
					}
					System.Object tempObject;
					tempObject = messageList[0];
					messageList.RemoveAt(0);
					return (Message) tempObject;
				}
			}
			
			/// <summary> returns an array of messages removed, or empty array if none removed.</summary>
			public virtual Message[] removeMessages(System.String[] msgTypeArr)
			{
				System.Collections.IList ret = new System.Collections.ArrayList();
				lock (messageList.SyncRoot)
				{
					while (!(messageList.Count == 0))
					{
						Message msg = (Message) messageList[0];
						if (BasicUtilities.contains(msgTypeArr, msg.MsgType))
						{
							System.Object tempObject;
							tempObject = messageList[0];
							messageList.RemoveAt(0);
							Message message = (Message) tempObject;
							ret.Add(message);
						}
						else
						{
							break;
						}
					}
				}
				// not an error if remove all messages -- return normally.
				return (Message[]) SupportClass.ICollectionSupport.ToArray(ret, new Message[ret.Count]);
			}
			
			/// <summary>called between tests to ensure no messages sent in one test affect the next one </summary>
			public virtual void  clearMessagesReceived()
			{
				lock (messageList.SyncRoot)
				{
					messageList.Clear();
				}
			}
		}
		
        public abstract void  disconnect();
	}
}