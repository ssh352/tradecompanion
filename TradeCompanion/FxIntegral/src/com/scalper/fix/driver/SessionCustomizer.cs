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
	
	public abstract class SessionCustomizer
	{
		
		/// <summary> should the given message be resent?
		/// If this session receives a resend request, the driver can respond either by retransmitting the message
		/// with the PossDupe flag set to true, or by sending an administrative "placeholder" message to fill the
		/// gap.  One example of a situation where an application would choose not to retransmit a message would be
		/// an aged order, because the market may have changed since the order was first sent.
		/// <p>
		/// When asked to retransmit a message, the application will normally do one of three things:
		/// <ol><li>return true, meaning the message should be resent.  The other side will receive the message with the
		/// "PossDupe" flag set.
		/// <li>return false, meaning the message should not be resent.  An administrative "placeholder" message will
		/// be sent instead.
		/// <li>return false but also send a new message (for example an order status message.)  DO NOT call
		/// S7 Software Solutions Pvt. Ltd.FixSession.transmit from inside the implemention of this method, as new messages cannot be put onto the
		/// wire until all relevant messages have been resent and a deadlock will result.  Instead call
		/// S7 Software Solutions Pvt. Ltd.FixSession.transmitLater, which will enqueue the message(s) until after the resend has completed.
		/// <p>
		/// The default behavior is to resend all messages.
		/// </summary>
		protected internal virtual bool shouldResendMessage(Message msg)
		{
			return true;
		}
	}
}