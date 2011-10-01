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
using Microsoft.Win32;
using System.Windows.Forms;

using BasicFixUtilities = Dukascopy.com.scalper.fix.BasicFixUtilities;
using Constants = Dukascopy.com.scalper.fix.Constants;
using FixConstants = Dukascopy.com.scalper.fix.FixConstants;
using FixException = Dukascopy.com.scalper.fix.FixException;
using FixMessageContentException = Dukascopy.com.scalper.fix.FixMessageContentException;
using FixMessageFormatException = Dukascopy.com.scalper.fix.FixMessageFormatException;
using Message = Dukascopy.com.scalper.fix.Message;
using TagHelper = Dukascopy.com.scalper.fix.TagHelper;
using BasicUtilities = Dukascopy.com.scalper.util.BasicUtilities;
using BoundedQueue = Dukascopy.com.scalper.util.BoundedQueue;
using System.Text.RegularExpressions;
using Dukascopy.com.scalper.util;


using log4net;
using log4net.Config;

namespace  Dukascopy.com.scalper.fix.driver
{

    /// <summary> <p>Description: This class represents a FIX session.
    /// FFillFixSession provides the methods/interfaces that user application
    /// should be using to send and receive FIX messages with counterparties
    /// on an established FIX connection.</p>
    /// </summary>
    public class ScalperFixSession : FIXListenRegistry
    {
        private void InitBlock()
        {
            transmitLock = new TransmitLock();
            inAdminMsgStoreLock = new AdminMessageStoreLock();
            sessionStateLock = new SessionStateLock();
            sessionState = new SessionState(SessionState.WAITING_FOR_LOGON, transmitLock);
        }
        virtual public MessageTypeTable MessageTypeTable
        {
            get
            {
                return listenerHelper.MessageTypeTable;
            }

        }
        virtual public int InMsgCount
        {
            get
            {
                return inMsgCount;
            }

        }
        virtual public int OutMsgCount
        {
            get
            {
                return outMsgCount;
            }

        }
        virtual public bool PerfOn
        {
            get
            {
                return isPerfOn_Renamed_Field;
            }

            set
            {
                isPerfOn_Renamed_Field = value;
            }

        }
        /// <summary> Checks if session status is alive or dead.</summary>
        /// <returns> true if session is alive otherwise false.
        /// </returns>
        virtual public bool Active
        {
            get
            {
                return !sessionClosed;
            }

        }
        /// <summary> Returns the FIX version usable for BeginString.</summary>
        /// <returns> Returns the fixVersion.
        /// </returns>
        private System.String BeginFixVersion
        {
            get
            {
                if (beginFixVersion == null)
                {
                    try
                    {
                        setFIXVersionFromProperty();
                    }
                    catch (System.Exception e)
                    {
                        log.Debug("Cannot set FIX Version.", e);
                    }
                    if (beginFixVersion == null)
                        throw new System.SystemException("Fix version not set yet.");
                }
                return beginFixVersion;
            }

        }
        /// <summary> returns true or false immediately to indicate isLoggedIn status</summary>
        virtual public bool LoggedIn
        {
            get
            {
                return sessionState.State == SessionState.SESSION_READY;
            }

        }
        /// <summary> Returns the maximum time to wait for logon response.</summary>
        /// <returns> Returns the maxLoginWaitTime.
        /// </returns>
        /// <summary> Sets the maximum time to wait for logon response.</summary>
        /// <param name="maxLoginWaitTime">The maxLoginWaitTime to set.
        /// </param>
        virtual public int MaxLoginWaitTime
        {
            get
            {
                return maxLoginRespWaitTime;
            }

            set
            {
                this.maxLoginRespWaitTime = value;
            }

        }
        /// <summary> Returns the maximum time to wait for logout response.</summary>
        /// <returns> Returns the maxLogoutWaitTime.
        /// </returns>
        /// <summary> Sets the maximum time to wait for logon response.</summary>
        /// <param name="maxLogoutWaitTime">The maxLogoutWaitTime to set.
        /// </param>
        virtual public int MaxLogoutWaitTime
        {
            get
            {
                return maxLogoutRespWaitTime;
            }

            set
            {
                this.maxLogoutRespWaitTime = value;
            }

        }
        /// <summary> Returns the maximum retry count for requesting a missed message.</summary>
        /// <returns> Returns the maxReqRetryCount.
        /// </returns>
        /// <summary> Sets the maximum retry count forrequesting a missed message.</summary>
        /// <param name="maxReqRetryCount">The maxReqRetryCount to set.
        /// </param>
        virtual public int MaxReqRetryCount
        {
            get
            {
                return maxReqRetryCount;
            }

            set
            {
                this.maxReqRetryCount = value;
            }

        }
        /// <summary> Returns the maximum retry count for resending a message.</summary>
        /// <returns> Returns the maxSendRetryCount.
        /// </returns>
        /// <summary> Sets the maximum retry count for resending a message.</summary>
        /// <param name="maxSendRetryCount">The maxSendRetryCount to set.
        /// </param>
        virtual public int MaxSendRetryCount
        {
            get
            {
                return maxSendRetryCount;
            }

            set
            {
                this.maxSendRetryCount = value;
            }

        }
        /// <returns> Returns the maxUndeliverCount.
        /// </returns>
        /// <param name="maxUndeliverCount">The maxUndeliverCount to set.
        /// </param>
        virtual public int MaxUndeliverCount
        {
            get
            {
                return maxUndeliverCount;
            }

            set
            {
                this.maxUndeliverCount = value;
            }

        }
        /// <returns> Returns the maxUnDeliverTime.
        /// </returns>
        /// <param name="maxUnDeliverTime">The maxUnDeliverTime to set.
        /// </param>
        virtual public int MaxUnDeliverTime
        {
            get
            {
                return maxUnDeliverTime;
            }

            set
            {
                this.maxUnDeliverTime = value;
            }

        }
        /// <returns> Returns the maxUnprocessCount.
        /// </returns>
        /// <param name="maxUnprocessCount">The maxUnprocessCount to set.
        /// </param>
        virtual public int MaxUnprocessCount
        {
            get
            {
                return maxUnprocessCount;
            }

            set
            {
                this.maxUnprocessCount = value;
            }

        }
        /// <returns> Returns the maxUnProcessTime.
        /// </returns>
        /// <param name="maxUnProcessTime">The maxUnProcessTime to set.
        /// </param>
        virtual public int MaxUnProcessTime
        {
            get
            {
                return maxUnProcessTime;
            }

            set
            {
                this.maxUnProcessTime = value;
            }

        }
        /// <returns> Returns the maxUnSentCount.
        /// </returns>
        /// <param name="maxUnSentCount">The maxUnSentCount to set.
        /// </param>
        virtual public int MaxUnSentCount
        {
            get
            {
                return maxUnSentCount;
            }

            set
            {
                this.maxUnSentCount = value;
            }

        }
        /// <summary> Returns TRUE if session is rejecting duplicate logon message from the counter-party.</summary>
        /// <returns> Returns the rejectDupLogon.
        /// </returns>
        /// <summary> Sets the flag that indicates if session need to reject duplicate logon.</summary>
        /// <param name="rejectDupLogon">The rejectDupLogon to set.
        /// </param>
        virtual public bool RejectDupLogon
        {
            get
            {
                return rejectDupLogon;
            }

            set
            {
                this.rejectDupLogon = value;
            }

        }
        /// <returns> Returns the inPersisterSize.
        /// </returns>
        /// <summary> Sets the inbound persistant store's size.</summary>
        /// <param name="inPersisterSize">The inPersisterSize to set.
        /// </param>
        virtual public int InPersisterSize
        {
            get
            {
                return inPersisterSize;
            }

            set
            {
                this.inPersisterSize = value;
            }

        }
        /// <summary> Checks if driver sets teh default values for the mandatory header fields or not.</summary>
        /// <returns> Returns the setDefaultValue.
        /// </returns>
        /// <summary> Sets the flag that tells driver if the default values for the mandatory tags has
        /// to be set or not.
        /// </summary>
        /// <param name="setDefaultValue">The setDefaultValue to set.
        /// </param>
        virtual public bool SetDefaultValue
        {
            get
            {
                return setDefaultValue;
            }

            set
            {
                this.setDefaultValue = value;
            }

        }
        /// <summary> Sets the flag that tells driver to send before a logon is received</summary>
        /// <param name="setDefaultValue">The setDefaultValue to set.
        /// </param>
        virtual public bool SendBeforeLogon
        {
            set
            {
                this.sendBeforeLogon = value;
            }

        }
        /// <summary> if not in test mode (normal) sets senderCompID, targetCompID.  Also sets senderSubID, targetSubID
        /// if the driver has values for them.
        /// If in test mode, sets values as above but only if there was no previous value set.  This allows the
        /// user to deliberately send incorrect values, or to leave the tag out entirely.
        /// </summary>
        private Message OptionalTags
        {
            set
            {
                if (!sendBeforeLogon)
                {
                    if (senderCompID == null || targetCompID == null)
                    {
                        throw new System.SystemException("Must set sender/targetCompID before sending messages.");
                    }
                }

                if (OverwriteCompID)
                {
                    // ignore the values in the original message, replace with "known" values.
                    if (senderCompID != null)
                        value.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i, senderCompID);
                    if (targetCompID != null)
                        value.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i, targetCompID);
                }
                else if (!testMode)
                //This is to provide facility in test mode to send the id's from def files.
                {
                    // leave values alone if they are set
                    if (senderCompID != null)
                        value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i, senderCompID);
                    if (targetCompID != null)
                        value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i, targetCompID);
                }

                if (value.Admin || SetSubIDOnApplicationMessages)
                {
                    // overwrite senderSubID if user wants it
                    if (senderSubID != null)
                    {
                        value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderSubID_i, senderSubID);
                    }

                    // overwrite targetSubID if user wants it
                    if (targetSubID != null)
                    {
                        value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetSubID_i, targetSubID);
                    }
                }

                value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderLocationID_i, senderLocationID, false);
                value.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetLocationID_i, targetLocationID, false);
            }

        }
        /// <returns> Returns the maxLoginRespWaitTime.
        /// </returns>
        /// <param name="maxLoginRespWaitTime">The maxLoginRespWaitTime to set.
        /// </param>
        virtual public int MaxLoginRespWaitTime
        {
            get
            {
                return maxLoginRespWaitTime;
            }

            set
            {
                this.maxLoginRespWaitTime = value;
            }

        }
        /// <returns> Returns the maxLogoutRespWaitTime.
        /// </returns>
        /// <param name="maxLogoutRespWaitTime">The maxLogoutRespWaitTime to set.
        /// </param>
        virtual public int MaxLogoutRespWaitTime
        {
            get
            {
                return maxLogoutRespWaitTime;
            }

            set
            {
                this.maxLogoutRespWaitTime = value;
            }

        }
        /// <returns> Returns the nextExpectedSeqNo.
        /// </returns>
        /// <param name="nextExpectedSeqNo">The nextExpectedSeqNo to set.
        /// </param>
        virtual public int InMsgSeqNo
        {
            get
            {
                return nextExpectedSeqNo;
            }

            set
            {
                this.nextExpectedSeqNo = value;
            }

        }
        /// <summary> returns this session's unique ID, or null if none was explicitly set.</summary>
        /// <summary> sets this session's unique ID, used in some logging statements.
        /// The ID, once set, cannot be changed.
        /// </summary>
        virtual public System.String ID
        {
            get
            {
                return id;
            }

            set
            {
                if (id != null)
                {
                    throw new System.SystemException("ID has already been set, was " + id + ", tried to set to " + value);
                }
                id = value;
            }

        }
        /// <returns> Returns the senderCompID.
        /// </returns>
        /// <param name="senderCompID">The senderCompID to set.
        /// </param>
        virtual public System.String SenderCompID
        {
            get
            {
                return senderCompID;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentException();
                }

                this.senderCompID = value;
            }

        }
        /// <returns> Returns the senderLocationID.
        /// </returns>
        /// <summary> sets the senderLocationID</summary>
        virtual public System.String SenderLocationID
        {
            get
            {
                return senderLocationID;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentException();
                }

                this.senderLocationID = value;
            }

        }
        /// <returns> Returns the targetLocationID.
        /// </returns>
        /// <summary> sets the targetLocationID</summary>
        virtual public System.String TargetLocationID
        {
            get
            {
                return targetLocationID;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentException();
                }

                this.targetLocationID = value;
            }

        }
        /// <returns> Returns the senderCompID.
        /// </returns>
        /// <param name="senderCompID">The senderCompID to set.
        /// </param>
        virtual public System.String SenderSubID
        {
            get
            {
                return senderSubID;
            }

            set
            {
                this.senderSubID = value;
            }

        }
        /// <returns> Returns the targetCompID.
        /// </returns>
        /// <param name="targetCompID">The targetCompID to set.
        /// </param>
        virtual public System.String TargetCompID
        {
            get
            {
                return targetCompID;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentException();
                }
                this.targetCompID = value;
            }

        }
        /// <returns> Returns the targetSubID.
        /// </returns>
        /// <param name="targetCompID">The targetCompID to set.
        /// </param>
        virtual public System.String TargetSubID
        {
            get
            {
                return targetCompID;
            }

            set
            {
                this.targetSubID = value;
            }

        }
        /// <returns> Returns the current heartbeat interval in ms
        /// </returns>
        /// <summary> sets the heartbeat interval to the given value.
        /// Also sets the test request interval to 20% more than the given value.
        /// (This is a ballpark figure suggested by the spec.)
        /// </summary>
        virtual public int HeartbeatInterval
        {
            get
            {
                return timer.HBeatInterval;
            }

            set
            {
                timer.HBeatInterval = value;
                timer.TestRequestInterval = value + value / 5;
            }

        }
        private bool StrictReceiveMode
        {
            get
            {
                return properties.StrictReceiveMode;
            }

        }
        private bool AnswerTestRequests
        {
            get
            {
                return properties.getBooleanProperty(SessionProperties.Prop_AnswerTestRequests);
            }

            set
            {
                if (!testMode && !value)
                {
                    throw new System.ArgumentException("Must be in test mode to not answer test requests.");
                }
                properties.setProperty(SessionProperties.Prop_AnswerTestRequests, value);
            }

        }
        private bool SuppressHeartbeats
        {
            get
            {
                return properties.getBooleanProperty(SessionProperties.Prop_SuppressHeartbeats);
            }

        }
        private bool PrintFixBefore
        {
            get
            {
                return properties.getBooleanProperty(SessionProperties.Prop_PrintFixBefore);
            }

        }
        private bool PrintFixAfter
        {
            get
            {
                return properties.getBooleanProperty(SessionProperties.Prop_PrintFixAfter);
            }

        }
        private bool PrintFixHumanReadable
        {
            get
            {
                return properties.getBooleanProperty(SessionProperties.Prop_PrintFixHumanReadable);
            }

        }
        virtual internal bool UseMillis
        {
            /* package */

            get
            {
                return minorVersion >= '2' && minorVersion <= '4' && properties.getBooleanProperty(SessionProperties.Prop_SendDatesWithMilliseconds);
            }

        }
        virtual protected internal int State
        {
            get
            {
                return sessionState.State;
            }

            set
            {
                sessionState.State = value;
            }

        }
        private bool CheckSendingTime
        {
            get
            {
                return properties.CheckSendingTime;
            }

        }
        private bool SetSubIDOnApplicationMessages
        {
            get
            {
                return properties.SetSubIDOnApplicationMessages;
            }

        }
        private bool OverwriteCompID
        {
            get
            {
                return properties.OverwriteCompID;
            }

        }
        private bool ReorderFields
        {
            get
            {
                return properties.ReorderFields;
            }

        }
        /// <returns> Returns the connection.
        /// </returns>
        virtual public FIXConnection Connection
        {
            get
            {
                return msgConn.Connection;
                // return connection;
            }

        }
        virtual public FIXPersister Persister
        {
            set
            {
                if (persister != null)
                {
                    throw new System.SystemException("Persister has already been set.");
                }
                if (value == null)
                {
                    throw new System.ArgumentException("NewPersister cannot be null.");
                }

                this.persister = value;
                nextExpectedSeqNo = persister.LastInSeqNo + 1;
                outMsgSeqNo = persister.LastOutSeqNo;
                uncompletedMessageStore.init(nextExpectedSeqNo);
            }

        }
        virtual public FIXPersister FIXPersister
        {
            get
            {
                return persister;
            }

        }
        private int LargestSessionRejectNumber
        {
            get
            {
                switch (minorVersion)
                {

                    case '2': return FixException.REJECT_REASON_INVALID_MSGTYPE;

                    case '3': return FixException.REJECT_REASON_NON_DATA_HAS_SOH;
                    // @todo this will probably need to be reworked when version 4.5 comes out

                    case '4': return FixException.REJECT_REASON_OTHER;

                    default: throw new System.ArgumentException();

                }
            }

        }
        private const System.String timeFormatString = "yyyyMMdd-HH:mm:ss";
        private const System.String timeFormatMillisString = "yyyyMMdd-HH:mm:ss.SSS";


        //Connection used for this session.
        private MessageConnection msgConn;
        //Reader thread that keeps polling socket for any new messages and
        //then frames it and throws it onto eigther admin msg queue or application msg queue.
        private FIXReaderThread reader;
        //Admin thread that looks for incoming admin messages and processes it.
        private FIXAdminProcessor adminProcessor;
        //Admin thread that looks for incoming test request messages and processes it.
        private FIXAdminProcessor testRequestProcessor;
        //Persistent store for application messages.
        private FIXPersister persister;
        //Session timer
        private FIXSessionTimer timer;
        //Internal queue for new inbound FIX messages.
        private BoundedQueue inMessageStore;
        //Internal queue for received application FIX messages that have not yet been marked as "completed"
        private UncompletedMessageStore uncompletedMessageStore;
        //Internal queue for new inbound admin messages.
        private System.Collections.ArrayList adminMessageStore;

        private System.Collections.ArrayList resendSeqs;
        //Flag that says whether session is closed. Accessed by multiple threads.
        private volatile bool sessionClosed = true;
        //This will be used to assign the next sequence number to outbound messages.
        //Used by multiple threads as when they try to send the message.
        private volatile int outMsgSeqNo = 0;

        //This is the counter to keep track of incoming messages and to see if we are receiving
        //messages in sequence or not.
        private volatile int nextExpectedSeqNo = 1;

        // if there's currently a resend in progress, this is the last MsgSeqNum that we're expecting
        // the other side to send us.  Note that the current implementation *always* sends out resends
        // with "infinity" as the upper bound; this variable is only for internal state.
        private volatile int lastExpectedFromResend = -1;

        // The transmit lock represents the right to put messages on the wire.
        // it protects not only I/O itself, but also the MsgSeqNum that the message gets,
        // the persister, and other related business logic.
        private System.Object transmitLock;
        //Lock for inbound business message store.
        //private final Object inMessageStoreLock = new InboundMessageStoreLock();
        //Lock for inbound admin message store.
        private System.Object inAdminMsgStoreLock;
        //Lock used to wait till session is in ready state.
        private System.Object sessionStateLock;
        //Initial session state.
        // private int sessionState.setState(SessionState.SESSION_NOT_READY);
        private SessionState sessionState;
        //used to configure the session
        private SessionProperties properties;
        //used to configure a client session, null if this is a server session.
        private ClientSessionCustomizer clientCustomizer;
        //Sender company ID for this session.
        private System.String senderCompID;
        //Sender sub ID for this session.
        private System.String senderSubID;
        //Sender location ID for this session.
        private System.String senderLocationID;
        //Target location ID for this session.
        private System.String targetLocationID;
        //target company ID for this session.
        private System.String targetCompID;
        //target company sub ID for this session.
        private System.String targetSubID;

        // license check variables
        internal static bool displayT = true;
        internal static System.String Exchanges = null;
        internal static System.Object licenseLock = new System.Object();

        //unique ID for this session (used in some logging)
        private System.String id;

        //BeginString for the session
        // private String beginString;

        //State variable to indicate if logon was successfull for this session.
        private bool logonSuccess = false;

        // these 3 are only used on startup to help the session figure out what it needs to do to make the transition
        // between WAITING_FOR_MESSAGE_SYNCH and SESSION_READY
        private bool sentTestRequest;
        private bool sendBeforeLogon = false;

        //This is a simple queue used to store new messages from resend operation.
        private System.Collections.ArrayList queuedMsgFromResend;

        //FIX protocol version.
        private System.String fixVersion; // = "4.4";
        private System.String beginFixVersion; //="FIX.4.4"
        // helper for header/footer tags
        private TagHelper tagHelper;
        //Test mode flag(off by default);
        private bool testMode = false;
        //value for resend infinity (see RESEND_INFINITY_40_41 and RESEND_INFINITY_42)
        private int resendInfinity = -1;
        //Maximum number of message that can be queued up in outbound store.
        private int maxUnSentCount;
        //Maximum time for which message can stay in inbound store.
        private int maxUnDeliverTime;
        //Maximum number of messages that can be queued up in inbound store.
        private int maxUndeliverCount = 100;
        //Maximum time for which message has not been processed.
        private int maxUnProcessTime;
        //Maximum number of messages that has not been processed.
        private int maxUnprocessCount;
        //Maximum number of retries made to request missing message.
        private int maxReqRetryCount = 3;
        //Maximum number of retries to resend the message.
        private int maxSendRetryCount = 3;
        //Maximum time to wait for login response.
        private int maxLoginRespWaitTime = 5000;
        //Maximum time to wait for logout response.
        private int maxLogoutRespWaitTime = 5000;
        //maximu size of the inbound persister, -1 indicates unlimited.
        private int inPersisterSize = -1;
        //Following are the flags to indicate if user want to handle the different admin messages.
        private bool rejectDupLogon = true;
        private FIXSessionListenerHelper listenerHelper;

        //Flag that tells if driver needs to set default fields to the value it has stored rom login.
        private bool setDefaultValue = true;
        //Character that stores the FIX minor version for the session.
        internal char minorVersion = '\x0000';

        // Format to be used while parsing the date from the string(source)
        // UTCTimestamp format for whole seconds
        private String timeFormatter = ScalperFixSession.timeFormatString;

        private long sendingTimeMaxGap = 120000;

        private bool persistAlways = false;

        private bool isPerfOn_Renamed_Field = false;
        private int inMsgCount = 0;
        private int outMsgCount = 0;

        private static readonly ILog log = LogManager.GetLogger(typeof(ScalperFixSession));

        private class TransmitLock
        {
        }
        private class AdminMessageStoreLock
        {
        }
        private class SessionStateLock
        {
        }

        public ScalperFixSession(FIXConnection conn, SessionProperties properties, SessionCustomizer sessionCustomizer)
            : this(new DefaultMessageConnection(conn), properties, sessionCustomizer)
        {
        }

        public ScalperFixSession(MessageConnection msgConn, SessionProperties properties, SessionCustomizer sessionCustomizer)
        {
            InitBlock();
            setProperties(properties);
            this.msgConn = msgConn;

            if (sessionCustomizer is ClientSessionCustomizer)
            {
                clientCustomizer = (ClientSessionCustomizer)sessionCustomizer;
            }
            else
            {
                throw new System.ArgumentException("sessionCusomizer " + sessionCustomizer + " must be a client or server session customizer.");
            }

            listenerHelper = new FIXSessionListenerHelper();
            //Internal queue for new inbound messages.
            inMessageStore = new BoundedQueue("Inbound FIX Message store");
            uncompletedMessageStore = new UncompletedMessageStore();
            //Internal queue for new admin messages.
            adminMessageStore = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            queuedMsgFromResend = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            resendSeqs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            //Follwing are the different thread that will be maintained inside the session.
            reader = new FIXReaderThread(this, "Reader thread");
            adminProcessor = createAdminProcessor();
            testRequestProcessor = createAdminProcessor();
            testRequestProcessor.Priority = (System.Threading.ThreadPriority)System.Threading.ThreadPriority.Highest;
            timer = new FIXSessionTimer(this, properties);
            // heartbeats every 30 seconds
            HeartbeatInterval = 30000;
            FIXPersister persister = clientCustomizer.Persister;
            if (persister == null)
            {
                log.Warn("No persister specified, using default in-memory persister.");
                Persister = new FIXVolatilePersister();
            }
            else
            {
                Persister = persister;
            }            
        }

        protected internal virtual FIXAdminProcessor createAdminProcessor()
        {
            return new FIXAdminProcessor(this);
        }

        private void setProperties(SessionProperties properties)
        {
            if (properties == null)
            {
                // use defaults
                properties = new SessionProperties();
            }
            this.properties = properties;
            try
            {
                persistAlways = properties.getBooleanProperty(SessionProperties.Prop_PersistAlways);
            }
            catch (System.Exception e)
            {
                log.Error("Error in set properties",e);
                SupportClass.WriteStackTrace(e, Console.Error);
                persistAlways = false;
            }
        }

        public virtual SessionProperties getProperties()
        {
            return properties;
        }

        /* package */
        /// <summary> Create the fields required for HeartBeatMessage FIX message.
        /// This method checks if the field TestReqID needs to be created for this
        /// Heartbeat message.
        /// This is for internal use by the driver.
        /// </summary>
        /// <param name="needTestReqID<br>">true - TestReqID field is created with the value set to timestamp.<br>
        /// false - TestReqID field is not created.
        /// </param>
        /// <returns> Heartbeat message
        /// </returns>
        /// <throws>  FixException </throws>
        internal virtual Message buildHeartBeatMessage(bool useTestRequestID)
        {
            System.String testReqID = null;
            if (useTestRequestID)
            {
                System.DateTime date = System.DateTime.Now.ToUniversalTime();
                testReqID = Message.buildDateString(ref date, false);
            }

            return buildHeartBeatMessage(testReqID);
        }

        /// <summary> Create the fields required for HeartBeatMessage FIX message.
        /// Set the field TestReqID with the value specified.
        /// TestReqID may be null, which indicates that this is a normal heartbeat
        /// (not in response to a test request), and no TestReqID tag is added to the
        /// heartbeat.
        /// 
        /// This is for internal use by the driver.
        /// </summary>
        /// <param name="testReqID">The field TestReqID is set to this value.
        /// </param>
        /// <returns> Heartbeat message
        /// </returns>
        /// <throws>  FIXException </throws>
        private Message buildHeartBeatMessage(System.String testReqID)
        {
            Message heartbeatMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGHeartbeat);

            // Add the TestReqID field
            if (testReqID != null)
                heartbeatMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i, testReqID);

            OptionalTags = heartbeatMsg;
            return heartbeatMsg;
        }

        /* package */
        /// <summary> Create the fields required for TestRequest FIX message.
        /// Sets the field TestReqID with the value specified.
        /// </summary>
        /// <param name="testReqID">
        /// </param>
        /// <throws>  FixException </throws>
        internal virtual Message buildTestRequestMessage(System.String testReqID)
        {
            Message testRequestMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGTestRequest);
            testRequestMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i, testReqID);

            OptionalTags = testRequestMsg;
            return testRequestMsg;
        }

        /// <summary> builds a new resend request message that requests just the single message specified
        /// 
        /// </summary>
        /// <param name="seqNo">the sequence number of the sender that you would like to request
        /// 
        /// private Message buildResendRequest(int seqNo) throws FixException
        /// {
        /// return buildResendRequest(seqNo, seqNo);
        /// } 
        /// </param>

        /// <summary> builds a new resend request message that requests all messages from the given sequence number onward
        /// 
        /// </summary>
        /// <param name="seqNo">the sequence number of the sender that you would like to request
        /// </param>
        private Message buildResendRequest_after(int seqNo, int lastExpected)
        {
            return buildResendRequest(seqNo, lastExpected, true);
        }

        /// <summary> Create the fields required for ResendRequest FIX message.
        /// Sets the fields BeinSeqNo and EndSeqNo with the values specified.
        /// This is for internal use by the driver.
        /// </summary>
        /// <param name="beginSeqNo"> 	The start of the range of messages to resend.
        /// </param>
        /// <param name="endSeqNo"> 		The end of the range of messages to resend. <br>
        /// To request a simgle message - beginSeqNo = endSeqNo<br>
        /// To request a range of messages - beginSeqNo = first message of range,
        /// endSeqNo = last message of range<br>
        /// To request all messages subsequent to a particular message - beginSeqNo = first message of range.
        /// FIX versions 4.0 and 4.1: endSeqNo = 999999 (represents infinity)<br>
        /// FIX versions 4.2 and up: endSeqNo = 0 (represents infinity)
        /// 
        /// </param>
        /// <throws>  	FixException </throws>
        private Message buildResendRequest(int beginSeqNo, int lastExpected, bool useInfinityInstead)
        {
            Message resendRequestMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGResendRequest);

            if (sessionState.otherSessionMustResend())
            {
                if (!properties.getBooleanProperty(SessionProperties.Prop_RequestMultipleResends))
                    return null;
                resendRequestMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossResend, "Y");
            }

            sessionState.setOtherSessionMustResend();

            resendRequestMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginSeqNo_i, beginSeqNo);
            if (useInfinityInstead)
            {
                resendRequestMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i, resendInfinity);
            }
            else
            {
                resendRequestMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i, lastExpected);
            }

            if (lastExpected >= lastExpectedFromResend)
            {
                lastExpectedFromResend = lastExpected;
            }
            else
            {
                log.Warn("LastExpectedFromResend is " + lastExpectedFromResend + ", issuing resend from " + beginSeqNo + " to " + (useInfinityInstead ? "infinity" : "" + lastExpected));
            }

            return resendRequestMsg;
        }

        /// <summary> Create the fields required for Reject FIX message.
        /// Set the field RefSeqNum with the value specified.
        /// 
        /// If the FixException is non-null, its getMessage() method will be used to populate
        /// the text of the Reject message.
        /// If we are in a version of FIX that supports it, the SessionRejectReason tag (373)
        /// will be set based on the code from the FixException.
        /// 
        /// This is for internal use by the driver.
        /// </summary>
        /// <param name="refSeqNum"> The sequence number of the message being referred.
        /// </param>
        /// <throws>  FixException </throws>
        private Message buildRejectMessage(Message badMsg, FixException fe)
        {
            log.Error("Rejecting " + badMsg.toString(false) + ", " + fe.Message);
            if (fe == null)
            {
                return buildRejectMessage(badMsg, FixException.REJECT_REASON_NOT_SET, null);
            }
            else
            {
                return buildRejectMessage(badMsg, fe.Code, fe.Message, fe.RefTagID);
            }
        }

        private Message buildRejectMessage(Message badMsg, int code, System.String reason)
        {
            return buildRejectMessage(badMsg, code, reason, -1);
        }

        private Message buildRejectMessage(Message badMsg, int code, System.String reason, int refTagID)
        {
            int badMsgSeqNum;
            if (badMsg == null)
            {
                badMsgSeqNum = -1;
            }
            else
            {
                badMsgSeqNum = badMsg.MsgSeqNum;
            }

            Message rejectMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGReject);

            rejectMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefSeqNum_i, badMsgSeqNum);

            if (reason != null && !reason.Equals(""))
            {
                rejectMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGText_i, reason);
            }

            if (minorVersion >= '2')
            {
                if (code != FixException.REJECT_REASON_NOT_SET)
                {
                    int largestSessionReject = LargestSessionRejectNumber;

                    if (code <= largestSessionReject)
                    {
                        rejectMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBusinessRejectReason_i, code);
                    }
                }
                if (refTagID > -1)
                {
                    rejectMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefTagID_i, refTagID);
                }
                System.String badMsgType = "";
                if (badMsg != null)
                    badMsgType = badMsg.MsgType;

                if (badMsgType != null && !badMsgType.Equals(""))
                    rejectMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefMsgType_i, badMsgType);
            }

            return rejectMsg;
        }

        /// <summary> Create the fields required for SequenceReset-GapFill FIX message.
        /// The msgSeqNum will be set to the sequence number that the other side is expecting.
        /// The newSeqNo is the next sequence number that we will be transmitting.
        /// See the FIX spec for more information.
        /// </summary>
        /// <param name="NewSeqNo"> value of the next sequence number to be expected by the recipient.
        /// </param>
        /// <throws>  FixException </throws>
        private Message buildSequenceReset_GapFillMessage(int msgSeqNum, int newSeqNo)
        {
            Message sequenceResetMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGSequenceReset);

            sequenceResetMsg.MsgSeqNum = msgSeqNum;
            sequenceResetMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGGapFillFlag_i, 'Y');
            sequenceResetMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i, newSeqNo);
            sequenceResetMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag_i, 'Y'); //PoosDupFlag has to be set.
            return sequenceResetMsg;
        }

        /// <summary> Create the fields required for Logout FIX message.
        /// This is for internal use by the driver.
        /// </summary>
        /// <throws>  FixException </throws>
        private Message buildLogoutMessage()
        {
            Message logoutMsg = createMessage(Dukascopy.com.scalper.fix.Constants_Fields.MSGLogout);

            return logoutMsg;
        }

        private Message createMessage(System.String msgType)
        {
            Message msg = createMessageTemplate();
            msg.createHeaderFields();
            // set the begin string to whatever version of FIX we are using
            msg.BeginStringField = BeginFixVersion;

            msg.setMsgTypeField(msgType);
            ScalperFixSession.createMandatoryFields(msg);
            //createTrailerFields();
            return msg;
        }


        /// <summary> Creates the mandatory fields for the FIX message based upon
        /// the specified value of MsgType.
        /// This method is for internal use by the driver. We are creating only
        /// the mandatory fields here, no optional fields are created.
        /// </summary>
        /// <param name="msgType">
        /// </param>

        /* package */
        // @todo private, non-static
        internal static void createMandatoryFields(Message msg)
        {
            System.String msgType = msg.MsgType;

            if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGHeartbeat))
            {
                // Do nothing
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGTestRequest))
            {
                // if Heartbeat message corresponding to TestRequest message
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i, "");
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGResendRequest))
            {
                // if ResendRequest message
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginSeqNo_i, 0);
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i, 0);
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGReject))
            {
                // if Reject message
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefSeqNum_i, 0);
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGSequenceReset))
            {
                // if SequenceReset message
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i, 0);
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGLogout))
            {
                // if Logout message
                // No mandatory fields - so by default, do nothing
            }
            else if (msgType.Equals("A"))
            {
                // If Logon message
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEncryptMethod_i, 0);
                msg.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGHeartBtInt_i, 0);
            }
        }

        /* package */
        internal virtual void readyToSendAppMessages()
        {
            if (sessionState.State != SessionState.WAITING_FOR_MESSAGE_SYNCH)
            {
                log.Warn("Session was told that it's legal to send application messages, but in wrong state: " + sessionState.State);
                return;
            }

            log.Info("Message sequence number synchronization complete, ready to send messages.");
            timer.stopWaitingForResend();

            //We are done with prework, now notify everybody who is waiting for session to be ready.
            lock (sessionStateLock)
            {
                //Set the session state to be ready
                State = SessionState.SESSION_READY;
                log.Info("FFillFixSession:Post logon work completed. Session is ready to send/receive messages");
                System.Threading.Monitor.PulseAll(sessionStateLock);
                possiblyNotifyListeners(this, FIXNotice.FIX_SESSION_ESTABLISHED, "FIX session established");
            }
        }

        /* package */
        internal virtual void clientSendTestRequest()
        {
            //Thread.dumpStack();
            // test requests can be sent through a different code path, but it doesn't matter
            if (sentTestRequest)
                return;
            sentTestRequest = true;
            // @todo do we need to track response?
            // @todo this waits if a resend is received, is that correct?
            try
            {
                if (properties.SyncTestRequest)
                    transmit(buildTestRequestMessage("TEST"));
                else
                    readyToSendAppMessages();
            }
            catch (FixException fe)
            {
                log.Error("Error in sending test request.", fe);
                SupportClass.WriteStackTrace(fe, Console.Error);
            }
        }

        /* package */
        internal virtual void waitForResendTimerExpired()
        {
            if (sessionState.State != SessionState.WAITING_FOR_MESSAGE_SYNCH)
            {
                //			return;
            }

            timer.resetWaitingForResend();
            clientSendTestRequest();
        }

        public virtual void addDisconnectListener(DisconnectListener dl)
        {
            listenerHelper.addDisconnectListener(dl);
        }

        public virtual void removeDisconnectListener(DisconnectListener dl)
        {
            listenerHelper.removeDisconnectListener(dl);
        }

        protected internal virtual void notifyDisconnectListeners()
        {
            listenerHelper.notifyDisconnectListeners(sessionState.LogoutSent, sessionState.LogoutReceived);
        }

        protected internal virtual bool possiblyNotifyListeners(System.Object source, int noticeCode, System.String noticeText)
        {
            return possiblyNotifyListeners(source, noticeCode, noticeText, null);
        }

        protected internal virtual bool possiblyNotifyListeners(System.Object source, int noticeCode, System.String noticeText, Message msg)
        {
            if ((listener != null) && listenerInterests[noticeCode])
            {
                FIXNotice notice = new FIXNotice(source, msg, this, noticeCode, noticeText);
                listener.takeNote(notice);
                return true;
            }
            return false;
        }

        protected internal virtual void notifyMessageSent(Message msg)
        {
            int sessionStateInt = sessionState.State;
            if (sessionStateInt != SessionState.SESSION_CLOSING && sessionStateInt != SessionState.SESSION_CLOSED)
            {
                listenerHelper.notifyMessageSent(msg, properties.CopyMessagesToListeners);
            }
            else
            {
                log.Info("Session is shutting down, not notifying message sent: " + msg);
            }
        }

        protected internal virtual void notifyMessageReceived(Message msg)
        {
            int sessionStateInt = sessionState.State;
            if (sessionStateInt != SessionState.SESSION_CLOSING && sessionStateInt != SessionState.SESSION_CLOSED)
            {
                listenerHelper.notifyMessageReceived(msg, properties.CopyMessagesToListeners);
            }
            else
            {
                log.Info("Session is shutting down, not notifying message received: " + msg);
            }
        }

        public virtual void addMessageListener(FIXMessageListener l)
        {
            listenerHelper.addMessageListener(l);
        }

        public virtual void removeMessageListener(FIXMessageListener l)
        {
            listenerHelper.removeMessageListener(l);
        }
        public virtual void flushAdminQueue()
        {
            adminProcessor.sendQueue();
        }
        /// <summary> Helper class to process admin messages
        /// <p>Copyright: Copyright (c) 2005</p>
        /// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
        /// </summary>
        /// <author>  Phaniraj Raghavendra
        /// </author>
        /// <version>  1.0
        /// </version>
        public class FIXAdminProcessor : SupportClass.ThreadClass
        {
            private void InitBlock(ScalperFixSession enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private ScalperFixSession enclosingInstance;
            public ScalperFixSession Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            private ResendCounter resendCounter;

            public FIXAdminProcessor(ScalperFixSession enclosingInstance)
            {
                InitBlock(enclosingInstance);
                resendCounter = new ResendCounter(this);
            }

            public volatile bool keepRunning;
            internal System.Collections.ArrayList vMessages = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            public virtual void requestStop()
            {
                lock (vMessages)
                {
                    System.Threading.Monitor.PulseAll(vMessages);
                }
            }
            public virtual void sendQueue()
            {
                while (!(vMessages.Count == 0) && keepRunning)
                {
                    SendCommand cmd;
                    lock (vMessages)
                    {
                        System.Object tempObject;
                        tempObject = vMessages[0];
                        vMessages.RemoveAt(0);
                        cmd = (SendCommand)tempObject;
                    }
                    if (cmd == null)
                        continue;
                    switch (cmd.cmdType)
                    {

                        //case Enclosing_Instance.SendCommand.SEND:
                        case SendCommand.SEND:
                            Message msg = cmd.msg;
                            if (msg.Admin)
                            {
                                System.String msgType = msg.MsgType;
                                bool checkIfResending = false;
                                if ((msgType != null) && msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGHeartbeat))
                                {
                                    System.String testReqID = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i);
                                    checkIfResending = (testReqID != null);
                                }
                                Enclosing_Instance.transmitInternal(msg, false, checkIfResending, true);
                                if (msgType != null)
                                {
                                    if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGReject))
                                    {
                                        Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_MESSAGE_REJECTED, "Message [" + cmd.origMsg + "] rejected", cmd.origMsg);
                                    }
                                }
                            }
                            else
                            {
                                Enclosing_Instance.transmitInternal(msg);
                            }
                            if ((vMessages.Count == 0))
                            {
                                sendQueuedResend();
                            }
                            break;

                        //case Enclosing_Instance.SendCommand.RESEND:
                        case SendCommand.RESEND:
                            serviceResend(cmd.msg, cmd.start, cmd.end);
                            break;
                    }
                }
            }
            internal virtual void sendQueuedResend()
            {
                try
                {
                    while (!(Enclosing_Instance.queuedMsgFromResend.Count == 0))
                    {
                        System.Object tempObject;
                        tempObject = Enclosing_Instance.queuedMsgFromResend[0];
                        Enclosing_Instance.queuedMsgFromResend.RemoveAt(0);
                        Message message = (Message)tempObject;
                        Enclosing_Instance.transmitInternal(message);
                    }
                }
                catch (FixException fe)
                {
                    // @todo what to do?
                    log.Error(fe);
                }
            }
            public virtual void serviceResend(Message msg, int startSeqNo, int endSeqNo)
            {
                lock (Enclosing_Instance.transmitLock)
                {
                    //Enclosing_Instance.sessionState.setThisSessionMustResend();
                    // persister will deal with the special case of infinity.
                    System.Collections.IEnumerator messagesToResend;
                    try
                    {
                        messagesToResend = Enclosing_Instance.persister.getOutboundFIXMessages(startSeqNo, endSeqNo);
                    }
                    catch (FixException fe)
                    {
                        log.Fatal("Couldn't get messages from persister, shutting down session. " + fe.Message);
                        //Enclosing_Instance.sessionState.clearThisSessionMustResend();
                        Enclosing_Instance.stopImmediately();
                        return;
                    }
                    if (endSeqNo == Enclosing_Instance.resendInfinity)
                        endSeqNo = Enclosing_Instance.persister.LastOutSeqNo;
                    int gapFillStartSeqNo = startSeqNo;

                    while (messagesToResend.MoveNext())
                    {
                        Message msgToSend = (Message)messagesToResend.Current;
                        int currentSeqNum = msgToSend.MsgSeqNum;
                        if (currentSeqNum < 1)
                        {
                            //Enclosing_Instance.sessionState.clearThisSessionMustResend();
                            throw new System.SystemException();
                        }

                        if (BasicFixUtilities.isAdmin(msgToSend))
                        {
                            //Enclosing_Instance.sessionState.clearThisSessionMustResend();
                            throw new System.ArgumentException("Can't resend admin message " + msgToSend);
                        }

                        // it's an application message, check if it's been resent more than the limit
                        resendCounter.incrementMessage(currentSeqNum);
                        if (resendCounter.getResendCount(currentSeqNum) > Enclosing_Instance.maxSendRetryCount)
                        {
                            log.Warn("FFillFixSession.transmit(): Failed to send the message " + msgToSend + " after trying " + Enclosing_Instance.maxSendRetryCount + "times.");

                            //Dont resend the message, send a notice and maybe a gapfill.
                            Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_RESEND_BAILOUT, "Failed to resend message,Sending gapfill");
                            continue;
                        }

                        // normal case - resend the given message
                        // @todo clone message?

                        // for application-level messages, ask the user whether the message should be resent or not.
                        // the user has two choices: have the message resent (with PossDup set to Y), or have
                        // a GapFill message sent instead.
                        bool resendMsg = Enclosing_Instance.clientCustomizer.shouldResendMessage(msg);
                        if (resendMsg)
                        {
                            // first transmit the gap fill for all previous messages
                            if (gapFillStartSeqNo < currentSeqNum)
                            {
                                try
                                {
                                    Message gapFillMsg = Enclosing_Instance.buildSequenceReset_GapFillMessage(gapFillStartSeqNo, currentSeqNum);
                                    Enclosing_Instance.transmitInternal(gapFillMsg, false, false, true);
                                }
                                catch (FixException fe)
                                {
                                    // @todo what to do?
                                    log.Error(fe);
                                }
                            }

                            try
                            {
                                Enclosing_Instance.transmitResent(msgToSend);
                                System.Threading.Thread.Sleep(0);
                            }
                            catch (FixException fe)
                            {
                                log.Error(fe);
                            }

                            // finally, get ready to potentially send a new gap fill message
                            gapFillStartSeqNo = currentSeqNum + 1;
                        }
                        else
                        {
                            // Means user doesn't want us to resend this message.
                            // In application-level code he may choose to add a new message to the resend queue,
                            // which will be sent out after the gap fill has completed.
                            // At this point we only care about completing the gap fill.  Any messages in that
                            // queue are checked after the resend request has been completely fulfilled and
                            // the other side is up to date with us.

                            //So lets do GapFIll
                            continue;
                        }
                    }

                    // one last GapFill message if necessary to cover all the admin messages at the end of the
                    // resend request
                    if (gapFillStartSeqNo <= endSeqNo)
                    {
                        try
                        {
                            Message gapFillMsg = Enclosing_Instance.buildSequenceReset_GapFillMessage(gapFillStartSeqNo, endSeqNo + 1);
                            Enclosing_Instance.transmitInternal(gapFillMsg, false, false, true);
                        }
                        catch (FixException fe)
                        {
                            // @todo what to do?
                            log.Error(fe);
                        }
                    }

                    // @todo should wait a certain amount of time before allowing other messages to be sent?
                    // open the floodgates -- other threads are now free to compete with this thread to transmit messages.
                    //Enclosing_Instance.sessionState.clearThisSessionMustResend();
                    //If there is anything queued up from the resend operation from user, send it now!
                    sendQueuedResend();
                    // now process the queued messages
                }
            }
            override public void Run()
            {
                keepRunning = true;
                while (keepRunning)
                {
                    try
                    {
                        lock (vMessages)
                        {
                            while ((vMessages.Count == 0))
                            {
                                System.Threading.Monitor.Wait(vMessages);
                                if (!keepRunning)
                                    break;
                            }
                        }
                        if (!keepRunning)
                            break;
                        sendQueue();
                    }
                    catch (System.Threading.ThreadInterruptedException e)
                    {
                        log.Error("Admin processor interrupted " + e.Message);
                    }
                    catch (FixException fe)
                    {
                        log.Error(fe);
                    }
                }
            }
            //({"unchecked","unchecked"})
            public virtual void queueTransmit(SendCommand cmd)
            {
                lock (vMessages)
                {
                    vMessages.Add(cmd);
                    System.Threading.Monitor.PulseAll(vMessages);
                }
            }

            public virtual void processMessage(Message msg)
            {
                try
                {
                    Enclosing_Instance.doValidate(msg);
                }
                catch (FixException fe)
                {
                    Enclosing_Instance.buildAndTransmitReject(msg, fe);

                    if (fe.shouldLogOutUser())
                    {
                        Message logoutMsg = null;
                        try
                        {
                            //Send logout mesage.
                            logoutMsg = Enclosing_Instance.buildLogoutMessage();
                            Enclosing_Instance.OptionalTags = logoutMsg;
                            log.Fatal("Fatal error: " + fe.Message + ".  Logging out other side");
                            Enclosing_Instance.transmitInternal(logoutMsg);
                        }
                        catch (FixException fe2)
                        {
                            log.Error("A problem occurred during the creation and sending of a logout message.");
                            log.Error("Message (from other side) that was rejected: " + msg);
                            log.Error("Our logout message: " + logoutMsg);
                            log.Error(fe2);
                        }
                    }

                    //Dont process message further.
                    return;
                }

                //Following cannot throw null pointer since message is already validated.
                //char type = msg.MsgType[0];
                String type = msg.MsgType;
                try
                {
                    //Process the different admin messages based on type.
                    switch (type)
                    {

                        case "A":  //Handle Logon message.
                            processLogon(msg);
                            break;


                        case "0":  //Handle heartbeat.
                            processHeartBeat(msg);
                            break;


                        case "1":  //Handle testRequest.
                            processTestRequest(msg);
                            break;


                        case "2":  //Handle ResendRequest.
                            processResendRequest(msg);
                            break;


                        case "3":  //Handle Reject.
                            processReject(msg);
                            break;


                        case "4":  //Handle SequenceReset
                            throw new System.ArgumentException("SequenceReset handled elsewhere");
                        // break;


                        case "5":  //Handle Logout.
                            processLogout(msg);
                            break;

                        case "BF": //pwreset ,  password reset
                            processPasswordReset(msg);
                            break;

                        case "AI": //Quote status Report
                            //Not handling the message ProcessQuoteStatus(msg);
                            break;

                        default:  // serious internal error -- this class only handles Admin messages
                            log.Debug("Correct this where you need to check"); 
                            throw new System.ArgumentException("FIXAdminThread can't handle message " + msg + " of type " + msg.MsgType);

                    }
                }
                catch (FixException fe)
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////
                    // @todo - reject?  what to do?
                    log.Error(fe);
                }
            }

            protected internal virtual void processLogout(Message msg)
            {
                log.Debug("Processing logout with sessionState " + Enclosing_Instance.sessionState);

                int sessionStateInt = Enclosing_Instance.sessionState.State;
                Enclosing_Instance.sessionState.LogoutReceived = msg;

                if (sessionStateInt != SessionState.LOGOUT_SENT)
                {
                    // we are the logout acceptor (other side has just sent a logout request which
                    // we must respond to)
                    // We should respond with a logout message
                    // According to FIX by default we should NOT disconnect immediately.  We should wait 10 seconds
                    // and then disconnect, but under normal circumstances the logout initiator will disconnect first.
                    String textMsg;
                    Enclosing_Instance.State = SessionState.LOGOUT_RECEIVED;
                    log.Info("FFillFixSession: Logout request received.");
                    textMsg = msg.getValue(Constants_Fields.TAGText);
                    if(string.IsNullOrEmpty(textMsg))
                    {
                        textMsg = "<Text message is Empty>";
                    }
                    Enclosing_Instance.stop(FIXNotice.FIX_LOGOUT_RECEIVED,textMsg);

                    //Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_LOGOUT_RECEIVED, "Received Logout", msg);

                    //try
                    //{
                    //    Message logoutMsg = Enclosing_Instance.buildLogoutMessage();
                    //    Enclosing_Instance.OptionalTags = logoutMsg;
                    //    //Send logout - after this method returns the session state will be set to
                    //    //LOGOUT_RESP_SENT
                    //    Enclosing_Instance.transmitInternal(logoutMsg);
                    //}
                    //catch (FixException fe)
                    //{
                    //    log.Error(fe);
                    //}
                }
                else
                {
                    // we are the logout initiator (other side is responding to confirm the logout)

                    Enclosing_Instance.State = SessionState.LOGOUT_RESP_RECEIVED;
                    Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_LOGOUT_RESP_RECEIVED, "Received Logout response.", msg);

                    //Means user just sent the logout request and this is the other party's response.
                    //Safe to disconnect now.
                    // setState(SessionState.SESSION_NOT_READY);
                    // logonSuccess = false;

                    log.Info("Received logout response, shutting down the session.");

                    // note: listeners get notified on a separate thread, fine to shut down session right away but
                    // listener-notification thread must not be shut down until it has sent the notification for
                    // the logout!
                    Enclosing_Instance.stop(FIXNotice.FIX_LOGOUT_RESP_RECEIVED, "Received Logout response.");
                }
            }

            protected internal virtual void ProcessQuoteStatus(Message msg)
            {
                //log.Debug("Quote Status message received ");
            }

            protected internal virtual void processPasswordReset(Message msg)
            {
                log.Debug("Processing password reset");

                Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_PASSWORD_RESET_RESP_RECEIVED, "Password reset response received", msg);
            }
            
            //Following processXXX functions are default handler for different admin messages.

            protected internal virtual void processLogon(Message msg)
            {
                if (!Enclosing_Instance.logonSuccess)
                {
                        log.Info("FFillFixSession: Logon response received");
                        Enclosing_Instance.timer.receivedLogonResp();
                        Enclosing_Instance.State = SessionState.LOGON_RESP_RECEIVED;
                        Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_LOGON_RESP_RECEIVED, "Logon response received", msg.Copy);
                        //We always do post processing of logon response.
                        processPostLogon(msg);
                }
                else
                {
                    if (!Enclosing_Instance.RejectDupLogon)
                    {
                        if ((Enclosing_Instance.listener != null) && Enclosing_Instance.listenerInterests[FIXNotice.FIX_DUP_LOGON])
                        {
                            FIXNotice logonNotice = new FIXNotice(this, msg, Enclosing_Instance, FIXNotice.FIX_DUP_LOGON, "Duplicate Logon");
                            Enclosing_Instance.listener.takeNote(logonNotice);
                        }
                        else
                            log.Error("FFillFixSession: No listeners registered to handle duplicate logon");
                    }
                    else
                    {
                        log.Error("FFillFixSession:Duplicate logon received. Disconnecting..");
                        Enclosing_Instance.stop(FIXNotice.FIX_DISCONNECTED, "Duplicate Logon");
                    }
                }
            }

            /// <summary> Default handler for HeartBeat messages.</summary>
            /// <param name="notice">FIXNotice with heartbeat message.
            /// </param>
            protected internal virtual void processHeartBeat(Message msg)
            {
                if (Enclosing_Instance.sessionState.State == SessionState.WAITING_FOR_MESSAGE_SYNCH)
                {
                    System.String tag = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i);
                    if (tag != null && !tag.Equals("") && !Dukascopy.com.scalper.fix.driver.ScalperFixSession.isPossDup(msg) && !com.scalper.fix.driver.ScalperFixSession.isPossResend(msg))
                    {
                        if (Enclosing_Instance.sessionState.thisSessionMustResend())
                        {
                            // can't happen
                            throw new System.SystemException();
                        }

                        Enclosing_Instance.readyToSendAppMessages();
                    }
                }
            }

            /// <summary> Default handler for TestRequest message.</summary>
            /// <param name="notice">FIXNotice with test request message.
            /// </param>
            protected internal virtual void processTestRequest(Message msg)
            {
                if (!Enclosing_Instance.AnswerTestRequests)
                {
                    if (Enclosing_Instance.testMode)
                    {
                        log.Warn("Deliberately not answering test request " + msg);
                        return;
                    }
                    else
                    {
                        // really, really, really shouldn't happen.
                        log.Warn("Configured to ignore test requests, but not in test mode.  Answering test request anyway.");
                        Enclosing_Instance.AnswerTestRequests = true;
                    }
                }

                try
                {
                    //We need to send heartbeat in response to test request by setting
                    //the TestRequestID to 'Y'
                    log.Info("FFillFixSession:Received TestRequest message.");
                    Message message;
                    System.String testReqID = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i);
                    if (testReqID != null)
                        message = Enclosing_Instance.buildHeartBeatMessage(testReqID);
                    else
                        message = Enclosing_Instance.buildHeartBeatMessage(true);

                    Enclosing_Instance.testRequestQueueTransmit(new SendCommand(enclosingInstance, message));
                    System.Threading.Thread.Sleep(0);
                }
                catch (FixException e)
                {
                    log.Error(e);
                }
            }

            /// <summary> Default handler for ResendRequest message.</summary>
            /// <param name="notice">FIXNotice object with resend request message.
            /// </param>
            protected internal virtual void processResendRequest(Message msg)
            {
                log.Debug("FFillFixSession:Received ResendRequest message.");
                // the first sequence number in the resend request
                int startSeqNo = msg.getIntValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginSeqNo_i);
                // the last sequence number in the resend request
                int endSeqNo = msg.getIntValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i);

                // the first sequence number that we will use a GapFill message to respond to
                Enclosing_Instance.checkStoredMsg(msg);

                if (endSeqNo == Enclosing_Instance.resendInfinity)
                {
                    //Special case where opposite counter-party expecting resend from
                    //startSeqNo to infinity, so send all the messages from startSeqNo through
                    //Last outbound message here and then continue with normal transmitting.

                    // do nothing
                }
                else if (endSeqNo == Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_40_41 || endSeqNo == com.scalper.fix.Constants_Fields.RESEND_INFINITY_42)
                {
                    // other side requested a resend using the wrong infinity.  If we don't treat as "infinity",
                    // we'll end up either sending nothing (if 0) or a gapfill up to 999,999 (if 999,999), both
                    // of which will probably cause the other side to get very confused.
                    log.Warn("Resend request received from " + startSeqNo + " to " + endSeqNo + ", treating as infinity." + "  (Wrong infinity for this FIX version");
                    endSeqNo = Enclosing_Instance.resendInfinity;
                }

                log.Info("Processing resend request from " + startSeqNo + " to " + endSeqNo);
                if (endSeqNo < startSeqNo && endSeqNo != Enclosing_Instance.resendInfinity)
                {
                    log.Warn("Received request to resend from " + startSeqNo + " to " + endSeqNo + ", no action taken.");
                    // @todo should send reject message
                    return;
                }
                else if (ScalperFixSession.isPossResend(msg))
                {
                    log.Warn("Received request to resend from " + startSeqNo + " to " + endSeqNo + ", poss resend, no action taken.");
                    // @todo should send reject message
                    return;
                }

                SendCommand cmd = new SendCommand(enclosingInstance, msg, SendCommand.RESEND, startSeqNo, endSeqNo);
                queueTransmit(cmd);
            }

            protected internal virtual bool logoutIfRejectResend(Message msg)
            {
                // check if this is in response to a resend
                System.Int32 refSeqNum = msg.getIntegerValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefSeqNum_i);
                //Commented and added by Ananda
