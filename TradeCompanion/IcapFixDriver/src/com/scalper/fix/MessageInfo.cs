/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd.
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India
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
namespace Icap.com.scalper.fix
{
	
	public class MessageInfo
	{
		virtual public System.String Detail
		{
			get
			{
				return msgDetail;
			}
			
		}
		virtual public System.String Summary
		{
			get
			{
				return msgSummary;
			}
			
		}
		internal System.String msgDetail = null, msgSummary = null;
		public MessageInfo(System.String detail, System.String summary)
		{
			msgDetail = detail;
			msgSummary = summary;
		}
	}
}