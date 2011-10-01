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
/// you entered shorto with S7 Software Solutions Pvt. Ltd..
/// 
/// File: S7FIXSession.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace Icap.com.scalper.fix
{
	
	/// <summary> a class that caches which tags are legal for each message type
	/// This object is immutable and may safely be shared across threads.
	/// </summary>
	public class TagHelper
	{
		// these arrays are indexed starting at 1.
		private bool[] headerArr;
		private bool[] footerArr;
		private int arrLength;
		
		/// <todo>  should not be publicly instanceable. </todo>
		public TagHelper(System.String fixVersion)
		{
            //System.Boolean tempAux = false;
            ////<-giri
            //FixDefinitions fixDefinitions = FixDefinitions.getFIXDefinition(fixVersion, ref tempAux);
            //int max = fixDefinitions.FIXMaxTag;
            //arrLength = max + 1;
            //headerArr = new bool[arrLength];
            //footerArr = new bool[arrLength];

            //System.Collections.Hashtable tableToCheck = fixDefinitions.validFIXTags;
            //SupportClass.HashSetSupport validHeaderFields = (SupportClass.HashSetSupport)tableToCheck["HEADER"];
            //System.Collections.IEnumerator it = validHeaderFields.GetEnumerator();
            //while (it.MoveNext())
            //{
            //    int num = System.Int32.Parse((System.String)it.Current);
            //    headerArr[num] = true;
            //}

            //SupportClass.HashSetSupport validFooterFields = (SupportClass.HashSetSupport)tableToCheck["FOOTER"];
            //System.Collections.IEnumerator it2 = validFooterFields.GetEnumerator();
            //while (it2.MoveNext())
            //{
            //    int num = System.Int32.Parse((System.String)it2.Current);
            //    footerArr[num] = true;
            //}
            //<-giri
		}
		
		public virtual bool isTagHeader(int tag)
		{
			if (tag >= arrLength)
				return false;
			return headerArr[tag];
		}
		
		public virtual bool isTagFooter(int tag)
		{
			if (tag >= arrLength)
				return false;
			return footerArr[tag];
		}
	}
}