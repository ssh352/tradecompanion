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
using BasicUtilities = com.scalper.util.BasicUtilities;
using ConfigurationProperties = com.scalper.util.ConfigurationProperties;
namespace com.scalper.fix.driver
{
	
	/// <summary> Settings used to construct new messages.</summary>
	[Serializable]
	public class SessionProperties:System.Collections.Specialized.NameValueCollection //, ConfigurationProperties
	{
		virtual public bool UseTargetSubID
		{
			get
			{
				return useTargetSubID;
			}
			
			set
			{
				checkNotLocked();
				useTargetSubID = value;
			}
			
		}
		virtual public bool AcceptLogonSeqNum
		{
			get
			{
				return acceptLogonSeqNum;
			}
			
			set
			{
				checkNotLocked();
				acceptLogonSeqNum = value;
			}
			
		}
		virtual public bool SyncTestRequest
		{
			get
			{
				return syncTestRequest;
			}
			
			set
			{
				syncTestRequest = value;
			}
			
		}
		/// <summary> for client:
		/// <br>true: sets the senderSubID on all messages
		/// <br>false: sets the senderSubID on protocol messages only
		/// <br>senderSubID should be explicitly set in the driver, if not set this flag has no effect.
		/// <p>
		/// for server:
		/// <br>true: sets the targetSubID on all messages
		/// <br>false: sets the targetSubID on protocol messages only
		/// <br>targetSubID can be explicitly set in the driver, default value is the senderSubID that the client
		/// sent in the logon message.  If the server's targetSubID is null this flag has no effect.
		/// <p>
		/// This flag has no effect on the targetSubID (for client) and the senderSubID (for server)
		/// </summary>
		virtual public bool SubIDOnApplicationMessages
		{
			set
			{
				this.setSubIDOnApplicationMessages_Renamed_Field = value;
			}
			
		}
		/// <summary> for client:
		/// <br>true: sets the senderSubID on all messages
		/// <br>false: sets the senderSubID on protocol messages only
		/// <br>senderSubID should be explicitly set in the driver, if not set this flag has no effect.
		/// <p>
		/// for server:
		/// <br>true: sets the targetSubID on all messages
		/// <br>false: sets the targetSubID on protocol messages only
		/// <br>targetSubID can be explicitly set in the driver, default value is the senderSubID that the client
		/// sent in the logon message.  If the server's targetSubID is null this flag has no effect.
		/// <p>
		/// This flag has no effect on the targetSubID (for client) and the senderSubID (for server)
		/// </summary>
		virtual public bool SetSubIDOnApplicationMessages
		{
			get
			{
				return setSubIDOnApplicationMessages_Renamed_Field;
			}
			
		}
		/// <summary> controls whether the driver will overwrite sender/target CompIDs in sent messages with the correct value.
		/// <br>If the application leaves sender/target comp ID blank the driver will always fill it in.
		/// <br>If the application sets sender/target compID to something other than the expected value:
		/// <li>if overwriteCompID is true the application's value is overwritten with the "correct" value from the driver
		/// <li>if overwriteCompID is false the application's value is left alone, allowing a message with the "wrong" value to be sent.
		/// </summary>
		/// <summary> controls whether the driver will overwrite sender/target CompIDs in sent messages with the correct value.
		/// <br>If the application leaves sender/target comp ID blank the driver will always fill it in.
		/// <br>If the application sets sender/target compID to something other than the expected value:
		/// <li>if overwriteCompID is true the application's value is overwritten with the "correct" value from the driver
		/// <li>if overwriteCompID is false the application's value is left alone, allowing a message with the "wrong" value to be sent.
		/// </summary>
		virtual public bool OverwriteCompID
		{
			get
			{
				return overwriteCompID;
			}
			
			set
			{
				overwriteCompID = value;
			}
			
		}
		virtual public bool SetChecksum
		{
			get
			{
				return setCheckSum;
			}
			
			set
			{
				setCheckSum = value;
			}
			
		}
		virtual public bool ReorderFields
		{
			get
			{
				return reorderFields;
			}
			
			set
			{
				reorderFields = value;
			}
			
		}
		virtual public bool OverwriteBodyLength
		{
			get
			{
				return overwriteBodyLength;
			}
			
			set
			{
				overwriteBodyLength = value;
			}
			
		}
		/// <summary> returns whether the sending time is checked on messages that are received by the session.
		/// If true, any time the session receives a messages it checks whether the message's sending time
		/// (tag 52) is more than two minutes off in either direction from the current time.  If the time is more
		/// than two minutes different the driver will issue a session-level reject and it will not be passed on to
		/// the application layer.
		/// <br>
		/// If this setting is false, no check is performed on sending time.
		/// <p>
		/// The default is that sending time is not checked.
		/// </summary>
		/// <summary> controls whether the sending time is checked on messages that are received by the session.
		/// If true, any time the session receives a messages it checks whether the message's sending time
		/// (tag 52) is more than two minutes off in either direction from the current time.  If the time is more
		/// than two minutes different the driver will issue a session-level reject and it will not be passed on to
		/// the application layer.
		/// <br>
		/// If this setting is false, no check is performed on sending time.
		/// <p>
		/// The default is that sending time is not checked.
		/// </summary>
		virtual public bool CheckSendingTime
		{
			get
			{
				return checkSendingTime;
			}
			
			set
			{
				checkSendingTime = value;
			}
			
		}
		virtual public bool CopyMessagesToListeners
		{
			get
			{
				return copyMessagesToListeners;
			}
			
			set
			{
				copyMessagesToListeners = value;
			}
			
		}
		/// <summary> determines whether a test request is sent immediately after login to force message
		/// sequence number synchronization.
		/// </summary>
		/// <summary> determines whether the session is careful to not send any application messages after the
		/// initial login until it is positive that both session's message sequence numbers are in synch.
		/// For the server, this means waiting a small amount of time before sending any new messages.
		/// For the client, this means sending a test request immediately after the logon response is received.
		/// 
		/// This is the behavior mandated by the FIX spec, and should never be changed except when in test mode.
		/// Doing so may lead to a resend request for each application message sent.
		/// </summary>
		virtual public bool WaitForMsgSeqNumSyncBeforeSendingAppMessages
		{
			get
			{
				return waitForMsgSeqNumSyncBeforeSendingAppMessages;
			}
			
			set
			{
				waitForMsgSeqNumSyncBeforeSendingAppMessages = value;
			}
			
		}
		/// <summary> get whether the driver adheres strictly to the FIX spec in determining whether a received message
		/// should be rejected at the session level or not.  The default is that the driver tries to be lenient when
		/// possible.  For example, the FIX spec says that all header tags must appear before all body tags; this check
		/// is only made if the driver is in strict mode.
		/// <p>
		/// Unless the driver is in test mode, it will never send an invalid or malformed message to the other side.
		/// </summary>
		/// <summary> controls whether the driver adheres strictly to the FIX spec in determining whether a received message
		/// should be rejected at the session level or not.  The default is that the driver tries to be lenient when
		/// possible.  For example, the FIX spec says that all header tags must appear before all body tags; this check
		/// is only made if the driver is in strict mode.
		/// <p>
		/// Unless the driver is in test mode, it will never send an invalid or malformed message to the other side.
		/// </summary>
		virtual public bool StrictReceiveMode
		{
			get
			{
				return strictReceiveMode;
			}
			
			set
			{
				strictReceiveMode = value;
			}
			
		}
		/// <summary> </summary>
		private const long serialVersionUID = 1L;
        //This line is added to compensate for defaults collection object in C# Collections object.
        //There is no 'defaults' NameValueCollection object as a member NameValueCollection object.
        private System.Collections.Specialized.NameValueCollection defaults = new System.Collections.Specialized.NameValueCollection();
		
