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
	
	
	/// <summary> Fix message interface. If a user defined FIX message is to be used
	/// with the fix add on the FIX Message needs to implement this interface
	/// </summary>
	public interface FIXGroup
	{
		/// <summary> Get the value for the corresponding tag
		/// 
		/// </summary>
		/// <param name="tag">   FIX tag to retrieve
		/// 
		/// </param>
		/// <returns> Value for tag or null if the field does not exist
		/// </returns>
		System.String getValue(int tag);
		
		
		void  addValue(int tag, System.String value_Renamed);
	}
}