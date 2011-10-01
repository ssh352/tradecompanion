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
/// File: FIXPersister.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	
	
	/// <summary> used by the application to indicated whether a logon has been accepted or not.
	/// Case 1: valid user.  Persister must be non-null, error text must be null.
	/// Case 2: invalid login, no logoff message desired (just drop connection)
	/// Persister must be null, error text must be non-null.
	/// Case 3: invalid login, logoff message desired
	/// Persister must be non-null, error text must be non-null.
	/// </summary>
	public class ServerLogonInfo
	{
		virtual public FIXPersister Persister
		{
			get
			{
				return persister;
			}
			
		}
		virtual public System.String ErrorText
		{
			get
			{
				return errorText;
			}
			
		}
		/// <summary> is this an error condition?</summary>
		/// <returns> true - user was not logged on successfully
		/// false - user was logged on successfully
		/// </returns>
		virtual public bool Error
		{
			get
			{
				return infoType != ServerLogonInfo.VALID_LOGON;
			}
			
		}
		private FIXPersister persister;
		private System.String errorText;
		private int infoType;
		
		internal const int VALID_LOGON = 1;
		internal const int INVALID_LOGON = 2;
		internal const int INVALID_USER = 3;
		
		
		
		/// <summary> constructor used to indicate that the user was logged on successfully.</summary>
		public static ServerLogonInfo onValidLogon(FIXPersister persister)
		{
			if (persister == null)
			{
				throw new System.ArgumentException("Persister cannot be null.");
			}
			return new ServerLogonInfo(ServerLogonInfo.VALID_LOGON, persister, null);
		}
		
		/// <summary> constructor used to indicate that the user could not be logged on and
		/// that a logout message should NOT be sent.
		/// </summary>
		public static ServerLogonInfo onInvalidUser(System.String errorText)
		{
			return new ServerLogonInfo(ServerLogonInfo.INVALID_USER, null, errorText);
		}
		
		/// <summary> constructor used to indicate that the user could not be logged on and
		/// that a logout message should NOT be sent, and that there is no errorText to record.
		/// </summary>
		public static ServerLogonInfo onInvalidUser()
		{
			return new ServerLogonInfo(ServerLogonInfo.INVALID_USER, null, null);
		}
		
		/// <summary> constructor used to indicate that the user could not be logged on and
		/// that a logout message should be sent.
		/// </summary>
		public static ServerLogonInfo onInvalidLogon(FIXPersister persister, System.String errorText)
		{
			if (persister == null && errorText == null)
			{
				throw new System.ArgumentException("Both persister and errorText must be non-null.");
			}
			return new ServerLogonInfo(ServerLogonInfo.INVALID_LOGON, persister, errorText);
		}
		
		private ServerLogonInfo(int infoType, FIXPersister persister, System.String errorText)
		{
			this.infoType = infoType;
			this.persister = persister;
			this.errorText = errorText;
		}
		
		/// <summary> If there was an error, should a logoff message be sent to the user?</summary>
		/// <returns> true - if there was an error, a logoff message should be sent.
		/// false - if there was an error, the driver should drop connection without sending
		/// a logoff messsage.
		/// </returns>
		public virtual bool sendLogoffMessage()
		{
			return infoType == ServerLogonInfo.INVALID_LOGON;
		}
	}
}