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
/// File: FIXVolatilePersister.cs
/// Created on Feb 2, 2004
/// ****************************************************
/// </summary>
using System;
using BasicFixUtilities = Dukascopy.com.scalper.fix.BasicFixUtilities;
using Constants = Dukascopy.com.scalper.fix.Constants;
using FixException = Dukascopy.com.scalper.fix.FixException;
using Message = Dukascopy.com.scalper.fix.Message;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	
	/// <summary> <p>Description:In memory implementation of persistant FIX message storage.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	
	public class FIXVolatilePersister : FIXPersister
	{
		virtual public bool CacheOutMessages
		{
			set
			{
				cacheOutMessages = value;
			}
			
		}
		/// <summary> Returns last inbound message sequence number.</summary>
		virtual public int LastInSeqNo
		{
			get
			{
				return lastInSeqNum;
			}
			
		}
		virtual public int LastOutSeqNo
		{
			get
			{
				return lastOutSeqNum;
			}
			
		}
		private int lastInSeqNum;
		private System.Collections.Hashtable outMessageStore;
		private int lastOutSeqNum;
		private bool setLastOutSeqNum;
		internal bool cacheOutMessages = true;
		
		public FIXVolatilePersister():this(0, 0)
		{
		}
		
		public FIXVolatilePersister(int lastInSeqNum, int lastOutSeqNum)
		{
			outMessageStore = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
			this.lastInSeqNum = lastInSeqNum;
			this.lastOutSeqNum = lastOutSeqNum;
		}
		public virtual void  storeOutboundAdminMessage(Message message)
		{
			if (!BasicFixUtilities.isAdmin(message))
			{
				throw new System.ArgumentException("Cannot persist outbound app messages.");
			}
			
			int msgSeqNum = message.MsgSeqNum;
			if (msgSeqNum == lastOutSeqNum + 1)
			{
				lastOutSeqNum++;
			}
			else
			{
				throw new System.ArgumentException("Illegal message sequence number: " + message.MsgSeqNum + ", expected " + (lastOutSeqNum + 1));
			}
		}
		
		public virtual void  storeOutboundAppMessage(Message message)
		{
			if (BasicFixUtilities.isAdmin(message))
			{
				throw new System.ArgumentException("Cannot persist outbound admin messages.");
			}
			
			int msgSeqNum = message.MsgSeqNum;
			if (msgSeqNum == lastOutSeqNum + 1)
			{
				if (cacheOutMessages)
                    //if (outMessageStore is com.scalper.fix.driver.SessionProperties)
                    //    ((com.scalper.fix.driver.SessionProperties) outMessageStore).put((System.Int32) msgSeqNum, message);
                    //else
						outMessageStore[(System.Int32) msgSeqNum] = message;
				lastOutSeqNum++;
			}
			else
			{
				throw new System.ArgumentException("Illegal message sequence number: " + message.MsgSeqNum + ", expected " + (lastOutSeqNum + 1));
			}
		}
		
		public virtual void  storeOutboundMessages(System.Object[] msgs, int offset, int count)
		{
			
			for (int i = offset; (i < msgs.Length) && (i - offset < count); i++)
			{
				Message message = (Message) msgs[i];
				int msgSeqNum = message.MsgSeqNum;
				if (msgSeqNum == lastOutSeqNum + 1)
				{
					if (cacheOutMessages)
                        //if (outMessageStore is com.scalper.fix.driver.SessionProperties)
                        //    ((com.scalper.fix.driver.SessionProperties) outMessageStore).put((System.Int32) msgSeqNum, message);
                        //else
							outMessageStore[(System.Int32) msgSeqNum] = message;
					lastOutSeqNum++;
				}
				else
				{
					throw new System.ArgumentException("Illegal message sequence number: " + message.MsgSeqNum + ", expected " + (lastOutSeqNum + 1));
				}
			}
		}
		
		private Message getOutboundFIXMessage(int msgSeqNum)
		{
			Message msg = (Message) outMessageStore[(System.Int32) msgSeqNum];
			if (msg != null)
			{
				return msg;
			}
			else
				return null;
		}
		
		
		public virtual System.Collections.IEnumerator getOutboundFIXMessages(int startSeqNo, int endSeqNo)
		{
            if (endSeqNo == Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_40_41 || endSeqNo == Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_42)
			{
				endSeqNo = lastOutSeqNum;
			}
			
			if (endSeqNo < startSeqNo)
				endSeqNo = startSeqNo;
			
			System.Collections.ArrayList result = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(endSeqNo - startSeqNo));
			for (int i = startSeqNo; i <= endSeqNo; i++)
			{
				System.Object msg = getOutboundFIXMessage(i);
				if (msg == null)
				{
					// not found - ok
					// S7FixLogging.error("FIXVolatilePersister.getOutboundFIXMessages: Couldn't find message with sequence number: " + i);
				}
				else
				{
					result.Add(msg);
				}
			}
			return result.GetEnumerator();
		}
		
		public virtual System.Collections.ArrayList getOutboundFIXMessagesVector(int startSeqNo, int endSeqNo)
		{
            if (endSeqNo == Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_40_41 || endSeqNo == Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_42)
			{
				endSeqNo = lastOutSeqNum;
			}
			
			System.Collections.ArrayList result = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(endSeqNo - startSeqNo));
			for (int i = startSeqNo; i <= endSeqNo; i++)
			{
				System.Object msg = getOutboundFIXMessage(i);
				if (msg == null)
				{
					// not found - ok
					// S7FixLogging.error("FIXVolatilePersister.getOutboundFIXMessages: Couldn't find message with sequence number: " + i);
				}
				else
				{
					result.Add(msg);
				}
			}
			return result;
		}
		
		/// <summary> stores the inbound message sequence number by setting a local value in memory.
		/// This implementation can never throw a FixException.
		/// </summary>
		public virtual void  storeInboundMsgSeqNum(int msgSeqNum)
		{
			this.lastInSeqNum = msgSeqNum;
		}
		
		public virtual void  test_setLastOutSeqNo(int newLastOutSeqNum)
		{
			if (setLastOutSeqNum)
			{
				throw new System.SystemException("After being instantiated, outbound msgSeqNum can only be set once more");
			}
			if (newLastOutSeqNum < lastOutSeqNum)
			{
				// if we don't catch this now, the receiving FIX session will just catch it as an error anyway
				// and it will be harder to find the problem
				throw new System.ArgumentException("Can't reduce message sequence number");
			}
			this.lastOutSeqNum = newLastOutSeqNum;
			setLastOutSeqNum = true;
		}
	}
}