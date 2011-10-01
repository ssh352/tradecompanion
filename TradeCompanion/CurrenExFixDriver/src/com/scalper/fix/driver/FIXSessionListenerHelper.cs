//lock (messageWouldHaveBeenSentMap.SyncRoot) is replaced with 
//lock (messageWouldHaveBeenSentMap)
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
using Constants = com.scalper.fix.Constants;
using Message = com.scalper.fix.Message;
using Logging = com.scalper.util.Logging;
using log4net;
namespace com.scalper.fix.driver
{
	
	class FIXSessionListenerHelper
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(FIXSessionListenerHelper));
		virtual public MessageTypeTable MessageTypeTable
		{
			get
			{
				return messageTypeTable;
			}
			
		}
		private System.Collections.IList messageListenerList;
		
		private MessageTypeTable messageTypeTable;
		/// <summary>map of msgType (String) -> objects that know how to generate the right kind of message </summary>
		private System.Collections.IDictionary messageWouldHaveBeenSentMap;
		private System.Collections.IList disconnectListeners;
		private System.Collections.IList messagesForListenersList;
		private MessageNotificationThread notificationThread;
		
		public FIXSessionListenerHelper()
		{
			messageWouldHaveBeenSentMap = new System.Collections.Hashtable();
			messageTypeTable = new MessageTypeTable();
			disconnectListeners = new System.Collections.ArrayList();
			messagesForListenersList = new System.Collections.ArrayList();
			setDefaults();
			messageListenerList = new System.Collections.ArrayList();
			notificationThread = new MessageNotificationThread(this);
		}
		
		private void  setDefaults()
		{
			// listener should be notified of all admin messages.
			messageTypeTable.setAdminDefault(true, false);
			// listener should be notified of all application messages.
			messageTypeTable.setApplicationDefault(true, false);
		}
		
		public virtual bool shouldTransmitMessage(Message message)
		{
			return getMessageModifier(message) == null;
		}
		
		public virtual void  messageWouldBeTransmitted(Message msg)
		{
			MessageModifier mf = getMessageModifier(msg);
			if (mf == null)
			{
				throw new System.ArgumentException();
			}
			mf.messageWouldHaveBeenSent(msg);
		}
		
		private MessageModifier getMessageModifier(Message msg)
		{
			return getMessageModifier(msg.MsgType);
		}
		
		private MessageModifier getMessageModifier(System.String msgType)
		{
			lock (messageWouldHaveBeenSentMap)
			{
				return (MessageModifier) messageWouldHaveBeenSentMap[msgType];
			}
		}
		
		public virtual void  registerMessageModifier(System.String msgType, MessageModifier mf)
		{
			if (getMessageModifier(msgType) != null)
			{
				// @todo allow more than one??
				// warn user ??
				throw new System.ArgumentException();
			}
			lock (messageWouldHaveBeenSentMap)
			{
				messageWouldHaveBeenSentMap[msgType] = mf;
			}
		}
		
		public virtual void  start()
		{
			notificationThread.Start();
		}
		
		public virtual void  stopAfterDeliverAllMessages()
		{
			notificationThread.requestStop();
		}
		
		public virtual void  addMessageListener(FIXMessageListener l)
		{
			lock (messageListenerList)
			{
				if (messageListenerList.Contains(l))
				{
					// don't re-add
				}
				else
				{
					messageListenerList.Add(l);
				}
			}
		}
		
		public virtual void  removeMessageListener(FIXMessageListener l)
		{
			lock (messageListenerList)
			{
				messageListenerList.Remove(l);
			}
		}
		
		public virtual void  notifyMessageSent(Message msg, bool copyMessage)
		{
			notifyMessageInternal(msg, copyMessage, true);
		}
		
		public virtual void  notifyMessageReceived(Message msg, bool copyMessage)
		{
			notifyMessageInternal(msg, copyMessage, false);
		}
		
		private void  notifyMessageInternal(Message msg, bool copyMessage, bool sent)
		{
			if (!messageTypeTable.isEventEnabled(msg))
			{
				return ;
			}
			
			// make a copy before notifying listeners if that's what the user requested.
			Message newMsg = copyMessage?(Message) msg.Copy:msg;
			lock (messagesForListenersList)
			{
				messagesForListenersList.Add(new MessageHolder(newMsg, sent));
				System.Threading.Monitor.PulseAll(messagesForListenersList);
			}
		}
		
		// disconnect listeners
		public virtual void  addDisconnectListener(DisconnectListener dl)
		{
			lock (disconnectListeners)
			{
				disconnectListeners.Add(dl);
			}
		}
		
		public virtual void  removeDisconnectListener(DisconnectListener dl)
		{
			lock (disconnectListeners)
			{
				disconnectListeners.Remove(dl);
			}
		}
		
		protected internal virtual void  notifyDisconnectListeners(Message logoutSent, Message logoutReceived)
		{
			lock (disconnectListeners)
			{
				System.Collections.IEnumerator it = disconnectListeners.GetEnumerator();
				while (it.MoveNext())
				{
					DisconnectListener dl = (DisconnectListener) it.Current;
					dl.sessionDisconnected(FIXSessionListenerHelper.makeDisconnectInfo(logoutSent, logoutReceived));
				}
			}
		}
		
		private static DisconnectInfo makeDisconnectInfo(Message logoutSent, Message logoutReceived)
		{
			return new DisconnectInfo(logoutSent != null, logoutReceived != null, FIXSessionListenerHelper.getText(logoutSent), FIXSessionListenerHelper.getText(logoutReceived));
		}
		
		private static System.String getText(Message msg)
		{
			if (msg == null)
				return null;
			return msg.getValue(com.scalper.fix.Constants_Fields.TAGText_i);
		}
		
		private class MessageHolder
		{
			public Message message;
			/// <summary> true - sent from this session.
			/// false - receieved by this session
			/// </summary>
			public bool sent;
			
			public MessageHolder(Message msg, bool sent)
			{
				this.message = msg;
				this.sent = sent;
			}
		}
		
		/// <summary> a dedicated thread to notify listeners that messages were received/sent from the session.
		/// This thread is a daemon thread, i.e. the fact that it is still running will not keep the JVM
		/// from exiting.  When the session is shut down (for whatever reason) no new message notifications will
		/// be sent out, but all existing notifications on the queue will be sent to all listeners, which could take
		/// a while.
		/// </summary>
		private class MessageNotificationThread:SupportClass.ThreadClass
		{
			private void  InitBlock(FIXSessionListenerHelper enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private FIXSessionListenerHelper enclosingInstance;
			public FIXSessionListenerHelper Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private volatile bool stopping;
			
			public MessageNotificationThread(FIXSessionListenerHelper enclosingInstance):base("MessageNotificationThread")
			{
				InitBlock(enclosingInstance);
				IsBackground = true;
			}
			
			override public void  Run()
			{
				//Logging.info("FIXSessionListenerHelper starting...");
                log.Info("FIXSessionListenerHelper starting...");
                while (true)
				{
					MessageHolder holder = popNextMessage();
					if (holder == null && stopping)
					{
						break;
					}
					lock (Enclosing_Instance.messageListenerList)
					{
						if ((Enclosing_Instance.messageListenerList.Count == 0))
						{
							lock (Enclosing_Instance.messagesForListenersList)
							{
								Enclosing_Instance.messagesForListenersList.Clear();
							}
							continue;
						}
						
						Message msg = holder.message;
						System.Collections.IEnumerator it = Enclosing_Instance.messageListenerList.GetEnumerator();
						if (holder.sent)
						{
							while (it.MoveNext())
							{
								try
								{
									// all listeners are always notified, even if this takes a while.
									// (no check for "stopping" here)
									FIXMessageListener listener = (FIXMessageListener) it.Current;
									listener.messageSent(msg);
								}
								catch (System.Exception t)
								{
									log.Error("First - FixSessionListenerHelper caught exception notifying listener: " + t);
                                    log.Error(t);
                                    //Logging.error("First - FixSessionListenerHelper caught exception notifying listener: " + t);
									//Logging.error(t);
								}
							}
						}
						else
						{
							while (it.MoveNext())
							{
								try
								{
									// all listeners are always notified, even if this takes a while.
									// (no check for "stopping" here)
									FIXMessageListener listener = (FIXMessageListener) it.Current;
									listener.messageReceived(msg);
								}
								catch (System.Exception t)
								{
									log.Error("Second - FixSessionListenerHelper caught exception notifying listener: " + t);
                                    log.Error(t);
                                    //Logging.error("Second - FixSessionListenerHelper caught exception notifying listener: " + t);
									//Logging.error(t);
								}
							}
						}
					}
				}
				if (stopping)
                    log.Info("FIXSessionListenerHelper stopping...");
					//Logging.info("FIXSessionListenerHelper stopping...");
				else
                    log.Error("FIXSessionListenerHelper stopping abnormally.");
					//Logging.error("FIXSessionListenerHelper stopping abnormally.");
			}
			
			public virtual void  requestStop()
			{
				stopping = true;
				Interrupt();
			}
			
			private MessageHolder popNextMessage()
			{
				lock (Enclosing_Instance.messagesForListenersList)
				{
					if ((Enclosing_Instance.messagesForListenersList.Count == 0))
					{
						try
						{
							System.Threading.Monitor.Wait(Enclosing_Instance.messagesForListenersList);
						}
						catch
						{
                            log.Info("Problem in popNextMesssage");
						}
					}
					if ((Enclosing_Instance.messagesForListenersList.Count == 0))
						return null;
					else
					{
						System.Object tempObject;
						tempObject = Enclosing_Instance.messagesForListenersList[0];
						Enclosing_Instance.messagesForListenersList.RemoveAt(0);
						return (MessageHolder) tempObject;
					}
				}
			}
		}
	}
}