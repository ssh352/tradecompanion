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
/// File: FIXNoticeListener.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using Message = FxIntegral.com.scalper.fix.Message;
namespace  FxIntegral.com.scalper.fix.driver
{
	
	/// <summary> represents the current state of a S7FIXSession.
	/// It is safe to use instance equality (==) when comparing objects of this type.
	/// </summary>
	public class SessionState
	{
		virtual public int State
		{
			get
			{
				return state;
			}
			
			set
			{
				this.state = value;
			}
			
		}
		virtual public Message LogoutSent
		{
			get
			{
				return logoutSent;
			}
			
			// logout message tracking __________________________________________________________________________
			
			
			set
			{
				ensureIsLogoutMessage(value);
				this.logoutSent = value;
			}
			
		}
		virtual public Message LogoutReceived
		{
			get
			{
				return logoutReceived;
			}
			
			set
			{
				ensureIsLogoutMessage(value);
				this.logoutReceived = value;
			}
			
		}
		// session state (logon/logoff, ability to send messages)
		public const int WAITING_FOR_LOGON = - 1;
		public const int LOGON_SENT = 0;
		public const int LOGON_RECEIVED = 1;
		public const int LOGON_RESP_SENT = 2;
		public const int LOGON_RESP_RECEIVED = 3;
		public const int WAITING_FOR_MESSAGE_SYNCH = 4;
		public const int SESSION_READY = 5;
		public const int LOGOUT_SENT = 6;
		public const int LOGOUT_RECEIVED = 7;
		public const int LOGOUT_RESP_RECEIVED = 8;
		public const int LOGOUT_RESP_SENT = 9;
		public const int SESSION_CLOSING = 10;
		public const int SESSION_CLOSED = 11;
		
		// related to the current state of resend requests
		public System.Object transmitLock;
		private bool thisSessionMustResend_Renamed_Field = false;
		private bool otherSessionMustResend_Renamed_Field;
		private int state;
		
		private Message logoutSent;
		private Message logoutReceived;
		
		public SessionState(int state, System.Object transmitLock)
		{
			State = state;
			this.transmitLock = transmitLock;
		}
		
		// resend state / right to transmit messages on the wire ______________________________________
		
		/*
		public void setThisSessionMustResend()
		{
		thisSessionMustResendLock.getWriteLock();
		thisSessionMustResend = true;
		}
		
		public void clearThisSessionMustResend()
		{
		thisSessionMustResend = false;
		thisSessionMustResendLock.releaseWriteLock();
		}
		
		public void takeSendMessageLock() // throws InterruptedException
		{
		thisSessionMustResendLock.getReadLock();
		
		if (thisSessionMustResend())
		{
		// can't happen
		throw new IllegalStateException();
		}
		}
		
		public void releaseSendMessageLock()
		{
		thisSessionMustResendLock.releaseReadLock();
		}*/
		
		
		
		/// <summary> access to this method must be externally synchronized -- not safe to just say
		/// if (thisSessionMustResend)
		/// putMessageOnWire
		/// because another thread may set the flag in the interval.
		/// </summary>
		public virtual bool thisSessionMustResend()
		{
			// no need to synchronized - access to a boolean variable is guaranteed to be atomic
			return thisSessionMustResend_Renamed_Field;
		}
		
		// other session must resend state (no interesting synchronization) ______________________________
		
		public virtual void  setOtherSessionMustResend()
		{
			otherSessionMustResend_Renamed_Field = true;
		}
		
		public virtual void  clearOtherSessionMustResend()
		{
			otherSessionMustResend_Renamed_Field = false;
		}
		
		public virtual bool otherSessionMustResend()
		{
			return otherSessionMustResend_Renamed_Field;
		}
		
		private void  ensureIsLogoutMessage(Message msg)
		{
			if (msg.MsgTypeChar != '5')
			{
				throw new System.ArgumentException("Not a logout message.");
			}
		}
		
		public override System.String ToString()
		{
			return "state: " + state + " " + thisSessionMustResend_Renamed_Field + " " + otherSessionMustResend_Renamed_Field;
		}
	}
}