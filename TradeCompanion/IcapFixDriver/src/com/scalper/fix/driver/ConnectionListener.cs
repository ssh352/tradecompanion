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
namespace Icap.com.scalper.fix.driver
{
	
	/// <summary> handles the low-level waiting for a session to be connected
	/// 
	/// Usage:
	/// <pre>
	/// // Construct a FIXAcceptor object.
	/// FIXSocketAcceptor acceptor = new FIXSocketAcceptor(port);
	/// 
	/// // Register a notice listener w/ the acceptor
	/// ConnectionListener logonListener = new ConnectionListener();
	/// // by default, register for all notices
	/// acceptor.registerNoticeListener(logonListener, true);
	/// 
	/// // Start listening for incoming client connections
	/// acceptor.start();
	/// <pre>
	/// It can be used in exactly the same way with a FIXSocketAcceptor (for the server) or FIXSocketInitiator
	/// (for the client)
	/// </summary>
	public class ConnectionListener : FIXNoticeListener
	{
		private System.Collections.IList connectionList;
		private System.Object waitObj = new System.Object();
		internal bool done = false;
		
		public ConnectionListener()
		{
			connectionList = new System.Collections.ArrayList();
		}
		
		public virtual FIXConnection waitForConnection()
		{
			lock (waitObj)
			{
				while ((connectionList.Count == 0) && !done)
				{
					try
					{
						System.Threading.Monitor.Wait(waitObj);
					}
					catch (System.Threading.ThreadInterruptedException ie)
					{
						stop();
					}
				}
				if (done)
					return null;
				System.Object tempObject;
				tempObject = connectionList[0];
				connectionList.RemoveAt(0);
				FIXConnection conn = (FIXConnection) tempObject;
				return conn;
			}
		}
		
		/// <summary> this method is public as an implementation side effect and should not be called directly.</summary>
		public virtual void  takeNote(FIXTNotice notice)
		{
			int code = notice.Code;
			if (code == FIXTNotice.FIX_CONNECTION_ESTABLISHED)
			{
				lock (waitObj)
				{
					FIXConnection connection = notice.Connection;
					connectionList.Add(connection);
					System.Threading.Monitor.PulseAll(waitObj);
				}
			}
		}
		public virtual void  stop()
		{
			connectionList.Clear();
			done = true;
			lock (waitObj)
			{
				System.Threading.Monitor.PulseAll(waitObj);
			}
		}
	}
}