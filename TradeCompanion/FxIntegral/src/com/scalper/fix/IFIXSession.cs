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
//import com.aegisoft.monitor.StatsKeeper;
using System;
namespace FxIntegral.com.scalper.fix
{
	
	public interface IFIXSession
	{
		int RecvSeqNum
		{
			get;
			
			//public int getRecvSeqNum(String BC);
			//public int getSendSeqNum(String BC);
			
			set;
			
		}
		int SendSeqNum
		{
			get;
			
			set;
			
		}
		System.Collections.IEnumerator AllMessages
		{
			get;
			
		}
		System.Collections.ArrayList AllMessagesVector
		{
			get;
			
		}
		System.String SenderSubID
		{
			get;
			
		}
		System.String SenderCompID
		{
			get;
			
		}
		System.String TargetCompID
		{
			get;
			
		}
		System.String LoginName
		{
			get;
			
		}
		int Port
		{
			get;
			
		}
		System.String Address
		{
			get;
			
		}
		bool LoggedIn
		{
			get;
			
		}
		bool Started
		{
			get;
			
		}
		bool Connected
		{
			get;
			
		}
		System.String SessionName
		{
			//public StatsKeeper getStatsKeeper();
			
			get;
			
		}
		System.Collections.IEnumerator getSentMessages(int begin, int end);
		System.Collections.ArrayList getSentMessagesVector(int begin, int end);
		void  clearPersistence();
		void  close();
		void  addMessageListener(MessageListener l);
		void  removeMessageListener(MessageListener l);
		void  sendTestRequest();
		void  resendMsgs(int from, int to);
	}
}