		public const System.String Prop_SuppressHeartbeats = "Prop_SuppressHeartbeats";
		
		/// <summary> whether to answer test requests (can only be turned off if in test mode.)</summary>
		public const System.String Prop_AnswerTestRequests = "Prop_AnswerTestRequests";
		
		/// <summary> How many test request intervals to wait before declaring other side dead and dropping
		/// connection.  If this number is 2, which is what the spec recommends, the session will
		/// wait for one test request interval, send a test request, wait for a second test request
		/// interval, and then drop connection.  This is assuming that no non-garbled messages are
		/// received from the other side in the interim (they do not have to be heartbeats.)
		/// 
		/// It is illegal for this value to be <= 1.
		/// </summary>
		public const System.String Prop_TestRequestBailout = "Prop_TestRequestBailout";
		
		/// <summary> controls whether FIX messages sent and received are logged the moment the driver receives them</summary>
		public const System.String Prop_PrintFixBefore = "Prop_PrintFixBefore";
		
		/// <summary> controls whether FIX messages sent and received are logged after all fields are set, immediately
		/// before they get sent over the transport layer
		/// </summary>
		public const System.String Prop_PrintFixAfter = "Prop_PrintFixAfter";
		
		/// <summary> controls whether, when FIX is printed out, it is in human readable (true) or raw (false) format.</summary>
		public const System.String Prop_PrintFixHumanReadable = "Prop_PrintFixHumanReadable";
		
