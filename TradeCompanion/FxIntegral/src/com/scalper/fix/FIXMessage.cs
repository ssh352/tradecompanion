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
	public interface FIXMessage
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
		
		/// <summary> Get the values for the corresponding tag.  This is used for repeating groups.
		/// 
		/// </summary>
		/// <param name="tag">   FIX tag to retrieve
		/// 
		/// </param>
		/// <returns> all value for tag or null if the field does not exist
		/// </returns>
		System.String[] getValues(int tag);
		/// <summary> Set the value for the corresponding tag. If the field does
		/// not exist it should be added to the fix message
		/// 
		/// </summary>
		/// <param name="tag">   Tag to be set
		/// 
		/// </param>
		/// <param name="value"> tag value
		/// </param>
		void  setValue(int tag, System.String value_Renamed);
		void  setValue(int tag, char value_Renamed);
		
		/// <summary> add a new field.  This is useful for adding repreating groups
		/// 
		/// </summary>
		/// <param name="tag">   Tag to be added
		/// 
		/// </param>
		/// <param name="value"> new value
		/// </param>
		void  addValue(int tag, System.String value_Renamed);
		
		/// <summary> get the next group that starts with the start tags
		/// 
		/// </summary>
		/// <param name="tag">      first tag of the group being looked up
		/// </param>
		/// <param name="groupTags">list of tags in this group
		/// 
		/// </param>
		/// <returns>
		/// </returns>
		FIXGroup getGroup(int tag, int[] groupTags);
		FIXGroup getGroup(int tag, int[] groupTags, FIXGroup group);
		
		/// <summary> return a string representation of this Fix message
		/// 
		/// </summary>
		/// <param name="summary">If set only a summary of the message is needed to be returned, otherwise the
		/// entire fix message should be returned.
		/// 
		/// </param>
		/// <returns> fix message as a string
		/// </returns>
		System.String toString(bool summary);
	}
}