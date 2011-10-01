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
/// you entered shorto with S7 Software Solutions Pvt. Ltd.
/// 
/// File: Logging.cs
/// Created on Feb 10, 2004
/// ****************************************************
/// </summary>
using System;
namespace Icap.com.scalper.util
{

    //import Icap.com.scalper.fix.driver.*;

    /// <summary> <p>Description:</p>
    /// <p>Copyright: Copyright (c) 2005</p>
    /// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
    /// </summary>
    /// <author>  Phaniraj Raghavendra
    /// </author>
    /// <version>  1.0
    /// </version>


    public class Logging
    {
        /// <summary> 	Sets the mode of logging to the specified flag(true or false).</summary>
        public static bool DebugMode
        {
            set
            {
                //Logging.debug = flag;
            }

        }
        /// <summary>Setting this to <CODE>true</CODE> enables debug logging </summary>
        public static bool debug_Renamed_Field = true;

        public static void trace(System.String msg)
        {
            //System.Console.Out.WriteLine(msg);
        }
        /// <summary> Logs a message with the DEBUG level.</summary>
        /// <param name="message	the">message to log
        /// </param>
        public static void debug(System.String message)
        {
            //Logging.appLog.debug(message);
            trace(message);
        }

        /// <summary> Logs a message with the INFO level.</summary>
        /// <param name="message	the">message to log
        /// </param>
        public static void info(System.String message)
        {
            trace(message);
        }

        /// <summary> Logs an exception with the WARN level.
        /// 
        /// </summary>
        /// <param name="t"> the exception to log
        /// </param>
        public static void warn(System.Exception t)
        {
            //Logging.logException(Logging.appLog, Level.WARN, t);
            trace(t.Message);
        }

        /// <summary> Logs a message with the WARN level.</summary>
        /// <param name="message	the">message to log
        /// </param>
        public static void warn(System.String message)
        {
            //Logging.appLog.warn(message);
            trace(message);
        }

        /// <summary> Logs an exception with the ERROR level.
        /// 
        /// </summary>
        /// <param name="t"> the exception to log
        /// </param>
        public static void error(System.Exception t)
        {
            //Logging.logException(Logging.appLog, Level.ERROR, t);
            trace(t.Message);
        }

        /// <summary> Logs an exception with the ERROR level using a logger category.</summary>
        /// <param name="category	the">logger category to log to
        /// </param>
        /// <param name="t			the">exception to log
        /// </param>
        public static void error(System.Object category, System.Exception t)
        {
            //Logging.logException(category, Level.ERROR, t);
            trace(t.Message);
        }

        /// <summary> Logs a message with the ERROR level.</summary>
        /// <param name="message"> the message to log
        /// </param>
        public static void error(System.String message)
        {
            //Logging.appLog.error(message);
            trace(message);
        }

        /// <summary> Logs a message with the ERROR level using a logger category.
        /// 
        /// </summary>
        /// <param name="category	the">logger category to log to
        /// </param>
        /// <param name="message	the">message to log
        /// </param>
        public static void error(System.Object category, System.String message)
        {
            //category.error(message);
            trace(message);
        }

        /// <summary> Logs a message with the FATAL level.</summary>
        /// <param name="message"> the message to log
        /// </param>
        public static void fatal(System.String message)
        {
            //Logging.appLog.fatal(message);
            trace(message);
        }

        /// <summary> Logs a message with the FATAL level using a logger category.
        /// 
        /// </summary>
        /// <param name="category	the">logger category to log to
        /// </param>
        /// <param name="message	the">message to log
        /// </param>
        public static void fatal(System.Object category, System.String message)
        {
            //category.fatal(message);
            trace(message);
        }

        /// <summary> Turns on Log4J internal logging.</summary>
        public static void turnOnInternalLogging()
        {
            //LogLog.setQuietMode(false);
        }
    }
}