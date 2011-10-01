



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
using System.Text.RegularExpressions;
using log4net;
namespace FxIntegral.com.scalper.fix
{

    public class BasicFixUtilities
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BasicFixUtilities));
        /// <summary>a pattern that matches "FIX.4.x" or "4.x" where x is a number.</summary>
        private static readonly String PATTERN_4x = "([^4]*[.])?[4][.](?<minorVersion>[0-9])*";

        internal static readonly System.String lclStr = FxIntegral.com.scalper.util.FFillFixGlobals.DisplayCopyRightStr;

        internal static bool isIgnoreTag(System.String inTagID, System.Collections.Hashtable ignoreTags)
        {
            try
            {
                if (ignoreTags == null)
                    return false;

                bool rc = ignoreTags.ContainsKey(inTagID);
                if (!rc)
                {
                    rc = ignoreTags.ContainsKey(System.Int32.Parse(inTagID));
                }

                return rc;
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }
            return false;
        }

        //("unchecked")
        public static System.String areEqual(Message step_msg, Message in_msg, System.Collections.Hashtable skipTags, bool checkIgnoreValue, bool checkSize, System.Collections.Hashtable clientOrderIDMap, System.Collections.Hashtable listIDMap, System.Collections.Hashtable absentTags, System.Collections.ArrayList vErrors)
        {
            //ErrorLog.println("areEqual -"+step_msg+", "+in_msg+", "+skipTags+", "+checkIgnoreValue+", "+checkSize+", "+clientOrderIDMap+", "+absentTags+", "+vErrors+")");
            try
            {
                for (int i = 0; i < step_msg.size(); i++)
                {
                    //Field stepField = stepFields.fieldAt(i);
                    System.String stepValue = step_msg.valueAt(i);
                    System.String stepTag = step_msg.tagAt(i);

                    if (!BasicFixUtilities.isIgnoreTag(stepTag, skipTags))
                    {
                        //System.err.println("stepField="+stepField+" in_msg="+in_msg.toString(false)+" stepTag="+stepTag);
                        System.String in_value = in_msg.getValue(stepTag);
                        if (clientOrderIDMap != null)
                        {
                            if ((stepTag.Equals(FxIntegral.com.scalper.fix.Constants_Fields.TAGClOrdID)) || (stepTag.Equals(FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigClOrdID)))
                            {
                                System.String clOrdId = (System.String)clientOrderIDMap[stepValue];
                                if (clOrdId != null)
                                {
                                    stepValue = clOrdId;
                                }
                            }
                        }

                        if (listIDMap != null)
                        {
                            if (stepTag.Equals(FxIntegral.com.scalper.fix.Constants_Fields.TAGListID))
                            {
                                System.String listID = (System.String)listIDMap[stepValue];
                                if (listID != null)
                                {
                                    stepValue = listID;
                                }
                            }
                        }


                        if (((stepValue != null) && stepValue.Equals("!")))
                        {
                            if (in_value != null)
                            {
                                if (vErrors == null)
                                {
                                    return stepTag;
                                }
                                else
                                {
                                    vErrors.Add("Tag[" + stepTag + "]  Expected Nothing  Received[" + in_value + "]");
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if ((stepValue != null) && (stepValue.Equals("?")))
                        {
                            continue;
                        }
                        if (((stepValue != null) && stepValue.Equals("*") && in_value == null) || ((stepValue != null) && !stepValue.Equals("*") && ((in_value == null) || ((in_value != null) && !in_value.Equals(stepValue)))))
                        {
                            if (vErrors == null)
                            {
                                return "See Tag " + stepTag;
                            }
                            else
                            {
                                System.String recvStr;
                                if (in_value == null)
                                {
                                    recvStr = "Received Nothing";
                                }
                                else
                                {
                                    recvStr = "Received[" + in_value + "]";
                                }
                                vErrors.Add("Tag[" + stepTag + "]  Expected[" + stepValue + "]  " + recvStr);
                            }
                        }
                    }
                } // End of for loop.

                if (checkSize)
                {
                    if (step_msg.size() != in_msg.size())
                    {
                        Message fieldsToCheck;
                        Message oppMsg;
                        System.String errorType;
                        if (step_msg.size() > in_msg.size())
                        {
                            fieldsToCheck = step_msg;
                            oppMsg = in_msg;
                            errorType = "Missing";
                        }
                        else
                        {
                            fieldsToCheck = in_msg;
                            oppMsg = step_msg;
                            errorType = "Extra";
                        }

                        for (int i = 0; i < fieldsToCheck.size(); i++)
                        {
                            System.String tag = fieldsToCheck.tagAt(i);
                            //String value = fieldsToCheck.valueAt();
                            if (oppMsg.getValue(tag) == null)
                            {
                                if (vErrors == null)
                                {
                                    return errorType + " Tag " + tag;
                                }
                                else
                                {
                                    vErrors.Add(errorType + " Tag " + tag);
                                }
                            }
                        }

                        //System.err.println("are equal is false 1 stepFields.size()="+stepFields.size()+" inFields.size()="+inFields.size());
                        return "Size Mismatch";
                    }
                }

                if (checkIgnoreValue)
                {
                    System.Collections.IEnumerator e = skipTags.Values.GetEnumerator();
                    while (e.MoveNext())
                    {
                        System.String nextTag = (System.String)e.Current;
                        System.String step_value = step_msg.getValue(nextTag);
                        System.String expectedStr;
                        System.String receivedStr;
                        System.String in_value = in_msg.getValue(nextTag);
                        if (((in_value != null) && (step_value == null)) || ((in_value == null) && (step_value != null)))
                        {
                            //System.err.println("are equal is false 3 stepTag="+nextTag+" in_value="+in_value+" step_value="+step_value);
                            if (vErrors == null)
                            {
                                return "See Tag " + nextTag;
                            }
                            else
                            {
                                expectedStr = (step_value == null ? "Expected Nothing" : "Expected[" + step_value + "]");
                                receivedStr = (in_value == null ? "Received Nothing" : "Received[" + in_value + "]");
                                vErrors.Add("Tag[" + nextTag + "]  " + expectedStr + "  " + receivedStr);
                            }
                        }
                    }
                } // End of checkIgnoreValue if statement.

                if (absentTags != null)
                {
                    System.Collections.IEnumerator e = absentTags.Values.GetEnumerator();
                    while (e.MoveNext())
                    {
                        System.String nextTag = (System.String)e.Current;
                        System.String in_value = in_msg.getValue(nextTag);
                        if (in_value != null)
                        {
                            if (vErrors == null)
                            {
                                return nextTag;
                            }
                            else
                            {
                                vErrors.Add(nextTag);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                //System.err.println("are equal is false 4");
                return "Mismatch";
            }

            return null;
        }

        [STAThread]
        public static void Main(System.String[] args)
        {
            System.String lVer;
            char c;
            lVer = "FIX.4.1";
            c = BasicFixUtilities.getFixMinorVersion(lVer);
            lVer = "TEST.4.4.8";
            c = BasicFixUtilities.getFixMinorVersion(lVer);
        }

        //("unchecked")
        public static FieldInfo getFieldInfo(System.String tagID)
        {
            System.Collections.Hashtable fixFields = FixDefinitions.getFIXDefinition().fixFields;
            FieldInfo fieldInfo = (FieldInfo)fixFields[tagID];
            if (fieldInfo == null)
            {
                fieldInfo = new FieldInfo();
                fieldInfo.tagID = tagID; // tagID
                fieldInfo.tagName = tagID; //
                fixFields[fieldInfo.tagID] = fieldInfo;
            }

            return fieldInfo;
        }

        /// <summary> Whether this message is an admin message.  Admin messages are:
        /// <UL>
        /// <LI>Logon</LI>
        /// <LI>Logout</LI>
        /// <LI>Resend Request</LI>
        /// <LI>Heartbeat</LI>
        /// <LI>Test Request</LI>
        /// <LI>Sequence Reset</LI>
        /// <LI>Reject</LI>
        /// </UL>
        /// 
        /// </summary>
        /// <param name="msgType">The message type
        /// </param>
        /// <returns> Whether that message type is admin or not
        /// </returns>
        public static bool isAdmin(System.String msgType)
        {
            try
            {
                bool isAdminMsg = (msgType != null);
                isAdminMsg = isAdminMsg && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogon) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogout) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGResendRequest) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGHeartbeat) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGTestRequest) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGSequenceReset) || msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGReject));
                return isAdminMsg;
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return false;
            }
        }

        public static bool isReject(Message msg)
        {
            try
            {
                System.String msgType = msg.getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType);
                return ((msgType != null) && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGReject)));
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return false;
            }
        }

        public static bool isResend(Message msg)
        {
            try
            {
                System.String msgType = msg.getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType);
                return ((msgType != null) && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGResendRequest)));
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return false;
            }
        }

        // @todo remove this method
        public static bool isAdmin(Message msg)
        {
            return msg.Admin;
        }

        public static bool isProtocolMsg(Message msg)
        {
            try
            {
                System.String msgType = msg.getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType);
                return BasicFixUtilities.isProtocolMsg(msgType);
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return (false);
            }
        }


        public static bool isProtocolMsg(System.String msgType)
        {
            return ((msgType != null) && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGHeartbeat) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGTestRequest)) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGResendRequest)) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogout)) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogon)) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGReject)) || (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGSequenceReset))));
        }

        public static bool isLogon(Message msg)
        {
            try
            {
                System.String msgType = msg.getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType);
                return BasicFixUtilities.isLogon(msgType);
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return (false);
            }
        }

        public static bool isLogon(System.String msgType)
        {
            try
            {
                return ((msgType != null) && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogon)));
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return (false);
            }
        }

        public static bool isLogout(System.String msgType)
        {
            try
            {
                return ((msgType != null) && (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogout)));
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return (false);
            }
        }

        /// <summary> takes a string of the form "FIX.4.x" or "4.x" and returns the character that represents
        /// the minor version.  Returns the null character (ASCII 0x0000) if the string is null
        /// or is not long enough.
        /// </summary>
        /// <param name="beginString">a String of the form "FIX.4.x" or "4.x"
        /// </param>
        /// <returns> a value '0'-'9' for FIX versions 0-9, or ASCII null (0x0000) for an illegal string.
        /// </returns>
        public static char getFixMinorVersion(System.String beginString)
        {
            //Matcher m = BasicFixUtilities.PATTERN_4x.matcher(beginString); 
            //ATTERN_4x = "([^4]*[.])?[4][.]([0-9])*";
            System.String version = null; 
            Regex fix = new Regex(BasicFixUtilities.PATTERN_4x);
            ///////
             bool isMatch = fix.IsMatch(beginString);
             if (isMatch)
             {
                 try
                 {
                     MatchCollection mc = fix.Matches(beginString);
                     Match MyMatch = mc[0];
                     version = MyMatch.Groups["minorVersion"].ToString();
                     //version = mc[0].Groups["minorVersion"];
                     //CaptureCollection cc = gc[0].Captures;
                     //try
                     //{
                     //    version = cc[1].Value;
                     //}
                 }
                 catch (System.Exception e)
                 {
                     log.Error(e);
                 }

             }   
            return version[0];
        }
    }
}