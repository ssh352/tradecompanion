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
using Message = Dukascopy.com.scalper.fix.Message;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	/// <summary> callback to allow user-specified code to send out a different message than the one the driver would have sent out.
	/// Normally this will be used to add tags to the message before it gets sent out.
	/// </summary>
	public interface MessageModifier
	{
		/// <summary> indicates that the driver would have sent out this message, but instead is passing it onto the MessageModifier
		/// for processing.  Normally an implementation of this method will call S7FIXSession.transmit with a modified version
		/// of the given message, however in some cases it may be appropriate to send another message type or to take no
		/// action at all.
		/// </summary>
		void  messageWouldHaveBeenSent(Message msg);
	}
}