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
/// File: FIXMessageListener.cs
/// Created on Feb 19, 2004
/// ****************************************************
/// </summary>
using System;
using Message = Icap.com.scalper.fix.Message;
namespace Icap.com.scalper.fix.driver
{
	
	/// <summary> <p>Description: Interface for message listener. The user application implements this interface
	/// and registers that with the session. Session accepts messages and notifies the user application
	/// using the callback method of this interface.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public interface FIXMessageListener
	{
		/// <summary>notification that a message has been sent by the driver </summary>
		void  messageSent(Message msg);
		
		/// <summary>notification that a message has been received by the driver </summary>
		void  messageReceived(Message msg);
	}
}