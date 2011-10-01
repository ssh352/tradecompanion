/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2006                                                                                                                                            Software Inc.,
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
using log4net;
namespace FxIntegral.com.scalper.util
{

    /// <summary> Generic globals class for all S7 Software Solutions Pvt. Ltd. applications</summary>
    public class FFillFixGlobals
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FFillFixGlobals));
        public const System.String DefaultCopyright = "Copyright (c) 1999-2006 S7 Software Solutions Pvt. Ltd. Software Inc.";

        public const System.String ShortCopyright = "(c) 1999-2006 S7 Software Solutions Pvt. Ltd. Software Inc.";

        public static readonly System.String CopyRightStr = FFillFixGlobals.DefaultCopyright;

        public const System.String DisplayCopyRightStr = "/*******************************************************\n" + "*\n" + "* Copyright (c) 1999-2006 S7 Software Solutions Pvt. Ltd. Software Inc.,\n" + "* #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.\n" + "* All rights reserved.\n" + "*\n" + "* This software is the confidential and proprietary information\n" + "* of S7 Software Solutions Pvt. Ltd.(\"Confidential Information\").  You\n" + "* shall not disclose such Confidential Information and shall use\n" + "* it only in accordance with the terms of the license agreement\n" + "* you entered into with S7 Software Solutions Pvt. Ltd.\n" + "*\n" + "******************************************************/\n";

        public static readonly System.String[] shortMonthArray = new System.String[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        public static bool show_QueuesFullAlert = false;
        static FFillFixGlobals()
        {
            {
                //Copyright agreement displayed here.
                //System.Console.Error.WriteLine(FFillFixGlobals.DisplayCopyRightStr);
            }
        }
    }
}