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
/// File: FIXPersister.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = Icap.com.scalper.fix.FixException;
using Message = Icap.com.scalper.fix.Message;
namespace Icap.com.scalper.fix.driver
{
	/// <summary> <p>Description: This abstract class represents the persistant message store.
	/// Provides facility to store/retrieve the FIX messages based on the message
	/// sequence numbers.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	public struct FIXPersister_Fields{
		public readonly static System.Collections.ArrayList EMPTY_VECTOR;
		static FIXPersister_Fields()
		{
			EMPTY_VECTOR = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
		}
	}
	public interface FIXPersister
	{
		/// <summary> Returns last inbound message sequence number.</summary>
		/// <returns> Last inbound message sequence number.
		/// </returns>
		int LastInSeqNo
		{
			get;
			
		}
		/// <summary> Returns last outbound message sequence number.</summary>
		/// <returns> Last outbound message sequence number.
		/// </returns>
		int LastOutSeqNo
		{
			get;
			
		}
		
		/// <summary> Called whenever an admin message is sent out from the session.
		/// Assigns the next sequence number and persists it, but does not store the message itself.
		/// After the message sequence number has been persisted the message will be sent
		/// over the communication link.
		/// </summary>
		/// <param name="message">	an admin FIX message (e.g. heartbeat, logon)
		/// </param>
		/// <throws>  	FixException </throws>
		void  storeOutboundAdminMessage(Message message);
		
		/// <summary> Called whenever an application-level message is sent out from the session.
		/// Assigns the next sequence number and persists it.
		/// Also persists the message.
		/// After the message has been persisted the message will be sent
		/// over the communication link.
		/// </summary>
		/// <param name="message">	FIX business message (e.g. New Order Single)
		/// </param>
		/// <throws>  	FixException </throws>
		void  storeOutboundAppMessage(Message message);
		
		/// <summary> Called whenever an application-level message is sent out from the session.
		/// Assigns the next sequence number and persists it.
		/// Also persists the message.
		/// After the message has been persisted the message will be sent
		/// over the communication link.
		/// </summary>
		/// <param name="message">	FIX business message (e.g. New Order Single)
		/// </param>
		/// <throws>  	FixException </throws>
		void  storeOutboundMessages(System.Object[] message, int offset, int count);
		
		/// <summary> This is the relative get method that returns the next fix message from the store
		/// that has not yet read.
		/// </summary>
		/// <returns> next available Message.
		/// </returns>
		/// <throws>  FixException </throws>
		/// <summary> 
		/// public Message getNextFIXMessage()
		/// throws FixException
		/// {
		/// return getFIXMessage(lastOutSeqNo+1);
		/// } 
		/// </summary>
		
		/// <summary> Retrieves all the messages specified in the range, messages are parsed and
		/// returned as Message objects.  Fix versions 4.0 and 4.1 use 999,999 to mean infinity;
		/// Fix versions after 4.1 use 0 to mean infinity.  Implementations must treat both of these
		/// values as infinity and return all messages from startSeqNo and onward.
		/// </summary>
		/// <param name="startSeqNo	Starting">sequence number of the range.
		/// </param>
		/// <param name="endSeqNo		Last">sequence number of the range.	0 and 999,999 are both treated as infinity,
		/// meaning that all messages with message sequence number >= startSeqNo are returned.
		/// </param>
		/// <returns> Vector 		Contains all the messages in specified range.  If there are no messages to return,
		/// returns an empty Vector (e.g. EMPTY_VECTOR)
		/// </returns>
		/// <throws>  FixException </throws>
		//Vector getOutboundFIXMessages(int startSeqNo, int endSeqNo)
		System.Collections.IEnumerator getOutboundFIXMessages(int startSeqNo, int endSeqNo);
		
		System.Collections.ArrayList getOutboundFIXMessagesVector(int startSeqNo, int endSeqNo);
		
		
		/// <summary> Assigns next sequence number and writes the new FIX business
		/// message onto the persistent store.
		/// This method should be used by the initiator session to store the incoming
		/// Message onto the persistant store which inturn, will be sent across the
		/// communication link.
		/// </summary>
		/// <param name="message">	FIX business message.
		/// </param>
		/// <throws>  	FixException </throws>
		void  storeInboundMsgSeqNum(int msgSeqNum);
	}
}