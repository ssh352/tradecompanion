/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd.("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
using System.Globalization;
namespace Icap.com.scalper.util
{

    /// <summary> Basic utilities functions.</summary>
    public class BasicUtilities
    {
        static CultureInfo culture = new CultureInfo("en-US");
        public static bool isTrue(System.String istrue)
        {
            if (istrue == null)
                return false;
            // equalsIgnoreCase does not exist in KVM.
            System.String toLower = istrue.ToLower();
            return ((toLower.Equals("on")) || (toLower.Equals("yes")) || (toLower.Equals("y")) || (toLower.Equals("true")));
        }

        public static bool contains(System.Object[] arr, System.Object val)
        {
            if (val == null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (val.Equals(arr[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool isNullOrEmpty(System.String s)
        {
            return s == null || "".Equals(s);
        }
        
        public static CultureInfo getCulture()
        {
             return culture;
        }
    }
}