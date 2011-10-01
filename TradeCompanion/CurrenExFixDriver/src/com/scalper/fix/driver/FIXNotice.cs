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
/// File: FIXNotice.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using Message = com.scalper.fix.Message;
namespace com.scalper.fix.driver
{
	
	/// <summary> Represents a an event of interest that happened to the FIX session.
	/// 
	/// Descriptions:
	/// <li>FIX_DISCONNECTED: the session has been disconnected for any reason.  This could be part of a normal
	/// logout, because the other side has not responded for too long, because of an error condidion, etc.
	/// 
	/// </summary>
	public class FIXNotice
	{
		/// <summary> Returns the object that flagged notice.</summary>
		/// <returns> Object that generated the notice.
		/// </returns>
		virtual public System.Object Source
		{
			get
			{
				return src;
			}
			
		}
		/// <summary> Returns Message subject of the notice.</summary>
		/// <returns> Message Subject of the notice.
		/// </returns>
		virtual public Message Message
		{
			get
			{
				return message;
			}
			
		}
		/// <summary> Returns current event code.</summary>
		/// <returns> Code that represent the type of the notice.
		/// </returns>
		virtual public int Code
		{
			get
			{
				return code;
			}
			
		}
		/// <summary> Returns text message that is attached with the current event.</summary>
		/// <returns> Returns the msg.
		/// </returns>
		virtual public System.String NoticeText
		{
			get
			{
				return text;
			}
			
		}
		/// <summary> Returns the source object from where the notice is been generated.</summary>
		/// <returns> Returns the src.
		/// </returns>
		virtual public System.Object Src
		{
			get
			{
				return src;
			}
			
		}
		/// <summary> Returns the connection subject of the notice.</summary>
		/// <returns> Returns the subConnection.
		/// </returns>
		virtual public FIXConnection Connection
		{
			get
			{
				return conn;
			}
			
		}
		/// <summary> Returns the session subject of the notice.</summary>
		/// <returns> Returns the subSession.
		/// </returns>
        virtual public ScalperFixSession Session
		{
			get
			{
				return session;
			}
			
		}
		
		/* Following are the different event codes for which user can register listeners.*/
		public const int FIX_CONNECTION_ESTABLISHED = 0;
		public const int FIX_CONNECTION_FAILED = 1;
		public const int FIX_DISCONNECTED = 2;
		public const int FIX_SESSION_ESTABLISHED = 3;
		public const int FIX_SESSION_SHUTTINGDOWN = 4;
		public const int FIX_SESSION_SHUTDOWN = 5;
		public const int FIX_SESSION_TIMEOUT = 6;
		//public static final int FIX_LOGON_RECEIVED = 7;
		public const int FIX_LOGON_RESP_RECEIVED = 8;
		public const int FIX_LOGOUT_RECEIVED = 9;
		public const int FIX_LOGOUT_RESP_RECEIVED = 10;
		public const int FIX_DUP_LOGON = 11;
		public const int FIX_DISCARD_MSG = 12;
		public const int FIX_TESTREQUEST_BAILOUT = 13;
		//public static final int FIX_SEQUENCE_RESET_GAPFILL_SENT = 14;
		public const int FIX_SEQUENCE_RESET_RESET_SENT = 15;
		//public static final int FIX_RESEND_REQUEST_SENT = 16;
		//public static final int FIX_LOGOUT_SENT = 17;
		//public static final int FIX_LOGOUT_RESP_SENT = 18;
		public const int FIX_LOGON_FAILED = 18;
		public const int FIX_SHUTDOWN_SESSION = 19;
		public const int FIX_FIELD_MISSING = 20;
		public const int FIX_RESEND_BAILOUT = 21;
		public const int FIX_MESSAGE_NOT_FOUND = 22;
		public const int FIX_VERSION_MISMATCH = 23;
		
		public const int FIX_COULDNT_PERSIST_INBOUND_MESSAGE = 24;
		public const int FIX_COULDNT_PERSIST_OUTBOUND_MESSAGE = 25;
		
		public const int FIX_MESSAGE_REJECTED = 26;
		
		public const int FIX_OTHER = 27;
		
		public const int FIX_FATAL_LOGOUT_SENT = 28;

        public const int FIX_PASSWORD_RESET_RESP_RECEIVED = 29;

        public const int NUM_NOTICE_TYPES = 30;

        //public const int HEART_BEAT_RECEIVED = 29;//HeartBeatCallback
		
		private System.Object src;
		private Message message;
        private ScalperFixSession session;
		private FIXConnection conn;
		private int code;
		private System.String text;
		
		/// <summary> Constructs FIXNotice object.</summary>
		/// <param name="src	Source">from where notice came from.
		/// </param>
		/// <param name="severity	Severity">level of the notice.
		/// </param>
		/// <param name="subject	Subject">object of the notice.
		/// </param>
		/// <param name="code	Code">that represents the type of notice.
		/// </param>
		/// <param name="msg	Additional">message that you want to attach to notice.
		/// </param>
		public FIXNotice(System.Object src, FIXConnection conn, int code, System.String text):this(src, null, null, conn, code, text)
		{
		}
		
		/// <summary> Constructs FIXNotice object.</summary>
		/// <param name="src	Source">from where notice came from.
		/// </param>
		/// <param name="severity	Severity">level of the notice.
		/// </param>
		/// <param name="subject	Subject">object of the notice.
		/// </param>
		/// <param name="code	Code">that represents the type of notice.
		/// </param>
		/// <param name="msg	Additional">message that you want to attach to notice.
		/// </param>
        public FIXNotice(System.Object src, Message msg, ScalperFixSession session, int code, System.String text)
            : this(src, msg, session, session.Connection, code, text)
		{
		}
		
		/// <summary> Constructs FIXNotice object.</summary>
		/// <param name="src	Source">from where notice came from.
		/// </param>
		/// <param name="severity	Severity">level of the notice.
		/// </param>
		/// <param name="subject	Subject">object of the notice.
		/// </param>
		/// <param name="code	Code">that represents the type of notice.
		/// </param>
		/// <param name="msg	Additional">message that you want to attach to notice.
        /// </param>                                          CurrenExSession
        private FIXNotice(System.Object src, Message message, ScalperFixSession session, FIXConnection conn, int code, System.String text)
		{
			
			this.src = src;
			this.message = message;
			this.session = session;
			this.conn = conn;
			this.code = code;
			this.text = text;
		}
	}
}