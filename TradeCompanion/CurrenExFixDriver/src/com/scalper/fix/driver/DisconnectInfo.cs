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
/// File: S7FIXSession.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace com.scalper.fix.driver
{
	
	public class DisconnectInfo
	{
		virtual public bool LogoutSent
		{
			get
			{
				return logoutSent;
			}
			
		}
		virtual public bool LogoutReceived
		{
			get
			{
				return logoutReceived;
			}
			
		}
		/// <summary> returns the value of the text field from the logout message that this session sent
		/// to the other session.  If this session did not send a logout message or if the text was not
		/// present in the logout message, returns null or "".
		/// </summary>
		virtual public System.String ThisSessionLogoutText
		{
			get
			{
				return thisSessionLogoutText;
			}
			
		}
		/// <summary> returns the value of the text field from the logout message that the other session sent
		/// to this session.  If the other session did not send a logout message or if the text was not
		/// present in the logout message, returns null or "".
		/// </summary>
		virtual public System.String OtherSessionLogoutText
		{
			get
			{
				return otherSessionLogoutText;
			}
			
		}
		/// <summary> was this a normal logout (both sides sent a logout message)</summary>
		virtual public bool NormalLogout
		{
			// convenience methods
			
			get
			{
				return LogoutSent && LogoutReceived;
			}
			
		}
		/// <summary> was this a raw disconnect?  (One side disconnected without either side sending a logoff message first)</summary>
		virtual public bool Disconnect
		{
			get
			{
				return !LogoutSent && !LogoutReceived;
			}
			
		}
		/// <summary> If a logon message was sent by either side without being answered, returns its text.
		/// Otherwise returns null.
		/// </summary>
		virtual public System.String LogoutMessageText
		{
			get
			{
				if (LogoutSent && !LogoutReceived)
					return ThisSessionLogoutText;
				if (!LogoutSent && LogoutReceived)
					return OtherSessionLogoutText;
				return null;
			}
			
		}
		private bool logoutSent;
		private bool logoutReceived;
		private System.String thisSessionLogoutText;
		private System.String otherSessionLogoutText;
		
		internal DisconnectInfo(bool logoutSent, bool logoutReceived, System.String thisSessionLogoutText, System.String otherSessionLogoutText)
		{
			this.logoutSent = logoutSent;
			this.logoutReceived = logoutReceived;
			this.thisSessionLogoutText = thisSessionLogoutText;
			this.otherSessionLogoutText = otherSessionLogoutText;
		}
		
		public override System.String ToString()
		{
			return "[ " + LogoutSent + " " + LogoutReceived + "]";
		}
	}
}