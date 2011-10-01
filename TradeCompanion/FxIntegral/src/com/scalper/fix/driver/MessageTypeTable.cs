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
using Message = FxIntegral.com.scalper.fix.Message;
namespace  FxIntegral.com.scalper.fix.driver
{
	
	public class MessageTypeTable
	{
		private const bool defaultPublic = false;
		private const bool defaultEventEnabled = false;
		
		private System.Collections.IDictionary messageTypeInfoMap;
		// represents all applications messages not otherwise specified.
		private MessageTypeInfo applicationInfo;
		// represents all admin messages not otherwise specified.
		private MessageTypeInfo adminInfo;
		
		public MessageTypeTable()
		{
			messageTypeInfoMap = new System.Collections.Hashtable();
		}
		
		public virtual bool isPublic(Message msg)
		{
			//String msgType = msg.getMsgType();
			MessageTypeInfo info = getInfo(msg);
			if (info == null)
			{
				return MessageTypeTable.defaultPublic;
			}
			return info.isPublic();
		}
		
		public virtual bool isEventEnabled(Message msg)
		{
			//String msgType = msg.getMsgType();
			MessageTypeInfo info = getInfo(msg);
			if (info == null)
			{
				return MessageTypeTable.defaultEventEnabled;
			}
			return info.isEventEnabled();
		}
		
		public virtual void  setAdminDefault(bool isEventEnabled, bool isPublic)
		{
			adminInfo = new MessageTypeInfo(null, isEventEnabled, isPublic);
		}
		
		public virtual void  setApplicationDefault(bool isEventEnabled, bool isPublic)
		{
			applicationInfo = new MessageTypeInfo(null, isEventEnabled, isPublic);
		}
		
		public virtual void  addInfo(MessageTypeInfo info)
		{
			lock (messageTypeInfoMap.SyncRoot)
			{
				messageTypeInfoMap[info.getMsgType()] = info;
			}
		}
		
		private MessageTypeInfo getInfo(Message msg)
		{
			lock (messageTypeInfoMap.SyncRoot)
			{
				// first look in hash table
				MessageTypeInfo info = (MessageTypeInfo) messageTypeInfoMap[msg.MsgType];
				if (info != null)
				{
					return info;
				}
				
				// look at globals
				return msg.Admin?adminInfo:applicationInfo;
			}
		}
	}
}