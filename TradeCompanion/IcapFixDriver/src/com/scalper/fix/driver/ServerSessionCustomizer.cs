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
using Message = Icap.com.scalper.fix.Message;
namespace Icap.com.scalper.fix.driver
{
	
	public abstract class ServerSessionCustomizer:SessionCustomizer
	{
		
		/// <summary> controls whether a logon request is accepted or not.
		/// See ServerLogonInfo for more information.
		/// </summary>
		public abstract ServerLogonInfo processLogon(Message logonMsg);
		
		protected internal virtual void  modifyLogonResponse(Message logonMsg, Message logonResponseMsg)
		{
		}
	}
}