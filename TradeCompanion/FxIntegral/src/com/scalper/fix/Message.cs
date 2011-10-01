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
using log4net;
using FxIntegral.com.scalper.util;
namespace FxIntegral.com.scalper.fix
{

    /// <summary> Representation of a FIX message containing name-value pairs.
    /// Most of the public methods in this class are getters and setters.
    /// 
    /// <p>For standard FIX tags use FxIntegral.com.scalper.fix.Constants.
    /// <p>For additional S7 Software Solutions Pvt. Ltd. FIX tags, use FxIntegral.com.scalper.fix.FixConstants.
    /// </summary>
    public class Message : FIXMessage, System.ICloneable
    {
        /// <summary> Get the msgType (Fix Tag 35) of this msg class
        /// 
        /// </summary>
        /// <returns> msgtype
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private static readonly ILog log = LogManager.GetLogger(typeof(Message));

        virtual public System.String MsgType
        {
            get
            {
                return getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i);
            }

        }
        /// <summary> Is this message a fix admin message.
        /// 
        /// </summary>
        /// <returns> true if it is an admin
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public bool Admin
        {
            get
            {
                System.String msgType = MsgType;
                if (msgType == null )//|| msgType.Length != 1) pwreset
                    return false;
                char msgTypeChar = msgType[0];
                return ((msgTypeChar >= '0' && msgTypeChar <= '5') || msgTypeChar == 'A' || msgType == "BF");
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public System.Collections.ArrayList Fields
        {
            //("unchecked")

            get
            {
                lock (this)
                {
                    // Return a copy of all fields for this message
                    //
                    // @return  A vector of String arrays.  The first element in the string array
                    // will be the tag and the second element will be the value.

                    System.Collections.ArrayList retFields = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                    for (int i = 0; i < elementCount; i++)
                    {
                        System.String[] field = new System.String[2];
                        field[Message.TAG] = tagIDs_s[i];
                        field[Message.VALUE] = values[i];
                        retFields.Add(field);
                    }

                    return retFields;
                }
            }

        }
        virtual public System.String Summary
        {
            get
            {
                System.String msgType = getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i);
                return MsgSeqNum + "," + Message.getDisplaySummery(msgType);
            }

        }
        /// <summary> Return a deep copy of this message</summary>
        virtual public Message Copy
        {
            get
            {
                Message newMsg = new Message();
                copyInto(newMsg);

                return newMsg;
            }

        }
        /// <summary>     Returns the message's sequence number.</summary>
        /// <returns> Message sequence number. -1 if there is no sequence number or it is bad.
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        /// <summary>     Sets the message's sequence number.</summary>
        /// <param name="Message">sequence number.
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public int MsgSeqNum
        {
            get
            {
                // default is -1
                return getIntValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum_i);
            }

            set
            {
                setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum, value);
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public bool Valid
        {
            get
            {
                return isValid_Renamed_Field;
            }

            set
            {
                isValid_Renamed_Field = value;
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public System.String ErrorString
        {
            get
            {
                return sError;
            }

            set
            {
                sError = value;
                isValid_Renamed_Field = (sError == null) || (sError.Length <= 0);
            }

        }
        /// <summary> Sets the BeginString of the FIX message with the specified value.</summary>
        /// <param name="beginString">- The string to be set as the BeginString.
        /// </param>
        /// <throws>  FixException - if the string passed is not a valid value for BeginString. </throws>
        /// <summary> 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public System.String BeginStringField
        {
            set
            {
                if ((value.Equals(FxIntegral.com.scalper.fix.Constants_Fields.FIX_VERSION_40)) || (value.Equals(FxIntegral.com.scalper.fix.Constants_Fields.FIX_VERSION_41)) || (value.Equals(FxIntegral.com.scalper.fix.Constants_Fields.FIX_VERSION_42)) || (value.Equals(FxIntegral.com.scalper.fix.Constants_Fields.FIX_VERSION_43)) || (value.Equals(FxIntegral.com.scalper.fix.Constants_Fields.FIX_VERSION_44)))
                    setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString_i, value);
                else
                    throw new FixException("Incorrect BeginString value: " + value);
            }

        }
        /// <summary> Returns the content of the FIX message as byte stream.</summary>
        /// <returns> ByteBuffer
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public FxIntegral.com.scalper.util.ByteBuffer ByteBuffer
        {
            get
            {
                FIXByteBuffer buf = new FIXByteBuffer(400);
                buf.append(this);

                return buf.ByteBuffer;
            }

        }
        /// <summary> returns the message type, or '\01' if it has not been set yet.
        /// 
        /// </summary>
        /// <returns> the message type or '\01'
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public char MsgTypeChar
        {
            get
            {
                return getCharValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i);
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public int MessageSeq
        {
            get
            {
                return this.MsgSeqNum;
            }

        }
        internal System.Collections.ArrayList groups = null;

        // An array of indexes created by setGroupIndexes(), used by message batching methods
        internal int[] batchIndexes;
        // index to indexes
        internal int batchIndex;
        // true if batchIndex is initialized and in range.
        internal bool inBatch;
        // holds index to the portion of the message after the repeating blocks
        internal int endOfBatchBlocks;

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public const int TAG = 0;
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public const int VALUE = 1;
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        /// <summary> Used by APS to avoid constructing send-buffers
        /// for multiple identical prices.
        /// If cachedAPSSendBuffer is non-null, 'toString' will
        /// always return its value rather than recalculating
        /// </summary>
        public System.String cachedAPSSendBuffer = null;
        /// <summary> Compute non-summary (e.g. transmit) string buffer, and
        /// cause subsequent calls to toString to return that
        /// value without recalculating
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void freezeToString()
        {
            // nulling cachedAPSSendBuffer forces 'toString' to recalculate
            cachedAPSSendBuffer = null;
            // forces 'toString' to always return cachedAPSSendBuffer
            cachedAPSSendBuffer = toString(false);
        }


        /// <summary>an optimization, holds String version of each character from ASCII 0 through ASCII 255 </summary>
        private const int static_values_length = 255;
        private static readonly System.String[] static_values = new System.String[Message.static_values_length];

        private const int CHECKSUM_MAX = 255;
        private static readonly System.String[] checksumStringArr = new System.String[Message.CHECKSUM_MAX + 1];

        private const int INTEGER_ARRAY_LENGTH = 1024;
        private static readonly System.String[] intArr = new System.String[Message.INTEGER_ARRAY_LENGTH];

        internal const int defaultIntialCapacity = 30;
        internal const int CAPACITY_MULTIPLIER = 4;
        internal int initialCapacity = Message.defaultIntialCapacity;
        public static System.String fixFieldSeparator = "" + (char)1;
        public static char fixCharSeparator = (char)1;
        //MessageFields fields = null;

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String[] values = null;
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String[] tagIDs_s = null;
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int[] tagIDs_i = null;
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int elementCount = 0;

        private static readonly String dateFormat = "yyyyMMdd";
        private static readonly String timeFormat = "yyyyMMdd-HH:mm:ss.fff";
        private static readonly string timeFormatMillis = "yyyyMMdd-HH:mm:ss.fff";
        private bool isValid_Renamed_Field = true;
        private System.String sError = null;
        internal Message privateMsg = null;

        /// <summary> Construct an empty message</summary>
        public Message()
        {
            init();
        }

        /// <summary> This constructer will set the msgType after initializing the message
        /// 
        /// </summary>
        /// <param name="msgType">The initail MsgType
        /// </param>
        public Message(System.String msgType)
        {
            init();
            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType, FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i, msgType);
        }

        /// <summary> Use this constructer to set the initial expectation for the number of tag-value pairs in this message.
        /// 
        /// </summary>
        /// <param name="initialCapacity">The initail capacity of the internal arrays used to store this messages data.
        /// </param>
        public Message(int initialCapacity)
        {
            init();
            this.initialCapacity = initialCapacity;
        }

        // diagnostic thread
        internal static long ctt = 0;
        internal static SupportClass.ThreadClass tt = null;
        internal void init()
        {
            values = new System.String[initialCapacity];
            tagIDs_s = new System.String[initialCapacity];
            tagIDs_i = new int[initialCapacity];
        }

        public override bool Equals(System.Object msg)
        {
            if (!(msg is Message))
                return false;

            Message inMsg = (Message)msg;

            if (inMsg == null)
                return false;

            return toString(false).Equals(inMsg.toString(false));
        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public override int GetHashCode()
        {
            return toString(false).GetHashCode();
        }

        /// <summary> return the internal index of the given tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns> The tag represented as a String
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int indexOf(System.String tag)
        {
            lock (this)
            {
                int fldID;
                try
                {
                    fldID = System.Int32.Parse((System.String)tag);
                }
                catch //(System.Exception e)
                {
                    return -1;
                }
                for (int i = 0; i < elementCount; i++)
                {
                    if (tagIDs_i[i] == fldID)
                        return i;
                }
                return -1;
            }
        }

        /// <summary> return the internal index of the given tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns> The tag represented as a int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int indexOf(int tag)
        {
            lock (this)
            {
                for (int i = 0; i < elementCount; i++)
                {
                    if (tagIDs_i[i] == tag)
                        return i;
                }
                return -1;
            }
        }

        /// <summary> return the internal index of the given tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="startIndex">  index to start the search at
        /// 
        /// </param>
        /// <returns> The tag represented as a String
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int indexOf(System.String tag, int startIndex)
        {
            lock (this)
            {
                int fldID;
                try
                {
                    fldID = System.Int32.Parse((System.String)tag);
                }
                catch //(System.Exception e)
                {
                    return -1;
                }

                for (int i = startIndex; i < elementCount; i++)
                {
                    if (tagIDs_i[i] == fldID)
                        return i;
                }
                return -1;
            }
        }

        /// <summary> return the internal index of the given tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="startIndex">  index to start the search at
        /// 
        /// </param>
        /// <returns> The tag represented as a int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int indexOf(int tag, int startIndex)
        {
            lock (this)
            {
                for (int i = startIndex; i < elementCount; i++)
                {
                    if (tagIDs_i[i] == tag)
                        return i;
                }
                return -1;
            }
        }

        /// <summary> return the internal index of the given tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="startIndex">  first element to be searched
        /// </param>
        /// <param name="endIndex	terminating">element of search (noninclusive)
        /// 
        /// </param>
        /// <returns> The tag represented as a int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int indexOf(int tag, int startIndex, int endIndex)
        {
            lock (this)
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (tagIDs_i[i] == tag)
                        return i;
                }
                return -1;
            }
        }


        /// <summary> Get all values for the given tag.  This function will be used for repeating groups.
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all values for the given tag
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String[] getValues(System.String tag)
        {
            lock (this)
            {
                int fldID;
                try
                {
                    fldID = System.Int32.Parse((System.String)tag);
                }
                catch //(System.Exception e)
                {
                    return null;
                }

                return getValues(fldID);
            }
        }

        /// <summary> Get all values for the given tag.  specefied between the start and end index
        /// This function will be used for repeating groups.
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all values for the given tag
        /// </returns>
        //("unchecked")
        private System.Collections.ArrayList getValues(int tag, int startIndex, int endIndex, System.Collections.ArrayList tmpValues)
        {
            while (startIndex >= 0)
            {
                int idx = indexOf(tag, startIndex);

                if (idx < 0 || (endIndex >= 0 && idx > endIndex))
                {
                    //startIndex = -1;
                    break;
                }

                if (tmpValues == null)
                    tmpValues = new System.Collections.ArrayList();

                startIndex = idx + 1;
                tmpValues.Add(values[idx]);
            }

            return tmpValues;
        }

        /// <summary> Get all values for the given tag.  This function will be used for repeating groups.
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all values for the given tag
        /// </returns>
        //("unchecked")
        public System.String[] getValues(int tag)
        {
            lock (this)
            {
                System.Collections.ArrayList tmpValues = null;

                // if the batchIndexes have been initialized
                if (batchIndexes != null)
                {
                    // this message was batched
                    if (batchIndex < (batchIndexes.Length - 1))
                    {
                        // if we are not at the last block of the batch
                        tmpValues = getValues(tag, batchIndexes[batchIndex], batchIndexes[batchIndex + 1], tmpValues);
                    }
                    else
                    {
                        tmpValues = getValues(tag, batchIndexes[batchIndex], endOfBatchBlocks, tmpValues);
                    }
                    // search header
                    tmpValues = getValues(tag, 0, batchIndexes[0], tmpValues);

                    // search trailer
                    tmpValues = getValues(tag, endOfBatchBlocks, -1, tmpValues);
                }
                else
                {
                    // this message was not batched so serach the entire message
                    tmpValues = getValues(tag, 0, -1, tmpValues);
                }

                if (tmpValues == null || tmpValues.Count == 0)
                    return null;

                System.String[] rc = new System.String[tmpValues.Count];
                SupportClass.ICollectionSupport.ToArray(tmpValues, rc);
                return rc;
            }
        }

        /// <summary> Get all internal indexes for the given tag.  This function will be used for repeating groups.
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all indexes for the given tag
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int[] getValueIndexes(System.String tag)
        {
            lock (this)
            {
                int fldID;

                try
                {
                    fldID = System.Int32.Parse((System.String)tag);
                }
                catch //(System.Exception e)
                {
                    return null;
                }

                return getValueIndexes(fldID);
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int[] getValueIndexes(int tag)
        {
            lock (this)
            {
                int indexCount = 0;
                int[] tmpIndexes = null;
                int startIndex = 0;

                while (startIndex >= 0)
                {
                    int idx = indexOf(tag, startIndex);

                    if (idx < 0)
                    {
                        //startIndex = -1;
                        break;
                    }

                    if (tmpIndexes == null)
                        tmpIndexes = new int[elementCount];

                    startIndex = idx + 1;
                    tmpIndexes[indexCount] = idx;
                    indexCount++;
                }

                if (tmpIndexes == null || indexCount == 0)
                    return null;

                int[] rc = new int[indexCount];
                Array.Copy(tmpIndexes, 0, rc, 0, indexCount);
                return rc;
            }
        }

        /// <summary>  Get the value for the specified tag</summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String getValue(System.String tag)
        {
            lock (this)
            {
                int fldID;
                try
                {
                    fldID = System.Int32.Parse((System.String)tag);
                }
                catch //(System.Exception e)
                {
                    return null;
                }

                return getValue(fldID);
            }
        }

        /// <summary>  Get the value for the specified tag starting at startIndex</summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="startIndex">  starting index for tag search
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String getValue(System.String tag, int startIndex)
        {
            lock (this)
            {
                if (startIndex < 0 || startIndex > elementCount)
                {
                    return null;
                }

                int idx = indexOf(tag, startIndex);

                return (idx < 0) ? null : values[idx];
            }
        }

        /// <summary>  Get the value for the specified tag starting at startIndex</summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="startIndex">  starting index for tag search
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String getValue(int tag, int startIndex)
        {
            lock (this)
            {
                if (startIndex < 0 || startIndex > elementCount)
                {
                    return null;
                }

                int idx = indexOf(tag, startIndex);

                return (idx < 0) ? null : values[idx];
            }
        }


        /// <summary>  Get the value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// </returns>
        public System.String getValue(int tag)
        {
            lock (this)
            {
                int idx = -1;

                // if the batchIndexes have been initialized
                if (batchIndexes != null)
                {
                    if (batchIndex < (batchIndexes.Length - 1))
                    {
                        // if we are not at the last block of the batch
                        idx = indexOf(tag, batchIndexes[batchIndex], batchIndexes[batchIndex + 1]);
                    }
                    else
                    {
                        idx = indexOf(tag, batchIndexes[batchIndex], endOfBatchBlocks);
                    }
                }

                /*
                * if there are no initialized indexes or the search
                * of the batch block failed:
                * 	Search the entire message if there are no batch indexes
                *
                *	Search the "header" & "trailer" of the message if there are batch indexes
                *
                */

                if (idx < 0)
                {
                    if (batchIndexes == null)
                        idx = indexOf(tag);
                    else
                    {
                        // search header
                        idx = indexOf(tag, 0, batchIndexes[0]);

                        if (idx < 0 && endOfBatchBlocks >= 0)
                        // if endOfBatchBlocks is -1 we could not determine the trailer
                        {
                            // search trailer
                            idx = indexOf(tag, endOfBatchBlocks);
                        }
                    }
                }

                return (idx < 0) ? null : values[idx];
            }
        }

        /// <summary>  Get the value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="defaultValue">  value to return if the value does not exist
        /// 
        /// </param>
        /// <returns>  the value or defaultValue if it does not exists
        /// </returns>
        public System.String getValue(int tag, System.String defaultValue)
        {
            lock (this)
            {
                System.String value_Renamed = getValue(tag);

                return (value_Renamed == null) ? defaultValue : value_Renamed;
            }
        }

        /// <summary> Parse a String representation of a Date in standard GMT fix format into a Date object
        /// 
        /// </summary>
        /// <param name="tag">  String representation of the date
        /// 
        /// </param>
        /// <returns>  the Date or null if the string is not in the right format
        /// </returns>
        public static System.DateTime parseDateValue(System.String value_Renamed)
        {
            System.DateTime formattedDate = new DateTime();
            try
            {
               // IFormatProvider IFormat = new IFormatProvider();
               
                 System.DateTime.TryParse(value_Renamed, out formattedDate);
                 return formattedDate;
                //return System.DateTime.Parse((value_Renamed, Message.timeFormat);
            }
            catch (System.Exception e)
            {
                 e = new Exception("Error in Formatting date.");
                 throw e;
            }
        }

        
        /// <summary>  Get the int value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not an int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int getIntValue(System.String tag)
        {
            return getIntValue(tag, -1);
        }

        /// <summary>  Get the int value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="defaultReturnCode">if tag does not exist this value will be returned
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not an int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int getIntValue(System.String tag, int defaultReturnCode)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null && value_Renamed.Length > 0)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    return defaultReturnCode;
                }
            }
            else
                return defaultReturnCode;
        }

        /// <summary>  Get the Integer value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists or it is not an int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.Int32 getIntegerValue(int tag)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                }
            }

            return 0;
        }

        /// <summary>  Get the Integer value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists or it is not an int
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.Int32 getIntegerValue(System.String tag)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                }
            }

            return 0;
        }


        /// <summary>  Get the Double value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists or it is not a double
        /// </returns>
        public System.Double getDoubleValue(int tag)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null)
            {
                try
                {
                    return System.Double.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                }
            }

            return 0;
        }


        /// <summary>  Get the Double value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists or it is not a double
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.Double getDoubleValue(System.String tag)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null)
            {
                try
                {
                    return System.Double.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                }
            }

            return 0;
        }

        /// <summary>  Get the Float value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists or it is not a float
        /// </returns>
        public System.Single getFloatValue(int tag)
        {
            System.String value_Renamed = getValue(tag);

            if (value_Renamed != null)
            {
                try
                {
                    return System.Single.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                }
            }

           return 0;
        }

        /// <summary>  Get the int value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not an int
        /// </returns>
        public int getIntValue(int tag)
        {
            return getIntValue(tag, -1);          
        }
        /// <summary>  Get the int value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not a int
        /// </returns>
        public int getIntValue(int tag, int defaultReturnCode)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null && value_Renamed.Length > 0)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    return defaultReturnCode;
                }
            }
            else
                return defaultReturnCode;
        }

        /// <summary>  Get the long value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not an long
        /// </returns>
        public long getLongValue(int tag)
        {
            return getLongValue(tag, -1);
        }
        /// <summary>  Get the long value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not a long
        /// </returns>
        public long getLongValue(int tag, long defaultReturnCode)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null && value_Renamed.Length > 0)
            {
                try
                {
                    return System.Int64.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    return defaultReturnCode;
                }
            }
            else
                return defaultReturnCode;
        }

        /// <summary>  Get the char value for the specified tag. If the values is a string,
        /// then the first character in the String will be returned
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or (char)-1 if it does not exists or it is not a char
        /// </returns>
        public char getCharValue(int tag)
        {
            return getCharValue(tag, (char)SupportClass.Identity(-1));
        }

        /// <summary>  Get the char value for the specified tag. If the values is a string,
        /// then the first character in the String will be returned
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or (char)-1 if it does not exists or it is not a char
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public char getCharValue(System.String tag)
        {
            return getCharValue(tag, (char)SupportClass.Identity(-1));
        }

        /// <summary>  Get the char value for the specified tag. If the values is a string,
        /// then the first character in the String will be returned
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="defaultValue">  this is returned if the tag does not exist
        /// 
        /// </param>
        /// <returns>  the value or defaultValue if it does not exists or it is not a char
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public char getCharValue(System.String tag, char defaultValue)
        {
            System.String value_Renamed = getValue(tag);
            try
            {
                if (value_Renamed != null)
                {
                    if (value_Renamed.Length > 1)
                        return defaultValue;
                    try
                    {
                        return value_Renamed[0];
                    }
                    catch //(System.Exception e)
                    {
                        return (char)defaultValue;
                    }
                }
                else
                    return (char)defaultValue;
            }
            catch //(System.Exception e)
            {
                return (char)defaultValue;
            }
        }

        /// <summary>  Get the char value for the specified tag. If the values is a string,
        /// then the first character in the String will be returned
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <param name="defaultValue">  this is returned if the tag does not exist
        /// 
        /// </param>
        /// <returns>  the value or defaultValue if it does not exists or it is not a char
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public char getCharValue(int tag, char defaultValue)
        {
            System.String value_Renamed = getValue(tag);
            try
            {
                if (value_Renamed != null)
                {
                    if (value_Renamed.Length > 1)
                        return defaultValue;
                    try
                    {
                        return value_Renamed[0];
                    }
                    catch //(System.Exception e)
                    {
                        return (char)defaultValue;
                    }
                }
                else
                    return (char)defaultValue;
            }
            catch //(System.Exception e)
            {
                return (char)defaultValue;
            }
        }

        /// <summary>  Get the boolean value for the specified tag. If the values is not a boolean
        /// then false will be returned
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or false if it does not exists or it is not a char
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public bool getBoolValue(System.String tag)
        {
            return isTrue(getValue(tag));
        }

        /// <summary>  Get the boolean value for the specified tag. If the values is not a boolean
        /// then false will be returned
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or false if it does not exists or it is not a char
        /// </returns>
        public bool getBoolValue(int tag)
        {
            return isTrue(getValue(tag));
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, ref System.DateTime date, bool isDate)
        {
            setDateValue(tag, ref date, isDate, null);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
           setValue(tag, ref date, isDate, timeZone);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis)
        {
            setDateValue(tag, ref date, isDate, useMillis, null);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis, System.TimeZone timeZone)
        {
            setValue(tag, ref date, isDate, useMillis, timeZone);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, ref System.DateTime date, bool isDate)
        {
            addDateValue(tag, ref date, isDate, null);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
            addValue(tag, ref date, isDate, timeZone);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis)
        {
            addDateValue(tag, ref date, isDate, useMillis, null);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis, System.TimeZone timeZone)
        {
            addValue(tag, ref date, isDate, useMillis, timeZone);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public bool compareInt(int tag, Message msg)
        {
            int val1 = this.getIntValue(tag, -1);
            int val2 = msg.getIntValue(tag, -1);
            return (val1 == val2);
        }

        /// <summary> Remove the value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag removing
        /// 
        /// </param>
        /// <returns>  the value removed or null if it does not exists
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String removeTag(System.String tag)
        {
            lock (this)
            {
                int index = indexOf(tag);
                if ((index < 0) || (index >= elementCount))
                    return null;
               
                return removeTagByIndex(index);
            }
        }

        /// <summary> Remove all values for the given tag, for repeating groups.
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all values for the given tag
        /// </returns>
        //("unchecked")
        public System.String[] removeTags(int tag)
        {
            lock (this)
            {
                System.Collections.ArrayList tmpValues = null;
                int startIndex = 0;

                while (startIndex >= 0)
                {
                    int idx = indexOf(tag, startIndex);

                    if (idx < 0)
                    {
                        //startIndex = -1;
                        break;
                    }

                    if (tmpValues == null)
                        tmpValues = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                    startIndex = idx;
                    tmpValues.Add(removeTagByIndex(idx));
                }

                if (tmpValues == null || tmpValues.Count == 0)
                    return null;

                System.String[] rc = new System.String[tmpValues.Count];
                tmpValues.CopyTo(rc);
                return rc;
            }
        }

        /// <summary> Remove the value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag removing
        /// 
        /// </param>
        /// <returns>  the value removed or null if it does not exists
        /// </returns>
        public System.String removeTag(int tag)
        {
            lock (this)
            {
                int index = indexOf(tag);
                if ((index < 0) || (index >= elementCount))
                    return null;
                
                return removeTagByIndex(index);
            }
        }

        /// <summary> Remove a range of values
        /// 
        /// </summary>
        /// <param name="startTag">  first tag to remove
        /// </param>
        /// <param name="end">  last tag to remove, -1 is infintiy
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void removeTagRange(int startTag, int endTag)
        {
            lock (this)
            {
                int[] indexes = new int[tagIDs_i.Length];
                int count = 0;
                for (int i = 0; i < elementCount; i++)
                {
                    if (tagIDs_i[i] >= startTag && tagIDs_i[i] <= endTag)
                    {
                        indexes[count] = i;
                        count++;
                    }
                }

                for (int i = (count - 1); i >= 0; i--)
                {
                    removeTagByIndex(indexes[i]);
                }
            }
        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String removeTagByIndex(int index)
        {
            System.String rc = values[index];
            int numMoved = elementCount - index - 1;
            if (numMoved > 0)
            {
                Array.Copy(values, index + 1, values, index, numMoved);
                Array.Copy(tagIDs_s, index + 1, tagIDs_s, index, numMoved);
                Array.Copy(tagIDs_i, index + 1, tagIDs_i, index, numMoved);
            }
            --elementCount;
            values[elementCount] = null;
            tagIDs_s[elementCount] = null;
            tagIDs_i[elementCount] = -1;

            return rc;
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(System.String tag, char value_Renamed)
        {
            if (value_Renamed == (char)SupportClass.Identity(-1))
                throw new System.ArgumentException("Trying to set an invalid character in the fix message [" + System.Convert.ToString(value_Renamed) + "]");

            setValue(tag, System.Convert.ToString(value_Renamed));
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, char value_Renamed)
        {
            if (value_Renamed == (char)SupportClass.Identity(-1))
                throw new System.ArgumentException("Trying to set an invalid character in the fix message [" + System.Convert.ToString(value_Renamed) + "]");

            setValue(tag, System.Convert.ToString(value_Renamed));
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, bool value_Renamed)
        {
            if (value_Renamed)
            {
                setValue(tag, "Y");
            }
            else
            {
                setValue(tag, "N");
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, bool value_Renamed)
        {
            if (value_Renamed)
            {
                setValue(tag, "Y");
            }
            else
            {
                setValue(tag, "N");
            }
        }


        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, int value_Renamed)
        {
            lock (this)
            {
                setValue(tag, System.Convert.ToString(value_Renamed));
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, long value_Renamed)
        {
            lock (this)
            {
                setValue(tag, System.Convert.ToString(value_Renamed));
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, double value_Renamed)
        {
            lock (this)
            {
                setValue(tag, value_Renamed.ToString());
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, float value_Renamed)
        {
            lock (this)
            {
                setValue(tag, value_Renamed.ToString());
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, System.String value_Renamed)
        {
            lock (this)
            {
                if (value_Renamed == null)
                    throw new System.NullReferenceException("Cannot set null for tag=" + tag);

                int idx = indexOf(tag);
                if (idx >= 0)
                {
                    values[idx] = value_Renamed;
                }
                else
                {
                    addValue(tag, value_Renamed);
                }
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="startingIndex">  starting index
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag, System.String value_Renamed, int startingIndex)
        {
            lock (this)
            {
                if (value_Renamed == null)
                    throw new System.NullReferenceException("Cannot set null for tag=" + tag);

                if (startingIndex < 0 || startingIndex > elementCount)
                {
                    throw new System.ArgumentException("Invalid startingIndex " + startingIndex);
                }

                int idx = indexOf(tag, startingIndex);
                if (idx >= 0)
                {
                    values[idx] = value_Renamed;
                }
                else
                {
                    addValue(tag, value_Renamed);
                }
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="startingIndex">  starting index
        /// </param>
        /// <param name="value">  new value
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(int tag, System.String value_Renamed, int startingIndex)
        {
            lock (this)
            {
                if (value_Renamed == null)
                    throw new System.NullReferenceException("Cannot set null for tag=" + tag);

                if (startingIndex < 0 || startingIndex > elementCount)
                {
                    throw new System.ArgumentException("Invalid startingIndex " + startingIndex);
                }

                int idx = indexOf(tag, startingIndex);
                if (idx >= 0)
                {
                    values[idx] = value_Renamed;
                }
                else
                {
                    addValue(tag, value_Renamed);
                }
            }
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, int value_Renamed)
        {
            setValue(tag, System.Convert.ToString(value_Renamed));
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, long value_Renamed)
        {
            setValue(tag, System.Convert.ToString(value_Renamed));
        }


        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, double value_Renamed)
        {
            setValue(tag, value_Renamed.ToString());
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag, float value_Renamed)
        {
            setValue(tag, value_Renamed.ToString());
        }

        /// <summary> Set the value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag_i">  tag being set
        /// </param>
        /// <param name="value">  new value
        /// </param>
        public void setValue(int tag_i, System.String value_Renamed)
        {
            lock (this)
            {
                if (value_Renamed == null)
                    throw new System.NullReferenceException("Cannot set null for tag=" + tag_i);

                int idx = indexOf(tag_i);

                if (idx >= 0)
                {
                    values[idx] = value_Renamed;
                }
                else
                {
                    System.String tag_s = null;

                    if (tag_i < Message.tags.Length)
                        tag_s = Message.tags[tag_i];

                    if (tag_s == null)
                        tag_s = System.Convert.ToString(tag_i);

                    addValue(tag_s, (int)tag_i, value_Renamed);
                }
            }
        }

        /// <summary> Set all values for the given tag.  This function will be used for repeating groups.
        /// If
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// </param>
        /// <returns>  array of all values for the given tag
        /// </returns>
        public void updateValues(int tag, System.String[] newValues)
        {
            lock (this)
            {
                System.Collections.ArrayList tmpValues = null;
                int startIndex = 0;
                int i = 0;

                while (startIndex >= 0 && i < values.Length)
                {
                    int idx = indexOf(tag, startIndex);

                    if (idx < 0)
                    {
                        //startIndex = -1;
                        break;
                    }

                    if (tmpValues == null)
                        tmpValues = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                    startIndex = idx + 1;
                    values[idx] = newValues[i++];
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag_s, int tag_i, System.String value_Renamed)
        {
            lock (this)
            {
                if (value_Renamed == null)
                    throw new System.NullReferenceException("Cannot set null for tag=" + tag_s);

                int idx = indexOf(tag_i);
                if (idx >= 0)
                {
                    values[idx] = value_Renamed;
                }
                else
                {
                    addValue(tag_s, tag_i, value_Renamed);
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, int tag_i, ref System.DateTime date, bool isDate)
        {
            System.String date_s = Message.buildDateString(ref date, isDate, null);
            setValue(tag, tag_i, date_s);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setDateValue(System.String tag, int tag_i, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
            System.String date_s = Message.buildDateString(ref date, isDate, timeZone);
            setValue(tag, tag_i, date_s);
        }


        /// <summary> Construct a string representation of the passed in date object in standard FIX GMT format
        /// 
        /// </summary>
        /// <param name="date">  Date converting
        /// </param>
        /// <param name="ignoreTime">true to convert to date only, false to convert to date and time
        /// 
        /// </param>
        /// <returns> New String
        /// </returns>
        public static System.String buildDateString(ref System.DateTime date, bool ignoreTime)
        {
            return Message.buildDateString(ref date, ignoreTime, null);
        }

        /// <summary> Construct a string representation of the passed in date object in standard FIX GMT format
        /// 
        /// </summary>
        /// <param name="date">  Date converting
        /// </param>
        /// <param name="ignoreTime">true to convert to date only, false to convert to date and time
        /// </param>
        /// <param name="timeZone">timzone to build this String in
        /// 
        /// </param>
        /// <returns> New String
        /// </returns>
        public static System.String buildDateString(ref System.DateTime date, bool ignoreTime, System.TimeZone timeZone)
        {
            return date.ToString(Message.dateFormat);
            
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag_s, int tag_i, double value_Renamed)
        {
            lock (this)
            {
                int idx = indexOf(tag_i);
                if (idx >= 0)
                {
                    values[idx] = value_Renamed.ToString();
                }
                else
                {
                    addValue(tag_s, tag_i, value_Renamed.ToString());
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag_s, int tag_i, long value_Renamed)
        {
            lock (this)
            {
                int idx = indexOf(tag_i);
                if (idx >= 0)
                {
                    values[idx] = System.Convert.ToString(value_Renamed);
                }
                else
                {
                    addValue(tag_s, tag_i, System.Convert.ToString(value_Renamed));
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag_s, int tag_i, int value_Renamed)
        {
            lock (this)
            {
                int idx = indexOf(tag_i);
                if (idx >= 0)
                {
                    values[idx] = System.Convert.ToString(value_Renamed);
                }
                else
                {
                    addValue(tag_s, tag_i, System.Convert.ToString(value_Renamed));
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValue(System.String tag_s, int tag_i, char value_Renamed)
        {
            lock (this)
            {
                int idx = indexOf(tag_i);

                System.String value_s;
                int static_value_idx = (int)value_Renamed;
                if (static_value_idx >= 0 && static_value_idx < Message.static_values.Length)
                {
                    System.String tmpVal = Message.static_values[static_value_idx];
                    if (tmpVal == null)
                    {
                        value_s = System.Convert.ToString(value_Renamed);
                        Message.static_values[static_value_idx] = value_s;
                    }
                    else
                        value_s = tmpVal;
                }
                else
                    value_s = System.Convert.ToString(value_Renamed);

                if (idx >= 0)
                {
                    values[idx] = value_s;
                }
                else
                {
                    addValue(tag_s, tag_i, value_s);
                }
            }
        }

        /// <summary> Set the date value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="date">  Date being set
        /// </param>
        /// <param name="isDate">true to convert to date only, false to convert to date and time
        /// </param>
        public void setValue(int tag, ref System.DateTime date, bool isDate)
        {
            setValue(System.Convert.ToString(tag), ref date, isDate, null);
        }

        /// <summary> Set the date value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="date">  Date being set
        /// </param>
        /// <param name="isDate">true to convert to date only, false to convert to date and time
        /// </param>
        public void setValue(System.String tag, ref System.DateTime date, bool isDate)
        {
           setValue(tag, ref date, isDate, false, null);
        }

        /// <summary> Set the date value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="date">  Date being set
        /// </param>
        /// <param name="isDate">true to convert to date only, false to convert to date and time
        /// </param>
        /// <param name="useMillis">whether to add milliseconds to the timestamp
        /// </param>
        public void setValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis)
        {
            setValue(tag, ref date, isDate, useMillis, null);
        }

        /// <summary> Set the date value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="date">  Date being set
        /// </param>
        /// <param name="isDate">true to convert to date only, false to convert to date and time
        /// </param>
        /// <param name="timeZone">timzone to build this String in
        /// </param>
        public void setValue(System.String tag, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
            setValue(tag, ref date, isDate, false, timeZone);
        }

        public void setValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis, System.TimeZone timeZone)
        {
            String df;
            if (isDate)
                df = Message.dateFormat;
            else
            {
                if (useMillis)
                    df = Message.timeFormatMillis;
                else
                    df = Message.timeFormat;
            }

            if (timeZone != null)
            {
                
            }

            setValue(tag, date.ToString(df));
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, ref System.Boolean value_Renamed)
        {
            addValue(tag, value_Renamed.ToString());
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, int value_Renamed)
        {
            addValue(tag, System.Convert.ToString(value_Renamed));
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, char value_Renamed)
        {
            addValue(tag, ""+value_Renamed);
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, double value_Renamed)
        {
            addValue(tag, value_Renamed.ToString());
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, System.String value_Renamed)
        {
            lock (this)
            {
                insertValueAt(tag, value_Renamed, elementCount);
            }
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        public void addValue(int tag, int value_Renamed)
        {
            addValue(tag, System.Convert.ToString(value_Renamed));
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        public void addValue(int tag, char value_Renamed)
        {
            addValue(tag, ""+value_Renamed);
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        public void addValue(int tag, double value_Renamed)
        {
            addValue(tag, value_Renamed.ToString());
        }

        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        public void addValue(int tag, System.String value_Renamed)
        {
            lock (this)
            {
                insertValueAt(tag, value_Renamed, elementCount);
            }
        }


        /// <summary> Add this field to the end of this message.  This function should be used when
        /// adding multiple values for one tag for repeating groups
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE </y.exclude>
        public void addValue(System.String tag, ref System.DateTime value_Renamed, bool isDate)
        {
            addValue(tag, Message.buildDateString(ref value_Renamed, isDate));
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, int tag_i, ref System.DateTime date, bool isDate)
        {
            System.String date_s = Message.buildDateString(ref date, isDate, null);
            addValue(tag, tag_i, date_s);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addDateValue(System.String tag, int tag_i, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
            System.String date_s = Message.buildDateString(ref date, isDate, timeZone);
            addValue(tag, tag_i, date_s);
        }


        /// <summary> Add the date value for the specified tag.  If the tag does not exist in the message a new one will be added.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="date">  Date being set
        /// </param>
        /// <param name="isDate">true to convert to date only, false to convert to date and time
        /// </param>
        /// <param name="timeZone">timzone to build this String in
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, ref System.DateTime date, bool isDate, System.TimeZone timeZone)
        {
            addValue(tag, ref date, isDate, false, timeZone);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String tag, ref System.DateTime date, bool isDate, bool useMillis, System.TimeZone timeZone)
        {
            String df;

            if (isDate)
                df = Message.dateFormat;
            else
            {
                if (useMillis)
                    df = Message.timeFormatMillis;
                else
                    df = Message.timeFormat;
            }

            if (timeZone != null)
            {
                
            }

            addValue(tag, date.ToString(df)); //SupportClass.FormatDateTime(df, date));
        }


        /// <summary>  Get the double value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not a double
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public double getDoubleEx(System.String tag)
        {
            System.Double d = this.getDoubleValue(tag);
            if (d == 0)
            {
                return -1;
            }

            return d;
        }

        /// <summary>  Get the double value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not a double
        /// </returns>
        public double getDoubleEx(int tag)
        {
            System.Double d = this.getDoubleValue(tag);
            if (d == 0)
            {
                return -1;
            }

            return d;
        }

        /// <summary>  Get the double value for the specified tag
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or -1 if it does not exists or it is not a double
        /// </returns>
        public double getDoubleEx(int tag, double defaultRC)
        {
            System.Double d = this.getDoubleValue(tag);
            if (d == 0)
            {
                return defaultRC;
            }

            return d;
        }

        /// <summary> Add this field to the message at the specified location.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <param name="idx">index to add this field
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void insertValueAt(int tag_i, System.String value_Renamed, int idx)
        {
            lock (this)
            {
                System.String tag_s = null;
                if (tag_i < Message.tags.Length)
                    tag_s = Message.tags[tag_i];

                insertValueAt(tag_i, tag_s, value_Renamed, idx);
            }
        }

        /// <summary> Add this field to the message at the specified location.
        /// 
        /// </summary>
        /// <param name="tag">  tag being set
        /// </param>
        /// <param name="value">being added
        /// </param>
        /// <param name="idx">index to add this field
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void insertValueAt(System.String tag_s, System.String value_Renamed, int idx)
        {
            lock (this)
            {
                int tag_i = System.Int32.Parse(tag_s);

                if (tag_i < Message.tags.Length)
                    tag_s = Message.tags[tag_i];

                insertValueAt(tag_i, tag_s, value_Renamed, idx);
            }
        }
        protected internal void insertValueAt(int tag_i, System.String tag_s, System.String value_Renamed, int idx)
        {
            if (tag_s == null)
                tag_s = System.Convert.ToString(tag_i);

            if (values == null || (elementCount >= values.Length))
            {
                expandCapacity();
            }

            try
            {
                if (value_Renamed != null && value_Renamed.Length == 1)
                {
                    int tmpIdx = (int)value_Renamed[0];
                    if (tmpIdx >= 0 && tmpIdx < Message.static_values.Length)
                    {
                        System.String tmpVal = Message.static_values[tmpIdx];
                        if (tmpVal == null)
                            Message.static_values[(int)value_Renamed[0]] = value_Renamed;
                        else
                            value_Renamed = tmpVal;
                    }
                }
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }

            if (idx < elementCount)
            {
                // this value is being add in the middle of the message so we need to move all the values over
                Array.Copy(values, idx, values, idx + 1, elementCount - idx);
                Array.Copy(tagIDs_i, idx, tagIDs_i, idx + 1, elementCount - idx);
                Array.Copy(tagIDs_s, idx, tagIDs_s, idx + 1, elementCount - idx);
            }
            values[idx] = value_Renamed;
            tagIDs_i[idx] = (int)tag_i;
            tagIDs_s[idx] = tag_s;

            elementCount++;
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String intag_s, int tag_i, double value_Renamed)
        {
            addValue(intag_s, tag_i, value_Renamed.ToString());
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String intag_s, int tag_i, int value_Renamed)
        {
            addValue(intag_s, tag_i, System.Convert.ToString(value_Renamed));
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String intag_s, int tag_i, char value_Renamed)
        {
            System.String value_s;
            int static_value_idx = (int)value_Renamed;
            if (static_value_idx >= 0 && static_value_idx < Message.static_values.Length)
            {
                System.String tmpVal = Message.static_values[static_value_idx];
                if (tmpVal == null)
                {
                    value_s = System.Convert.ToString(value_Renamed);
                    Message.static_values[static_value_idx] = value_s;
                }
                else
                    value_s = tmpVal;
            }
            else
                value_s = System.Convert.ToString(value_Renamed);

            addValue(intag_s, tag_i, value_s);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValue(System.String intag_s, int tag_i, System.String value_Renamed)
        {
            lock (this)
            {
                if (values == null || (elementCount >= values.Length))
                {
                    expandCapacity();
                }

                try
                {
                    if (value_Renamed.Trim().Length == 1)
                    {
                        int idx = (int)value_Renamed[0];
                        if (idx >= 0 && idx < Message.static_values.Length)
                        {
                            System.String tmpVal = Message.static_values[idx];
                            if (tmpVal == null)
                                Message.static_values[(int)value_Renamed[0]] = value_Renamed;
                            else
                                value_Renamed = tmpVal;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    log.Error(e);
                    SupportClass.WriteStackTrace(e, Console.Error);
                }

                values[elementCount] = value_Renamed;
                //		tagIDs_s[elementCount] = tag_s;
                tagIDs_i[elementCount] = tag_i;

                System.String tag_s = null;
                if (tag_i < Message.tags.Length)
                    tag_s = Message.tags[tag_i];

                if (tag_s != null)
                    tagIDs_s[elementCount] = tag_s;
                else
                    tagIDs_s[elementCount] = intag_s;

                elementCount++;
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void addValues(System.Collections.ArrayList newFields)
        {
            // Vector should contain 2 dimensional arrays of fields and values
            for (int i = 0; i < newFields.Count; i++)
            {
                System.String[] field = (System.String[])newFields[i];
                addValue(field[Message.TAG], field[Message.VALUE]);
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValues(System.Collections.ArrayList newFields)
        {
            setValues(newFields, null);
        }
        public void setValues(System.Collections.ArrayList newFields, System.String removeStr)
        {
            // Set all the fields in the passed in Vector.
            //
            // A vector of 2 dimensional String arrays.  The first element in the string array
            // will be the tag and the second element will be the value.

            // Vector should contain 2 dimensional arrays of fields and values
            bool hasRemove = (removeStr != null);
            for (int i = 0; i < newFields.Count; i++)
            {
                System.String[] field = (System.String[])newFields[i];
                if (hasRemove && (field[Message.VALUE] != null) && field[Message.VALUE].Equals(removeStr))
                    removeTag(field[Message.TAG]);
                else
                    setValue(field[Message.TAG], field[Message.VALUE]);
            }
        }

        internal void expandCapacity()
        {
            lock (this)
            {
                if (values == null)
                {
                    values = new System.String[initialCapacity];
                    tagIDs_s = new System.String[initialCapacity];
                    tagIDs_i = new int[initialCapacity];
                }
                else
                {
                    int newCapacity = values.Length * Message.CAPACITY_MULTIPLIER;
                    System.String[] tmpValues = new System.String[newCapacity];
                    System.String[] tmptagIDs_s = new System.String[newCapacity];
                    int[] tmptagIDs_i = new int[newCapacity];
                    Array.Copy(values, 0, tmpValues, 0, values.Length);
                    Array.Copy(tagIDs_i, 0, tmptagIDs_i, 0, tagIDs_i.Length);
                    Array.Copy(tagIDs_s, 0, tmptagIDs_s, 0, tagIDs_s.Length);
                    values = tmpValues;
                    tagIDs_s = tmptagIDs_s;
                    tagIDs_i = tmptagIDs_i;
                }
            }
        }

        /// <summary> Change the tag at the specified index.  This will not affects the current value at that localtion.
        /// 
        /// </summary>
        /// <param name="tag">   new tag being set
        /// </param>
        /// <param name="i">     index being modified
        /// 
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setTagAt(System.String tag, int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:setTagAt:Invalid index " + i);

            tagIDs_i[i] = System.Int32.Parse(tag);
            tagIDs_s[i] = tag;
        }

        /// <summary> return the tag located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String tagAt(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:tagAt:Invalid index " + i);

            return tagIDs_s[i];
        }

        /// <summary> return the tag located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int tagAtInt(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:tagAtInt:Invalid index " + i);

            return tagIDs_i[i];
        }

        /// <summary> return the tag located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String tagAtString(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:tagAtString:Invalid index " + i);

            return tagIDs_s[i];
        }

        /// <summary> set the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="value"> new value
        /// </param>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setValueAt(System.String value_Renamed, int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:setValueAt:Invalid index " + i);

            values[i] = value_Renamed;
        }

        /// <summary> return the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String valueAt(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:valueAt:Invalid index " + i);

            return values[i];
        }

        /// <summary> return the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String valueAtString(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:valueAtString:Invalid index " + i);

            return values[i];
        }

        /// <summary> return the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public int valueAtInt(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:valueAtInt:Invalid index " + i);

            try
            {
                return System.Int32.Parse(values[i]);
            }
            catch //(System.Exception e)
            {
                throw new System.Exception("Invalid Fix format: " + ToString());
            }
        }

        /// <summary> return the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public float valueAtFloat(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:valueAtInt:Invalid index " + i);

            try
            {
                return System.Single.Parse(values[i]);
            }
            catch //(System.Exception e)
            {
                throw new System.Exception("Invalid Fix format: " + ToString());
            }
        }

        /// <summary> return the value located at the specified internal index
        /// 
        /// </summary>
        /// <param name="i"> index being accessed
        /// 
        /// </param>
        /// <returns> value
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public double valueAtDouble(int i)
        {
            if ((i < 0) || (i >= elementCount))
                throw new System.IndexOutOfRangeException("Message:valueAtInt:Invalid index " + i);

            try
            {
                return System.Double.Parse(values[i]);
            }
            catch //(System.Exception e)
            {
                throw new System.Exception("Invalid Fix format: " + ToString());
            }
        }

        /// <summary> Number of elements in the message
        /// 
        /// </summary>
        /// <returns> the size
        /// </returns>
        public int size()
        {
            return elementCount;
        }

        /// <summary> A summary representation of this message.</summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public override System.String ToString()
        {
            return toString(true);
        }

        /// <summary> A string representation of this message.
        /// 
        /// </summary>
        /// <param name="summary">- if false the entire message will be displayed
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String toString(bool summary)
        {
            return toString(summary, Message.fixFieldSeparator);
        }

        /// <summary> A string representation of this message.
        /// 
        /// </summary>
        /// <param name="summary">- if false the entire message will be displayed
        /// </param>
        /// <param name="separator">- separator to be used when generation the message
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public System.String toString(bool summary, System.String separator)
        {
            // if the string is frozen, return that
            if (cachedAPSSendBuffer != null)
                return cachedAPSSendBuffer;

            System.String str;
            if (summary)
            {
                str = Summary;
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(20 * elementCount);
                //StringBuffer sb = new StringBuffer(200);
                for (int i = 0; i < elementCount; i++)
                {
                    sb.Append(tagIDs_s[i]).Append('=').Append(values[i]).Append(separator);
                }

                str = sb.ToString();
                /*
                str = BasicFixUtilities.printTags(this,
                true,
                null,
                fixFieldSeparator,
                "=",
                "",
                null,
                false);
                */
            }

            return str;
        }

        /// <summary> A String representation of the msg type
        /// 
        /// </summary>
        /// <param name="msg">type
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public static System.String getDisplaySummery(System.String msgType)
        {
            System.String theMsg;
            try
            {
                if (msgType == null)
                {
                    return "MsgType=null";
                }

                if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGOrder))
                {
                    theMsg = "New Order - Single";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGOrderCancelReplaceRequest))
                {
                    theMsg = "Order Replace Request";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGOrderCancelRequest))
                {
                    theMsg = "Order Cancel Request";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGOrderStatusRequest))
                {
                    theMsg = "Order Status Request";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGOrderCancelReject))
                {
                    theMsg = "Order Cancel Reject";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGExecution))
                {
                    theMsg = "Execution Report";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogon))
                {
                    theMsg = "Logon";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGLogout))
                {
                    theMsg = "Logout";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGHeartbeat))
                {
                    theMsg = "Heartbeat";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGTestRequest))
                {
                    theMsg = "Test Request";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGResendRequest))
                {
                    theMsg = "Resend Request";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGReject))
                {
                    theMsg = "Reject";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGBusinessMessageReject))
                {
                    theMsg = "Business Reject";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGSequenceReset))
                {
                    theMsg = "Sequence Reset";
                }
                else if (msgType.Equals(FxIntegral.com.scalper.fix.Constants_Fields.MSGDK))
                {
                    theMsg = "Don't Know Trade";
                }
                else
                {
                    theMsg = "MsgType=" + msgType;
                }
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
                return ("");
            }

            return (theMsg);
        }

        protected internal Message(Message copyFrom)
        {
            copyFrom.copyInto(this);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.Object Clone()
        {
            return Copy;
        }

        /// <summary> Perform a deep copy of this message</summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void copyInto(Message newMsg)
        {
            lock (this)
            {
                newMsg.values = new System.String[values.Length];
                newMsg.tagIDs_s = new System.String[tagIDs_s.Length];
                newMsg.tagIDs_i = new int[tagIDs_i.Length];

                Array.Copy(values, 0, newMsg.values, 0, values.Length);
                Array.Copy(tagIDs_s, 0, newMsg.tagIDs_s, 0, tagIDs_s.Length);
                Array.Copy(tagIDs_i, 0, newMsg.tagIDs_i, 0, tagIDs_i.Length);

                newMsg.elementCount = elementCount;
                newMsg.sError = sError;
                newMsg.isValid_Renamed_Field = isValid_Renamed_Field;
            }
        }

        /// <summary> Perform a deep copy of this message</summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void copyInto(Message newMsg, int srcOffsetIndex, int destOffsetIndex, int length)
        {
            lock (this)
            {
                newMsg.values = new System.String[length + destOffsetIndex];
                newMsg.tagIDs_s = new System.String[length + destOffsetIndex];
                newMsg.tagIDs_i = new int[length + destOffsetIndex];

                Array.Copy(values, srcOffsetIndex, newMsg.values, destOffsetIndex, length);
                Array.Copy(tagIDs_s, srcOffsetIndex, newMsg.tagIDs_s, destOffsetIndex, length);
                Array.Copy(tagIDs_i, srcOffsetIndex, newMsg.tagIDs_i, destOffsetIndex, length);

                newMsg.elementCount = length;
                newMsg.sError = sError;
                newMsg.isValid_Renamed_Field = isValid_Renamed_Field;
            }
        }

        /// <summary> Perform a deep copy of this message tags and values</summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void copyInto(System.String[] tags, System.String[] values)
        {
            lock (this)
            {
                Array.Copy(this.values, 0, values, 0, values.Length);
                Array.Copy(this.tagIDs_s, 0, tags, 0, tags.Length);
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public void setBatchIndexes(int tag, int endOfRepeatingBlocks)
        {
            batchIndexes = getValueIndexes(tag);
            batchIndex = 0;

            if (batchIndexes != null)
            {
                inBatch = true;
            }

            endOfBatchBlocks = indexOf(endOfRepeatingBlocks);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public FIXGroup getGroup(int tag, int[] groupTags)
        {
            return getGroup(tag, groupTags, null);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        //("unchecked")
        public FIXGroup getGroup(int tag, int[] groupTags, FIXGroup inGroup)
        {
            lock (this)
            {
                Group lastGroup = (Group)inGroup;
                if (groups == null)
                    groups = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                Group group = null;

                for (int i = 0; i < groups.Count; i++)
                {
                    Group tmpGroup = (Group)groups[i];
                    if (tmpGroup.tag == tag)
                    {
                        if (lastGroup != null)
                        {
                            if (lastGroup.endIdx < tmpGroup.startIdx)
                            {
                                group = tmpGroup;
                            }
                        }
                        else
                        {
                            group = tmpGroup;
                        }
                    }

                    if (group != null)
                    {
                        break;
                    }
                }

                if (group == null)
                {
                    bool addedGroup = false;
                    int searchIndex = 0;
                    if (lastGroup != null)
                    {
                        searchIndex = lastGroup.endIdx + 1;
                    }

                    group = new Group(this, tag, groupTags, searchIndex);
                    if (group.startIdx < 0)
                    {
                        return null;
                    }

                    if (group != null)
                    {
                        for (int i = 0; i < groups.Count; i++)
                        {
                            Group tmpGroup = (Group)groups[i];
                            if (tmpGroup.startIdx >= group.startIdx)
                            {
                                groups.Insert(i, group);
                                addedGroup = true;
                            }
                        }
                    }

                    if (!addedGroup)
                    {
                        groups.Add(group);
                    }
                }
                return group;
            }
        }

        /// <summary> If a Message includes several instances of the same tag,
        /// it must utilize a Group to handle the repeating sequence.
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        sealed public class Group : FIXGroup
        {
            private void InitBlock(Message enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private Message enclosingInstance;
            public Message Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            internal int tag;
            internal int startIdx = 0;
            internal int endIdx = 0;

            internal Group(Message enclosingInstance, int tag, int[] groupTags, int searchIndex)
            {
                InitBlock(enclosingInstance);
                this.tag = (int)tag;
                lock (this)
                {
                    startIdx = Enclosing_Instance.indexOf(tag, searchIndex);

                    if (startIdx >= 0)
                    {
                        endIdx = startIdx;
                        while ((endIdx + 1) < Enclosing_Instance.tagIDs_i.Length)
                        {
                            int nextTag = Enclosing_Instance.tagIDs_i[endIdx + 1];

                            if (nextTag == tag)
                                break;

                            bool validTag = false;

                            for (int i = 0; i < groupTags.Length; i++)
                            {
                                if (groupTags[i] == nextTag)
                                {
                                    validTag = true;
                                    break;
                                }
                            }

                            if (!validTag)
                                break;

                            endIdx++;
                        }
                    }
                }
            }

            /// <summary>  Get the int value for the specified tag
            /// 
            /// </summary>
            /// <param name="tag">  tag searching for
            /// 
            /// </param>
            /// <returns>  the value or -1 if it does not exists or it is not a int
            /// </returns>
            public int getIntValue(int tag)
            {
                return getIntValue(tag, -1);
            }
            /// <summary>  Get the int value for the specified tag
            /// 
            /// </summary>
            /// <param name="tag">  tag searching for
            /// </param>
            /// <param name="defaultReturnCode">  tag searching for
            /// 
            /// </param>
            /// <returns>  the value or defaultReturnCode if it does not exists or it is not an int
            /// </returns>
            public int getIntValue(int tag, int defaultReturnCode)
            {
                System.String value_Renamed = getValue(tag);
                if (value_Renamed != null && value_Renamed.Length > 0)
                {
                    try
                    {
                        return System.Int32.Parse(value_Renamed);
                    }
                    catch //(System.Exception e)
                    {
                        return defaultReturnCode;
                    }
                }
                else
                    return defaultReturnCode;
            }

            public System.String getValue(int tag)
            {
                lock (this)
                {
                    int idx = Enclosing_Instance.indexOf(tag, startIdx);
                    if (idx < 0 || (idx > endIdx))
                        return null;

                    return Enclosing_Instance.values[idx];
                }
            }


            public void addValue(int tag, System.String value_Renamed)
            {
                lock (this)
                {
                    Enclosing_Instance.insertValueAt(System.Convert.ToString(tag), value_Renamed, endIdx + 1);
                    for (int i = 0; i < Enclosing_Instance.groups.Count; i++)
                    {
                        // update the start and end indexes of all the current groups
                        Group tmpGroup = (Group)Enclosing_Instance.groups[i];
                        if (tmpGroup.endIdx > endIdx)
                        {
                            tmpGroup.startIdx++;
                            tmpGroup.endIdx++;
                        }
                    }

                    endIdx++;
                }
            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setIfNull(int tag, System.String value_Renamed)
        {
            setIfNull(tag, value_Renamed, true);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setIfNull(int tag, System.String value_Renamed, bool errorIfValueNull)
        {
            System.String s = getValue(tag);
            if (s == null || s.Equals(""))
            {
                if (value_Renamed == null || value_Renamed.Equals(""))
                {
                    if (errorIfValueNull)
                    {
                        throw new System.ArgumentException("Value is null.");
                    }
                    else
                    {
                        // no error, don't set.
                        return;
                    }
                }
                setValue(tag, value_Renamed);
            }
        }

        /// <summary> Creates the mandatory fields for the FIX message header.
        /// This method is for internal use by the driver. We are creating only
        /// the mandatory fields here (initializing with empty values),
        /// no optional fields are created.
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void createHeaderFields()
        {
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString, "");
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength, 0);
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType, "");
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderCompID, "");
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetCompID, "");
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum, -1);
            addValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingTime, "");
        }

        /// <summary> Sets the MsgType for the FIX message with the specified character value.</summary>
        /// <param name="msgType">*
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setMsgTypeField(char msgType)
        {
            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i, msgType);
        }
        /// <summary> Sets the MsgType for the FIX message with the specified string value.</summary>
        /// <param name="msgType">*
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setMsgTypeField(System.String msgType)
        {
            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i, msgType);
        }

        /// <summary> returns a String that represents the given char, and if possible is the same instance as another
        /// one already in the JVM
        /// </summary>
        public static System.String toString(char ch)
        {
            int asciiValue = (int)ch;
            if (asciiValue >= 0 && asciiValue < Message.static_values_length)
            {
                return Message.static_values[asciiValue];
            }

            return System.Convert.ToString(ch);
        }

        /// <summary> returns a String that represents the int, and if possible is the same instance as another
        /// one already in the JVM
        /// </summary>
        public static System.String toString(int s)
        {
            if (s < 0 || s >= Message.INTEGER_ARRAY_LENGTH)
                return System.Convert.ToString(s);
            return Message.intArr[s];
        }

        /// <summary> It is a faster method to validate the checksum of a FIX message.
        /// It works with the byte array representation of the FIX message.
        /// It is provided for use in the driver code.
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <throws>  FixException </throws>
        /// <summary> 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void validateChecksumFast(sbyte[] msg)
        {
            // Obtain the value of checksum - should be the 3 characters before the last character.
            System.String checkSumReadStr = getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum_i);

            if (checkSumReadStr == null || checkSumReadStr.Length != 3)
            {
                throw new FixException("Invalid checksum");
            }

            System.String checkSumActualStr = generateCheckSumFast(msg);

            if (!checkSumReadStr.Equals(checkSumActualStr))
            {
                log.Debug("Message -> " + toString(false));
                log.Debug("Checksum(read) -> " + checkSumReadStr);
                log.Debug("Checksum(actual) -> " + checkSumActualStr);
                throw new FixException("Invalid checksum");
            }
        }

        /*
        * This method is ~10 times faster than the String.getBytes() method
        * However, this uses casting from 'char' to 'byte' which may not work for all character-sets.
        *
        * @y.exclude FOR INTERNAL USE ONLY
        */
        internal virtual sbyte[] getBytesFast(System.String str)
        {
            char[] buffer = new char[str.Length];
            int length = str.Length;
            sbyte[] b = new sbyte[length];
            SupportClass.GetCharsFromString(str, 0, length, buffer, 0);
            for (int j = 0; j < length; j++)
                b[j] = (sbyte)buffer[j];

            return b;
        }

        /// <summary> Calculates the checksum of the FIX message and returns it as String.</summary>
        /// <returns> checksum
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String generateCheckSum()
        {
            FIXByteBuffer buf = new FIXByteBuffer(400);
            buf.append(this);

            //byte[] msg = this.getBytes();
            sbyte[] msg = buf.theBytes;
            int limit = buf.count;

            System.String msgStr = new System.String(SupportClass.ToCharArray(msg), 0, limit);
            System.String delimiterBeforeCheckSumTag = FxIntegral.com.scalper.fix.Constants_Fields.delimiter + FxIntegral.com.scalper.fix.Constants_Fields.checksumPattern;
            // the index of the "1" in "...<SOH>10=", or -1 if not found.
            int indexOfChecksum = msgStr.IndexOf(delimiterBeforeCheckSumTag) + 1;
            // bug in String.indexOf?? seems to return 0 when not found.
            if (indexOfChecksum > 0)
                limit = indexOfChecksum;

            int sum = 0;
            for (int i = 0; i < limit; i++)
            {
                sum += (msg[i] & 0xFF);
            }

            int modulo = sum % 256;
            return Message.checksumStringArr[modulo];
        }

        /// <summary> Calculates the checksum of the FIX message and returns it as String.
        /// This is faster method as it directly works on the byte array representation
        /// of the FIX message. We do not need to obtain the bytes from the FIX message.
        /// Note : Make sure that the byte array representation of the FIX message is available
        /// otherwise it returns 'null'.
        /// 
        /// </summary>
        /// <returns>  checksum if byte array representation of the FIX message is available
        /// null 	 if byte array is not present
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private System.String generateCheckSumFast(sbyte[] msg)
        {
            int checkSumFieldLen = 7;
            int limit = msg.Length - checkSumFieldLen;

            int sum = 0;
            for (int i = 0; i < limit; i++)
            {
                sum += ((int)msg[i] & 0xFF);
            }

            int modulo = sum % 256;
            return Message.checksumStringArr[modulo];
        }

        /// <summary> get a a readable version of this message classes contents
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String toHumanReadableString()
        {
            int msgSeqNum = MsgSeqNum;
            char isPossDup = booleanCharValue(getCharValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGPossDupFlag));
            char msgType = getCharValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType);
            return msgSeqNum + " " + msgType + " " + isPossDup + ", " + getMessageSpecificHumanReadableString(msgType);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private System.String getMessageSpecificHumanReadableString(char msgType)
        {
            switch (msgType)
            {

                case 'A':
                    return "";

                case '2':
                    int from = getIntValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginSeqNo);
                    int to = getIntValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGEndSeqNo);
                    return from + " " + to;

                case '4':
                    char gapFill = booleanCharValue(getCharValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGGapFillFlag));
                    int nextSeqNo = getIntValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGNewSeqNo);
                    return gapFill + " " + nextSeqNo;

                default:
                    return "";

            }
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private char booleanCharValue(char ch)
        {
            return ch == 'Y' ? 'Y' : ' ';
        }

        /// <summary> Returns the value of sending time for the FIX message.</summary>
        /// <returns> SendingTime as String.
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String getSendingTime()
        {
            // UTCTimeStamp - GMT
            return getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingTime_i);
        }

        /// <summary> Sets the SendingTime of the FIX message in YYYYMMDD-HH:MM:SS format</summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setSendingTime()
        {
            setSendingTime(false);
        }

        /// <summary> Sets the SendingTime of the FIX message based on the current time.</summary>
        /// <param name="useMillis">if true, the sending time will be set to the current time in YYYYMMDD-HH:MM:SS format.
        /// if false, will be set to the current time in YYYYMMDD-HH:MM:SS.sss format
        /// 
        /// If you want to set an incorrect time (for testing purposes) you should use the appropriate setTag method
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setSendingTime(bool useMillis)
        {
            System.DateTime tempAux = System.DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
            System.String dateString = Message.buildDateString(ref tempAux, false, useMillis);
            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingTime_i, dateString);
        }

        /// <summary> Construct a string representation of the passed in date object in standard FIX GMT format
        /// 
        /// </summary>
        /// <param name="date">  Date converting
        /// </param>
        /// <param name="isDate">ignoreTime to convert to date only, false to convert to date and time
        /// </param>
        /// <param name="useMillis"> use milliseconds in the time format
        /// 
        /// </param>
        /// <returns> New String
        /// </returns>
        public static System.String buildDateString(ref System.DateTime date, bool ignoreTime, bool useMillis)
        {
            return Message.buildDateString(ref date, ignoreTime, useMillis, null);
        }

        private static System.String buildDateString(ref System.DateTime date, bool ignoreTime, bool useMillis, System.TimeZone timeZone)
        {
            String df;
            string tempDateTime = string.Empty;
            if (ignoreTime)
            {
                df = Message.dateFormat;
            }
            else
            {
                if (useMillis)
                {
                    
                    df = Message.timeFormat;
                    tempDateTime = date.ToString(df);
                }
                else
                {
                    df = Message.timeFormat;
                    tempDateTime = date.ToString(df);
                }
            }

            if (timeZone != null)
            {
                
            }
            return tempDateTime; 
        }

        /// <summary> Returns the bodylength of the FIX message.</summary>
        /// <returns> BodyLength of the message.
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual int getBodyLength()
        {
            try
            {
                return System.Int32.Parse(getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i));
            }
            catch //(System.Exception e)
            {
                // logger.error("Error in obtaining the BodyLength ", e);
            }
            return -1;
        }

        /// <summary> Calculates the bodylength of the FIX message and returns the
        /// value.  Tags must already be in the right order.  See setBodyLength().
        /// </summary>
        /// <returns> bodyLength
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private int calculateBodyLength()
        {
            /*
            bodylength = number of characters in the message following the
            BodyLength field and up to(including) the delimiter
            immediately preceding the CheckSum tag.
            FIX message format :-
            8=FIX.m.n<SOH>9=nnn<SOH>35=0<SOH>49=CALVIN<SOH>56=ASTERIX<SOH>34=1<SOH>52=...<SOH>10=xxx<SOH>
            ^                                                         ^
            | <---------  BodyLength (up to CheckSum tag)  ---------> |
            OR
            8=FIX.m.n<SOH>9=nnn<SOH>35=0<SOH>49=CALVIN<SOH>56=ASTERIX<SOH>34=1<SOH>52=...<SOH>
            | <---------  BodyLength (up to the end ) --------------> |
            */

            if (elementCount <= 3)
            {
                // @todo include msgType as required?
                throw new System.SystemException("Message must have begin string, body length, and checksum tags.");
            }

            if (tagAtInt(0) != FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString_i)
            {
                throw new System.SystemException("Begin String must be the first tag.");
            }

            if (tagAtInt(1) != FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i)
            {
                throw new System.SystemException("Body length must be the second tag.");
            }

            // @todo should require MsgType as well?

            //String beginString = getValue(Constants.TAGBeginString_i);
            //String bodyLength = getValue(Constants.TAGBodyLength_i);
            //String checkSum = getValue(Constants.TAGCheckSum_i);

            int sum = 0;

            for (int i = 2; i < elementCount - 1; i++)
            {
                sum += tagIDs_s[i].Length;
                sum += values[i].Length;
                // add extra character for the "=" sign
                // add extra character for the delimiter
                sum += 2;
            }

            int max = 1;
            for (int i = 0; i < FxIntegral.com.scalper.fix.Constants_Fields.MAX_BODY_LENGTH_DIGITS; i++)
            {
                max *= 10;
            }
            max--;
            if (sum > max)
            {
                throw new FixMessageContentException("BodyLength value is large number");
            }

            return sum;
        }

        /// <summary> Check if a specified tag exists in this message
        /// 
        /// </summary>
        /// <returns> true if this tag exists
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual bool containsTag(System.String tag)
        {
            return indexOf(tag) >= 0;
        }

        /// <summary> Check if a specified tag exists in this message
        /// 
        /// </summary>
        /// <returns> true if this tag exists
        /// </returns>
        public virtual bool containsTag(int tag)
        {
            return indexOf(tag) >= 0;
        }

        /*
        public void addTag(int tag, String value, int location)
        {
        //** @todo remove
        addTag(tag, value);
        shiftField(elementCount - 1, location);
        }
        */

        /// <summary> Reorders the fields in the given FIX message such that the first three fields
        /// in the header are the mandatory BeginString, BodyLength & MsgType followed by
        /// MsgSeqNum.  Moves CheckSum to be the final field.  Also, moves all header
        /// fields before all body fields, which are moved before all trailer fields.
        /// No validation is done and no check is made that all required fields are filled in.
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void reorderFields(TagHelper tagHelper)
        {

            
            shiftField(indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString_i), 0);
            shiftField(indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i), 1);
            shiftField(indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i), 2);
            shiftField(indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum_i), 3);
            shiftField(indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderCompID_i), 4);
          
            // checkSum field is optional
            int checksumIndex = indexOf(FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum_i);
            if (checksumIndex > -1)
            {
                shiftField(checksumIndex, elementCount - 1);
            }

            int firstNonHeader = 3;
            int tagVal = tagIDs_i[firstNonHeader];
            while (firstNonHeader < elementCount && tagHelper.isTagHeader(tagVal))
            {
                tagVal = tagIDs_i[++firstNonHeader];
            }

            for (int i = firstNonHeader; i < elementCount; i++)
            {
                if (tagHelper.isTagHeader(tagIDs_i[i]))
                {
                    shiftField(i, firstNonHeader);
                    // before:
                    // 8=,9=,35=,4000=,34= (lastNonHeader=3, i=4)

                    // after shifting:
                    // 8=,9=,35=,34=,4000= (lastNonHeader=3, i=4)

                    // fields have shifted, so the
                    firstNonHeader++;
                }
            }

            // @todo move other 2 trailer fields to the back

            
        }

        private void shiftField(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= elementCount)
            {
                throw new System.IndexOutOfRangeException("Illegal fromIndex: " + fromIndex);
            }

            if (toIndex < 0 || toIndex >= elementCount)
            {
                throw new System.IndexOutOfRangeException("Illegal toIndex: " + toIndex);
            }

            if (fromIndex == toIndex)
            {
                return;
            }

            System.String tempValue = values[fromIndex];
            System.String tempTagID_s = tagIDs_s[fromIndex];
            int tempTagID_i = tagIDs_i[fromIndex];

            if (fromIndex > toIndex)
            {
                for (int i = fromIndex; i > toIndex; i--)
                {
                    values[i] = values[i - 1];
                    tagIDs_s[i] = tagIDs_s[i - 1];
                    tagIDs_i[i] = tagIDs_i[i - 1];
                }
            }
            else
            {
                for (int i = fromIndex; i < toIndex; i++)
                {
                    values[i] = values[i + 1];
                    tagIDs_s[i] = tagIDs_s[i + 1];
                    tagIDs_i[i] = tagIDs_i[i + 1];
                }
            }

            values[toIndex] = tempValue;
            tagIDs_s[toIndex] = tempTagID_s;
            tagIDs_i[toIndex] = tempTagID_i;
        }

        /// <summary> Swaps two fields in the FIX message given the indices of the two fields.</summary>
        /// <param name="fromIndex">The initial position of the field (which needs to be reordered) in the FIX message.
        /// </param>
        /// <param name="toIndex">The destination position in the FIX message where the field needs to be placed after reorder.
        /// 
        /// </param>
        /// <todo>  should be private </todo>
        /// <summary> 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void swapFields(int fromIndex, int toIndex)
        {

            if (fromIndex < 0 || fromIndex >= elementCount)
            {
                throw new System.IndexOutOfRangeException("Illegal fromIndex: " + fromIndex);
            }

            if (toIndex < 0 || toIndex >= elementCount)
            {
                throw new System.IndexOutOfRangeException("Illegal toIndex: " + toIndex);
            }

            System.String tempValue = this.values[toIndex];
            System.String tempTagID_s = this.tagIDs_s[toIndex];
            int tempTagID_i = this.tagIDs_i[toIndex];

            this.values[toIndex] = this.values[fromIndex];
            this.tagIDs_s[toIndex] = this.tagIDs_s[fromIndex];
            this.tagIDs_i[toIndex] = this.tagIDs_i[fromIndex];

            this.values[fromIndex] = tempValue;
            this.tagIDs_s[fromIndex] = tempTagID_s;
            this.tagIDs_i[fromIndex] = tempTagID_i;
        }

        /// <summary> Calculates the value of bodylength of the FIX message and sets it
        /// to the BodyLength field of the FIX message.  Application-level code
        /// never needs to worry about this -- it is the responsibility of the driver
        /// to set the body-length tag to the correct value.
        /// <p>
        /// The message must already have the following tags, in the order given:
        /// <br>beginString (8): tag #1
        /// <br>bodyLength (9): tag #2
        /// <br>checkSum (10): <the last tag in the message>
        /// <p>
        /// Call setTag(int, String) with "" as the value to add an empty tag.
        /// Call reorderFields to put the tags in the right order.
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setBodyLength()
        {
            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i, this.calculateBodyLength());
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual int getIntegerFieldValue(System.String tag)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    throw new FixMessageFormatException("Invalid Fix format: " + ToString());
                }
            }
            else
                throw new FixMessageContentException("Missing field, tag=" + tag);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual int getIntegerFieldValue(int tag)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null)
            {
                try
                {
                    return System.Int32.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    throw new FixMessageFormatException("Invalid Fix format: " + ToString());
                }
            }
            else
                throw new FixMessageContentException("Missing field, tag=" + tag);
        }

        /// <summary>  Get the value for the specified tag as String
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String getStringFieldValue(System.String tag)
        {
            return getValue(tag);
        }

        /// <summary>  Get the value for the specified tag as String
        /// 
        /// </summary>
        /// <param name="tag">  tag searching for
        /// 
        /// </param>
        /// <returns>  the value or null if it does not exists
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String getStringFieldValue(int tag)
        {
            return getValue(tag);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual bool getBooleanFieldValue(System.String tag)
        {
            return isTrue(getValue(tag));
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual double getDoubleFieldValue(System.String tag)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null)
            {
                try
                {                    
                    return System.Double.Parse(value_Renamed,BasicUtilities.getCulture());
                }
                catch //(System.Exception e)
                {
                    throw new FixMessageFormatException("Invalid Fix format: " + ToString());
                }
            }
            else
                throw new FixMessageContentException("Missing field, tag=" + tag);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual float getFloatFieldValue(System.String tag)
        {
            System.String value_Renamed = getValue(tag);
            if (value_Renamed != null)
            {
                try
                {
                    return System.Single.Parse(value_Renamed);
                }
                catch //(System.Exception e)
                {
                    throw new FixMessageFormatException("Invalid Fix format: " + ToString());
                }
            }
            else
                throw new FixMessageContentException("Missing field, tag=" + tag);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setStringFieldValue(System.String tag, System.String value_Renamed)
        {
            setValue(tag, value_Renamed);
        }
        /// <summary> Sets the BodyLength field of the FIX message with the
        /// specified value.
        /// </summary>
        /// <param name="length">*
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setBodyLength(int bodyLength)
        {
            int noOfDigits = System.Convert.ToString(bodyLength).Length;
            if (noOfDigits > FxIntegral.com.scalper.fix.Constants_Fields.MAX_BODY_LENGTH_DIGITS)
                throw new FixMessageContentException("Bodylength can not exceed " + FxIntegral.com.scalper.fix.Constants_Fields.MAX_BODY_LENGTH_DIGITS + " digits.");

            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i, bodyLength);
        }

        /// <summary> Returns the checksum of the FIX message.</summary>
        /// <returns> CheckSum of the message, or null if it has not been set
        /// 
        /// </returns>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String getCheckSum()
        {
            return getValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum_i);
        }

        /// <summary> Calculates the checksum of the contents of the FIX message and
        /// sets it to the CheckSum field of the FIX message.
        /// 
        /// </summary>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setCheckSum()
        {
            try
            {
                setCheckSum(generateCheckSum());
            }
            catch (FixException e)
            {
                // logger.error("Error in setting the CheckSum ", e);
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }
        }

        /// <summary> Calculates the checksum of the contents of the FIX message and
        /// sets it to the CheckSum field of the FIX message.
        /// 
        /// public void setCheckSumFast()
        /// {
        /// try
        /// {
        /// setCheckSum(generateCheckSumFast());
        /// }
        /// catch ( FixException e )
        /// {
        /// e.printStackTrace();
        /// // logger.error("Error in setting the CheckSum ", e);
        /// }
        /// } 
        /// </summary>

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public static void copyField(System.String tag, int tag_i, FIXGroup sourceMsg, Message targetMsg)
        {
            System.String val = sourceMsg.getValue(tag_i);
            if (val != null)
                targetMsg.setValue(tag, tag_i, val);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public static void copyField(System.String tag, int tag_i, Message sourceMsg, Message targetMsg)
        {
            System.String val = sourceMsg.getValue(tag_i);
            if (val != null)
                targetMsg.setValue(tag, tag_i, val);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public static void copyField(int tag_i, Message sourceMsg, Message targetMsg)
        {
            if (targetMsg != null)
            {
                System.String val = sourceMsg.getValue(tag_i);
                if (val != null)
                    targetMsg.setValue(tag_i, val);
            }
        }

        /// <summary> Sets the CheckSum field of the FIX message with the specified value.</summary>
        /// <param name="checkSum">*
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual void setCheckSum(int checkSum)
        {
            setCheckSum("" + checkSum);
        }

        /// <summary> Sets the CheckSum field of the FIX message with the specified value.</summary>
        /// <param name="checkSum">*
        /// </param>
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        private void setCheckSum(System.String checkSum)
        {
            if (checkSum.Length > FxIntegral.com.scalper.fix.Constants_Fields.MAX_CHECKSUM_DIGITS)
                throw new FixMessageContentException("Checksum value can not exceed " + FxIntegral.com.scalper.fix.Constants_Fields.MAX_CHECKSUM_DIGITS + " characters.");

            if (checkSum.Length == (FxIntegral.com.scalper.fix.Constants_Fields.MAX_CHECKSUM_DIGITS - 1))
                checkSum = "0" + checkSum;
            else if (checkSum.Length == (FxIntegral.com.scalper.fix.Constants_Fields.MAX_CHECKSUM_DIGITS - 2))
                checkSum = "00" + checkSum;

            setValue(FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum_i, checkSum);
        }

        /// <y.exclude>  FOR INTERNAL USE </y.exclude>
        public static readonly System.String[] tags = new System.String[500];
        public static bool isTrue(System.String istrue)
        {
            if (istrue == null)
                return false;
            // equalsIgnoreCase does not exist in KVM.
            System.String toLower = istrue.ToLower();
            return ((toLower.Equals("on")) || (toLower.Equals("yes")) || (toLower.Equals("y")) || (toLower.Equals("true")));
        }

        public static bool isFalse(System.String istrue)
        {
            if (istrue == null)
                return false;
            // equalsIgnoreCase does not exist in KVM.
            System.String toLower = istrue.ToLower();
            return ((toLower.Equals("off")) || (toLower.Equals("no")) || (toLower.Equals("n")) || (toLower.Equals("false")));
        }
        static Message()
        {
            {
               
                // initialize static values array
                for (int i = 0; i < Message.static_values_length; i++)
                {
                    char ch = (char)i;
                    Message.static_values[i] = System.Convert.ToString(ch);
                }

                String df = "000";
                for (int i = 0; i <= Message.CHECKSUM_MAX; i++)
                {
                    Message.checksumStringArr[i] = i.ToString(df);
                }

                for (int i = 0; i < Message.INTEGER_ARRAY_LENGTH; i++)
                {
                    Message.intArr[i] = System.Convert.ToString(i);
                }
            }
            {
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAccount_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAccount;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvId_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvId;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvSide_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvSide;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvTransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAdvTransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAvgPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAvgPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginSeqNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginSeqNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBeginString;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBodyLength;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCheckSum;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGClOrdID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGClOrdID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCommission_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCommission;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCommType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCommType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCumQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCumQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCurrency_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCurrency;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEndSeqNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEndSeqNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecTransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecTransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGHandlInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGHandlInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIDSource_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIDSource;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIid_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIid;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIOthSvc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIOthSvc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIQltyInd_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIQltyInd;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIShares_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIShares;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOITransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOITransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastCapacity_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastCapacity;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastMkt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastMkt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastShares_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastShares;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLinesOfText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLinesOfText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgSeqNum;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNewSeqNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNewSeqNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigClOrdID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigClOrdID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPossDupFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPossDupFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPrice_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPrice;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRefSeqNum_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRefSeqNum;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRelatdSym_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRelatdSym;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRule80A_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRule80A;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderCompID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderCompID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderSubID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderSubID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingDate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingDate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSendingTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGShares_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGShares;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSide_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSide;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSymbol_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSymbol;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetCompID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetCompID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetSubID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetSubID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTimeInForce_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTimeInForce;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTransactTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTransactTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUrgency_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUrgency;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGValidUntilTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGValidUntilTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlmntTyp_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlmntTyp;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGFutSettDate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGFutSettDate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSymbolSfx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSymbolSfx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListSeqNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListSeqNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTotNoOrders_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTotNoOrders;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListExecInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListExecInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocTransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocTransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRefAllocID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRefAllocID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoOrders_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoOrders;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAvgPrxPrecision_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAvgPrxPrecision;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeDate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeDate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecBroker_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecBroker;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOpenClose_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOpenClose;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoAllocs_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoAllocs;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocAccount_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocAccount;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocShares_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocShares;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGProcessCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGProcessCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRpts_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRpts;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRptSeq_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRptSeq;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoDlvyInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoDlvyInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDlvyInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDlvyInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocRejCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocRejCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSignature_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSignature;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureDataLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureDataLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureData_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureData;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBrokerOfCredit_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBrokerOfCredit;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSignatureLength_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSignatureLength;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEmailType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEmailType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRawDataLength_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRawDataLength;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRawData_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRawData;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPossResend_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPossResend;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncryptMethod_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncryptMethod;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStopPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStopPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExDestination_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExDestination;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlRejReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlRejReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdRejReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrdRejReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIQualifier_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOIQualifier;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGWaveNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGWaveNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIssuer_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIssuer;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityDesc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityDesc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGHeartBtInt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGHeartBtInt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGClientID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGClientID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMinQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMinQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxFloor_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxFloor;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTestReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTestReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGReportToExch_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGReportToExch;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLocateReqd_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLocateReqd;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfCompID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfCompID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfSubID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfSubID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNetMoney_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNetMoney;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrAmt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrAmt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrency_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrency;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGForexReq_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGForexReq;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigSendingTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrigSendingTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGGapFillFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGGapFillFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoExecs_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoExecs;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExpireTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExpireTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDKReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDKReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToCompID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToCompID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToSubID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToSubID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIOINaturalFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIOINaturalFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidSize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidSize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferSize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferSize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMiscFees_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMiscFees;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeAmt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeAmt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeCurr_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeCurr;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMiscFeeType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPrevClosePx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPrevClosePx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGResetSeqNumFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGResetSeqNumFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderLocationID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSenderLocationID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetLocationID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTargetLocationID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfLocationID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfLocationID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToLocationID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDeliverToLocationID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRelatedSym_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRelatedSym;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSubject_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSubject;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGHeadline_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGHeadline;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGURLLink_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGURLLink;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLeavesQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLeavesQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashOrderQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashOrderQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocAvgPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocAvgPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocNetMoney_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocNetMoney;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrFxRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrFxRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrFxRateCalc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlCurrFxRateCalc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNumDaysInterest_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNumDaysInterest;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAccruedInterestRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAccruedInterestRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAccruedInterestAmt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAccruedInterestAmt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstMode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstMode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstTransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstTransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEmailThreadID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEmailThreadID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstSource_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstSource;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlLocation_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlLocation;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEffectiveTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEffectiveTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStandInstDbID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlDeliveryType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlDeliveryType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlDepositoryCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlDepositoryCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlBrkrCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlBrkrCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentAcctNum_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentAcctNum;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentAcctName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentAcctName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentContactName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentContactName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentContactPhone_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecuritySettlAgentContactPhone;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentCode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentCode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentAcctNum_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentAcctNum;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentAcctName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentAcctName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentContactName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentContactName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentContactPhone_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCashSettlAgentContactPhone;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidSpotRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidSpotRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidForwardPoshorts_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidForwardPoshorts;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferSpotRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferSpotRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferForwardPoshorts_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOfferForwardPoshorts;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderQty2_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOrderQty2;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGFutSettDate2_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGFutSettDate2;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastSpotRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastSpotRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastForwardPoshorts_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastForwardPoshorts;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocLinkID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocLinkID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocLinkType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocLinkType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecondaryOrderID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecondaryOrderID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoIOIQualifiers_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoIOIQualifiers;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMaturityMonthYear_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMaturityMonthYear;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPutOrCall_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPutOrCall;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStrikePrice_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStrikePrice;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCoveredOrUncovered_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCoveredOrUncovered;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCustomerOrFirm_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCustomerOrFirm;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMaturityDay_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMaturityDay;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOptAttribute_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOptAttribute;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityExchange_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityExchange;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNotifyBrokerOfCredit_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNotifyBrokerOfCredit;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocHandlInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocHandlInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxShow_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxShow;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPegDifference_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPegDifference;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGXmlDataLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGXmlDataLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGXmlData_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGXmlData;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSettlInstRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRoutingIDs_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoRoutingIDs;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRoutingType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRoutingType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRoutingID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRoutingID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSpreadToBenchmark_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSpreadToBenchmark;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBenchmark_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBenchmark;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCouponRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCouponRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGContractMultiplier_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGContractMultiplier;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSubscriptionRequestType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSubscriptionRequestType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMarketDepth_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMarketDepth;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDUpdateType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDUpdateType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAggregatedBook_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAggregatedBook;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMDEntryTypes_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMDEntryTypes;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMDEntries_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMDEntries;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntrySize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntrySize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryDate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryDate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTickDirection_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTickDirection;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDMkt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDMkt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteCondition_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteCondition;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeCondition_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeCondition;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDUpdateAction_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDUpdateAction;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDReqRejReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDReqRejReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryOriginator_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryOriginator;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLocationID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLocationID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDeskID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDeskID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDeleteReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDeleteReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOpenCloseSettleFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOpenCloseSettleFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSellerDays_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSellerDays;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryBuyer_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryBuyer;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntrySeller_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntrySeller;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryPositionNo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMDEntryPositionNo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGFinancialStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGFinancialStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCorporateAction_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCorporateAction;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDefBidSize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDefBidSize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDefOfferSize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDefOfferSize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoQuoteEntries_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoQuoteEntries;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoQuoteSets_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoQuoteSets;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteAckStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteAckStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteCancelType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteCancelType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteEntryID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteEntryID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteRejectReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteRejectReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteResponseLevel_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteResponseLevel;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteSetID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteSetID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteRequestType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteRequestType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTotQuoteEntries_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTotQuoteEntries;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingIDSource_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingIDSource;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingIssuer_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingIssuer;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityDesc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityDesc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityExchange_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityExchange;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSecurityType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSymbol_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSymbol;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSymbolSfx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingSymbolSfx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingMaturityMonthYear_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingMaturityMonthYear;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingMaturityDay_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingMaturityDay;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingPutOrCall_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingPutOrCall;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingStrikePrice_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingStrikePrice;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingOptAttribute_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingOptAttribute;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingCurrency_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingCurrency;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRatioQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRatioQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityRequestType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityRequestType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityResponseID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityResponseID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityResponseType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityResponseType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityStatusReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityStatusReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnsolicitedIndicator_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnsolicitedIndicator;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityTradingStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSecurityTradingStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGHaltReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGHaltReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGInViewOfCommon_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGInViewOfCommon;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDueToRelated_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDueToRelated;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBuyVolume_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBuyVolume;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSellVolume_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSellVolume;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGHighPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGHighPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLowPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLowPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAdjustment_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAdjustment;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesReqID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesReqID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradingSessionID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradingSessionID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTrader_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTrader;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesMethod_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesMethod;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesMode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesMode;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesStartTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesStartTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesOpenTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesOpenTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesPreCloseTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesPreCloseTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesCloseTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesCloseTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesEndTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradSesEndTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNumberOfOrders_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNumberOfOrders;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMessageEncoding_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMessageEncoding;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedIssuerLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedIssuerLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedIssuer_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedIssuer;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSecurityDescLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSecurityDescLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSecurityDesc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSecurityDesc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListExecInstLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListExecInstLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListExecInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListExecInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedTextLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedTextLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSubjectLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSubjectLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSubject_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedSubject;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedHeadlineLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedHeadlineLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedHeadline_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedHeadline;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedAllocTextLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedAllocTextLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedAllocText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedAllocText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingIssuerLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingIssuerLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingIssuer_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingIssuer;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingSecurityDescLe_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingSecurityDescLe;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingSecurityDesc_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedUnderlyingSecurityDesc;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocPrice_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGAllocPrice;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteSetValidUntilTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteSetValidUntilTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteEntryRejectReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGQuoteEntryRejectReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLastMsgSeqNumProcessed_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLastMsgSeqNumProcessed;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfSendingTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOnBehalfOfSendingTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRefTagID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRefTagID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGRefMsgType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGRefMsgType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSessionRejectReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSessionRejectReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidRequestTransType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidRequestTransType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGContraBroker_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGContraBroker;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGComplianceID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGComplianceID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSolicitedFlag_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSolicitedFlag;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExecRestatementReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExecRestatementReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBusinessRejectRefID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBusinessRejectRefID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBusinessRejectReason_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBusinessRejectReason;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGGrossTradeAmt_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGGrossTradeAmt;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoContraBrokers_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoContraBrokers;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxMessageSize_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMaxMessageSize;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMsgTypes_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoMsgTypes;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgDirection_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMsgDirection;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoTradingSessions_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoTradingSessions;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTotalVolumeTraded_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTotalVolumeTraded;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDiscretionInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDiscretionInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDiscretionOffset_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDiscretionOffset;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGClientBidID_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGClientBidID;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListName_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListName;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTotalNumSecurities_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTotalNumSecurities;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNumTickets_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNumTickets;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValue1_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValue1;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValue2_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValue2;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoBidDescriptors_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoBidDescriptors;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidDescriptorType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidDescriptorType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBidDescriptor_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBidDescriptor;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValueInd_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGSideValueInd;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityPctLow_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityPctLow;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityPctHigh_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityPctHigh;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityValue_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityValue;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEFPTrackingError_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEFPTrackingError;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGFairValue_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGFairValue;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOutsideIndexPct_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOutsideIndexPct;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGValueOfFutures_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGValueOfFutures;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityIndType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityIndType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGWtAverageLiquidity_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGWtAverageLiquidity;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExchangeForPhysical_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExchangeForPhysical;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGOutMainCntryUIndex_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGOutMainCntryUIndex;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCrossPercent_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCrossPercent;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGProgRptReqs_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGProgRptReqs;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGProgPeriodInterval_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGProgPeriodInterval;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGIncTaxInd_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGIncTaxInd;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNumBidders_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNumBidders;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTradeType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGBasisPxType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGBasisPxType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoBidComponents_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoBidComponents;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCountry_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCountry;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGTotNoStrikes_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGTotNoStrikes;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGPriceType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGPriceType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDayOrderQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDayOrderQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDayCumQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDayCumQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGDayAvgPx_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGDayAvgPx;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGGTBookingInst_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGGTBookingInst;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNoStrikes_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNoStrikes;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListStatusType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListStatusType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGNetGrossInd_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGNetGrossInd;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListOrderStatus_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListOrderStatus;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGExpireDate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGExpireDate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListExecInstType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListExecInstType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlRejResponseTo_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCxlRejResponseTo;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingCouponRate_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingCouponRate;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingContractMultiplier_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGUnderlyingContractMultiplier;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTradeQty_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTradeQty;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTradeTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGContraTradeTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGClearingFirm_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGClearingFirm;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGClearingAccount_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGClearingAccount;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityNumSecurities_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGLiquidityNumSecurities;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGMultiLegReportingType_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGMultiLegReportingType;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGStrikeTime_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGStrikeTime;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGListStatusText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGListStatusText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListStatusTextLen_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListStatusTextLen;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListStatusText_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGEncodedListStatusText;
                Message.tags[FxIntegral.com.scalper.fix.Constants_Fields.TAGCFICode_i] = FxIntegral.com.scalper.fix.Constants_Fields.TAGCFICode;
                // added for FIX 4.4 + FPL
                //tags[TAGAllocReportID_i] = TAGAllocReportID;
                //tags[TAGAllocReportRefID_i] = TAGAllocReportRefID;
                //tags[TAGIndividualAllocID_i] = TAGIndividualAllocID;
            }
        }
    }
}