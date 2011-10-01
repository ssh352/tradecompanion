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
/// File: FIXSocketInitiator.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = com.scalper.fix.FixException;
using Logging = com.scalper.util.Logging;
using log4net;
namespace com.scalper.fix.driver
{
	
	/// <summary> <p>Description: This class represents the initiator that will try to establish
	/// the socket based connection with the specified counterparty.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public class FIXSocketInitiator:FIXInitiator
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(FIXSocketInitiator));
		/// <returns> Returns the retryInterval.
		/// </returns>
		/// <summary> Sets retry interval.</summary>
		/// <param name="seconds">
		/// </param>
		/// <throws>  FixException </throws>
		virtual public int RetryInterval
		{
			get
			{
				return retryInterval;
			}
			
			set
			{
				this.retryInterval = value;
			}
			
		}
		virtual public FIXConnection Connection
		{
			// Internal temperory method to established connection out of listener framework.
			
			get
			{
				if (connection == null || !((FIXSocketConnection) connection).Connected)
				{
					try
					{
						sockInitiator = new System.Net.Sockets.TcpClient(address.ToString(), port);
						connection = new FIXSocketConnection(sockInitiator);
						return connection;
					}
					catch 
					{
                        log.Info("FIXSocketInitiator: Failed to connect to server " + address.ToString() + ":" + port);
                        //Logging.info("FIXSocketInitiator: Failed to connect to server " + address.ToString() + ":" + port);
						return null;
					}
				}
				return connection;
			}
			
		}
		/// <returns> Returns the retryCount.
		/// </returns>
		/// <param name="retryCount">The retryCount to set.
		/// </param>
		virtual public int RetryCount
		{
			get
			{
				return retryCount;
			}
			
			set
			{
				this.retryCount = value;
			}
			
		}
		private System.Net.Sockets.TcpClient sockInitiator;
		private System.Net.IPAddress address;
		private int port;
		//Retry interval incase of failure, by default its 1 second
		private int retryInterval = 1;
		//Number of tries. by default thre will not be any retries.
		private int retryCount = 0;
		//Connection object.
		private FIXConnection connection;
		
		/// <summary> Constructs the socket based FIX initiator.</summary>
		/// <param name="address	InetAdress">of the counterparty.
		/// </param>
		/// <param name="port	Port">number of the counterparty where it is listening
		/// for incoming connections.
		/// </param>
		/// <throws>  FixException </throws>
		public FIXSocketInitiator(System.Net.IPAddress address, int port)
		{
			this.address = address;
			this.port = port;
		}
		
		/// <summary> Starts the thread that will tries to establish the connection with the counterparty
		/// and flags the notice on succesfull establishment of connection.
		/// </summary>
		public override void  start()
		{
			for (int i = 0; i <= retryCount; i++)
			{
				try
				{
					connection = Connection;
				}
				catch (System.Exception ex)
				{
                    log.Error(ex); 
				}
				
				if (i > 0)
				{
					try
					{
						System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * retryInterval * 1000));
					}
                    catch (System.Exception ex)
                    {
                        log.Error(ex);
                    }
				}
				
				if (connection != null)
				{
					// successful connection
					FIXNotice notice = new FIXNotice(this, connection, FIXNotice.FIX_CONNECTION_ESTABLISHED, "Connection established");
					if (listener != null)
					{
						if (listenerInterests[FIXNotice.FIX_CONNECTION_ESTABLISHED])
							listener.takeNote(notice);
						else
                            log.Error("Listener is not turned ON for the event 'connected' ");
							//Logging.error("Listener is not turned ON for the event 'connected' ");
					}
					else
						throw new FixException("No listener is registered");
					
					// done connecting - break out of "for" loop
					break;
				}
			}
			if (connection == null)
			{
				throw new FixException("Couldn't establish connection with server. address=[" + address + "] port=[" + port + "]");
			}
		}
		
		/// <summary> Stops trying to connect to counterparty, flags the notice.</summary>
		public override void  stop()
		{
			FIXNotice notice = new FIXNotice(this, connection, FIXNotice.FIX_DISCONNECTED, "Disconnected");
			if (listener != null)
			{
				if (listenerInterests[FIXNotice.FIX_DISCONNECTED])
					listener.takeNote(notice);
				else
                    log.Error("Listener is not turned ON for the event 'Disconnected' ");
					//Logging.error("Listener is not turned ON for the event 'Disconnected' ");
			}
			else
				throw new FixException("No listener is registered");
			if (connection != null)
				connection.close();
		}
	}
}