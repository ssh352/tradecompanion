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
	
	public class FieldInfo
	{
		virtual public System.String TagID
		{
			get
			{
				return tagID;
			}
			
		}
		virtual public System.String TagName
		{
			get
			{
				return tagName;
			}
			
		}
		virtual public System.String Name
		{
			// depreceted, included because was in cameron fix
			
			get
			{
				return tagName;
			}
			
		}
		virtual public System.String TagDataType
		{
			get
			{
				return tagDataType;
			}
			
		}
		virtual public System.String TagDescription
		{
			get
			{
				return tagDescription;
			}
			
		}
		virtual public System.Collections.IEnumerator TagValidValues
		{
			get
			{
				if (tagValidValues == null)
					return null;
				
				return tagValidValues.GetEnumerator();
			}
			
		}
		public System.String tagID; // tagID
		public System.String tagName; //
		public System.String tagDataType;
		public System.String tagDescription;
		public int constantGroupID;
		public System.Collections.ArrayList tagValidValues;
		
		public FieldInfo()
		{
		}
		
		public override System.String ToString()
		{
			return "FieldInfo: tagID=" + tagID + " tagName=" + tagName;
		}
	}
}