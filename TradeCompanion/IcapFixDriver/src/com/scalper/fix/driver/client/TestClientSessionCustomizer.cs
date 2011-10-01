/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd. Software Inc. ("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
using ClientSessionCustomizer = Icap.com.scalper.fix.driver.ClientSessionCustomizer;
using FIXPersister = Icap.com.scalper.fix.driver.FIXPersister;
using FIXVolatilePersister = Icap.com.scalper.fix.driver.FIXVolatilePersister;
namespace Icap.com.scalper.fix.driver.client
{
	
	/// <summary> a SessionCustomizer that can be used for testing purposes.
	/// uses an in-memory persister.
	/// </summary>
	class TestClientSessionCustomizer:ClientSessionCustomizer
	{
		/// <summary> returns an in-memory persister.</summary>
		override public FIXPersister Persister
		{
			get
			{
				return new FIXVolatilePersister(lastInSeqNum, lastOutSeqNum);
			}
			
		}
		private int lastInSeqNum;
		private int lastOutSeqNum;
		
		/// <summary> normal test session customizer</summary>
		public TestClientSessionCustomizer():this(0, 0)
		{
		}
		
		/// <summary> test session customizer that starts on a certain inbound/outbound message
		/// sequence number.
		/// </summary>
		public TestClientSessionCustomizer(int lastInSeqNum, int lastOutSeqNum)
		{
			this.lastInSeqNum = lastInSeqNum;
			this.lastOutSeqNum = lastOutSeqNum;
		}
	}
}