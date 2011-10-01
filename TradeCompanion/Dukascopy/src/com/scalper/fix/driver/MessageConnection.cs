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
/// File: S7FIXSession.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixMessageContentException = Dukascopy.com.scalper.fix.FixMessageContentException;
using FixMessageFormatException = Dukascopy.com.scalper.fix.FixMessageFormatException;
using Message = Dukascopy.com.scalper.fix.Message;
using TagHelper = Dukascopy.com.scalper.fix.TagHelper;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	/// <summary> something that knows how to transform a FIX message into a certain format and send it across a certain kind of transport</summary>
	public struct MessageConnection_Fields{
		/// <summary>the number index where the minor version is found ("8=FIX.4.X").
		/// The value is 8, i.e. it is the 9th character so if it is in an array it will be at index 8.
		/// </summary>
		public readonly static int FIX_MINOR_VERSION_INDEX;
		static MessageConnection_Fields()
		{
			FIX_MINOR_VERSION_INDEX = "8=FIX.4.X".Length - 1;
		}
	}
	public interface MessageConnection
	{
		/// <summary> fully reads from the transport until a message is available and returns an implementation-dependant form
		/// for the message.  This method will block until a new message is available.
		/// </summary>
		System.Object MessageFirstPass
		{
			get;
			
		}
		FIXConnection Connection
		{
			// @todo remove
			
			get;
			
		}
		
		/// <summary> sends the message over the channel, performing any conversion that may be necessary</summary>
		void  sendMessage(Message msg);
		
		/// <summary> sends the batch of messages over the channel, performing any conversion that may be necessary</summary>
		void  sendMessages(System.Object[] msg);
		
		/// <summary> parses the temporary message from getMessageFirstPass and returns a single character
		/// representing the FIX minor version.
		/// </summary>
		char getFixMinorVersion(System.Object tmpMessage);
		
		/// <summary> completes parsing the message.
		/// Will throw a FixException to indicate a reject or a DenialOfServiceException to indicate a fatal error.
		/// (e.g. checksum failure)
		/// </summary>
		void  getParsedMessage(Message blankMessage, System.Object unparsedMessage, TagHelper tagHelper, bool strictReceiveMode);
		
		/// <summary> flushes and closes the underlying channel and wakes up all thread(s) that are waiting for a message to arrive.</summary>
		void  close();
	}
}