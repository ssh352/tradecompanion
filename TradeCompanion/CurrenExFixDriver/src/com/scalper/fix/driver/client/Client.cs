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
using com.scalper.fix;
using com.scalper.fix.driver;
namespace com.scalper.fix.driver.client
{
	
	public interface Client
	{
		bool Connected
		{
			get;
			
		}
        ScalperFixSession Session
		{
			// @todo remove?
			
			get;
			
		}
		void  connect(SessionProperties properties);
		
		void  connect(SessionProperties properties, bool sendNewOrderAfterConnect);
		
		void  disconnect();
		
		bool waitUntilDisconnected(long timeout);
		
		void  sendMessage(Message msg);
		
		Message accept(long timeout);
		
		void  clearMessagesReceived();
		
		/// <summary>for use in testing only, to enforce a certain message sequence </summary>
		void  readerThreadSleep(int waitTime);
		
		// @todo remove?
		void  removeMessages(System.String[] msgTypeArr);
	}
}