		/// <summary> controls whether the driver will add milliseconds to outgoing date fields.
		/// This option is only available for FIX versions >= 4.2.
		/// <br>True: send times in YYYYMMDD-HH:MM:SS.sss format
		/// <br>False: send times in YYYYMMDD-HH:MM:SS format
		/// <p>
		/// It is not an error to specify this value as true if the FIX version is < 4.2, but it will have no effect.
		/// </summary>
		public const System.String Prop_SendDatesWithMilliseconds = "Prop_SendDatesWithMilliseconds";
		
		/// <summary> the version of FIX to use.  Supported values are:
		/// <li>FIX.4.0
		/// <li>FIX.4.1
		/// <li>FIX.4.2
		/// <li>FIX.4.3
		/// <li>FIX.4.4
		/// </summary>
		public const System.String Prop_FixVersion = "Prop_FixVersion";
		
		//public static final String Prop_LoginTimeout = "Prop_LoginTimeout";
		//public static final String Prop_LogoutTimeout = "Prop_LogoutTimeout";
		
		
		/// <summary> This parameter is used to set the maximum wait time for a login confirmation from the exchange.
		/// This value is 10000 by default.
		/// <P>Example: <CODE>LoginTimeout=20000</CODE>
		/// </summary>
		public const System.String Prop_LoginTimeout = "Prop_LoginTimeout";
		/// <summary> This parameter is used to set the maximum wait time for a logout confirmation from the exchange.
		/// This value is 10000 by default.
		/// <P>Example: <CODE>LogoutTimeout=20000</CODE>
		/// </summary>
		public const System.String Prop_LogoutTimeout = "Prop_LogoutTimeout";
		public const System.String Prop_SyncTestRequest = "Prop_SyncTestRequest";
		public const System.String Prop_UseTargetSubID = "Prop_UseTargetSubID";
		public const System.String Prop_CopyMessagesToListeners = "Prop_CopyMessagesToListeners";
		public const System.String Prop_CheckSendingTime = "Prop_CheckSendingTime";
		public const System.String Prop_WaitForMsgSeqNumSyncBeforeSendingAppMessages = "Prop_WaitForMsgSeqNumSyncBeforeSendingAppMessages";
		public const System.String Prop_StrictReceiveMode = "Prop_StrictReceiveMode";
		public const System.String Prop_SubIDOnApplicationMessages = "Prop_SubIDOnApplicationMessages";
		public const System.String Prop_OverwriteCompID = "Prop_OverwriteCompID";
		public const System.String Prop_OverwriteBodyLength = "Prop_OverwriteBodyLength";
		public const System.String Prop_RequestMultipleResends = "Prop_RequestMultipleResends";
		public const System.String Prop_SocketQueueSize = "Prop_SocketQueueSize";
		/// <summary> Indicates if we should persist message even if connectin is down.</summary>
		public const System.String Prop_PersistAlways = "Prop_PersistAlways";
		
		/// <summary> for a server, determines whether the session will cache the client's SenderSubID and automatically
		/// put that onto all outgoing messages.  If true, will cache the client's SenderSubID from the initial
		/// logon message and automatically
		/// add it to all outgoing messages.  A null SenderSubID is legal; no SenderSubID will be set on outgoing
		/// messages for this session.
		/// If setTargetSubID is false the driver will not take any action based on SenderSubID, and it is the
		/// user's responsibility to set or not set targetSubID for each message as appropriate.
		/// </summary>
		private bool useTargetSubID;
		/// <summary> Determines whether to accept the logon sequence number of the counter party's logon message, even if lower
		/// default: false
		/// </summary>
		private bool acceptLogonSeqNum;
		
