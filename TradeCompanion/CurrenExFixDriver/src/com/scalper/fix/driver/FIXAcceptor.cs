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
/// File: FIXAcceptor.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = com.scalper.fix.FixException;
namespace com.scalper.fix.driver
{
	
	/// <summary> <p>Description: This abstract class represents the
	/// server side mechanism to establish communication between counterparties at
	/// FIX connection level.</p>
	/// <p>Copyright: Copyright (c) 2005<br>
	/// Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public abstract class FIXAcceptor:FIXListenRegistry
	{
		
		/// <summary> Abstract method that starts listening for incoming connections.
		/// As each connections comes in, FIXNotice object will be formed embedding
		/// the FIXConnection object and registered listeners will get corresponding notice.
		/// </summary>
		/// <throws>  FixException </throws>
		public abstract void  start();
		
		/// <summary> Abstract method that stops listening for incoming connections.
		/// Corresponding notice will be sent to registered listeners.
		/// </summary>
		/// <throws>  FixException </throws>
		public abstract void  stop();
	}
}