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
/// File: FIXListenRegistry.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = Dukascopy.com.scalper.fix.FixException;
using Dukascopy.com.scalper.util;
using log4net;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	/// <summary> <p>Description:This class represents the registry that maintains a
	/// FIX listener and its interests to receive notifications for the events.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  
	/// </author>
	/// <version>  1.0
	/// </version>
	public class FIXListenRegistry
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(FIXListenRegistry));
		public FIXListenRegistry()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			listenerInterests = new bool[FIXNotice.NUM_NOTICE_TYPES];
		}
		/// <summary> Notice listener that will be listening for event notifications.</summary>
		protected internal FIXNoticeListener listener;
		
		/// <summary> Boolean array that maintains all the listeners registered.
		/// Each element is eigther true or false based on the interest.
		/// Comment for <code>listenersList</code>
		/// </summary>
		protected internal bool[] listenerInterests;
		
		
		/// <summary> Registers a notice listener.</summary>
		/// <param name="listener	FIX">notice listener.
		/// </param>
		/// <param name="all	Flag">that tells whether to listen everything or nothing.
		/// </param>
		public virtual void  registerNoticeListener(FIXNoticeListener listener, bool all)
		{
            if (this.listener != null)

                log.Warn("FIXListenRegistry:registerNoticeListener: 'Listener is already registered, overriding it.'");
				//Logging.warn("FIXListenRegistry:registerNoticeListener: 'Listener is already registered, overriding it.'");
			
			this.listener = listener;
			if (all)
			{
				for (int i = 0; i < listenerInterests.Length; i++)
					listenerInterests[i] = true;
			}
		}
		
		/// <summary> Turns 'off' the notice listener for certain events.</summary>
		/// <param name="events	Array">of event codes that need to be skipped.
		/// </param>
		/// <throws>  FixException </throws>
		public virtual void  skipEventCodes(int[] eventCodes)
		{
			if (listener == null)
				throw new FixException("No notice listener is registered");
			for (int i = 0; i < eventCodes.Length; i++)
			{
				listenerInterests[eventCodes[i]] = false;
			}
		}
		
		/// <summary> Turns 'on' the notice listener for certain events.</summary>
		/// <param name="code	Array">of event codes that need to be listened to.
		/// </param>
		/// <throws>  FixException </throws>
		public virtual void  noteEventCodes(int[] eventCodes)
		{
			if (listener == null)
				throw new FixException("No notice listener is registered");
			for (int i = 0; i < eventCodes.Length; i++)
			{
				listenerInterests[eventCodes[i]] = true;
			}
		}
		
		/// <summary> Turns off a particular event notification.</summary>
		/// <param name="eventCode">event code to skip
		/// </param>
		/// <throws>  FixException </throws>
		public virtual void  skipEvent(int eventCode)
		{
			if (listener == null)
				throw new FixException("No notice listener is registered");
			listenerInterests[eventCode] = false;
		}
		
		
		/// <summary> Turns ON a particular event notification.</summary>
		/// <param name="eventCode">
		/// </param>
		/// <throws>  FixException </throws>
		public virtual void  noteEvent(int eventCode)
		{
			if (listener == null)
				throw new FixException("No notice listener is registered");
			listenerInterests[eventCode] = true;
		}
		
		/// <summary> Unregisters the listener.</summary>
		/// <throws>  FixException </throws>
		public virtual void  unregisterListener()
		{
			if (listener == null)
				throw new FixException("No notice listener is registered");
			listener = null;
		}
	}
}