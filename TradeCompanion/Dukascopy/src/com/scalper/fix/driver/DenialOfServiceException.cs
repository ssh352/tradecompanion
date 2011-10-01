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
	
	/// <summary> represents an unrecoverable problem encountered while reading that prevents a message from being parsed.</summary>
	[Serializable]
	public class DenialOfServiceException:System.Exception
	{
		virtual public sbyte[] MsgRead
		{
			get
			{
				return msgRead;
			}
			
		}
		/// <summary> </summary>
		private const long serialVersionUID = 1L;
		private sbyte[] msgRead;
		
		public DenialOfServiceException(System.String error):this(error, new sbyte[0])
		{
		}
		
		public DenialOfServiceException(System.String error, sbyte[] msgRead):base(error)
		{
			this.msgRead = msgRead;
		}
	}
}