//                if ((refSeqNum != null) && Enclosing_Instance.resendSeqs.Contains(refSeqNum))
                if ((refSeqNum != 0) && Enclosing_Instance.resendSeqs.Contains(refSeqNum))
                {
                    try
                    {
                        Message logoutMsg = Enclosing_Instance.buildLogoutMessage();
                        Enclosing_Instance.OptionalTags = logoutMsg;
                        //Send logout
                        Enclosing_Instance.transmitInternal(logoutMsg);
                    }
                    catch (FixException fe)
                    {
                        log.Error(fe);
                    }

                    // @todo should drop connection right away
                    Enclosing_Instance.stop(FIXNotice.FIX_DISCONNECTED, "logoutIfRejectResend");
                    return true;
                }
                return false;
            }
            /// <summary> Default handler for Reject message.</summary>
            /// <param name="notice">FIXNotice object with reject message.
            /// </param>
            protected internal virtual void processReject(Message msg)
            {
                //Spec defines action to just increase the inbound sequence number and continue processing.
                //I'm not incrementing sequence number here since its already done in reader thread!
                log.Info("FFillFixSession:Received Reject message");
                if (!logoutIfRejectResend(msg))
                    log.Info(" ignoring it");
            }

            /// <summary> This method used to process post 'logon' and before 'ready' state. Will be used
            /// to negotiate between 2 counter parties and get in sync before start exchanging
            /// new messages.
            /// </summary>
            protected internal virtual void processPostLogon(Message msg)
            {
                int sessionStateInt = Enclosing_Instance.sessionState.State;
                if ((sessionStateInt != SessionState.LOGON_RESP_SENT) && (sessionStateInt != SessionState.LOGON_RESP_RECEIVED))
                {
                    // can't happen
                    throw new System.SystemException();
                }

                // we have a successful login.
                Enclosing_Instance.State = SessionState.WAITING_FOR_MESSAGE_SYNCH;
                log.Info("FFillFixSession:Post logon work completed. Session is waiting for message sequence number synchronization.");

                // for client:
                // - case A, server is too high.  send resend request, server will reply.
                // - case B, server is correct.  Wait a short time and send a test request.
                Enclosing_Instance.timer.startWaitingForResend(500);

                //Set the logon status.
                Enclosing_Instance.logonSuccess = true;
                //Start the heartbeat, test request monitors for the acceptor side.
                // @todo maybe these things should only happen after MsgSeqNum synchronization?

                //start the timer
                Enclosing_Instance.timer.start();
                //Initialize heartbeat ticker
                Enclosing_Instance.startHeartbeatTimer();
                //Initialize test request ticker.
                Enclosing_Instance.timer.startTestRequestTimer();
            }

            private class ResendCounter
            {
                private void InitBlock(FIXAdminProcessor enclosingInstance)
                {
                    this.enclosingInstance = enclosingInstance;
                    resentMsgs = new int[ResendCounter.capacity];
                    
                    resentCount = new int[ResendCounter.capacity];
                    
                }
                private FIXAdminProcessor enclosingInstance;
                public FIXAdminProcessor Enclosing_Instance
                {
                    get
                    {
                        return enclosingInstance;
                    }

                }
                private const int capacity = 1000;

                //Following 2 arrays holds the resend message number along with their resent count.
                //Will be used to check how many times a message is been resent in a single session.
                //It will be a circular one so once 1000 slots are filled, it will start using from 0.
                private int[] resentMsgs;
                private int[] resentCount;

                //variable to hold the next slot/index of resending list.
                internal int nextResentSlot = 0;

                public ResendCounter(FIXAdminProcessor enclosingInstance)
                {
                    InitBlock(enclosingInstance);
                    // no-op
                }

                public virtual void incrementMessage(int msgSeqNo)
                {
                    if (msgSeqNo == -1)
                    {
                        log.Error("Trying to increment message with a sequence number of -1");
                        return;
                    }
                    int index = indexOf(msgSeqNo);
                    if (index == -1)
                    {
                        index = ++nextResentSlot;
                        if (index >= resentMsgs.Length)
                            resentMsgs = expandCapacity(resentMsgs);

                        resentMsgs[index] = msgSeqNo;
                    }

                    if (index >= resentCount.Length)
                        resentCount = expandCapacity(resentCount);
                    resentCount[index]++;
                }

                public virtual int getResendCount(int msgSeqNo)
                {
                    int index = indexOf(msgSeqNo);
                    return resentCount[index];
                }

                private int indexOf(int msgSeqNo)
                {
                    for (int i = 0; i < resentMsgs.Length; i++)
                    {
                        if (resentMsgs[i] == msgSeqNo)
                        {
                            return i;
                        }
                    }
                    return -1;
                }

                internal virtual int[] expandCapacity(int[] inArray)
                {
                    int newCapacity = inArray.Length * 2;
                    int[] newArray = new int[newCapacity];
                    Array.Copy(inArray, 0, newArray, 0, inArray.Length);
                    return newArray;
                }
            }
        }

        private void startHeartbeatTimer()
        {
            bool suppressHeartbeats = SuppressHeartbeats;
            if (!suppressHeartbeats)
            {
                timer.startHeartbeatTimer();
            }
            else
            {
                if (testMode)
                {
                    log.Warn("NOT starting heartbeat timer");
                }
                else
                {
                    log.Warn("Must be in test mode to suppress heartbeats, starting heartbeat timer anyway.");
                    timer.startHeartbeatTimer();
                }
            }
        }

        /* package */
        internal static System.String getString(sbyte[] data)
        {
            return ScalperFixSession.getString(data, 0, data.Length);
        }

        private static System.String getString(sbyte[] data, int off, int len)
        {
            System.Text.StringBuilder buf = new System.Text.StringBuilder();
            if (len == 0)
            {
                len = data.Length - off;
            }
            for (int i = off; i < len; i++)
            {
                buf.Append((char)data[i]);
                if (data[i] == 1)
                {
                    buf.Append(" ");
                }
            }
            return buf.ToString();
        }

        /// <summary> a hook so that subclasses can substitute their own subclass of Message.
        /// This hook is used for all received messages.  It is not used for sent messages.
        /// </summary>
        protected internal virtual Message createMessageTemplate()
        {
            return new Message();
        }

        /// <summary> <p>Description:Reader thread that polls the socket for any incoming data,
        /// frames the FIX message and if it is an admin message, stores it onto the fast
        /// processing admin queue else stores in the application message queue and then
        /// notifies the admin/delivery thread to process the message.</p>
        /// <p>Copyright: Copyright (c) 2005</p>
        /// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
        /// </summary>
        /// <author>  Phaniraj Raghavendra
        /// </author>
        /// <version>  1.0
        /// </version>
        private class FIXReaderThread : SupportClass.ThreadClass
        {
            private void InitBlock(ScalperFixSession enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private ScalperFixSession enclosingInstance;
            public ScalperFixSession Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            //Extracted framed message from working copy.
            internal sbyte[] framedMessage = null;

            //Message type of the current message
            internal char msgType = '\x0000';
            internal int delimiterlength = 1;
            internal sbyte[] delimiterBytes = new sbyte[] { (sbyte)Dukascopy.com.scalper.fix.FixConstants_Fields.delimiter };

            //should this thread continue to run and processing data?
            private volatile bool keepRunning;
            //How long to sleep for, used for testing purposes to enforce a certain sequence of messages
            private int sleepAmt;

            public FIXReaderThread(ScalperFixSession enclosingInstance, System.String name)
                : base(name)
            {
                InitBlock(enclosingInstance);
            }

            public virtual void requestStop()
            {
                keepRunning = false;
                Enclosing_Instance.adminProcessor.keepRunning = false;
                Enclosing_Instance.adminProcessor.requestStop();
                Enclosing_Instance.testRequestProcessor.keepRunning = false;
                Enclosing_Instance.testRequestProcessor.requestStop();
                try
                {
                    Enclosing_Instance.msgConn.close();
                }
                catch (System.IO.IOException ie)
                {
                    log.Error(ie);
                }
            }

            override public void 
                Run()
            {
                log.Info("Starting reader");
                Enclosing_Instance.adminProcessor.Start();
                Enclosing_Instance.testRequestProcessor.Start();
                keepRunning = true;
                if (Enclosing_Instance.isPerfOn_Renamed_Field)
                {
                    Enclosing_Instance.inMessageStore.expandForever();
                }
                try
                {
                    //Loop till session closes or if there is an attack.
                    while (keepRunning)
                    {
                        System.Object tmpMessage = Enclosing_Instance.msgConn.MessageFirstPass;
                        // Start READ instrumentation//
                        Message parsedMsg = Enclosing_Instance.createMessageTemplate();

                        if (Enclosing_Instance.fixVersion == null)
                        {
                            // for the client, the FIX version must already be set.
                            throw new System.SystemException();
                        }

                        bool messageParsed = false;
                        try
                        {
                            Enclosing_Instance.msgConn.getParsedMessage(parsedMsg, tmpMessage, Enclosing_Instance.tagHelper, Enclosing_Instance.StrictReceiveMode);
                            messageParsed = true;
                            //Do not print if message is market data or Quote status report response.
                            if (parsedMsg.MsgType != "W" && parsedMsg.MsgType != "AI" && parsedMsg.MsgType != "U2" && parsedMsg.MsgType != "U3")
                            {
                                Enclosing_Instance.afterReceiveMessage(parsedMsg);
                            }
               
                        }
                        catch (FixException e)
                        {
                            SupportClass.WriteStackTrace(e, Console.Error);
                            log.Error("Error in parsing the incoming message",e);
                            int sessionStateInt = Enclosing_Instance.sessionState.State;
                            bool rejectingInitialLogon = (sessionStateInt == SessionState.WAITING_FOR_LOGON || sessionStateInt == SessionState.LOGON_SENT);

                            // this logic (printing message and notifying listeners) is duplicated below
                            // if the message is really processed.
                            printMessageReceived(parsedMsg);
                          
                            // notify listeners that the message has been received
                            Enclosing_Instance.notifyMessageReceived(parsedMsg);

                            try
                            {
                                if (Enclosing_Instance.senderCompID != null && Enclosing_Instance.targetCompID != null)
                                {
                                    // @todo if msgSeqNum missing from message should send logout and drop connection
                                    Enclosing_Instance.buildAndTransmitReject(parsedMsg, e);
                                    if (!rejectingInitialLogon)
                                    {
                                        // increment inbound message sequence number
                                        Enclosing_Instance.complete(parsedMsg);
                                        // @todo shouldn't always increment nextExpectedSeqNo
                                        Enclosing_Instance.nextExpectedSeqNo++;
                                    }
                                    // log error
                                    log.Error("Session-level Reject : " + e.Message);
                                }
                                else
                                {
                                    // @todo fix
                                    log.Warn("Can't transmit reject message, SenderCompID or TargerCompID is null.");
                                }
                            }
                            catch (FixException e2)
                            {
                                log.Error(e2);
                                SupportClass.WriteStackTrace(e2, Console.Error);
                            }

                            if (rejectingInitialLogon)
                            {
                                System.String msgString;
                                switch (parsedMsg.MsgTypeChar)
                                {

                                    case 'A': msgString = "logon message"; break;

                                    case '5': msgString = "logout message"; break;

                                    default: msgString = "message"; break;

                                }
                                log.Fatal("Rejecting " + msgString + ", logging out.");
                                log.Fatal(msgString + " was " + parsedMsg.toString(false));
                                try
                                {
                                    Enclosing_Instance.transmitLogout(true, "Received a malformed " + msgString);
                                }
                                catch (FixException fe)
                                {
                                    log.Error("Error in transmitting the logout message.", fe);
                                    SupportClass.WriteStackTrace(fe, Console.Error);
                                }
                                keepRunning = false;
                            }
                            continue;
                        }

                        if (messageParsed)
                        {
                            try
                            {
                                    frameMessage(parsedMsg);
                                    if (Enclosing_Instance.isPerfOn_Renamed_Field)
                                    {
                                        Enclosing_Instance.inMsgCount++;
                                    }
                                    //HeartBeatCallback Heartbeat call back
                                    //if (parsedMsg.Summary.Contains("Heartbeat"))
                                    //{
                                    //    //Enclosing_Instance.sessionState.State = 4;
                                    //    //Enclosing_Instance
                                    //    Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.HEART_BEAT_RECEIVED, "Heartbeat message received");
                                    //    //Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_CONNECTION_FAILED, "Heartbeat message received");
                                    //}
                            }
                            catch (FixException fe)
                            {
                                // @todo
                                log.Error("Error occured in storing the incoming message in admin/application message store",fe);
                                SupportClass.WriteStackTrace(fe, Console.Error);
                            }
                        }
                    } // end of main "while" loop

                    // if we get here the thread will drop out of its main "while" loop and stop running soon.
                }
                catch (DenialOfServiceException dos)
                {
                    SupportClass.WriteStackTrace(dos, Console.Error);
                    sbyte[] workingCopyArr = dos.MsgRead;
                    log.Fatal("FFillFixSession: Denial of service attack!  " + dos.Message);
                    log.Fatal("Denial of service attack, shutting down the session.");
                    Enclosing_Instance.stopImmediately();

                    // the rest of this block is to print out a diagnostic error message to aid in recovery
                    ///////////////////////////

                    // this string may include the next message, no easy/fast way to parse it out, it's only for a log
                    // file...
                    System.String msgString = ScalperFixSession.getString(workingCopyArr);
                    log.Fatal("\nMessage was " + msgString + "\n");
                    msgString = msgString.Replace(" ", "");
                    //System.String patternString = "8=FIX\\.4\\..^9=([\\d]+)(^.*^)10=[\\d]{3}^";
                    System.String patternString = "8=FIX[.]4[.].^9=([\\d]+)(^.*^)10=[\\d]{3}^";
                    patternString = patternString.Replace('^', Dukascopy.com.scalper.fix.FixConstants_Fields.delimiter);

                    Regex FixMessagePattern = new Regex(patternString);
                    bool isMatch = FixMessagePattern.IsMatch(msgString);
                    
                    //String[] groups = FixMessagePattern.Split(msgString);
                    MatchCollection mc = FixMessagePattern.Matches(msgString);

                    /////////////////////////////
                    /*
                     Match MyMatch = mc[0];
                     version = MyMatch.Groups["minorVersion"].ToString();
                    */
                    // the code below doesno t work 
                    //replace it with the above code.
                    /////////////////////////////


                    if (isMatch)
                    {
                        int i = System.Int32.MaxValue;
                        GroupCollection gc = mc[0].Groups;
                        CaptureCollection cc = gc[0].Captures;
                        try
                        {
                            
                            i = Int32.Parse(cc[1].Value);
                        }
                        catch (System.FormatException nfe)
                        {
                            log.Error(" ", nfe);
                        }

                        int expectedLength = cc[2].Value.Length;
                        if (i != System.Int32.MaxValue && i != expectedLength)
                        {
                            log.Fatal("Body length was " + i + ", should have been " + expectedLength);
                        }
                    }
                    else
                    {
                        // message is horribly wrong, ignore.
                        // throw new IllegalStateException();
                    }
                }
                catch (System.IO.IOException e)
                {
                    log.Error(e);
                    SupportClass.WriteStackTrace(e, Console.Error);
                    //You will come here if the counter-party disconnects. send notice and close the session.
                    int sessionStateInt = Enclosing_Instance.sessionState.State;

                    if (String.Compare(e.Message, "Socket is closed") == 0)
                    {
                        //In case of socket close just go for reconnection 
                    }
                    else
                    {
                        switch (sessionStateInt)
                        {

                            case SessionState.SESSION_CLOSING:
                            case SessionState.SESSION_CLOSED:
                                log.Info("Normal logoff, we dropped connection.  Shutting down.");
                                break;

                            case SessionState.LOGOUT_RESP_SENT:
                                log.Info("Normal logoff, other side dropped connection.  Shutting down.");
                                break;

                            case SessionState.LOGOUT_RESP_RECEIVED:
                                // technically we should be the ones to close down the connection, not the other side.
                                // We've already exchanged logout messages, so all that will happen is that the next time
                                // we log in either us or them (or both) will have to retransmit some messages.
                                log.Warn("Received logon response but other side disconnected first.  Session will be shut down.");
                                break;

                            default:
                                SupportClass.WriteStackTrace(e, Console.Error);
                                log.Error(e);
                                log.Error("Counterparty is dead, session will shut down.");
                                Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_CONNECTION_FAILED, "Counterparty is dead, session will shut down.");
                                break;

                        }
                    }
                    Enclosing_Instance.stop(FIXNotice.FIX_DISCONNECTED, "Socket is closed");
                }
                catch (System.SystemException rt)
                {
                    SupportClass.WriteStackTrace(rt, Console.Error);
                    log.Error(rt);
                }

                //set the thread status to be 'dead'.
                keepRunning = false;
                log.Info("Exiting.. reader thread...");

                // the Reader thread could have beeen closed as a result of someone else calling stop(),
                // or the counterparty could have dropped connection and the Reader thread should *call* stop()
                // to stop everything else.  This potential infinite loop is avoided by looking at the
                // SessionStatus
                Enclosing_Instance.stop(FIXNotice.FIX_DISCONNECTED, "reader end");
            }

            private void printMessageReceived(Message msg)
            {
                    if ((Enclosing_Instance.PrintFixBefore || Enclosing_Instance.PrintFixAfter) && (true))
                    {
                        System.Text.StringBuilder buf = new System.Text.StringBuilder();
                        buf.Append("<==== FIX:Receive [ ");
                        if (Enclosing_Instance.id != null)
                        {
                            buf.Append(Enclosing_Instance.id);
                        }

                        buf.Append(",");
                        buf.Append(msg.MsgSeqNum);
                        buf.Append(",35=");
                        buf.Append(msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType_i));
                        buf.Append(" ] ");

                        if (Enclosing_Instance.PrintFixHumanReadable && false)
                        {
                            buf.Append(msg.toHumanReadableString());
                        }
                        else
                        {
                            buf.Append("[");
                            buf.Append(msg.toString(false));
                            buf.Append("]");
                        }

                        System.String logString = buf.ToString();

                        log.Fatal(logString);
                    }
            }

            /// <summary> extracts and stores the framed message bytes onto eigther admin or application
            /// message store based on the message type.
            /// </summary>
            /// <param name="data">Source array that contains framed data.
            /// </param>
            /// <param name="off	Starting">position in the source array.
            /// </param>
            /// <param name="len	The">number of array elements to be extracted.
            /// </param>
            protected internal virtual void frameMessage(Message parsedMsg)
            {

                //////////////////////////////////////////////////////////////////////////////////////////////////
                // @todo are rejects handled correctly?

                // note on the next expected message sequence number:
                // at every exit point from this method, the next expected sequence number must be correct.
                // ONLY this method should change the next expected sequence number.  Otherwise you will tear your
                // hair out.

                int sessionStateInt = Enclosing_Instance.sessionState.State;

                if (sessionStateInt == SessionState.SESSION_CLOSING || sessionStateInt == SessionState.SESSION_CLOSED)
                {
                    // do not process message, just ignore it.  At the next login we'll detect the sequence number
                    // gap and request a resend.
                    return;
                }

                bool isAdmin = BasicFixUtilities.isAdmin(parsedMsg);
                
                //Do not print if message is market data or Quote status report response.
                if (parsedMsg.MsgType != "W" && parsedMsg.MsgType != "AI" && parsedMsg.MsgType != "U2" && parsedMsg.MsgType != "U3")
                {
                    printMessageReceived(parsedMsg);
                }
                if (sessionStateInt == SessionState.WAITING_FOR_MESSAGE_SYNCH)
                {
                    Enclosing_Instance.timer.resetWaitingForResend();
                }

                if (sessionStateInt != SessionState.WAITING_FOR_LOGON && sessionStateInt != SessionState.LOGON_SENT && sessionStateInt != SessionState.WAITING_FOR_MESSAGE_SYNCH && sessionStateInt != SessionState.SESSION_READY && sessionStateInt != SessionState.LOGOUT_SENT && sessionStateInt != SessionState.LOGOUT_RESP_SENT)
                {
                    // can't happen
                    throw new System.SystemException("FIXReaderThread.frameMessage called from illegal state: " + sessionStateInt);
                }

                //Get the message type.
                char msgType = parsedMsg.MsgTypeChar;
                //Get the sequence number.
                int seqNo = parsedMsg.MsgSeqNum;
                // all the different cases:
                // application messages are never marked completed by the driver, only by the application.
                //
                // CASE 1: protocol message whose msgSeqNum is too low
                //
                // ANY message whose msgSeqNum is too low is a fatal error, no reason to complete it.
                // - exception: SeqReset_Reset messages have their sequence numbers ignored.
                //            - If the reset is valid (trying to increase the sequence number), the method
                //              gapFillComplete will be called which will complete all necessary messages.
                //            - If the reset is invalid the gapFillComplete method will not be called, reject
                //              will be sent, driver still expecting the same sequence number for the next message
                //
                // CASE 2: protocol message whose msgSeqNum is correct
                // call complete, done.
                // - exception: SeqReset_Reset messages have their sequence numbers ignored.
                //            - do NOT call complete for SeqReset_Reset messages, complete will be called if gapfill
                //              is valid (see exception to case 1)
                // - special case: if this is a SeqReset_GapFill message there may also be a gap to be filled
                //
                // CASE 3: protocol message whose msgSeqNum is too high
                // - case A, must be responded to anyway (logon, logoff, Test Request, resend request) but not SeqReset_Reset
                //            1. don't call complete here
                //            2. resend request gets issued
                //            3. as gap gets filled it's either the driver's or the app's responsibility to call
                //               complete, depending on whether it's an app or a protocol message.
                // - case B, should be processed in order (SeqReset_Gapfill, Heartbeat,  Reject)
                //            1. don't call complete here
                //            2. driver drops the message on the floor and issues a resend request.
                //            3. as gap gets filled it's either the driver's or the app's responsibility to call
                //               complete, depending on whether it's an app or a protocol message.
                // - case C, SeqReset_Reset
                //            - do NOT call complete for GapFill_Reset messages, complete will be called if gapfill
                //              is valid (see exception to case 1)
                if (isAdmin)
                {
                    if (seqNo == Enclosing_Instance.nextExpectedSeqNo && !ScalperFixSession.isSequenceReset_Reset(parsedMsg))
                        Enclosing_Instance.complete(parsedMsg);
                }

                // Reset the test request ticker. - valid message
                Enclosing_Instance.timer.resetTestRequestTimer();

                //////////////////////////////////////////////////////////////////////////////////
                ///
                // @todo if acceptor, first message must be logon, otherwise drop connection

                bool keepProcessing = true;
                // if sequence number is too low it might be an error condition
                if (seqNo < Enclosing_Instance.nextExpectedSeqNo)
                {
                    if (Enclosing_Instance.properties.AcceptLogonSeqNum && BasicFixUtilities.isLogon(parsedMsg))
                    {
                        Enclosing_Instance.nextExpectedSeqNo = seqNo;
                        Enclosing_Instance.uncompletedMessageStore.init(seqNo + 1);
                    }
                    else
                        keepProcessing = checkMsgSeqNumTooLow(parsedMsg, seqNo);
                }

                if (isAdmin)
                {
                    Enclosing_Instance.enqueueAdminMessage(parsedMsg);
                }

                // notify listeners that the message has been received
                Enclosing_Instance.notifyMessageReceived(parsedMsg);

                if (!keepProcessing)
                {
                    Enclosing_Instance.timer.resetWaitingForResend();
                    return;
                }
                // at this point the only way that the sequence number could be too low is if it's a
                // sequence reset-reset type message, where the sequence number is ignored.

                // if we're expecting a logon, only a logon is acceptable and anything else is a fatal error.

                if (sessionStateInt == SessionState.LOGON_SENT && msgType == '5')
                {
                    System.String text = parsedMsg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGText_i);
                    if (text == null || text.Equals(""))
                    {
                        text = "<no message available>";
                    }

                    //Enclosing_Instance.possiblyNotifyListeners(this, FIXNotice.FIX_LOGON_FAILED, "Server refused login attempt: " + text);

                    log.Error("Server refused login attempt: " + text);
                    Enclosing_Instance.nextExpectedSeqNo++;
                    // @todo should drop connection right away
                    if (text.Equals("Authorization failed"))
                    {
                        Enclosing_Instance.stop(FIXNotice.FIX_LOGON_FAILED, text);
                    }
                    else
                    {
                        Enclosing_Instance.stop(FIXNotice.FIX_LOGOUT_RECEIVED, text);
                    }
                    return;
                }

                if (sessionStateInt == SessionState.WAITING_FOR_LOGON || sessionStateInt == SessionState.LOGON_SENT)
                {
                    if (msgType != 'A')
                    {

                        System.String errorText = "First message not a logon.";
                        log.Error(errorText);

                        // client first sends logout message, then immediately disconnects
                        Message logoutMsg = Enclosing_Instance.buildLogoutMessage();
                        Enclosing_Instance.OptionalTags = logoutMsg;
                        logoutMsg.setValue(Constants_Fields.TAGText_i, errorText);
                        //Send logout
                        Enclosing_Instance.transmitInternal(logoutMsg);
                        Enclosing_Instance.nextExpectedSeqNo++;
                        // @todo should drop connection right away
                        Enclosing_Instance.stop(FIXNotice.FIX_DISCONNECTED, "First message not a logon.");
                        return;
                    }


                    // at this point it's a login message so we process it.
                    Enclosing_Instance.adminProcessor.processLogon(parsedMsg);

                    // if the sequence number is too high, we send a resend request *after* sending the login response.
                    if (seqNo > Enclosing_Instance.nextExpectedSeqNo)
                    {
                        Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                        if (resendRequest != null)
                        {
                            Enclosing_Instance.OptionalTags = resendRequest;
                            if (Enclosing_Instance.testMode)
                            {
                                System.String waitValue = parsedMsg.getValue("12345");
                                if (waitValue != null)
                                {
                                    try
                                    {
                                        System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64)10000 * System.Int32.Parse(waitValue)));
                                    }
                                    catch (System.Exception ex)
                                    {
                                        log.Error(" " + ex);
                                    }
                                }
                            }
                            Enclosing_Instance.queueTransmit(new SendCommand(enclosingInstance, resendRequest));
                        }
                    }
                    else
                    {
                        Enclosing_Instance.nextExpectedSeqNo++;
                    }

                    return;
                }

                if (sessionStateInt == SessionState.LOGOUT_SENT)
                {
                    // from the FIX spec:
                    // After sending the Logout message, the logout initiator should not send any messages unless requested to do so
                    // by the logout acceptor via a ResendRequest.

                    // for logout initiator: *ignore* if other side's message sequence numbers are too high -- we'll get them on the next
                    // login.


                    if (msgType == '5')
                    {
                        // If we get a logout, that is the answer to our logout response and we drop connection immediately.
                        Enclosing_Instance.adminProcessor.processLogout(parsedMsg);
                    }
                    else if (msgType == '2')
                    {
                        // if we get a resend request then answer it, but *don't* send out a resend request ourselves.
                        Enclosing_Instance.adminProcessor.processResendRequest(parsedMsg);
                    }
                    else
                    {
                        // could be an admin message:
                        // (login, heartbeat, test request, reject, SequenceReset-Reset, SequenceReset-GapFill)
                        // could be an application message

                        // in either case, ignore it and we'll ask for it to be resent as part of the next login process.
                    }
                    if (seqNo == Enclosing_Instance.nextExpectedSeqNo)
                    {
                        Enclosing_Instance.nextExpectedSeqNo++;
                    }
                    return;
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                // at this point session state can only be "ready/waiting for message synch";
                // "ready/really ready" or "logout response sent",
                // and they all handle
                // things identically.
                //
                // technically if we're in "waiting for message synch" mode other side shouldn't send
                // us application messages.  But we make no attempt to police that here.  However, if
                // this session wants to put an application message on the wire, including in response
                // to an application message sent by the other side, this session will force the message
                // to wait until message synchronization has been completed.
                //
                // @todo initiator really "shouldn't" send any new messages after sending the logout except in response
                // to a resend request, i.e. no admin messages except for logout or reject
                if (sessionStateInt != SessionState.WAITING_FOR_MESSAGE_SYNCH && sessionStateInt != SessionState.SESSION_READY && sessionStateInt != SessionState.LOGOUT_RESP_SENT)
                {
                    throw new System.ArgumentException();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                // @todo does the resend state (ours/theirs) affect anything?

                // Handle special admin message "SequenceReset-Reset" this need to be handled immediately
                // and msgSeqNum is ignored.
                // SequenceReset-GapFill messages need to have the correct MsgSeqNum, and a resend must be
                // generated if they do not, so they are handled later.
                if (ScalperFixSession.isSequenceReset_Reset(parsedMsg))
                {
                    processSequenceReset_Reset(parsedMsg);
                    return;
                }

                // at this point the message sequence number is either equal to or greater than the expected value.

                if (seqNo < Enclosing_Instance.nextExpectedSeqNo)
                {
                    // can't happen
                    throw new System.SystemException();
                }

                // case 1: message has the correct sequence number.
                // process it normally or stick it on the appropriate queue.

                if (seqNo == Enclosing_Instance.nextExpectedSeqNo)
                {
                    if (sessionStateInt == SessionState.WAITING_FOR_MESSAGE_SYNCH)
                    {
                        Enclosing_Instance.updateOtherSessionResendStatus(parsedMsg);
                    }

                    // SequenceReset-Gapfill messages must also be handled immediately
                    // because otherwise the next message received will appear to have the wrong
                    // sequence number
                    if (ScalperFixSession.isSequenceReset_GapFill(parsedMsg))
                    {
                        processSequenceReset_GapFill(parsedMsg);
                        // do NOT change sequence number here - the processSequenceReset_GapFill
                        // method is responsible for setting the next expected sequence number to the
                        // new value (if accepted) or just incrementing it (if rejected)
                        return;
                    }

                    // both kinds of SequenceReset messages (Reset and GapFill) have already been handled.
                    if (msgType == '4')
                    {
                        // can't happen
                        throw new System.SystemException();
                    }

                    // normal case - regular message that needs to be processed.

                    // verifySendingTime(msg);

                    if (BasicFixUtilities.isAdmin(parsedMsg))
                    {
                        // if we received a resend request, nothing can go out until we send our gap fill
                        if (msgType == '2')
                        {
                            // @todo block outgoing admin messages
                            // sessionState.setThisSessionMustResend(true);
                            log.Debug("Processing resend request with correct msgSeqNum.");
                        }
                        Enclosing_Instance.processAdminMsg(parsedMsg);
                    }
                    else
                    {
                        //If somebody is waiting to accept the incoming framed message, notify them.
                        Enclosing_Instance.enqueueAppMessage(parsedMsg);
                    }
                    Enclosing_Instance.nextExpectedSeqNo++;
                    return;
                }

                // at this point the sequence number is too high and a resend needs to be requested.
                // However, for certain messages the incoming message must be processed first, before the
                // resend request is sent out.

                // note: if the resend request has a sequence number that is too high, we first send
                // out our gap fill and then send a resend request of our own to make up the sequence gap.
                // from this point onwards, we do not increment the nextExpectedSeqNo, because the other side
                // has messages that it must resend.

                if (seqNo <= Enclosing_Instance.nextExpectedSeqNo)
                {
                    // can't happen
                    throw new System.SystemException();
                }

                // If it is a logout message then we are the logoff acceptor
                // (other side is requesting to log off.)
                // we first send a resend request and then send a logoff as confirmation to the other side.
                // note that the other side might have send the logoff message and then disconnected immediately
                // because of something bad that we did.
                if (msgType == '5')
                {
                    // other party is requesting logout - first request missing messages
                    log.Debug("Other party is requesting logout with a msgSeqNum that is too high.  Issuing resend request.");
                    Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                    if (resendRequest != null)
                    {
                        Enclosing_Instance.OptionalTags = resendRequest;
                        Enclosing_Instance.queueTransmit(new SendCommand(enclosingInstance, resendRequest));
                    }
                    Enclosing_Instance.processAdminMsg(parsedMsg);
                    // start 10-second timer to log out.
                    // don't reset timer if this is a new logout message
                    Enclosing_Instance.timer.receivedLogout();
                    return;
                }

                if (msgType == '2')
                {
                    // resend requests are a special case, must be answered first, then after the response
                    // we issue our own resend request
                    Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                    if (resendRequest != null)
                    {
                        Enclosing_Instance.OptionalTags = resendRequest;
                        Enclosing_Instance.transmitLater(resendRequest);
                    }

                    // resend request gets handled in the usual way, as if its sequence number were correct,
                    // and our resend request will go out afterwards.
                   Enclosing_Instance.processAdminMsg(parsedMsg);
                    return;
                }


                if (msgType == 'A')
                {

                    // For logon, if msgSeqNo is too high first answer with logon, then
                    // issue resend request for missed messages.
                    Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                    if (resendRequest != null)
                    {
                        Enclosing_Instance.OptionalTags = resendRequest;
                        Enclosing_Instance.transmitLater(resendRequest);
                    }

                    Enclosing_Instance.processAdminMsg(parsedMsg);
                }

                if (msgType == '0')
                {

                    // For logon, if msgSeqNo is too high first answer with logon, then
                    // issue resend request for missed messages.
                    Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                    if (resendRequest != null)
                    {
                        Enclosing_Instance.OptionalTags = resendRequest;
                        Enclosing_Instance.queueTransmit(new SendCommand(enclosingInstance, resendRequest));
                    }

                    Enclosing_Instance.processAdminMsg(parsedMsg);
                }

                if (msgType == '1')
                {
                    Message resendRequest = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                    if (resendRequest != null)
                    {
                        Enclosing_Instance.OptionalTags = resendRequest;
                        Enclosing_Instance.transmitLater(resendRequest);
                    }

                    Enclosing_Instance.processAdminMsg(parsedMsg);
                    return;
                }
                // except for the above special cases (logout, logon, and resend),
                // we don't process the message.  It will either be retransmitted as a gap fill
                // (in which case there is no action needed) or as the same message with the PossDup
                // flag set (in which case we will put it on the admin or the application queue for
                // the right thread to handle.)

                if (Enclosing_Instance.adminProcessor.logoutIfRejectResend(parsedMsg))
                    return;
                Message resendRequest2 = Enclosing_Instance.buildResendRequest_after(Enclosing_Instance.nextExpectedSeqNo, seqNo);
                if (resendRequest2 != null)
                {
                    Enclosing_Instance.OptionalTags = resendRequest2;
                    Enclosing_Instance.queueTransmit(new SendCommand(enclosingInstance, resendRequest2));
                }

                // at this point we've sent the resend request and are waiting for the gap fill to come in.
            }

            /// <summary>  called to check if a message's sequence number is too low, which may be an error condition.
            /// this method takes all necessary diagnostic action and returns true if the message should continue
            /// to be processed normally, or false if this method has already handled the message fully.
            /// </summary>
            protected internal virtual bool checkMsgSeqNumTooLow(Message msg, int seqNo)
            {
                //If the "PossDup" flag is not set, its an error.
                if (ScalperFixSession.isSequenceReset_Reset(msg))
                {
                    // sequence number is ignored, process message normally
                    // NOTE: it's NOT automatically safe to process the reset request.
                    // for example, the first message received must be a login, so if this
                    // is the first message, it's an error condition.
                    return true;
                }
                else if (ScalperFixSession.isPossDup(msg))
                {
                    // message with this sequence number was already received.
                    // not an error, but do no further processing on this message.
                    // (does not matter whether message is admin or application type)
                    return false;
                }
                else
                {
                    // if the message sequence number is lower than expected and not possDup,
                    // it is a fatal error.

                    Enclosing_Instance.transmitMsgSeqNumTooLow(msg);
                    return false;
                }
            }

            public virtual void sleepFor(int sleepAmt)
            {
                this.sleepAmt = sleepAmt;
            }

            protected internal virtual void maybeSleep()
            {
                if (sleepAmt > 0)
                {
                    int tempSleepAmt = sleepAmt;
                    sleepAmt = 0;
                    if (Enclosing_Instance.testMode)
                    {
                        log.Info("Reader thread sleeping for " + sleepAmt + " ms.");
                        try
                        {
                            System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64)10000 * tempSleepAmt));
                        }
                        catch (System.Threading.ThreadInterruptedException ie)
                        {
                            // could be interrupted by shutdown sequence, ignore here, will be picked up later
                            // and message will not be processed.
                            log.Error(" ", ie);
                        }
                    }
                    else
                    {
                        log.Warn("NOT sleeping even though sleepAmt set, not in test mode.");
                    }
                }
            }

            private void gapFillComplete(int startMsgSeqNum, int newMsgSeqNum)
            {
                // For SeqReset_Reset messages, complete has NOT been called on the current message sequence number
                // so need to call complete from current message sequence number (inclusive) to end range (exclusive).

                // For SeqReset_Gapfill messages, complete HAS been called on the current message sequence number
                // so need to call complete from current message sequence number (exclusive) to end range (exclusive).

                // in other words it might or might not be an error to call complete() on the first message.
                // as a temporary measure, call complete anyway and catch the sanity check exception.

                // other crazy case: other side's logon message/ack is ONE HIGHER than expected.  Resend request goes
                // out, gap fill comes back, gap fill message is now complete (N) and logon is complete (N+1).
                // I'm tired of dealing with special cases, so just mark messages complete until you hit the first error.

                if (startMsgSeqNum >= newMsgSeqNum)
                {
                    throw new System.ArgumentException("Trying to decrease sequence numbers with gap fill " + startMsgSeqNum + " " + newMsgSeqNum);
                }

                // as an optimization, run backwards (high to low) so that the persister will only
                // be called once.
                try
                {

                    for (int i = newMsgSeqNum - 1; i >= startMsgSeqNum; i--)
                    {
                        try
                        {
                            Enclosing_Instance.complete(i, false);
                        }
                        catch (FixException fe)
                        {
                            log.Error(fe);
                            SupportClass.WriteStackTrace(fe, Console.Error);
                        }
                    }
                }
                catch (System.ArgumentException iae)
                {
                    // not an error (Well, might not be an error)
                    // Ignore and drop out of loop (no reason to complete the rest, they've already been completed.)
                    log.Error(" ", iae);
                }

                if (Enclosing_Instance.sessionState.State == SessionState.WAITING_FOR_MESSAGE_SYNCH)
                {
                    Enclosing_Instance.updateOtherSessionResendStatus(newMsgSeqNum - 1, '4');
                }
            }

            /// <summary>processes a sequence reset message of type gap fill
            /// (GapFill flag set to 'Y')
            /// 
            /// This method should only be called if the message's msgSeqNum is equal to the one that we are
            /// expecting.  All other logic is in frameMessage.
            /// After this method completes, the msgSeqNum will be equal to the value in the message (if
            /// the message is accepted) or to one more than the previous value of msgSeqNum (if the message was trying
            /// to lower the sequence number.)
            /// </summary>
            protected internal virtual void processSequenceReset_GapFill(Message msg)
            {
                int msgSeqNum = msg.MsgSeqNum;
                int newSeqNo = msg.getIntValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i);

                log.Info("FFillFixSession: Processing SequenceReset-GapFill, msgSeqNum=" + msgSeqNum + ", newSeqNo=" + newSeqNo);

                if (msgSeqNum != Enclosing_Instance.nextExpectedSeqNo)
                {
                    // can't happen
                    throw new System.ArgumentException();
                }

                if (newSeqNo > msgSeqNum)
                {
                    // normal case - increase sequence number and then we're done.
                    gapFillComplete(msgSeqNum, newSeqNo);
                    Enclosing_Instance.nextExpectedSeqNo = newSeqNo;
                    return;
                }

                //attempt to lower the sequence number, send Reject message and increment sequence number.
                log.Error("Rejecting " + msg.toString(false));
                FixException fe = new FixException("Attempt to lower sequence number, invalid value NewSeqNum=<" + newSeqNo + ">", FixException.REJECT_REASON_OTHER);
                Enclosing_Instance.buildAndTransmitReject(msg, fe);
                Enclosing_Instance.nextExpectedSeqNo++;
            }

            /// <summary>processes a sequence reset message of type reset
            /// (GapFill flag not present or set to 'N')
            /// </summary>
            protected internal virtual void processSequenceReset_Reset(Message msg)
            {

                int newSeqNo = msg.getIntValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i);

                log.Info("FFillFixSession: Received SequenceReset - Reset.");
                if (newSeqNo > Enclosing_Instance.nextExpectedSeqNo)
                {
                    //Normal case, just set next expected sequence number.
                    gapFillComplete(Enclosing_Instance.nextExpectedSeqNo, newSeqNo);
                    Enclosing_Instance.nextExpectedSeqNo = newSeqNo;
                }
                else if (newSeqNo == Enclosing_Instance.nextExpectedSeqNo)
                {
                    log.Warn("Sequence reset received with NewSeqNo equal to Current expected sequence number.");
                    // do *not* increment nextExpectedSeqNo -- it is already equal to newSeqNo.
                    // the reset message itself doesn't count in terms of incrementing the next expected sequence number.
                    // nextExpectedSeqNo++;
                }
                else
                {
                    // other side tried to lower the sequence number, which is illegal.
                    // send a reject message.
                    FixException fe = new FixException("Value is incorrect (out of range) for this tag", 5);
                    Enclosing_Instance.buildAndTransmitReject(msg, fe);
                }
            }
        }
        // End of reader thread.
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fix Session, Reader Thread-related

        public virtual void readerThreadSleep(int sleepAmt)
        {
            reader.sleepFor(sleepAmt);
        }


        /// <summary> This method is in place to check the validity of the message that is being retreived
        /// from the store.For now it just sends the notice to the user when there is a version
        /// mismatch. Different other checks and validitation will be added in phase 2.
        /// </summary>
        /// <param name="msg">Message that need to ve validated.
        /// </param>
        public virtual void checkStoredMsg(Message msg)
        {
            if (testMode)
            {
                return;
            }

            char mVersion = msg.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginString_i)[6];
            if (minorVersion != mVersion)
            {
                //Version mismatch in the middle of the session!!
                possiblyNotifyListeners(this, FIXNotice.FIX_VERSION_MISMATCH, "Version mismatch");
                log.Error("FFillFixSession:Version mismatch in the middle of the session.");
            }
        }

        // getters/setters
        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary> method that does all required housekeeping after a logout message has been transmitted.
        /// This includes updating listeners and setting the session's state to the correct value depending
        /// on whether we are the logout initiator or the logout acceptor.
        /// 
        /// </summary>
        /// <param name="message	FIX">Logout message.
        /// </param>
        /// <throws>  FixException </throws>
        /// <throws>  IllegalArgumentException the message is not a logout message </throws>
        protected internal virtual void logoutTransmitted(Message message)
        {
            if (!message.MsgType.Equals("5"))
            {
                throw new System.ArgumentException("Cannot log out using a non-logout message");
            }

            sessionState.LogoutSent = message;
            if (sessionState.State != SessionState.LOGOUT_RECEIVED)
            {
                // we are the logout initiator, other side should respond with a logout message to confirm
                State = SessionState.LOGOUT_SENT;
                log.Info("FFillFixSession:Logout sent");
                //Start the timer that waits for 10 seconds before disconnecting.
                timer.sentLogout();
            }
            else
            {
                // we are the logout acceptor, other side has sent a logout request which we have just confirmed

                State = SessionState.LOGOUT_RESP_SENT;
                log.Info("FFillFixSession:Logout response sent");
            }
        }

        /// <summary> Special transmit method supposed to be used by user when he decides to send a resend
        /// request while servicing an admin (login, logout, resend, test request)
        /// get serviced by admin thread after all send and service resend commands are processed
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <throws>  FixException </throws>
        //("unchecked")
        public virtual void transmitLater(Message msg)
        {
            queuedMsgFromResend.Add(msg);
        }

        protected internal virtual void processAdminMsg(Message msg)
        {
            adminProcessor.processMessage(msg);
        }
        protected internal virtual void enqueueAdminMessage(Message msg)
        {
            if (msg.MsgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGReject))
            {
                Message newMsg = properties.CopyMessagesToListeners ? msg.Copy : msg;
                //synchronized(inMessageStoreLock)
                //{
                //inMessageStore.put(newMsg);
            }
        }
        /// <summary>enqueues an application-level (non-admin) message and notifies listeners that the message is available </summary>
        protected internal virtual void enqueueAppMessage(Message msg)
        {
            if (sessionState.State == SessionState.WAITING_FOR_MESSAGE_SYNCH)
            {
                int msgSeqNum = msg.MsgSeqNum;
                updateOtherSessionResendStatus(msgSeqNum, 'D');
            }

            // make a copy before putting it on the queue if that's what the user requested.
            Message newMsg = properties.CopyMessagesToListeners ? msg.Copy : msg;
            //inMessageStore.put(newMsg);
        }

        protected internal virtual void updateThreadNames(System.String name)
        {
            if (reader != null)
            {
                reader.Name = "Reader thread [" + name + "]";
            }
        }

        private void updateOtherSessionResendStatus(Message msg)
        {
            updateOtherSessionResendStatus(msg.MsgSeqNum, msg.MsgTypeChar);
        }

        private void updateOtherSessionResendStatus(int msgSeqNum, char msgType)
        {
            int sessionStateInt = sessionState.State;
            if (sessionStateInt != SessionState.WAITING_FOR_MESSAGE_SYNCH)
            {
                // there's a race condition where this could legitimately happen (logout message sent or receieved
                // at exactly the right time) but it's not worth thinking too hard about.
                log.Warn("updateOtherSessionResendStatus called with wrong state " + sessionStateInt);
                return;
            }

            if (msgSeqNum >= lastExpectedFromResend)
            {
                if (sessionState.otherSessionMustResend())
                {
                    lastExpectedFromResend = -1;
                    sessionState.clearOtherSessionMustResend();
                }

                // for these message (if correct msgSeqNum) the session is established:
                // - any application message
                // - reject
                if (!ScalperFixSession.isAdmin(msgType) || (msgType == '3'))
                {
                    readyToSendAppMessages();
                }
                else
                {
                    // can't look at this session's resend state right now and declare the session "ready",
                    // might still receieve a resend request for cases where both sessions are too high.
                    timer.resetWaitingForResend();
                }
            }
        }

        private static bool isAdmin(char msgType)
        {
            return ((msgType >= '0' && msgType <= '5') || msgType == 'A');
        }


        /// <summary> Called internally when a message from the other side has a MsgSeqNum that is too low.
        /// transmits a logout message and shuts down the session.
        /// </summary>
        private void transmitMsgSeqNumTooLow(Message msg)
        {
            int seqNo = msg.MsgSeqNum;
            try
            {
                Message logoutMsg = buildLogoutMessage();
                OptionalTags = logoutMsg;

                logoutMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGText_i, "MsgSeqNum too low, expecting " + nextExpectedSeqNo + " but received " + seqNo);

                log.Fatal("FFillFixSession: Received a message with sequence number lower than expected without 'PossDup' flag set.  " + "Expected " + nextExpectedSeqNo + " but received " + seqNo);

                //Send logout
                transmitInternal(logoutMsg);
            }
            catch (FixException fe)
            {
                log.Error("Problem creating or transmitting logout message.");
                log.Error(fe);
            }
            this.stop(FIXNotice.FIX_DISCONNECTED, "MsgSeqNum too low");
        }

        /// <summary>convenience method for when the driver is resending a message.
        /// This sets the OrigSendingTime flag correctly and sends it immediately (without being put on a queue)
        /// </summary>
        private void transmitResent(Message message)
        {
            if (!ScalperFixSession.isAdmin(message.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType)))
                message.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag_i, "Y");

            // must recalculate and body length checksum even if in test mode.
            // They will automatically get recalculated and readded
            // at the end of the transmit method.
            message.removeTag(Dukascopy.com.scalper.fix.Constants_Fields.TAGBodyLength_i);
            message.removeTag(Dukascopy.com.scalper.fix.Constants_Fields.TAGCheckSum_i);
            transmitInternal(message, true, false, true);
        }

        /// <summary>convenience method for when the driver is rejecting a message.
        /// transmits the message and notifies listeners that the message was rejected.
        /// </summary>
        private void buildAndTransmitReject(Message origMsg, FixException fe)
        {
            Message rejectMsg = null;
            try
            {
                rejectMsg = buildRejectMessage(origMsg, fe);
                queueTransmit(new SendCommand(this, rejectMsg, origMsg));
            }
            catch (FixException fe2)
            {
                log.Error("A problem occurred during the creation and sending of a reject message.");
                log.Error("Message (from other side) that was rejected: " + origMsg);
                log.Error("Our reject message: " + rejectMsg);
                log.Error(fe2);
            }
        }

        /// <summary> Sends an outbound message.
        /// This method assigns the next sequence number to the message and blocks until
        /// it has been sent over the communication link.  The message will never be put
        /// on a queue, but the message will not be put onto the wire until any pending resend
        /// request has been answered, which could take a long time if there are many messages
        /// to send.  (If the other side requests that messages be resent, this side cannot send
        /// any new messages until all relevant messages have been resent.  If this side requests
        /// that messages be resent there is no impact on outgoing messages.)
        /// 
        /// Implementation note:  Even internal parts of the driver (i.e. the thread that sends out
        /// heartbeats at regular intervals) must
        /// wait if there is a pending resend request, so they use this method rather than the private methods
        /// that send messages out immediately.
        /// 
        /// </summary>
        /// <param name="message">Message that need to be transmitted.
        /// </param>
        /// <throws>  FixException </throws>
        public virtual void transmit(Message message)
        {
            if (!testMode && !persistAlways && sessionState.State == SessionState.WAITING_FOR_LOGON && !"A".Equals(message.MsgType))
            {
                // this error applies to either client or server
                throw new FixException("First message must be a logon.");
            }
            transmitInternal(message, false, true, false);
        }

        public virtual void transmit(System.Object[] msgs, int offset, int count)
        {
            transmitInternal(msgs, offset, count, false, true, false);
        }

        /// <summary> initiates a logout exchange.  The logout message has an optional field that represents human-readable error
        /// text which is filled in with the given parameter.
        /// If this is a fatal error, the logout message will be put on the wire right away, and the session will be
        /// disconnected immediately.  Note that if you want to disconnect without sending a logout message you should
        /// call stop() or stopImmediately() instead.
        /// If this is not a fatal error, this session will wait for the other side to respond before disconnecting.
        /// (After 10 seconds this session will automatically disconnect if no response is received.)
        /// </summary>
        public virtual void transmitLogout(bool fatalError, System.String errorText)
        {
            Message logoutMsg = buildLogoutMessage();
            if (senderCompID != null && targetCompID != null)
                OptionalTags = logoutMsg;
            if (errorText != null && !"".Equals(errorText))
            {
                logoutMsg.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGText_i, errorText);
            }
            //Send logout - after this method returns the session state will be set to LOGOUT_SENT
            if (fatalError)
            {
                if (senderCompID != null && targetCompID != null)
                {
                    try
                    {
                        transmitInternal(logoutMsg);
                    }
                    catch (FixException fe)
                    {
                        log.Warn("Couldn't send logout message.", fe);
                    }
                }
                else
                {
                    log.Warn("Can't send logout message, don't know other sender or target CompID.");
                }

                if (BasicUtilities.isNullOrEmpty(errorText))
                    errorText = "<no reason specified.>";
                log.Fatal("Dropping connection: " + errorText);
                stopImmediately();
            }
            else
            {
                transmit(logoutMsg);
            }
        }

        private void testRequestQueueTransmit(SendCommand cmd)
        {
            testRequestProcessor.queueTransmit(cmd);
        }
        private void queueTransmit(SendCommand cmd)
        {
            adminProcessor.queueTransmit(cmd);
        }
        /// <summary> Method for the driver to send a message.  The message will go on the wire right away regardless
        /// of any resend request that may be in progress.  Note that some messages that are generated from the
        /// driver (e.g. heartbeats, test requests) should still be delayed by a resend request in progress.
        /// </summary>
        private void transmitInternal(Message message)
        {
            transmitInternal(message, false, false, true);
        }

        /// <summary> a simple wrapper  that makes sure things only get sent when the resend state is OK.</summary>
        private void transmitInternal(Message message, bool setOrigSendingTime, bool checkIfResending, bool messageIsFromDriver)
        {

            if (checkIfResending)
            {
                
                _transmitInternal(message, setOrigSendingTime, checkIfResending, messageIsFromDriver);
                
            }
            else
            {
                _transmitInternal(message, setOrigSendingTime, checkIfResending, messageIsFromDriver);
            }
        }


        private void transmitInternal(System.Object[] msgs, int offset, int count, bool setOrigSendingTime, bool checkIfResending, bool messageIsFromDriver)
        {

            if (checkIfResending)
            {
                
                _transmitInternal(msgs, offset, count, setOrigSendingTime, checkIfResending, messageIsFromDriver);
                
            }
            else
            {
                _transmitInternal(msgs, offset, count, setOrigSendingTime, checkIfResending, messageIsFromDriver);
            }
        }

        /// <summary> actually transmits the message.  If the driver is currently responding to a resend request, only
        /// SequenceReset-GapFill messages or application messages with possDup flag set and msgSequence number
        /// < the next outbound msgSeqNum can be sent.  (This requirement is ignored if the driver is in test mode.)
        /// 
        /// This method assigns the next sequence number to the message and places
        /// it on the queue(message store) and sends it across communication link.
        /// </summary>
        /// <param name="message">Message that need to be transmitted.
        /// </param>
        /// <param name="setOrigSendingTime">indicates that OrigSendingTime needs to be set on the message based
        /// on the old value
        /// </param>
        /// <param name="checkIfResending">if true, this thread may block until a resend that is in progress is completed.
        /// If false, this message was generated from the driver itself and is either in response to a resend request
        /// or is safe to exclude from checks for other reasons.
        /// </param>
        /// <throws>  FixException </throws>

        ////////////////////////////////////////////////
        // NEVER, EVER, EVER call this method directly!!!
        // use transmitInternal(3 booleans) or one of the other ones instead.
        /////////////////////////////////////////////////
        //("unchecked")
        private void _transmitInternal(Message message, bool setOrigSendingTime, bool checkIfResending, bool messageIsFromDriver)
        {
            if (message == null)
            {
                log.Error("Cannot transmit null message.");
                throw new System.ArgumentException("Cannot transmit null message.");
            }

            preprocessBeforeSend1(message, setOrigSendingTime, messageIsFromDriver);

            // grab the transmit lock as late as possible -- once we get it
            // we will be able to put messages on the wire immediately.
            lock (transmitLock)
            {
                bool persistFlag = preprocessBeforeSend2(message, setOrigSendingTime, messageIsFromDriver);
                // persist the message as long as it has the last sequence number (not a resend)
                // @todo rejects are allowed to be resent
                // @todo should check be whether isPossDup?
                if (BasicFixUtilities.isAdmin(message) && BasicFixUtilities.isResend(message) && !ScalperFixSession.isPossResend(message))
                {
                    resendSeqs.Add((System.Int32)message.MsgSeqNum);
                }
                if (!setOrigSendingTime)
                {
                    if (!(message.MsgSeqNum < outMsgSeqNo || sessionState.thisSessionMustResend()) && persistFlag)
                    {
                        try
                        {
                            if (BasicFixUtilities.isAdmin(message))
                            {
                                persister.storeOutboundAdminMessage(message);
                            }
                            else
                            {
                                persister.storeOutboundAppMessage(message);
                            }
                        }
                        catch (FixException fe)
                        {
                            possiblyNotifyListeners(this, FIXNotice.FIX_COULDNT_PERSIST_OUTBOUND_MESSAGE, "Couldn't persist message " + message);
                            throw fe;
                        }
                    }
                    else if ((message.MsgType == null) || !message.MsgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGSequenceReset))
                    {
                        if (testMode)
                        {
                            // not an error, don't persist, just send.
                            // other side should kick us off.
                        }
                        else
                        {
                            // this case should have been caught earlier in this method, this is a sanity
                            // check that should never happen.
                            throw new System.ArgumentException("Attempt to transmit message with MsgSeqNum < expectedSeqNum and possDup flag not set.");
                        }
                    }
                }

                postprocessBeforeSend1(message);
            }

            postprocessBeforeSend2(message);
        }

        private void _transmitInternal(System.Object[] msgs, int offset, int count, bool setOrigSendingTime, bool checkIfResending, bool messageIsFromDriver)
        {


            // new Exception("transmitmessage " + message.toString(false)).printStackTrace();
            if (msgs == null || msgs.Length == 0)
            {
                log.Error("Cannot transmit null message.");
                throw new System.ArgumentException("Cannot transmit null message.");
            }

            for (int i = offset; (i < msgs.Length) && (i - offset < count); i++)
            {
                Message message = (Message)msgs[i];
                preprocessBeforeSend1(message, setOrigSendingTime, messageIsFromDriver);
            }

            // grab the transmit lock as late as possible -- once we get it
            // we will be able to put messages on the wire immediately.
            lock (transmitLock)
            {
                for (int i = offset; (i < msgs.Length) && (i - offset < count); i++)
                {
                    Message message = (Message)msgs[i];
                    preprocessBeforeSend2(message, setOrigSendingTime, messageIsFromDriver);
                }

                // persist the message as long as it has the last sequence number (not a resend)
                // @todo rejects are allowed to be resent
                // @todo should check be whether isPossDup?
                if (!setOrigSendingTime)
                {
                    try
                    {
                        persister.storeOutboundMessages(msgs, offset, count);
                    }
                    catch (FixException fe)
                    {
                        possiblyNotifyListeners(this, FIXNotice.FIX_COULDNT_PERSIST_OUTBOUND_MESSAGE, "Couldn't persist message group");
                        throw fe;
                    }
                }

                int j = 0;
                Message[] messages = new Message[count];
                for (int i = offset; (i < msgs.Length) && (i - offset < count); i++)
                {
                    messages[j++] = (Message)msgs[i];
                }
                if (j > 0)
                    postprocessBeforeSend1(messages); //New method to send batched messages.
            }

            for (int i = offset; (i < msgs.Length) && (i - offset < count); i++)
            {
                Message message = (Message)msgs[i];
                postprocessBeforeSend2(message);
            }
        }

        private void preprocessBeforeSend1(Message message, bool setOrigSendingTime, bool messageIsFromDriver)
        {
            // Start instrumentation - PRESEND1
            bool debugEnabled = true;

            if (debugEnabled)
            {
                log.Debug("begin transmit " + message);
            }

            if (PrintFixBefore)
            {
                bool infoEnabled = true;
                System.Text.StringBuilder buf = new System.Text.StringBuilder();
                buf.Append(">==== FIX:Send [ ");
                if (id != null)
                {
                    buf.Append(id);
                }

                buf.Append(",");
                buf.Append(message.MsgSeqNum);
                buf.Append(",35=");
                buf.Append(message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType_i));
                buf.Append(" ]");

                if (PrintFixHumanReadable)
                {
                    if (infoEnabled)
                        log.Fatal(buf.Append(message.toHumanReadableString()).ToString());
                }
                else
                {
                    if (infoEnabled)
                        log.Fatal(buf.Append(message.toString(false)).ToString());
                }
            }

            char msgType = message.MsgTypeChar;
            if (msgType == '\x0000')
            {
                if (testMode)
                {
                    log.Warn("FFillFixSession.transmit(): Deliberately sending message with message type NULL.");
                }
                else
                {
                    log.Error("FFillFixSession.transmit(): FIX message format error, message type is NULL.");
                    throw new FixException("FIX message format error, message type is NULL.");
                }
            }

            //Set the beginString, sender compID and targetID if user hasn't opt for setting it by himself.
            if (testMode)
            {
                message.setIfNull(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginString_i, BeginFixVersion);
            }
            else
            {
                message.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginString_i, BeginFixVersion);
            }

            OptionalTags = message;

            int sessionStateInt = sessionState.State;
            if (msgType == '5')
            {
                // do nothing - always legal to send a logoff message, because other side might
                // have done something wrong and we need to log them off.
            }
            else if (sessionStateInt == SessionState.LOGON_RESP_RECEIVED && msgType == '2')
            {
                // do nothing (?) - not illegal
            }
            else if (sessionStateInt != SessionState.WAITING_FOR_MESSAGE_SYNCH && sessionStateInt != SessionState.SESSION_READY && sessionStateInt != SessionState.LOGOUT_RECEIVED && sessionStateInt != SessionState.LOGON_RESP_SENT)
            {
                // sanity check: the first message sent from this session must be a logon.
                if (msgType != 'A' && !testMode)
                {
                    // in test mode can send whatever you want.

                    // exception to the above: if server's logon ack is malformed, send (optional) reject
                    // followed by logout.
                    if (msgType == '3' && messageIsFromDriver && (sessionStateInt == SessionState.LOGON_SENT))
                    {
                        // ok to send reject, do nothing
                    }
                    else if (sessionStateInt == SessionState.LOGOUT_SENT && messageIsFromDriver && ((msgType == '4') || (message.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag_i) == 'Y')))
                    {
                        // ok, we're just responding to a resend request
                    }
                    else if (!persistAlways)
                    {
                        if (sessionClosed)
                        {
                            log.Error(new FixException("Session closed"));
                        }

                        //log.Warn("Tried to send " + message + " before logon response received, state: " + sessionState.getState());
                        throw new FixException("Cannot send message [" + message + "] before logon response has been received");
                    }
                }
                else if (msgType == 'A')
                {
                    //You come here only for logon message!!!
                    System.String fieldName = null;
                    bool missing = false;
                    //Extract and store the basic header information from initiatl logon.
                    System.String beginString = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginString_i);
                    if (beginString == null)
                    {
                        missing = true;
                        fieldName = "beginString";
                    }
                    if (!missing)
                    {
                        SenderCompID = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i);
                        if (senderCompID == null)
                        {
                            missing = true;
                            fieldName = "senderCompID";
                        }
                    }
                    if (!missing)
                    {
                        TargetCompID = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i);
                        if (targetCompID == null)
                        {
                            missing = true;
                            fieldName = "targetCompID";
                        }
                    }
                    if (!missing)
                    {
                        int hbInt = message.getIntValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGHeartBtInt_i);
                        if (hbInt == -1)
                        {
                            missing = true;
                            fieldName = "heartBeatInt";
                        }
                        else
                        {
                            hbInt *= 1000;
                            timer.HBeatInterval = hbInt;
                            // spec says to use 20% extra as a ballpark figure
                            timer.TestRequestInterval = hbInt + hbInt / 5;
                        }
                    }
                    if (missing)
                    {
                        log.Error("FFillFixSession.transmit(): Required field [" + fieldName + "]  missing in logon message");
                        FIXNotice fieldMissing = new FIXNotice(this, message, this, FIXNotice.FIX_FIELD_MISSING, fieldName + " is null!");
                        if ((listener != null) && listenerInterests[FIXNotice.FIX_FIELD_MISSING])
                        {
                            listener.takeNote(fieldMissing);
                        }
                        else
                            log.Error("No listener registered to handle event 'Mandatory field missing'");
                        throw new FixMessageFormatException("Required field [" + fieldName + "]  missing in logon message");
                        // return;
                    }
                    else
                    {
                        State = SessionState.LOGON_SENT;
                        message.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGHeartBtInt_i, timer.HBeatInterval / 1000);
                        log.Info("FFillFixSession:Logon sent");
                        //Start the heartbeat, test request monitors for the initiator side.
                        timer.start();
                        startHeartbeatTimer();
                        timer.startTestRequestTimer();
                        timer.sentLogon();
                    }
                }
            }

            // unfortunately these sanity checks have to go inside the "synchronized" block:
            if (sessionState.thisSessionMustResend() && message.MsgTypeChar != '3' && message.MsgTypeChar != '5')
            {
                if (!ScalperFixSession.isPossDup(message))
                {
                    log.Error("Responding to a resend request with a message that does not have possDup flag set: " + message);
                }
                if (!setOrigSendingTime)
                {
                    log.Error("Responding to a resend request but told not to set the OrigSendingTime field on message " + message);
                }
                if (message.MsgSeqNum > outMsgSeqNo || message.MsgSeqNum <= 0)
                {
                    log.Error("Responding to a resend request with a message with illegal msgSeqNum " + message + ", current outbound MsgSeqNum is " + outMsgSeqNo);
                }
                if (BasicFixUtilities.isAdmin(message))
                {
                    if (!ScalperFixSession.isSequenceReset_GapFill(message))
                    {
                        log.Error("This session must resend, only legal admin message is SequenceReset-GapFill.  Tried to send " + message);
                    }
                }
                else
                {
                    // any application message could be legal as long as it fulfills the above requirements
                }
            }
        }

        private bool preprocessBeforeSend2(Message message, bool setOrigSendingTime, bool messageIsFromDriver)
        {
            bool debugEnabled = true;
            bool persistFlag = true;
            //Reorder the header fields.
            System.String origSendingTime = null;

            if (setOrigSendingTime)
            {
                // actual value will be set later if necessary
                origSendingTime = message.getSendingTime();
            }


            // if the sending time hasn't been set, always set it to the correct value.
            // if this is a resend, always set to the correct value.
            if (!testMode || BasicUtilities.isNullOrEmpty(message.getSendingTime()) || setOrigSendingTime)
            {
                //Set sending time.
                // straighforward but too slow in high-bandwith situations:
                // message.setSendingTime( getUseMillis() );

                // timer caches value and queries the session as to whether milliseconds should
                // be used or not.
                System.String sendingTime = timer.TimeStamp;
                message.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGSendingTime_i, sendingTime);

                if (debugEnabled)
                {
                    log.Debug("Setting sending time to " + message.getSendingTime());
                }
            }

            if (setOrigSendingTime)
            {
                // if the original sending time is unavailable, use the current sending time
                if (origSendingTime == null || origSendingTime.Equals(""))
                {
                    origSendingTime = message.getSendingTime();
                }

                message.setValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGOrigSendingTime_i, origSendingTime);
            }

            //Assign next sequence number.

            // if we're in test mode, only set the sequence number if the user hasn't set it yet to allow them
            // to deliberately send the wrond thing.
            // if we're not in test mode, the only time the sequence number shouldn't be set is if we're in
            // "resend" mode, in which case we assume that the driver is resending old messages with the PosDup
            // flag set.
            int msgSeqNum = message.MsgSeqNum;
            if (ScalperFixSession.isSequenceReset_Reset(message))
            {
                // sequence number will be ignored by sender, so just make sure it isn't -1
                if (message.MsgSeqNum == -1)
                {
                    message.MsgSeqNum = 0;
                }
            }
            else if (sessionState.thisSessionMustResend())
            {
                if (msgSeqNum == -1 && !testMode)
                {
                    // error - we're sending a message in resend mode that has
                    // no msgSeqNum.. there is no default that the driver can fill in at this point
                    throw new FixException("Can't send a message in resend mode that has no sequence number");
                }
                // we do not change the message sequence number on the message.
                // we do not change the driver's next outbound sequence number
            }
            else
            {
                // normal send - driver knows what the correct msgSeqNo is
                if (msgSeqNum == -1)
                {
                    // normal case user/driver just wants the correct value
                    message.MsgSeqNum = ++outMsgSeqNo;
                }
                else if (msgSeqNum == outMsgSeqNo + 1)
                {
                    // user has explicitly set the message sequence number to the correct value
                    // do nothing to message -- already has correct message sequence number
                    ++outMsgSeqNo;
                }
                else
                {
                    // message has wrong messageSeqNo explicitly set
                    if (testMode)
                    {
                        // do nothing - ?
                        log.Warn("Deliberately sending message " + message + " with wrong sequence number, should have sequence number " + (outMsgSeqNo + 1) + "\n\tNot updating next expected outbound sequence number.");
                        persistFlag = false;
                    }
                    else
                    {
                        ++outMsgSeqNo;
                        message.MsgSeqNum = outMsgSeqNo;
                    }
                }
            }

            // must do things in the right order:
            // (1) add body field if the user didn't put it in
            // (2) put all fields in the correct order.  In particular
            // BeginString and BodyLength must be in their correct positions or
            // the bodyLength calculation won't give the right results.
            // (3) use that value to populate the Body Field

            // Note: it is now impossible to deliberately set body length / checksum to
            // be wrong when in test mode.  @todo.

            if (!message.containsTag(Dukascopy.com.scalper.fix.Constants_Fields.TAGBodyLength_i))
                message.insertValueAt(Dukascopy.com.scalper.fix.Constants_Fields.TAGBodyLength_i, "", 1);

            if (!message.containsTag(Dukascopy.com.scalper.fix.Constants_Fields.TAGCheckSum_i))
                message.addValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGCheckSum, "");

            return persistFlag;
        }

        private void postprocessBeforeSend1(Message message)
        {

            /* We needed the following check for driver test cases.
            * it shouldn't affect the performance much since its just a boolean check.
            */
            if (testMode)
            {
                if (ReorderFields)
                    message.reorderFields(tagHelper);

                if (getOverwriteBodyLength(message))
                    message.setBodyLength();

                if (getSetChecksum(message))
                    message.setCheckSum();
            }
            else
            {
                message.reorderFields(tagHelper);
                message.setBodyLength();
                message.setCheckSum();
            }

            try
            {
                //Send the message on socket.

                if (testMode && message.getCharValue(Dukascopy.com.scalper.fix.FixConstants_Fields.TAGDropMessage) == 'Y' && !ScalperFixSession.isPossDup(message))
                {
                    displayFixSend(message);
                    log.Warn("Deliberately dropping message " + message + ", in test mode and has drop flag set.");
                }
                else
                {
                    displayFixSend(message);

                    // this is separated out so they will show up on different lines for OptimizeIt.
                    if (!sessionClosed)
                    {
                        msgConn.sendMessage(message);
                        afterSendMessage(message);
                        if (isPerfOn_Renamed_Field)
                        {
                            outMsgCount++;
                        }
                    }
                    // Reset the test request ticker.  We assume at this point that the line is ok.
                    if (!message.Admin)
                        //timer.markTestRequestTimerForReset();
                        timer.resetTestRequestTimer();
                }
            }
            catch (System.Exception e)
            {
                SupportClass.WriteStackTrace(e, Console.Error);
                log.Error(e);
                timer.stopWaitingForResend();
                throw new FixException(e.Message);
            }
        }


        protected internal virtual bool isTestMode(Message msg)
        {
            return false;
        }
        private void postprocessBeforeSend1(Message[] messages)
        {
            /* We needed the following check for driver test cases.
            * it shouldn't affect the performance much since its just a boolean check.
            */
            Message message = null;
            for (int i = 0; i < messages.Length; i++)
            {
                message = (Message)messages[i];
                if (testMode || isTestMode(message))
                {
                    if (ReorderFields)
                        message.reorderFields(tagHelper);

                    if (getOverwriteBodyLength(message))
                        message.setBodyLength();

                    if (getSetChecksum(message))
                        message.setCheckSum();
                }
                else
                {
                    message.reorderFields(tagHelper);
                    message.setBodyLength();
                    message.setCheckSum();
                }
                // Reset the test request ticker.  We assume at this point that the line is ok.
                if (!message.Admin)
                    timer.resetTestRequestTimer();

                displayFixSend(message);
            }

            try
            {
                //Send the message on socket.

                // this is separated out so they will show up on different lines for OptimizeIt.
                if (!sessionClosed)
                {
                    msgConn.sendMessages(messages);
                    if (isPerfOn_Renamed_Field)
                    {
                        outMsgCount += messages.Length;
                    }
                }
            }
            catch (System.Exception e)
            {
                SupportClass.WriteStackTrace(e, Console.Error);
                log.Error(e);
                timer.stopWaitingForResend();
                throw new FixException(e.Message);
            }
        }

        private void displayFixSend(Message message)
        {
            log.Debug("Send:" + message.toString(false));
            if (PrintFixAfter)
            {
                System.Text.StringBuilder buf = new System.Text.StringBuilder();
                buf.Append(">==== FIX:Send [ ");
                if (id != null)
                {
                    buf.Append(id);
                }

                buf.Append(",");
                buf.Append(message.MsgSeqNum);
                buf.Append(",35=");
                buf.Append(message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType_i));
                buf.Append(" ] ");

                if (PrintFixHumanReadable)
                {
                    log.Debug(buf.Append(message.toHumanReadableString()).ToString());
                }
                else
                {
                    log.Fatal(buf.Append("[").Append(message.toString(false)).Append("]").ToString());
                }
            }
        }

        protected internal virtual void afterSendMessage(Message message)
        {
            log.Info("After SendMesg: " + message.toString(false));
        }

        protected internal virtual void afterReceiveMessage(Message message)
        {
                log.Info("After ReceiveMsg: " + message.toString(false));
        }

        private void postprocessBeforeSend2(Message message)
        {
            char msgType = message.MsgTypeChar;
            if (msgType == '5')
            {
                // notify listeners and do other housekeeping for a logout message
                logoutTransmitted(message);
            }
            //Reset the heart beat monitor.
            timer.resetHeartbeatTimer();

            notifyMessageSent(message);
        }

        /// <summary> This methods will be used by user application to retrieve the
        /// inbound application messages.
        /// </summary>
        /// <returns> Returns FIX message from inbound message queue.
        /// </returns>
        /// <throws>  FixException </throws>
        public virtual Message accept()
        {
            if (sessionClosed)
            {
                throw new FixException("Called S7 Software Solutions Pvt. Ltd.FixSession.accept() on a session that is already closed.");
            }

            //Loop till session is alive and you get a message.
            while (!sessionClosed)
            {
                Message msg;
                // there's a message available for us
                try
                {
                    msg = (Message)inMessageStore.get_Renamed();
                }
                catch (System.IO.IOException e)
                {
                    throw new FixException(e);
                }
                //}
                if (msg == null)
                {
                    if (sessionClosed)
                        throw new FixException("FFillFixSession.accept():FIX Session is closed");
                    else
                        throw new FixException("FFillFixSession.accept():Read invalid message");
                }

                if (validate(msg))
                {
                    return msg;
                }
            }
            throw new FixException("FFillFixSession.accept():FIX Session is closed");
        }

        /// <summary> This methods will be used by user application to retrieve the
        /// inbound application messages in a batch. If you do this then each message
        /// needs to be validated before being processed
        /// </summary>
        /// <returns> Returns A BoundedQueue of all the messages
        /// </returns>
        /// <throws>  FixException </throws>
        public virtual BoundedQueue batchAccept()
        {
            if (sessionClosed)
            {
                throw new FixException("Called S7 Software Solutions Pvt. Ltd.FixSession.accept() on a session that is already closed.");
            }

            //Loop till session is alive and you get a message.
            while (!sessionClosed)
            {
                // there's a message available for us
                try
                {
                    return inMessageStore.backupQueue();
                }
                catch (System.IO.IOException e)
                {
                    throw new FixException(e);
                }
            }
            throw new FixException("FFillFixSession.accept():FIX Session is closed");
        }

        protected internal virtual void enqueueInMessage(Message msg)
        {
            //inMessageStore.put(msg);
        }

        /// <summary> Validates that the given FIX message has all the required fields, CompID, SendingTime etc.
        /// In case these validation fail we need to respond through 'Reject(session-level)' message.
        /// 
        /// </summary>
        /// <returns> true if this message is valid
        /// </returns>
        /// <throws>  FixException </throws>
        /// <seealso cref="SessionProperties.getCheckSendingTime. if the message is invalid then a reject will">
        /// </seealso>
        public virtual bool validate(Message msg)
        {
            try
            {
                doValidate(msg);
                return true;
            }
            catch (FixException e2)
            {
                Message reject = buildRejectMessage(msg, e2);
                queueTransmit(new SendCommand(this, reject, msg));
                log.Error("Rejecting " + msg.toString(false));
            }
            return false;
        }
        /// <summary> By calling this method user application will indicate that processing
        /// of the inbound message is complete. This method will save the sequence
        /// number and removes the message from internal inbound message store.  If
        /// this is the next application message in order, the persister will be notified
        /// that the message is no longer needed and the message will not be rerequested
        /// at startup if the session goes down unexpectedly.  If the message is not the
        /// next application message in order, the fact that it has been marked "complete" will
        /// be noted internally but not told to the persister.  The persister will only be
        /// told to complete a message when it has been marked complete by the application
        /// and all application messages prior to it have also been marked "completed" by the application.
        /// </summary>
        /// <param name="msg">message whose processing is complete.
        /// </param>
        /// <throws>  FixException </throws>
        public virtual void complete(Message msg)
        {
            complete(msg.MsgSeqNum, true);
        }

        public virtual void complete(int msgSeqNo)
        {
            complete(msgSeqNo, true);
        }

        private void complete(int msgSeqNo, bool logWarnings)
        {
            int highestCompleted = uncompletedMessageStore.completeMessage(msgSeqNo);
            if (highestCompleted > 0)
                persister.storeInboundMsgSeqNum(highestCompleted);
        }

        /// <summary> Starts reader and admin threads. Reader thread will reads buffered byte
        /// stream from the FIX connection, builds the FIX message from it and queue the
        /// message for delivery. admin thread will process the admin messages.
        /// </summary>
        /// <throws>  FixException </throws>
        public virtual void start()
        {
            if (Active)
            {
                throw new System.ArgumentException("Already started.");
            }
            if (sessionState.State != SessionState.WAITING_FOR_LOGON)
            {
                throw new System.SystemException("Session in wrong state: " + sessionState.State);
            }
            properties.setLocked();
            setFIXVersionFromProperty();
            sessionClosed = false;
            reader.Start();
            listenerHelper.start();
        }

        internal virtual void setFIXVersionFromProperty()
        {
            System.String fixVersion = properties.Get(SessionProperties.Prop_FixVersion);
            if (fixVersion == null)
                throw new FixException("Must explicitly set FIX version when in client mode");
            setFixVersion(fixVersion);
        }

        /// <summary> Stops the session activity.</summary>
        /// <throws>  FixException </throws>
        public virtual void stop(int _FIXNOTICE, String msgReason)
        {
            // to find out where we were called from:
            int sessionStateInt = sessionState.State;
            if (sessionStateInt != SessionState.SESSION_CLOSING && sessionStateInt != SessionState.SESSION_CLOSED)
            {
                stopInboundImpl(_FIXNOTICE, msgReason);
            }
        }

        public virtual void stopImmediately()
        {
            int sessionStateInt = sessionState.State;
            if (sessionStateInt != SessionState.SESSION_CLOSING && sessionStateInt != SessionState.SESSION_CLOSED)
            {
                stopImmediatelyImpl();
            }
        }

        protected internal virtual void stopImmediatelyImpl()
        {
            //Set the session state.
            State = SessionState.SESSION_CLOSED;
            log.Info("FFillFixSession.stop():Shutting down the session immediately...");
            sessionClosed = true;
            logonSuccess = false;

            // close the conection. - reader thread will get an exception the next time it tries to read
            // and figure out that the system has been shut down.
            // writer threads should also look at the state and figure out why the error happened.
            try
            {
                msgConn.close();
            }
            catch (System.IO.IOException ioe)
            {
                log.Error("Error in closing the connection.", ioe);
                SupportClass.WriteStackTrace(ioe, Console.Error);
            }


            //Interrupt reader and amdin thread.
            reader.requestStop();
            listenerHelper.stopAfterDeliverAllMessages();

            timer.stop();

            //Notify the threads that are waiting to transmit the message
            lock (sessionStateLock)
            {
                System.Threading.Monitor.PulseAll(sessionStateLock);
            }
            inMessageStore.clear(); // delete all messages -- we're not processing them
            //Notify the threads that are waiting on incoming admin messages
            lock (inAdminMsgStoreLock)
            {
                adminMessageStore.Clear(); // delete all messages -- we're not processing them
                System.Threading.Monitor.PulseAll(inAdminMsgStoreLock);
            }

            possiblyNotifyListeners(this, FIXNotice.FIX_DISCONNECTED, "Disconnected");

            notifyDisconnectListeners();
            resendSeqs.Clear();

            log.Info("FFillFixSession.stop():Session shutdown successfully");
        }

        protected internal virtual void stopInboundImpl(int _FIXNOTICE, String msgReason)
        {
            //Set the session state.
            State = SessionState.SESSION_CLOSING;
            log.Info("FFillFixSession.stop():Shutting down the session...");
            sessionClosed = true;
            logonSuccess = false;

            //Interrupt reader and amdin thread.
            reader.requestStop();
            listenerHelper.stopAfterDeliverAllMessages();

            //Notify the threads that are waiting to transmit the message
            lock (sessionStateLock)
            {
                System.Threading.Monitor.PulseAll(sessionStateLock);
            }
            //Notify the threads that are waiting on incoming business messages.
            inMessageStore.close();
            //Notify the threads that are waiting on incoming admin messages
            lock (inAdminMsgStoreLock)
            {
                System.Threading.Monitor.PulseAll(inAdminMsgStoreLock);
            }

            timer.stop();

            //close the conection.
            try
            {
                msgConn.close();
            }
            catch (System.IO.IOException ioe)
            {
                SupportClass.WriteStackTrace(ioe, Console.Error);
                log.Error("Error in closing the connection.",ioe);
            }
            resendSeqs.Clear();
            State = SessionState.SESSION_CLOSED;
            //possiblyNotifyListeners(this, FIXNotice.FIX_DISCONNECTED, "Disconnected");
            possiblyNotifyListeners(this, _FIXNOTICE, msgReason);

            notifyDisconnectListeners();

            log.Info("FFillFixSession.stop():Session shutdown successfully");
        }


        /// <summary> Returns byte array of the specified string.</summary>
        /// <param name="str">
        /// </param>
        /// <returns> byte-array representation of the string
        /// </returns>
        public virtual sbyte[] getBytesFast(System.String str)
        {
            char[] buffer = new char[str.Length];
            int length = str.Length;
            sbyte[] b = new sbyte[length];
            SupportClass.GetCharsFromString(str, 0, length, buffer, 0);
            for (int j = 0; j < length; j++)
                b[j] = (sbyte)buffer[j];

            return b;
        }

        /// <summary> Validates that the given FIX message has all the required fields, CompID, SendingTime etc.
        /// In case these validation fail we need to respond through 'Reject(session-level)' message.
        /// </summary>
        /// <seealso cref="SessionProperties.getCheckSendingTime">
        /// </seealso>
        /// <param name="message">
        /// </param>
        /// <throws>  FixException </throws>
        private void doValidate(Message message)
        {
            // Validate that tags required for given MsgType are present
            checkFieldsForMsgType(message);

            // Validate SenderCompID field is present
            System.String sender = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i);
            if (sender == null)
            {
                FixMessageFormatException fMfe = new FixMessageFormatException("Required tag missing - SenderCompID.", FixException.REJECT_REASON_REQD_TAG_MISSING);
                fMfe.RefTagID = Dukascopy.com.scalper.fix.Constants_Fields.TAGSenderCompID_i;
                throw fMfe;
            }
            else if (StrictReceiveMode && !message.MsgType.Equals("A"))
            {
                // We have already saved the SenderCompID and TargetCompID in variables
                // 'targetCompID' and 'senderCompID', respectively. (party-counterparty will have reverse values).
                if (!sender.Equals(this.targetCompID))
                {
                    throw new FixMessageFormatException("CompID problem.", FixException.REJECT_REASON_COMPID_PROBLEM);
                    // after Reject message, we ought to Logout and disconnect ! 
                }
            }

            // Validate TargetCompID field is present
            System.String target = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i);
            if (target == null)
            {
                FixMessageFormatException fMfe = new FixMessageFormatException("Required tag missing - TargetCompID.", FixException.REJECT_REASON_REQD_TAG_MISSING);
                fMfe.RefTagID = Dukascopy.com.scalper.fix.Constants_Fields.TAGTargetCompID_i;
                throw fMfe;
            }
            else if (!message.MsgType.Equals("A"))
            {
                // We have already saved the SenderCompID and TargetCompID in variables
                // 'targetCompID' and 'senderCompID', respectively. (party-counterparty will have reverse values).
                if (StrictReceiveMode && !target.Equals(this.senderCompID))
                {
                    throw new FixMessageFormatException("CompID problem.", FixException.REJECT_REASON_COMPID_PROBLEM);
                    // after Reject message, we ought to Logout and disconnect ! 
                }
            }

            long currentTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;

            if (CheckSendingTime)
            {
                // Validate SendingTime accuracy
                System.String sendingTime = message.getSendingTime();
                System.DateTime sendingDate;
                
                if (sendingTime == null || sendingTime.Equals(""))
                {
                    throw new FixMessageFormatException("Required tag missing - SendingTime.", FixException.REJECT_REASON_REQD_TAG_MISSING);
                }

                try
                {
                    if (sendingTime.Length == ScalperFixSession.timeFormatMillisString.Length)
                    {
                        //sendingDate = System.DateTime.Parse(sendingTime, timeFormatterMillis);
                        DateTime.TryParse(ScalperFixSession.timeFormatMillisString, out sendingDate);
                    }
                    else
                    {
                        //sendingDate = System.DateTime.Parse(sendingTime, timeFormatter);
                        DateTime.TryParse(timeFormatter, out sendingDate);
                    }
                    if(sendingDate.Equals(System.DateTime.MinValue)) 
                    {
                        System.FormatException e1 = new FormatException("Wrong date time format.");
                        throw e1;
                    }
                }
                catch (System.FormatException e1)
                {
                    throwSendingTimeAccuracyProblem(e1);
                    // the above will *always* throw an exception but the compiler complains, so make it explicit:
                    return;
                }

                long sendingTime_l = sendingDate.Ticks;
                long diff = System.Math.Abs(currentTime - sendingTime_l);

                if (diff > sendingTimeMaxGap)
                {
                    System.DateTime tempAux = new System.DateTime(currentTime);
                    throwSendingTimeAccuracyProblem(ref tempAux, ref sendingDate);
                }
            }
        }

        private void throwSendingTimeAccuracyProblem(System.FormatException pe)
        {
            if (pe == null)
            {
                log.Error("SendingTime accuracy problem");
            }
            else
            {
                log.Error("SendingTime accuracy problem " + pe);
                log.Error(pe);
            }

            FixException fe = new FixMessageFormatException("SendingTime accuracy problem", FixException.REJECT_REASON_SENDING_TIME_INACCURATE);
            fe.ShouldLogOutUser = true;
            throw fe;
        }

        private void throwSendingTimeAccuracyProblem(ref System.DateTime expectedTime, ref System.DateTime sendingTime)
        {
            log.Error("SendingTime accuracy problem, expected: " + expectedTime.ToString("r") + ", got: " + sendingTime.ToString("r"));

            FixException fe = new FixMessageFormatException("SendingTime accuracy problem", FixException.REJECT_REASON_SENDING_TIME_INACCURATE);
            fe.ShouldLogOutUser = true;
            throw fe;
        }

        /// <summary> Checks if the mandatory fields required for a FIX message
        /// having a given MsgType are present or not.
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <throws>  FixException </throws>
        private void checkFieldsForMsgType(Message message)
        {
            System.String msgType = message.MsgType;
            if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGLogon))
            {
                // Logon message
                // Check that EncryptMethod & HeartBtInt fields are specified
                // and they have int values
                try
                {
                    System.String encryptMethodStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEncryptMethod_i);
                    //int encryptMethod=0;
                    if (encryptMethodStr != null)
                        //encryptMethod =
                        System.Int32.Parse(encryptMethodStr);
                    else
                        throw new System.NullReferenceException("EncryptMethod field is missing.");

                    System.String heartBtIntStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGHeartBtInt_i);
                    if (heartBtIntStr != null)
                        System.Int32.Parse(heartBtIntStr);
                    else
                        throw new System.NullReferenceException("HeartBtInt field is missing.");
                }
                catch (System.FormatException e)
                {
                    FixMessageContentException ex = new FixMessageContentException("Incorrect data format for the value." + e.Message);
                    ex.Code = FixException.REJECT_REASON_INCORRECT_DATA_FORMAT;
                    throw ex;
                }
                catch (System.NullReferenceException e)
                {
                    FixMessageFormatException ex = new FixMessageFormatException("Required tag(s) missing." + e.Message);
                    ex.Code = FixException.REJECT_REASON_REQD_TAG_MISSING;
                    throw ex;
                }
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGHeartbeat))
            {
                // Heartbeat message
                // Do nothing
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGTestRequest))
            {
                // TestRequest message
                try
                {
                    System.String testReqID = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGTestReqID_i);
                    if (testReqID == null)
                        throw new System.NullReferenceException("TestReqID field is missing.");
                }
                catch (System.NullReferenceException e)
                {
                    FixMessageFormatException ex = new FixMessageFormatException("Required tag(s) missing. " + e.Message);
                    ex.Code = FixException.REJECT_REASON_REQD_TAG_MISSING;
                    throw ex;
                }
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGResendRequest))
            {
                // ResendRequest message
                try
                {
                    System.String beginSeqNoStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGBeginSeqNo_i);
                    if (beginSeqNoStr != null)
                        System.Int32.Parse(beginSeqNoStr);
                    else
                        throw new System.NullReferenceException("BeginSeqNo field is missing.");

                    System.String endSeqNoStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i);
                    if (endSeqNoStr != null)
                        System.Int32.Parse(endSeqNoStr);
                    else
                        throw new System.NullReferenceException("EndSeqNo field is missing.");
                }
                catch (System.FormatException e)
                {
                    FixMessageContentException ex = new FixMessageContentException("Incorrect data format for the value. " + e.Message);
                    ex.Code = FixException.REJECT_REASON_INCORRECT_DATA_FORMAT;
                    throw ex;
                }
                catch (System.NullReferenceException e)
                {
                    FixMessageFormatException ex = new FixMessageFormatException("Required tag(s) missing. "
                        + e.Message);
                    ex.Code = FixException.REJECT_REASON_REQD_TAG_MISSING;
                    throw ex;
                }
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGReject))
            {
                // Reject message
                try
                {
                    System.String refSeqNumStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGRefSeqNum_i);
                    if (refSeqNumStr != null)
                        System.Int32.Parse(refSeqNumStr);
                    else
                        throw new System.NullReferenceException("RefSeqNum field is missing.");
                }
                catch (System.FormatException e)
                {
                    FixMessageContentException ex = new FixMessageContentException("Incorrect data format for the value. " + e.Message);
                    ex.Code = FixException.REJECT_REASON_INCORRECT_DATA_FORMAT;
                    throw ex;
                }
                catch (System.NullReferenceException e)
                {
                    FixMessageFormatException ex = new FixMessageFormatException("Required tag(s) missing. " + e.Message);
                    ex.Code = FixException.REJECT_REASON_REQD_TAG_MISSING;
                    throw ex;
                }
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGSequenceReset))
            {
                // SequenceReset message
                try
                {
                    System.String newSeqNoStr = message.getValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i);
                    if (newSeqNoStr != null)
                        System.Int32.Parse(newSeqNoStr);
                    else
                        throw new System.NullReferenceException("NewSeqNo field is missing.");
                }
                catch (System.FormatException e)
                {
                    FixMessageContentException ex = new FixMessageContentException("Incorrect data format for the value. " + e.Message);
                    ex.Code = FixException.REJECT_REASON_INCORRECT_DATA_FORMAT;
                    throw ex;
                }
                catch (System.NullReferenceException e)
                {
                    FixMessageFormatException ex = new FixMessageFormatException("Required tag(s) missing " + e.Message);
                    ex.Code = FixException.REJECT_REASON_REQD_TAG_MISSING;
                    throw ex;
                }
            }
            else if (msgType.Equals(Dukascopy.com.scalper.fix.Constants_Fields.MSGLogout))
            {
                // Logout message
                // Do nothing
            }
        }

        /// <summary> Returns the FIX version.</summary>
        /// <returns> Returns the fixVersion.
        /// </returns>
        public virtual System.String getFixVersion()
        {
            if (fixVersion == null)
            {
                throw new System.SystemException("Fix version not set yet.");
            }
            return fixVersion;
        }
        /// <summary> Sets the FIXVersion of current session.</summary>
        /// <param name="fixVersion">The fixVersion to set.
        /// </param>
        private void setFixVersion(System.String newFixVersion)
        {
            if (newFixVersion.StartsWith("FIX."))
            {
                newFixVersion = newFixVersion.Substring("FIX.".Length);
            }

            // there's an instance variable "minorVersion", can't use that one unless we're sure this is a valid change.
            char minorVersionTemp = BasicFixUtilities.getFixMinorVersion(newFixVersion);

            if (fixVersion != null && !fixVersion.Equals(newFixVersion))
            {
                throw new InvalidFixVersionException("Cannot change FIX version from " + fixVersion + " to " + newFixVersion);
            }


            switch (minorVersionTemp)
            {

                // @todo cache these somewhere
                case '0': tagHelper = new TagHelper("FIX.4.0"); break;

                case '1': tagHelper = new TagHelper("FIX.4.1"); break;

                case '2': tagHelper = new TagHelper("FIX.4.4"); break;

                case '3': tagHelper = new TagHelper("FIX.4.3"); break;

                case '4': tagHelper = new TagHelper("FIX.4.4"); break;

                default: throw new InvalidFixVersionException("Unknown fix version: " + newFixVersion);

            }

            // valid fix version, set state.

            this.fixVersion = newFixVersion;
            this.beginFixVersion = "FIX." + fixVersion;

            //Extract FIX minor sequence number. Will be used to consistency of all the incoming message.
            this.minorVersion = minorVersionTemp;
            if (minorVersion >= '2')
            {
                resendInfinity = Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_42;
            }
            else
            {
                resendInfinity = Dukascopy.com.scalper.fix.Constants_Fields.RESEND_INFINITY_40_41;
            }
        }

        /// <summary> returns true or false to indicate isLoggedIn status</summary>
        public virtual bool waitUntilLoggedIn()
        {
            return waitUntilReady(0);
        }

        /// <summary> returns true or false to indicate isLoggedIn status, but waits up until timeOut before returning false</summary>
        public virtual bool waitUntilReady(long timeOut)
        {
            if (LoggedIn)
                return true;
            else if (timeOut == -1)
                return false;
            lock (sessionStateLock)
            {
                try
                {
                    System.Threading.Monitor.Wait(sessionStateLock, TimeSpan.FromMilliseconds(timeOut));
                }
                catch (System.Threading.ThreadInterruptedException ie)
                {
                    log.Error(" ", ie);
                }
                return LoggedIn;
            }
        }

        /// <summary> waits until the transport-level connection is disconnected or for timeOut ms,
        /// whichever comes first.  Returns true if the connection has been disconnected and
        /// false if the connection is still active.  Note that if the connection is active this
        /// says nothing about whether the client is still logged in or not.
        /// </summary>
        public virtual bool waitUntilDisconnected(long timeOut)
        {
            if (!Active)
                return true;
            else if (timeOut == -1)
                return false;
            lock (sessionStateLock)
            {
                try
                {
                    System.Threading.Monitor.Wait(sessionStateLock, TimeSpan.FromMilliseconds(timeOut));
                }
                catch (System.Threading.ThreadInterruptedException ie)
                {
                    log.Error(" ", ie);
                }
                return !Active;
            }
        }

        /// <summary> Returns TRUE if the session is run under test mode.</summary>
        /// <returns> Returns the testMode.
        /// </returns>
        public virtual bool isTestMode()
        {
            return testMode;
        }

        /// <summary> Sets the test mode flag.</summary>
        /// <param name="testMode">The testMode to set.
        /// </param>
        public virtual void setTestMode(bool testMode)
        {
            this.testMode = testMode;
        }

        
        /// <summary> all application messages must release this lock after they are sent.
        /// The driver already takes care of this; calling this method is only necessary if there is a
        /// dependency between the application-level implementation of the persister and the application-level
        /// sending logic.
        /// </summary>
        //public virtual void releaseSendMessageLock()
        //{
        //    sessionState.releaseSendMessageLock();
        //}

        private bool getOverwriteBodyLength(Message message)
        {
            if (!properties.OverwriteBodyLength)
            {
                if (message.getBodyLength() <= 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        private bool getSetChecksum(Message message)
        {
            if (!properties.SetChecksum)
            {
                if ((message.getCheckSum() == null) || (message.getCheckSum().Trim().Length <= 0))
                    return true;
                else
                    return false;
            }
            return true;
        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void test_syncMsgSeqNumWithPersister()
        {
            // do NOT set nextExpectedSeqNo -- the value in the driver is correct, and the value
            // in the persister is just the last *persisted* (application) message
            // nextExpectedSeqNo = persister.getLastInSeqNo() + 1;
            int newOutMsgSeqNo = persister.LastOutSeqNo;
            if (newOutMsgSeqNo < outMsgSeqNo)
            {
                throw new System.SystemException();
            }

            outMsgSeqNo = newOutMsgSeqNo;
        }

        // convenience methods - should probably move to BasicFixUtilities
        private static bool isSequenceReset_Reset(Message msg)
        {
            char msgType = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType_i);
            char gapFillChar = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGGapFillFlag_i);

            return msgType == '4' && gapFillChar != 'Y';
        }

        private static bool isSequenceReset_GapFill(Message msg)
        {
            char msgType = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGMsgType_i);
            char gapFillChar = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGGapFillFlag_i);

            return msgType == '4' && gapFillChar == 'Y';
        }

        private static bool isPossDup(Message msg)
        {
            char possDupeFlag = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossDupFlag_i);
            return possDupeFlag == 'Y';
        }
        private static bool isPossResend(Message msg)
        {
            char possResendFlag = msg.getCharValue(Dukascopy.com.scalper.fix.Constants_Fields.TAGPossResend_i);
            return possResendFlag == 'Y';
        }

        //("serial")
        [Serializable]
        private class InvalidFixVersionException : FixException
        {
            /// <summary> </summary>
            private const long serialVersionUID = 1L;

            public InvalidFixVersionException(System.String error)
                : base(error)
            {
            }
        }

        public class SendCommand
        {
            private void InitBlock(ScalperFixSession enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
                //cmdType = Enclosing_Instance.SendCommand.SEND;
                cmdType = Dukascopy.com.scalper.fix.driver.ScalperFixSession.SendCommand.SEND;
            }
            private ScalperFixSession enclosingInstance;
            public ScalperFixSession Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public const int SEND = 0;
            public const int RESEND = 1;
            public Message msg = null, origMsg = null;
            public int cmdType;
            public int start = 0, end = 0;

            internal SendCommand(ScalperFixSession enclosingInstance, Message msg)
                : this(enclosingInstance, msg, null)
            {
            }
            internal SendCommand(ScalperFixSession enclosingInstance, Message msg, Message origMsg)
            {
                InitBlock(enclosingInstance);
                this.msg = msg;
                this.origMsg = origMsg;
            }
            internal SendCommand(ScalperFixSession enclosingInstance, Message msg, int cmdType, int start, int end)
            {
                InitBlock(enclosingInstance);
                this.msg = msg;
                this.cmdType = cmdType;
                this.start = start;
                this.end = end;
            }
        }

        public static void print(sbyte[] data, System.String name)
        {
            System.Text.StringBuilder buf1 = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                buf1.Append(data[i] + ",");
            }
        }
    }
}