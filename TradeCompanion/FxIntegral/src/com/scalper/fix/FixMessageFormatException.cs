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
namespace FxIntegral.com.scalper.fix
{
	
	[Serializable]
	public class FixMessageFormatException:FixException
	{
		/// <summary> </summary>
		private const long serialVersionUID = 1L;
        internal static readonly System.String lclStr = FxIntegral.com.scalper.util.FFillFixGlobals.CopyRightStr;
		public FixMessageFormatException():base()
		{
		}
		
		public FixMessageFormatException(System.String s):base(s)
		{
		}
		
		public FixMessageFormatException(System.String s, int code):base(s, code)
		{
		}
		
		public FixMessageFormatException(System.String s, int code, int tag):base(s, code)
		{
			RefTagID = tag;
		}
	}
}