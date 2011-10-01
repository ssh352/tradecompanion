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
/// File: FIXInitiator.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = FxIntegral.com.scalper.fix.FixException;
namespace  FxIntegral.com.scalper.fix.driver
{
	
	/// <summary> <p>Description: This abstract class represents the
	/// client side mechanism to establish communication link between counterparties.
	/// <p>Copyright: Copyright (c) 2005<br>
	/// Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public abstract class FIXInitiator:FIXListenRegistry
	{
		/// <summary> Abstract method that starts the process of establishing connection
		/// to an acceptor. Once the connection is established, FIXNotice object
		/// will be formed by embedding the FIXConnection object and registered
		/// listeners will get corresponding notice.
		/// </summary>
		/// <throws>  FixException </throws>
		public abstract void  start();
		
		/// <summary> Abstract method that disconnects from an acceptor.
		/// Corresponding notice will be sent to registered listeners.
		/// </summary>
		/// <throws>  FixException </throws>
		public abstract void  stop();
	}
}