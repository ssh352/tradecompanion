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
//using FixException = Icap.com.scalper.fix.FixException;
//using Logging = Icap.com.scalper.util.Logging;
using log4net;
namespace Icap.com.scalper.fix.driver
{
	
	/// <summary> a server that listens on the given port and processes connections as they come in.
	/// All business logic is done in the processConnection method.
	/// </summary>
	abstract public class ScalperFixServer:SupportClass.ThreadClass
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ScalperFixServer));
		private int port;
		private volatile bool running;
		private FIXSocketAcceptor acceptor;
		
		/// <summary> constructs a new server that, when started, will listen for connections on the given port.</summary>
        public ScalperFixServer(int port)
		{
			Name = "FFillFixServer";
			this.port = port;
		}
		
		/// <summary> runs in an infinite loop accepting connections, doing some processing, and calling
		/// processConnection on the results.
		/// </summary>
		override public void  Run()
		{
			running = true;
			try
			{
				acceptor = new FIXSocketAcceptor(port);
				ConnectionListener logonListener = new ConnectionListener();
				acceptor.registerNoticeListener(logonListener, true);
				acceptor.start();
				
				while (running)
				{
					FIXConnection conn = logonListener.waitForConnection();
					if (conn != null)
					{
						processConnection(conn);
                        log.Info("Server processing new connection.");
						//Logging.info("Server processing new connection.");
					}
				}
			}
			catch (System.Exception e)
			{
                log.Error(e);
				SupportClass.WriteStackTrace(e, Console.Error);
			}
		}
		
		/// <summary> closes all existing connections and stops listening for new ones.</summary>
		public virtual void  close()
		{
			try
			{
				if (acceptor != null)
				{
					acceptor.stop();
				}
			}
			catch (FixException fe)
			{
                log.Error(fe);
				SupportClass.WriteStackTrace(fe, Console.Error);
			}
			running = false;
			this.Interrupt();
		}
		
		/// <summary> does whatever business logic is necessary to handle the incoming client connection.</summary>
		abstract protected internal void  processConnection(FIXConnection conn);
	}
}