		/// <summary> determines whether messages are copied before being sent to listeners, or before being put on the internal
		/// queue that holds application messages.  If true, a copy is made before someone calls accept() and a new copy
		/// is made for each listener.  If false, the same instance is given to all listeners as well as to whoever
		/// calls the accept() method.
		/// </summary>
		private bool copyMessagesToListeners;
		
		private bool syncTestRequest;
		private bool checkSendingTime;
		
		// @todo remove, not sure if this flag works when set (true), default is false
		private bool waitForMsgSeqNumSyncBeforeSendingAppMessages;
		
		/// <summary> controls whether the driver adheres strictly to the FIX spec in determining whether a received message
		/// should be rejected at the session level or not.
		/// </summary>
		private bool strictReceiveMode;
		
		/// <summary> for client:
		/// <br>true: sets the senderSubID on all messages
		/// <br>false: sets the senderSubID on protocol messages only
		/// <br>senderSubID should be explicitly set in the driver, if not set this flag has no effect.
		/// <p>
		/// for server:
		/// <br>true: sets the targetSubID on all messages
		/// <br>false: sets the targetSubID on protocol messages only
		/// <br>targetSubID can be explicitly set in the driver, default value is the senderSubID that the client
		/// sent in the logon message.  If the server's targetSubID is null this flag has no effect.
		/// <p>
		/// This flag has no effect on the targetSubID (for client) and the senderSubID (for server)
		/// </summary>
		private bool setSubIDOnApplicationMessages_Renamed_Field;
		
		/// <summary> controls whether the driver will overwrite sender/target CompIDs in sent messages with the correct value.
		/// <br>If the application leaves sender/target comp ID blank the driver will always fill it in.
		/// <br>If the application sets sender/target compID to something other than the expected value:
		/// <li>if overwriteCompID is true the application's value is overwritten with the "correct" value from the driver
		/// <li>if overwriteCompID is false the application's value is left alone, allowing a message with the "wrong" value to be sent.
		/// </summary>
		private bool overwriteCompID;
		
		/// <summary> Controls whether driver will overwrite bodylength in transmitting message.</summary>
		private bool overwriteBodyLength;
		
		/// <summary> Controls if it needs to re order the fields.</summary>
		private bool reorderFields = true;
		
		/// <summary> Controls if driver needs to set check sum or not.</summary>
		private bool setCheckSum = true;
		
		/// <summary> some properties can't be changed after session has started.</summary>
		private bool locked;
		
		public virtual void  setupExplicitFromProps()
		{
			System.String val;
			val = this.Get(SessionProperties.Prop_UseTargetSubID);
			if (val != null && val.Trim().Length > 0)
			{
				UseTargetSubID = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_SyncTestRequest);
			if (val != null && val.Trim().Length > 0)
			{
				SyncTestRequest = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_CopyMessagesToListeners);
			if (val != null && val.Trim().Length > 0)
			{
				CopyMessagesToListeners = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_CheckSendingTime);
			if (val != null && val.Trim().Length > 0)
			{
				CheckSendingTime = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_WaitForMsgSeqNumSyncBeforeSendingAppMessages);
			if (val != null && val.Trim().Length > 0)
			{
				WaitForMsgSeqNumSyncBeforeSendingAppMessages = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_StrictReceiveMode);
			if (val != null && val.Trim().Length > 0)
			{
				StrictReceiveMode = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_SubIDOnApplicationMessages);
			if (val != null && val.Trim().Length > 0)
			{
				SubIDOnApplicationMessages = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_OverwriteCompID);
			if (val != null && val.Trim().Length > 0)
			{
				OverwriteCompID = BasicUtilities.isTrue(val);
			}
			
			val = this.Get(SessionProperties.Prop_OverwriteBodyLength);
			if (val != null && val.Trim().Length > 0)
			{
				OverwriteBodyLength = BasicUtilities.isTrue(val);
			}
		}
		
