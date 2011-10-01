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
/// File: FIXNoticeListener.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace com.scalper.fix.driver
{
	
	/// <summary> <p>Description: This interface represents notice listener and
	/// provides the way to handle notice.</p>
	/// <p>Copyright: Copyright (c) 2005<br>
	/// Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public interface FIXNoticeListener
	{
		/// <summary> This method will be called by the source of the notice.
		/// Source will form and passes the FIXNotice object with all the details
		/// that are needed to handle the notice
		/// </summary>
		/// <param name="notice">
		/// </param>
		void  takeNote(FIXNotice notice);
	}
}