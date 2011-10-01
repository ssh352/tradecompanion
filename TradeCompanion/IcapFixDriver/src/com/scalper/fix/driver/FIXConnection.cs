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
/// File: FIXConnection.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace Icap.com.scalper.fix.driver
{
	
	/// <summary> <p>Description: This abstract class provides the mechanism to send and receive
	/// the data over a established connection.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  
	/// </author>
	/// <version>  1.0
	/// </version>
	public interface FIXConnection
	{
		/// <summary> Sets the blocking mode of the connection.</summary>
		bool Blocking
		{
			set;
			
		}
		/// <summary> Sets the tcp no delay mode of the connection.</summary>
		bool TcpNoDelay
		{
			set;
			
		}
		/// <summary> Sets the Send Buffer Size of the connection.</summary>
		int SendBufferSize
		{
			set;
			
		}
		/// <summary> Sets the Receive Buffer Size of the connection.</summary>
		int ReceiveBufferSize
		{
			set;
			
		}
		
		/// <summary> Sends the stream of bytes over established connection.</summary>
		/// <param name="buf">The byte buffer storage that has the data to be sent.
		/// </param>
		/// <throws>  FixException </throws>
		void  send(Icap.com.scalper.util.ByteBuffer buf);
		
		/// <summary> Receives the stream of bytes from established connection.
		/// If in blocking mode, will block until the array has been filled.
		/// If not in blocking mode, will read as many bytes as are available
		/// </summary>
		/// <param name="buf">The byte buffer storage to receive the data.
		/// </param>
		/// <param name="startIndex">the index to start filling the buffer at.
		/// </param>
		/// <throws>  FixException </throws>
		int receive(sbyte[] buf, int startIndex);
		
		/// <summary> Closes the current connection safely.</summary>
		void  close();
	}
}