		public SessionProperties():base(new System.Collections.Specialized.NameValueCollection())
		{
			
			setDefault(SessionProperties.Prop_AnswerTestRequests, true);
			setDefault(SessionProperties.Prop_SuppressHeartbeats, false);
			setDefault(SessionProperties.Prop_TestRequestBailout, 2);
			setDefault(SessionProperties.Prop_PrintFixBefore, false);
			setDefault(SessionProperties.Prop_PrintFixAfter, false);
			setDefault(SessionProperties.Prop_PrintFixHumanReadable, true);
			setDefault(SessionProperties.Prop_SendDatesWithMilliseconds, true);
			setDefault(SessionProperties.Prop_PersistAlways, false);
			setDefault(SessionProperties.Prop_RequestMultipleResends, false);
			
			UseTargetSubID = true;
			AcceptLogonSeqNum = false;
			SyncTestRequest = true;
			CopyMessagesToListeners = false;
			// note: this default is not FIX compliant
			CheckSendingTime = false;
			WaitForMsgSeqNumSyncBeforeSendingAppMessages = true;
			StrictReceiveMode = false;
			SubIDOnApplicationMessages = false;
			OverwriteCompID = true;
			OverwriteBodyLength = true;
			
			// no defaults for:
			// - Prop_FixVersion
			
			/*
			defaults.setProperty(Prop_EndLine,"\r\n");
			defaults.setProperty(Prop_EndMsg,"\n");
			
			defaults.setProperty(Prop_Parse_EndLine,"\n");
			
			defaults.setProperty(Prop_ResendRequestThreshold, Integer.toString(5000));
			defaults.setProperty(Prop_MaxInSeqNum, Integer.toString(999999));
			defaults.setProperty(Prop_MaxOutSeqNum, Integer.toString(999999));
			defaults.setProperty(Prop_UseActivityID,String.valueOf(false));
			defaults.setProperty(Prop_UseLine4bStandard,String.valueOf(false));
			defaults.setProperty(Prop_UseLine5Standard,String.valueOf(false));
			defaults.setProperty(Prop_UseLine5StandardOptions,String.valueOf(false));
			defaults.setProperty(Prop_Line5ContraSeperator," ");
			defaults.setProperty(Prop_Line5RepeatExInfo,String.valueOf(false));
			defaults.setProperty(Prop_Line5RepeatExInfo,String.valueOf(false));
			defaults.setProperty(Prop_UseSSTEX,String.valueOf(true));
			defaults.setProperty(Prop_PossDupInTrailer,String.valueOf(false));
			defaults.setProperty(Prop_ResendInTrailer,String.valueOf(true));
			defaults.setProperty(Prop_ReportTimeFormat,"HHmm");
			defaults.setProperty(Prop_RefDateFormat,"MMddyyyy");
			defaults.setProperty(Prop_TrailerFormat,"OLA {######}");
			defaults.setProperty(Prop_CMSLine,Integer.toString(1));
			defaults.setProperty(Prop_TcpipInterface,Integer.toString(CMSConstants.TCPIP_CAPInterface));
			defaults.setProperty(Prop_CmsTrace,String.valueOf(false));
			defaults.setProperty(Prop_UseAccountTypeLine,String.valueOf(false));
			//defaults.setProperty(Prop_SendFirmIdentifier,String.valueOf(false));
			defaults.setProperty(Prop_DefaultBranchCode, "HHH");
			defaults.setProperty(Prop_SisHeartbeatInterval,Integer.toString(30));
			defaults.setProperty(Prop_HeartbeatInterval,Integer.toString(30));
			defaults.setProperty(Prop_MaxCacheSize,Integer.toString(0));
			defaults.setProperty(Prop_HeartbeatSendThreshold,String.valueOf(2));
			defaults.setProperty(Prop_SeqNumIncrement,Integer.toString(10));
			defaults.setProperty(Prop_SystemCheckText,"SYSTEM CHECK\r\n");
			defaults.setProperty(Prop_CancelReplaceTLTCAdminText,"TLTC");
			defaults.setProperty(Prop_LogLevel,String.valueOf(ErrorLog.INFO));
			defaults.setProperty(Prop_XPRUrOutsEnabled,String.valueOf(true));
			defaults.setProperty(Prop_LoggerName,"CMSLogger");
			defaults.setProperty(Prop_CMSTimeZone,CMSConstants.defaultTimeZone);
			defaults.setProperty(Prop_NumHeaderLines,String.valueOf(1));
			defaults.setProperty(Prop_ResendLastSeconds,String.valueOf(30));
			defaults.setProperty(Prop_AppendTrailer,String.valueOf(true));
			defaults.setProperty(Prop_TrimBeforeAppendTrailer,String.valueOf(false));
			*/
			
			// parseDateFormat = new SimpleDateFormat("MMddyy", TimeZone.getTimeZone(CMSConstants.defaultTimeZone));
		}
		
