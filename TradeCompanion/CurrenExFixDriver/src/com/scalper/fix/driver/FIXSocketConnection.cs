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
/// File: FIXSocketConnection.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using Logging = com.scalper.util.Logging;
using log4net;
namespace com.scalper.fix.driver
{
	
	/// <summary> <p>Description: Represents the socket based FIX connection established
	/// between counterparties.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public class FIXSocketConnection : FIXConnection
	{
        
		virtual public int Port
		{
			get
			{
				if (sock != null)
				{
                    
					System.Net.IPEndPoint ep = (System.Net.IPEndPoint)sock.Client.LocalEndPoint;
                    return ep.Port;
				}
				return -1;
			}
			
		}
		virtual public System.String Address
		{
			get
			{
				if (sock != null)
				{
                    System.Net.IPEndPoint ep = (System.Net.IPEndPoint)sock.Client.LocalEndPoint;

					System.String generatedAux = System.Net.Dns.GetHostEntry(ep.Address).HostName;
				}
				return null;
			}
			
		}
		/// <summary> Checks if connection is active.</summary>
		/// <returns> true if connected.
		/// </returns>
		virtual public bool Connected
		{
			get
			{
				if (sock != null)
					return sock.Connected;
				else
					return false;
			}
			
		}
		virtual public bool Blocking
		{
			set
			{
				try
				{
					sock.ReceiveTimeout = 0;
				}
				catch (System.IO.IOException e)
				{
                    log.Error(e);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
			
		}
		virtual public bool TcpNoDelay
		{
			set
			{
				try
				{
					sock.NoDelay = value;
				}
				catch (System.Exception e)
				{
                    log.Error(e);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
			
		}
		virtual public int SendBufferSize
		{
			set
			{
				try
				{
					sock.SendBufferSize = value;
				}
				catch (System.Exception e)
				{
                    log.Error(e);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
			
		}
		virtual public int ReceiveBufferSize
		{
			set
			{
				try
				{
					sock.ReceiveBufferSize = value;
				}
				catch (System.Exception e)
				{
                    log.Error(e);
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
			
		}

        private static readonly ILog log = LogManager.GetLogger(typeof(FIXSocketConnection));
		internal System.Net.Sockets.TcpClient sock;
		internal System.IO.Stream out_Renamed;
		internal System.IO.Stream in_Renamed;
		
		/// <summary> Creates a new connection.</summary>
		/// <param name="sock">
		/// </param>
		public FIXSocketConnection(System.Net.Sockets.TcpClient sock)
		{
			this.sock = sock;
			out_Renamed = sock.GetStream();
			in_Renamed = sock.GetStream();
		}
		
		public virtual void  send(com.scalper.util.ByteBuffer writeBuffer)
		{
			if (sock.Connected)
			{
				writeBuffer.rewind();
				sbyte[] temp_sbyteArray;
				temp_sbyteArray = writeBuffer.limitArray();
				out_Renamed.Write(SupportClass.ToByteArray(temp_sbyteArray), 0, temp_sbyteArray.Length);
				out_Renamed.Flush();
			}
			else
			{
                log.Error("FIXSocketConnection: Cannot send message: " + new System.String(SupportClass.ToCharArray(SupportClass.ToByteArray(writeBuffer.limitArray()))));
				//Logging.error("FIXSocketConnection: Cannot send message: " + new System.String(SupportClass.ToCharArray(SupportClass.ToByteArray(writeBuffer.limitArray()))));
				throw new System.IO.IOException("Channel is not connected");
			}
		}
		
		public virtual int receive(sbyte[] buf, int startIndex)
		{
			int maxRequested = buf.Length - startIndex;
			int numToRead = 0;
			if (sock.Connected)
            {
                try
                {
                    numToRead = SupportClass.ReadInput(in_Renamed, buf, startIndex, maxRequested);
                }
                catch //(Exception e)
                {
                    //do nothing.
                }
				if (numToRead == - 1)
					throw new System.IO.IOException("Socket is closed");
				
				return numToRead;
			}
			else
				throw new System.IO.IOException("Channel is not connected");
		}
		
		/// <summary> Closes the connection.</summary>
		public virtual void  close()
		{
			try
			{
				lock (this)
				{
					// write all previously written output, followed by normal TCP termination sequence
					try
					{
						out_Renamed.Flush();
					}
					catch (System.IO.IOException ioe)
					{
                        log.Error(ioe);
						SupportClass.WriteStackTrace(ioe, Console.Error);
					}
					sock.Close();
				}
			}
			catch
			{
				//Logging.info("AsynchronousClose of the socket");
                log.Info("AsynchronousClose of the socket");
			}
		}
	}
}