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
/// File: S7FIXSession.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	public interface DisconnectListener
	{
		void  sessionDisconnected(DisconnectInfo info);
	}
}