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
/// ****************************************************
/// </summary>
using System;
namespace Icap.com.scalper.fix.driver
{
	
	/// <summary> This class is used to register how to access different FIX messages.
	/// There are 2 ways to access all messages that are sent and received
	/// by the S7FIXSession.
	/// <ul>
	/// <li>{@link CMSSession#getCMSMessage()} - to access messages this way they need to be public; {@link #setPublic(boolean)}</li>
	/// <li>{@link MessageListener} event listener - to access messages this way they need to be eventEnabled; {@link #setEventEnabled(boolean)}</li>
	/// </ul>
	/// 
	/// <p>
	/// By default none of the messages are public.
	/// <br><br> By default the following messages are eventEnabled:
	/// <ul>
	/// <li>CMSMessage.MSGExecutionReport</li>
	/// <li>CMSMessage.MSGAdminResponse</li>
	/// <li>CMSMessage.MSGStatus</li>
	/// <li>CMSMessage.MSGOrderAck</li>
	/// <li>CMSMessage.MSGAdminAck</li>
	/// <li>CMSMessage.MSGCancelReject</li>
	/// <li>CMSMessage.MSGReject</li>
	/// </ul>
	/// </p>
	/// </summary>
	public class MessageTypeInfo
	{
		private System.String msgType;
		
		// Indicates if the message can be captured by the application
		private bool isPublic_Renamed_Field;
		
		// Indicates if the driver engine will trigger an event for this message type
		private bool isEventEnabled_Renamed_Field = true;
		
		// Indicates if the driver engine will process the message internally
		// private boolean isProcessInternal = true;
		
		
		public MessageTypeInfo(System.String msgType, bool isEventEnabled, bool isPublic)
		{
			setMsgType(msgType);
			this.isPublic_Renamed_Field = isPublic;
			this.isEventEnabled_Renamed_Field = isEventEnabled;
			// this.isProcessInternal=isProcessInternal;
		}
		
		/// <summary> Is this message type accessible via the sessions getMessage.
		/// 
		/// </summary>
		/// <returns> is this message type accessible
		/// </returns>
		public virtual bool isPublic()
		{
			return isPublic_Renamed_Field;
		}
		
		/// <summary> Set whether this message type will be accessible via the sessions getMessage.
		/// 
		/// </summary>
		/// <returns> is this message type accessible
		/// </returns>
		private void  setPublic(bool v)
		{
			isPublic_Renamed_Field = v;
		}
		
		/// <summary> Is this message type accessible via the MessageListener.
		/// 
		/// </summary>
		/// <returns> is this message type accessible
		/// </returns>
		public virtual bool isEventEnabled()
		{
			return isEventEnabled_Renamed_Field;
		}
		
		/// <summary> Set whether this message type will be accessible via the MessageListener.</summary>
		private void  setEventEnabled(bool v)
		{
			isEventEnabled_Renamed_Field = v;
		}
		
		public virtual System.String getMsgType()
		{
			return msgType;
		}
		
		private void  setMsgType(System.String v)
		{
			msgType = v;
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("MessageTypeInfo[" + msgType + "] isPublic=" + isPublic_Renamed_Field + " isEventEnabled=" + isEventEnabled_Renamed_Field).ToString();
		}
	}
}