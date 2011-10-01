/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd. Software Inc. ("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using Dukascopy.com.scalper.fix;
using Dukascopy.com.scalper.fix.driver;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Text;
using log4net;
using log4net.Config;



namespace Dukascopy.com.scalper.fix.driver.client
{
	public abstract class DefaultClient:AbstractClient
	{
		private class AnonymousClassClientSessionCustomizer:ClientSessionCustomizer
		{
			public AnonymousClassClientSessionCustomizer(Dukascopy.com.scalper.fix.driver.FIXPersister persister, DefaultClient enclosingInstance)
			{
				InitBlock(persister, enclosingInstance);
			}
			private void  InitBlock(Dukascopy.com.scalper.fix.driver.FIXPersister persister, DefaultClient enclosingInstance)
			{
				this.persister = persister;
				this.enclosingInstance = enclosingInstance;
			}
			private Dukascopy.com.scalper.fix.driver.FIXPersister persister;
			private DefaultClient enclosingInstance;
 
    		override public FIXPersister Persister
			{
				get
				{
					return persister;
				}
				
			}
			public DefaultClient Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
		}
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
		virtual public System.String SenderCompID
		{
			get
			{
				return senderCompID;
			}
			
			set
			{
				this.senderCompID = value;
			}
			
		}
		virtual public System.String TargetSubID
		{
			get
			{
				return targetSubID;
			}
			
			set
			{
				this.targetSubID = value;
			}
			
		}
		virtual public System.String TargetCompID
		{
			get
			{
				return targetCompID;
			}
			
			set
			{
				this.targetCompID = value;
			}
			
		}
        
        public System.Int32 InSeqNo
        {
            get
            {
                return inSeqNo;
            }

            set
            {
                this.inSeqNo = value;
            }

        }

        public System.Int32 OutSeqNo
        {
            get
            {
                return outSeqNo;
            }

            set
            {
                this.outSeqNo = value;
            }

        }

		private System.Net.IPAddress host;
		
		private int port;
		
		private FIXInitiator initiator;

		private System.String senderSubID;

        private System.String senderCompID;
		
		private System.String targetSubID;
		
		private System.String targetCompID;

        private System.Int32 inSeqNo;

        private System.Int32 outSeqNo;

		protected internal const System.String delimiter = "\u0001";

        private System.Int32 heartBeatInterval;

        public int HeartBeatInterval 
        {
            get
            {
                return heartBeatInterval;
            }
            
            set
            {
                heartBeatInterval = value;
            }
        }

        protected static readonly ILog log = LogManager.GetLogger(typeof(DefaultClient));

		private void init() 
        {
            HeartBeatInterval = 30; //30 sec
            //Configure logging (using Log4Net package in this software).
            XmlConfigurator.Configure(new System.IO.FileInfo(Application.StartupPath + "\\logging.xml"));

            connect(CommonTest.DefaultProperties, false, new FIXVolatilePersister());
        }

		public DefaultClient(int port):this(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0], port)
		{
		}

        public DefaultClient(System.String hostname, int port)
            : this(System.Net.Dns.GetHostEntry(hostname).AddressList[0], port)
        {
		}
		
		private DefaultClient(System.Net.IPAddress host, int port)
		{
			this.host = host;
			this.port = port;
            init();
		}

        protected internal override ScalperFixSession createSession(SessionProperties properties, FIXPersister persister)
        {
            System.String sHost = properties.Get("host");
            System.Net.IPAddress iHost = host;
            try
            {
                if (sHost != null)
                {
                    iHost = System.Net.Dns.GetHostEntry(sHost).AddressList[0];
                }
            }
            catch (System.Exception e)
            {
               log.Error(e);
               // SupportClass.WriteStackTrace(e, Console.Error);
            }
            int iPort = port;
            if (properties.Get("host") != null)
                sHost = properties.Get("host");
            System.String sPort = properties.Get("port");
            try
            {
                iPort = System.Int32.Parse(sPort);
            }
            catch //(System.Exception e)
            {
                //                log.Error(" ", e);
                iPort = port;
            }
            try
            {
                initiator = new FIXSocketInitiator(iHost, iPort);
            }
            catch (System.Exception ioe)
            {
                throw new FixException(ioe);
            }

             ConnectionListener logonListener = new ConnectionListener();
            // by default, register for all notices
            initiator.registerNoticeListener(logonListener, true);

            log.Info("Connecting to the FIX Server: " + iHost.ToString() + ":" + iPort.ToString());

            // start the process of connecting
            initiator.start();

            FIXConnection conn = logonListener.waitForConnection();

            ScalperFixSession session = new ScalperFixSession(new DefaultMessageConnection(conn), properties, new AnonymousClassClientSessionCustomizer(persister, this));
            log.Info("Created session");
            if (senderSubID != null)
                session.SenderSubID = senderSubID;
            if (senderCompID != null)
                session.SenderCompID = senderCompID;
            if (targetSubID != null)
                session.TargetSubID = targetSubID;
            if (targetCompID != null)
                session.TargetCompID = targetCompID;

            session.addMessageListener(new DefaultClientMessageListener(this));
            return session;
        }
		
		public override void  disconnect()
		{
			if (initiator != null)
			{
				initiator.stop();
			}
        }
		
		public void  sendMessage(System.String msgString)
		{
			Message msg = createMessage(msgString);
			
			// need to tell the persister manually that it's sending a message whose
			// sequence number is too high.
			// Normally persister insists on messages being sent in the right order.
			
			int msgSeqNum = msg.MsgSeqNum;
			int expectedSeqNum = Session.FIXPersister.LastOutSeqNo + 1;

			
            if (msgSeqNum > expectedSeqNum)
			{
				for (int i = expectedSeqNum; i < msgSeqNum; i++)
				{
					Session.FIXPersister.storeOutboundAdminMessage(createFakeAdminMessage(i));
				}
				Session.test_syncMsgSeqNumWithPersister();
			}
			
			sendMessage(msg);
		}
		
		public virtual void  sendMessages(System.String[] msgs)
		{
			Message[] msgs2Send = new Message[msgs.Length];
			for (int i = 0; i < msgs.Length; i++)
			{
				Message msg = createMessage(msgs[i]);
				msgs2Send[i] = msg;
			}
			log.Debug("Sending messages:" + msgs2Send.Length);
			base.sendMessages(msgs2Send);
		}
		
		private static bool isUseMillis(System.String msg)
		{
			char ch = getFixMinorVersion(msg);
			return ch >= '2' && ch <= '4';
		}
		
		/// <summary> returns the FIX minor version, or '\01' if the begin string cannot be
		/// parsed.
		/// </summary>
		private static char getFixMinorVersion(System.String msg)
		{
			System.String beginString = "8=FIX.4.";
			if (!msg.StartsWith(beginString))
			{
				// badly formatted message, can't get FIX version.
				return (char) 0;
			}
			if (msg.Length <= beginString.Length)
			{
				// badly formatted message, can't get FIX version.
				return (char) 0;
			}
			return msg[beginString.Length];
		}
		
		protected internal /*static*/ Message createMessage(System.String text)
		{
			text = text.Replace('^', '\u0001');
			Message msg = new Message();
			
			bool useMillis = isUseMillis(text);
			
			SupportClass.Tokenizer tok = new SupportClass.Tokenizer(text, delimiter);
			while (tok.HasMoreTokens())
			{
				System.String tagAndValue = tok.NextToken();
				SupportClass.Tokenizer tok2 = new SupportClass.Tokenizer(tagAndValue, "=");
				if (tok2.Count != 2)
				{
					throw new FixException("tag/value " + tagAndValue + " has wrong number of '=' characters");
				}
				System.String tag = tok2.NextToken();
				System.String value_Renamed = tok2.NextToken();
				if (tag.Trim().Equals("") || value_Renamed.Trim().Equals(""))
				{
					throw new FixException("can't have empty tag/value " + tagAndValue);
				}
				if (value_Renamed.StartsWith("<TIME"))
				{
					value_Renamed = buildDateString(value_Renamed, useMillis);
				}
				msg.addValue(tag, value_Renamed);
			}
			return msg;
		}

        //Following 4 methods has to be moved to the sub classes.
        
        
        public void logout()
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                return;

            Message msg = new Message(Constants_Fields.MSGLogout);
            sendMessage(msg);

            Session.waitUntilDisconnected(5000);
        }

        public void closeLogger()
        {
            LogManager.Shutdown();
        }

		protected internal static System.String buildDateString(System.String timeString, bool useMillis)
		{
            if (timeString.Equals("<TIME>"))
            {
                System.DateTime tempAux = DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
                return Message.buildDateString(ref tempAux, false, useMillis);
            }

            return null;
		}

            

        //Must be implemented by the derived classes.
        protected abstract void handleMsgReceived(Dukascopy.com.scalper.fix.Message msg);
        private class DefaultClientMessageListener : FIXMessageListener
        {
            private DefaultClient enclosingInstance;
            public DefaultClientMessageListener(DefaultClient cl)
            {
                this.enclosingInstance = cl;
            }

            public virtual DefaultClient EnclosingInstance
            {
                get
                {
                    return enclosingInstance;
                }
            }

            public virtual void messageReceived(Dukascopy.com.scalper.fix.Message msg)
            {
                 enclosingInstance.handleMsgReceived(msg);
            }
            /// <summary>notification that a message has been sent by the driver </summary>
            public virtual void messageSent(Dukascopy.com.scalper.fix.Message msg)
            {
                //enclosingInstance.handleMsgSent(msg);
            }
        }
	}
}