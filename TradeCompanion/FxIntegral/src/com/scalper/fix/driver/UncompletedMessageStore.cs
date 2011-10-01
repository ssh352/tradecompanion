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
namespace  FxIntegral.com.scalper.fix.driver
{
	
	/// <summary> a helper class used to keep track of which received application-level messages
	/// have been marked as complete by the application.
	/// This class is safe for concurrent use by multiple threads.
	/// </summary>
	class UncompletedMessageStore
	{
		virtual public int LowestUncompletedIndex
		{
			get
			{
				return lowestUncompleted;
			}
			
		}
		private const bool printFlag = false;
		// the amount of bytes that would have to be saved before BitSet is shifted over
		private const int COMPACT_THRESHOLD = 8;
		
		private System.Collections.BitArray completedBitSet;
		private int lowestUncompleted = - 1;
		// represents the offset between the numbers in the bitset and the real MsgSeqNum that they represent.
		private int offset;
		private bool errorOnInit;
		
		public UncompletedMessageStore()
		{
			completedBitSet = new System.Collections.BitArray(64);
			offset = 0;
		}
		
		public virtual void  init(int startMsgSeqNum)
		{
			lock (this)
			{
				lowestUncompleted = startMsgSeqNum;
			}
		}
		
		public virtual int completeMessage(int msgSeqNum)
		{
			lock (this)
			{
				
				if (lowestUncompleted < 0)
				{
					if (errorOnInit)
						throw new System.SystemException("Must call init first");
					else
						errorOnInit = true;
				}
				if (msgSeqNum < lowestUncompleted)
				{
					return - 1;
				}
				SupportClass.BitArraySupport.Set(completedBitSet, msgSeqNum - offset);
				if (msgSeqNum > lowestUncompleted)
				{
					return - 1;
				}
				while (completedBitSet.Get(msgSeqNum - offset))
				{
					msgSeqNum++;
				}
				lowestUncompleted = msgSeqNum;
				if (lowestUncompleted - offset > UncompletedMessageStore.COMPACT_THRESHOLD)
					compactBitSet();
				
				return lowestUncompleted - 1;
			}
		}
		
		private void  compactBitSet()
		{
			int amountToShift = (lowestUncompleted - offset);
            System.Collections.BitArray tempCompletedBitSet = new System.Collections.BitArray(completedBitSet.Length);
			for (int i = lowestUncompleted - offset , j=0 ; i < completedBitSet.Length; i++,j++)
            {
                tempCompletedBitSet[j] = completedBitSet.Get(i);
            }
            completedBitSet = tempCompletedBitSet; //completedBitSet.get_Renamed(lowestUncompleted - offset, completedBitSet.Length);
			offset += amountToShift;
			
		}
		
		
	}
}