		/* package */
		internal virtual void  setLocked()
		{
			locked = true;
		}
		
		private void  checkNotLocked()
		{
			if (locked)
			{
				throw new System.SystemException("Can't change this property after the session has started.");
			}
		}
		
		// logic for String keys ____________________________________________________________________
		
		// old school - @remove
		
		protected internal virtual System.Object setDefault(System.String key, bool value_Renamed)
		{
			return setDefault(key, System.Convert.ToString(value_Renamed));
		}
		
		protected internal virtual System.Object setDefault(System.String key, int value_Renamed)
		{
			return setDefault(key, System.Convert.ToString(value_Renamed));
		}
		
		protected internal virtual System.Object setDefault(System.String key, System.String value_Renamed)
		{
			System.Object tempObject;
            tempObject = defaults[key];
            defaults[key] = value_Renamed;
            return defaults is com.scalper.fix.driver.SessionProperties?((com.scalper.fix.driver.SessionProperties) defaults).setProperty(key, value_Renamed):tempObject;
		}
		
		public virtual System.Object setProperty(System.String key, bool value_Renamed)
		{
			System.Object tempObject;
			tempObject = base[key];
			base[key] = System.Convert.ToString(value_Renamed);
			return tempObject;
		}
		
		public virtual System.Object setProperty(System.String key, int value_Renamed)
		{
			System.Object tempObject;
			tempObject = base[key];
			base[key] = System.Convert.ToString(value_Renamed);
			return tempObject;
		}
		
		public System.Object setProperty(System.String key, System.String value_Renamed)
		{
			value_Renamed = handleInternal(key, value_Renamed);
			
			System.Object tempObject;
			tempObject = base[key];
			base[key] = value_Renamed;
			return tempObject;
		}
		
		public System.Object put(System.Object key, System.Object value_Renamed)
		{
			value_Renamed = handleInternal((System.String) key, (System.String) value_Renamed);
			
			if (value_Renamed != null)
			{
				System.Object tempObject;
				tempObject = base[(System.String) key];
				base[(System.String) key] = (System.String) value_Renamed;
				return tempObject;
			}
			else
				return null;
		}
		
		internal virtual System.String handleInternal(System.String key, System.String value_Renamed)
		{
			System.String newValue = value_Renamed;
			if ((key != null) && (value_Renamed != null))
			{
				int idx = value_Renamed.IndexOf("\\n");
				while (idx >= 0)
				{
					newValue = value_Renamed.Substring(0, (idx) - (0)) + "\n" + value_Renamed.Substring(idx + 2);
					idx = newValue.IndexOf("\\n");
				}
				idx = value_Renamed.IndexOf("\\r");
				while (idx >= 0)
				{
					newValue = value_Renamed.Substring(0, (idx) - (0)) + "\r" + value_Renamed.Substring(idx + 2);
					idx = newValue.IndexOf("\\r");
				}
			}
			return newValue;
		}
		
		public override System.String Get(System.String key)
		{
			String val = base.Get(key);
            if (val == null)
            {
                val = defaults[key];
            }
            return val;
		}
		
		public virtual bool getBooleanProperty(System.String key)
		{
			return BasicUtilities.isTrue(Get(key));
		}
		
		public virtual int getIntegerProperty(System.String key)
		{
			System.String val = Get(key);
			if (val == null)
			{
				return 0;
			}
			try
			{
				return System.Int32.Parse(val);
			}
			catch 
			{
				return 0;
			}
		}
		
		
		
        
	}
}