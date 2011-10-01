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
/// File: FFillFixSession.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using Constants = FxIntegral.com.scalper.fix.Constants;
using FIXByteBuffer = FxIntegral.com.scalper.fix.FIXByteBuffer;
using FixConstants = FxIntegral.com.scalper.fix.FixConstants;
using FixException = FxIntegral.com.scalper.fix.FixException;
using FixMessageContentException = FxIntegral.com.scalper.fix.FixMessageContentException;
using FixMessageFormatException = FxIntegral.com.scalper.fix.FixMessageFormatException;
using Message = FxIntegral.com.scalper.fix.Message;
using TagHelper = FxIntegral.com.scalper.fix.TagHelper;
using FxIntegral.com.scalper.util;
using log4net;
namespace  FxIntegral.com.scalper.fix.driver
{
	
	/// <summary> reads and writes FIX messages using a FIXConnection, sending them in FIX format.</summary>
	public class DefaultMessageConnection:AbstractMessageConnection
	{
        

		/// <summary> returns the next message available from the channel, blocking if necessary</summary>
		override public System.Object MessageFirstPass
		{
			get
			{
				return readInternal();
			}
			
		}
		/// <summary> tests whether the underlying communication channel is still open
		/// 
		/// public boolean isOpen()
		/// {
		/// } 
		/// </summary>
		override public FIXConnection Connection
		{
			
			
			get
			{
				return conn;
			}
			
		}
        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultMessageConnection));
		//Maximum body lenght digits that can be present in the FIX message, that also
		//decides the maximum allowed size of the FIX message body.
		private static readonly int maxDigits = FxIntegral.com.scalper.fix.FixConstants_Fields.MAX_BODY_LENGTH_DIGITS;
		
		private static readonly int START_READ_LENGTH = "8=FIX.4.X^9=^".Length + FxIntegral.com.scalper.fix.FixConstants_Fields.MAX_BODY_LENGTH_DIGITS;
		
		//Index to keep track of current position in working copy.
		private int workingCopyCurIndex = 0;
		
		/// <summary>the underlying connection object that reads bytes off the wire. </summary>
		private FIXConnection conn;
		
		public DefaultMessageConnection(FIXConnection conn)
		{
			this.conn = conn;
			// not sure if this is necessary but this is how it's currently done, just trying to maintain existing
			// functionality.
			if (conn != null)
				conn.Blocking = true;
		}
		
		/// <summary> sends the message over the channel, performing any conversion that may be necessary</summary>
		public override void  sendMessage(Message msg)
		{
			//Temp to chk that server close the socket then what will get back server response
            //if (msg.MsgType != "0" && msg.MsgSeqNum < 10)
            //{
                ByteBuffer buf = msg.ByteBuffer;
                conn.send(FxIntegral.com.scalper.util.ByteBuffer.wrap(buf.array()));
            //}
		}
		
		public override void  sendMessages(System.Object[] msgs)
		{
			FIXByteBuffer dataToSend = new FIXByteBuffer(msgs.Length * 500);
			for (int i = 0; i < msgs.Length; i++)
			{
				if (msgs[i] != null && msgs[i] is Message)
					dataToSend.append((Message) msgs[i]);
			}
			ByteBuffer buf = dataToSend.ByteBuffer;
            conn.send(FxIntegral.com.scalper.util.ByteBuffer.wrap(buf.array()));
		}
		
		public override char getFixMinorVersion(System.Object tmpMessage)
		{
			sbyte[] msgByte = (sbyte[]) tmpMessage;
			return (char) msgByte[FxIntegral.com.scalper.fix.driver.MessageConnection_Fields.FIX_MINOR_VERSION_INDEX];
		}
		
		/// <summary> This method is responsible for reading from the socket and update the working copy
		/// accordingly. Will be called whenever reader needs more bytes to process the
		/// complete message.  Blocks until the buffer is full.
		/// </summary>
		/// <throws>  IOException </throws>
		private void  readAndUpdateWorkingCopy(sbyte[] buf)
		{
			readAndUpdateWorkingCopy(buf, 0);
		}
		
		/// <summary> This method is responsible for reading from the socket and update the working copy
		/// accordingly. Will be called whenever reader needs more bytes to process the
		/// complete message.  Blocks until the buffer is full.
		/// </summary>
		/// <throws>  IOException </throws>
		private void  readAndUpdateWorkingCopy(sbyte[] buf, int startIndex)
		{
			// current position in the array
			int index = startIndex;
			// the index to stop at (exclusive)
			int stopIndex = buf.Length;
			
			
			while (index < stopIndex)
			{
				int bytesRead = conn.receive(buf, index);
				index += bytesRead;
			}
		}
		
		/// <summary> reads a complete FIX message and returns a byte array that represents the (semi-parsed)
		/// FIX message.  If an unrecoverable error occurs then the read process is aborted with a DenialOfServiceException.
		/// If the method returns normally it is safe to parse the byte array.
		/// </summary>
		private sbyte[] readInternal()
		{
			workingCopyCurIndex = 0;
			sbyte[] initialBuffer = new sbyte[DefaultMessageConnection.START_READ_LENGTH];
			
			readAndUpdateWorkingCopy(initialBuffer);
			
			
			matchTag(initialBuffer, "8", "beginString");
			match(initialBuffer, "FIX.4.", "BeginString value doesn't starts with 'FIX.4.'");
			matchFixMinorVersion(initialBuffer);
			matchDelimiter(initialBuffer, "beginString");
			matchTag(initialBuffer, "9", "bodyLength");
			ensureDelimiterNotNext(initialBuffer, "bodyLength");
			int bodyLength = readBodyTag(initialBuffer, DefaultMessageConnection.maxDigits, "bodyLength");
			matchDelimiter(initialBuffer, "bodyLength");
			int startBodyIndex = workingCopyCurIndex;
			
			
			int totalSize = bodyLength + startBodyIndex + FxIntegral.com.scalper.fix.FixConstants_Fields.END_FIXMSG_SIZE;
			
			sbyte[] messageBuffer = new sbyte[totalSize];
			
			Array.Copy(initialBuffer, 0, messageBuffer, 0, initialBuffer.Length);
			
			readAndUpdateWorkingCopy(messageBuffer, initialBuffer.Length);
			
			int msgTypeBeginIndex = workingCopyCurIndex;
			
			matchTag(messageBuffer, "35", "message type");
			ensureDelimiterNotNext(messageBuffer, "message type");
			workingCopyCurIndex = msgTypeBeginIndex + bodyLength - 1;
			matchDelimiter(messageBuffer, true, "Couldn't find delimiter at end of body.");
			matchTag(messageBuffer, "10", "checksum");
			workingCopyCurIndex += 3; // skip ahead to after checksum
			matchDelimiter(messageBuffer, true, "Couldn't find the end of message delimiter.");
			if (workingCopyCurIndex != totalSize)
			{
				throw new System.SystemException("bad working copy current index " + workingCopyCurIndex + ", expected " + totalSize);
			}
			
			return messageBuffer;
		}
		
		
		private void  matchTag(sbyte[] buf, System.String tag, System.String tagName)
		{
			for (int i = 0; i < tag.Length; i++)
			{
				if (buf[workingCopyCurIndex + i] != tag[i])
				{
					throw new DenialOfServiceException("Couldn't find the " + tagName + " tag.", getCopyOfWorkingCopyArr(buf));
				}
			}
			workingCopyCurIndex += tag.Length;
			if (buf[workingCopyCurIndex++] != '=')
			{
				throw new DenialOfServiceException("Couldn't find the " + tagName + " tag.", getCopyOfWorkingCopyArr(buf));
			}
		}
		
		private void  match(sbyte[] buf, System.String val, System.String errorString)
		{
			for (int i = 0; i < val.Length; i++)
			{
				if (buf[workingCopyCurIndex + i] != val[i])
				{
					throw new DenialOfServiceException(errorString, getCopyOfWorkingCopyArr(buf));
				}
			}
			workingCopyCurIndex += val.Length;
		}
		
		private void  matchFixMinorVersion(sbyte[] buf)
		{
			matchFixMinorVersion(buf, false, '\x0000');
		}
		
		private void  matchFixMinorVersion(sbyte[] buf, bool checkMinorVersion, char expectedMinorVersion)
		{
			char fixMinorVersion = (char) buf[workingCopyCurIndex++];
			if (fixMinorVersion < '0' || fixMinorVersion > '9')
			{
				throw new DenialOfServiceException("Invalid fix minor version.", getCopyOfWorkingCopyArr(buf));
			}
			
			if (checkMinorVersion && fixMinorVersion != expectedMinorVersion)
			{
				throw new DenialOfServiceException("Version mismatch in the middle of the session, expected " + expectedMinorVersion + " got " + fixMinorVersion, getCopyOfWorkingCopyArr(buf));
			}
		}
		
		private void  matchDelimiter(sbyte[] buf, System.String tagName)
		{
			if (buf[workingCopyCurIndex++] != FxIntegral.com.scalper.fix.FixConstants_Fields.delimiter)
			{
				throw new DenialOfServiceException("Couldn't find the end of " + tagName + " tag", getCopyOfWorkingCopyArr(buf));
			}
		}
		
		/// <summary> the boolean is never used, it is used to distinguish this method from the overloaded version.</summary>
		private void  matchDelimiter(sbyte[] buf, bool b, System.String error)
		{
			if (buf[workingCopyCurIndex++] != FxIntegral.com.scalper.fix.FixConstants_Fields.delimiter)
			{
				throw new DenialOfServiceException(error, getCopyOfWorkingCopyArr(buf));
			}
		}
		
		private void  ensureDelimiterNotNext(sbyte[] buf, System.String tagName)
		{
			if (buf[workingCopyCurIndex] == FxIntegral.com.scalper.fix.FixConstants_Fields.delimiter)
			{
				throw new DenialOfServiceException(tagName + " is null.", getCopyOfWorkingCopyArr(buf));
			}
		}
		
		private int readBodyTag(sbyte[] buf, int maxDigits, System.String tagName)
		{
			int length = 0;
			int i = 0;
			char ch = '0';
			while (i <= maxDigits)
			{
				ch = (char) buf[workingCopyCurIndex++];
				if (ch == FxIntegral.com.scalper.fix.FixConstants_Fields.delimiter)
				{
					workingCopyCurIndex--;
					break;
				}
				
				length *= 10;
				length += ch - '0';
				i++;
			}
			
			if (length == 0 || (ch != FxIntegral.com.scalper.fix.FixConstants_Fields.delimiter))
			{
				throw new DenialOfServiceException("Couldn't find the end of body length field.", getCopyOfWorkingCopyArr(buf));
			}
			
			return length;
		}
		
		private sbyte[] getCopyOfWorkingCopyArr(sbyte[] buf)
		{
			return (sbyte[]) buf.Clone();
		}
		
		/// <summary> This method fully parses the message so as to verify the first 3 mandatory fields and the MsgSeqNum field.</summary>
		/// <param name="blankMessage">a newly instantiated, blank message to populate
		/// </param>
		/// <param name="unparsedMessage		Array">of bytes to parse.
		/// </param>
		/// <returns>  Message represents all the tags and values in the message.
		/// </returns>
		/// <throws>  	FixException </throws>
		public override void  getParsedMessage(Message blankMessage, System.Object unparsedMessageObj, TagHelper tagHelper, bool strictReceiveMode)
		{
			Message message = blankMessage;
			sbyte[] unparsedMessage = (sbyte[]) unparsedMessageObj;
			// stores the first problem, if any, with the message.
			// other problem(s) are ignored and the message is parsed as completely as possible.
			FixMessageFormatException fe = null;
			
			int index = 0;
			int msgLen = unparsedMessage.Length;
			//byte fieldSeparator = FixConstants.delimiter;
			int headerBodyFooter = AbstractMessageConnection.HEADER;
			
			// holds the value part of the tag (i.e. in "35=8", the String "8")
			System.Text.StringBuilder buf = new System.Text.StringBuilder();
			ByteBuffer buf1 = ByteBuffer.allocateDirect(unparsedMessage.Length); //Max length
			int lenToRead = - 1;
			while (index < msgLen)
			{
				buf.Remove(0, buf.Length - 0);
				buf1.clear();
				// first get tag
				
				// the integer value of the tag
				int tag = 0;
				char ch = (char) unparsedMessage[index];
				while (ch >= '0' && ch <= '9')
				{
					tag = tag * 10 + (int) (ch - '0');
					ch = (char) unparsedMessage[++index];
				}
				
				if (tag == 0)
				{
					// either no tag (<SOH>=55<SOH>) or tag is zero (<SOH>0=55<SOH>)
					// or tag starts with illegal character (<SOH>a34=55</SOH>)
					if (fe == null)
						fe = new FixMessageFormatException("Invalid tag number", FixException.REJECT_REASON_INVALID_TAG_NUM);
				}
				// @todo set max tag length?
				
				// next character must be equals sign
				if (ch != FxIntegral.com.scalper.fix.Constants_Fields.tagValueSeparatorChar)
				{
					// tag has illegal character in it (<SOH>35a=55</SOH>) or wrong separator character is used
					// (<SOH>34!55</SOH>)
					if (fe == null)
						fe = new FixMessageFormatException("Invalid tag number", FixException.REJECT_REASON_INVALID_TAG_NUM);
					// skip this tag/value
					while (index < msgLen && (char) unparsedMessage[index] != FxIntegral.com.scalper.fix.Constants_Fields.delimiter)
						index++;
					if (index < msgLen)
						index++;
					continue;
				}
				
				index++;
				
				// now read data
				int startValueIndex = index;
				while (index < msgLen && ((lenToRead != - 1 && ((index - startValueIndex) < lenToRead)) || unparsedMessage[index] != FxIntegral.com.scalper.fix.Constants_Fields.delimiter))
				{
					index++;
				}
				
				if (index >= msgLen)
				{
					// no <SOH> as the last character in message, probably means that body length is wrong
					// this case is already handled earlier as DOS, this should never happen
					// if you need to change this, change it in the "run" method of FIXReaderThread, before
					// the call to frameMessage().
					throw new System.SystemException();
				}
				
				if (index == startValueIndex)
				{
					// no value for tag (<SOH>34=<SOH>)
					if (fe == null)
						fe = new FixMessageFormatException("Tag specified without a value.", FixException.REJECT_REASON_NO_VALUE_FOR_TAG, tag);
					message.addValue(tag, "");
				}
				else
				{
					// value = new String(unparsedMessage, startValueIndex, index - startValueIndex);
					for (int i = startValueIndex; i < index; i++)
					{
						if (tag == FxIntegral.com.scalper.fix.Constants_Fields.TAGSignature_i || tag == FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureData_i)
							buf1.put(unparsedMessage[i]);
						else
							buf.Append((char) unparsedMessage[i]);
					}
					if (tag != FxIntegral.com.scalper.fix.Constants_Fields.TAGSignature_i && tag != FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureData_i)
					{
						message.addValue(tag, buf.ToString());
					}
					else
					{
						try
						{
							buf1.flip();
							sbyte[] bytes = new sbyte[buf1.remaining()];
							buf1.get_Renamed(bytes);
							message.addValue(tag, System.Text.Encoding.GetEncoding(FxIntegral.com.scalper.fix.Constants_Fields.CHARSET_USED).GetString(SupportClass.ToByteArray(bytes)));
						}
						catch (System.Exception e)
						{
                            log.Error(e);
							SupportClass.WriteStackTrace(e, Console.Error);
						}
					}
					if (tag == FxIntegral.com.scalper.fix.Constants_Fields.TAGSignatureLength_i || tag == FxIntegral.com.scalper.fix.Constants_Fields.TAGSecureDataLen_i)
					{
						lenToRead = System.Int32.Parse(buf.ToString().Trim());
					}
					else
						lenToRead = - 1;
				}
				index++;
				
				if (strictReceiveMode)
				{
					int newHeaderBodyFooter = getHeaderBodyFooter(tagHelper, tag);
					if (newHeaderBodyFooter != headerBodyFooter)
					{
						if (newHeaderBodyFooter < headerBodyFooter)
						{
							if (fe == null)
								fe = new FixMessageFormatException("Tag specified out of required order", FixException.REJECT_REASON_TAG_OUT_OF_ORDER, tag);
						}
					}
					headerBodyFooter = newHeaderBodyFooter;
				}
			}
			try
			{
				message.validateChecksumFast(unparsedMessage);
			}
			catch (FixException fe2)
			{
				throw new DenialOfServiceException(fe2.Message);
			}
			
			// message's fields have been set as much as possible.
			if (fe != null)
			{
				// message would normally be rejected at the session level
				if (strictReceiveMode)
					throw fe;
				else
				{
                    log.Warn("FFillFixSession message parsing: Would have sent session-level reject for message " + message + " but in lenient receive mode, passing on to the application.");
                    log.Warn("FFillFixSession message parsing: Error was " + fe.Message);
                    log.Warn("FFillFixSession message parsing: Message was " + ScalperFixSession.getString(unparsedMessage));
					//Logging.warn("FFillFixSession message parsing: Would have sent session-level reject for message " + message + " but in lenient receive mode, passing on to the application.");
					//Logging.warn("FFillFixSession message parsing: Error was " + fe.Message);
					//Logging.warn("FFillFixSession message parsing: Message was " + FFillFixSession.getString(unparsedMessage));
				}
			}
		}
		
		/// <summary> flushes and closes the underlying channel and wakes up all thread(s) that are waiting for a message to arrive.</summary>
		public override void  close()
		{
			if (conn != null)
			{
				conn.close();
			}
		}
	}
}