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
using System;
namespace Icap.com.scalper.fix
{
	
	/// <summary> Fix Constants used by S7 Software Solutions Pvt. Ltd..
	/// 
	/// <p>This is the reference point for any S7 Software Solutions Pvt. Ltd. tags you want to add to a FIX Message.
	/// <p>By convention, custom message types defined within this file begin MSG.  Custom tags begin TAG.
	/// All tags are represented twice: as a string (e.g., TAGRawData) and as a short (e.g., TAGRawData_i).
	/// Using the short value will result in faster running code.
	/// </summary>
	public struct FixConstants_Fields{
		/// <y.exclude>  FOR INTERNAL USE </y.exclude>
		public readonly static System.String lclStr;
		/// <y.exclude>  FOR INTERNAL USE </y.exclude>
		public readonly static System.String YES = "Y";
		/// <y.exclude>  FOR INTERNAL USE </y.exclude>
		public readonly static System.String NO = "N";
		public readonly static System.String MSGSequenceReset = "XX";
		// Side
		public readonly static System.String SideBuyStr = "Buy";
		public readonly static System.String SideBuyStr_alt0 = "B";
		public readonly static System.String SideSellStr = "Sell";
		public readonly static System.String SideSellStr_alt0 = "S";
		public readonly static char SideBuy = '1';
		public readonly static char SideSell = '2';
		public readonly static System.String SideBuy_s = "1";
		public readonly static System.String SideSell_s = "2";
		// ExecType
		public readonly static char ExecTypeNew = '0';
		public readonly static char ExecTypePartialFill = '1';
		public readonly static char ExecTypeFill = '2';
		public readonly static char ExecTypeDoneForDay = '3';
		public readonly static char ExecTypeCanceled = '4';
		public readonly static char ExecTypeReplace = '5';
		public readonly static char ExecTypePendingCancel = '6';
		public readonly static char ExecTypeStopped = '7';
		public readonly static char ExecTypeRejected = '8';
		public readonly static char ExecTypeSuspended = '9';
		public readonly static char ExecTypePendingNew = 'A';
		public readonly static char ExecTypeCalculated = 'B';
		public readonly static char ExecTypeExpired = 'C';
		public readonly static char ExecTypeRestated = 'D';
		public readonly static char ExecTypePendingReplace = 'E';
		public readonly static char ExecTypeTrade = 'F';
		public readonly static char ExecTypeTradeCorrect = 'G';
		public readonly static char ExecTypeTradeCancel = 'H';
		public readonly static char ExecTypeStatus = 'I';
		public readonly static char ExecTypeManualTrade = 'Y';
		public readonly static char ExecTypeNone;
		// ExecType
		public readonly static System.String ExecTypeNew_s = "0";
		public readonly static System.String ExecTypePartialFill_s = "1";
		public readonly static System.String ExecTypeFill_s = "2";
		public readonly static System.String ExecTypeDoneForDay_s = "3";
		public readonly static System.String ExecTypeCanceled_s = "4";
		public readonly static System.String ExecTypeReplace_s = "5";
		public readonly static System.String ExecTypePendingCancel_s = "6";
		public readonly static System.String ExecTypeStopped_s = "7";
		public readonly static System.String ExecTypeRejected_s = "8";
		public readonly static System.String ExecTypeSuspended_s = "9";
		public readonly static System.String ExecTypePendingNew_s = "A";
		public readonly static System.String ExecTypeCalculated_s = "B";
		public readonly static System.String ExecTypeExpired_s = "C";
		public readonly static System.String ExecTypeRestated_s = "D";
		public readonly static System.String ExecTypePendingReplace_s = "E";
		public readonly static System.String ExecTypeTrade_s = "F";
		public readonly static System.String ExecTypeTradeCorrect_s = "G";
		public readonly static System.String ExecTypeTradeCancel_s = "H";
		public readonly static System.String ExecTypeStatus_s = "I";
		public readonly static System.String ExecTypeManualTrade_s = "Y";
		// OrdStatus
		public readonly static char OrdStatusNew = '0';
		public readonly static char OrdStatusPartiallyFilled = '1';
		public readonly static char OrdStatusFilled = '2';
		public readonly static char OrdStatusDoneForDay = '3';
		public readonly static char OrdStatusCanceled = '4';
		public readonly static char OrdStatusReplaced = '5';
		public readonly static char OrdStatusPendingCancel = '6';
		public readonly static char OrdStatusStopped = '7';
		public readonly static char OrdStatusRejected = '8';
		public readonly static char OrdStatusSuspended = '9';
		public readonly static char OrdStatusPendingNew = 'A';
		public readonly static char OrdStatusCalculated = 'B';
		public readonly static char OrdStatusExpired = 'C';
		public readonly static char OrdStatusAcceptedForBidding = 'D';
		public readonly static char OrdStatusPendingReplace = 'E';
		public readonly static char OrdStatusRestated = 'T';
		public readonly static char OrdStatusPendingFill = 'U';
		public readonly static char OrdStatusAdminAck = 'V';
		public readonly static char OrdStatusBuilding = 'W';
		public readonly static char OrdStatusDeleted = 'X';
		public readonly static char OrdStatusManualTrade = 'Y';
		public readonly static char OrdStatusManualCancel = 'Z';
		public readonly static char OrdStatusNone;
		// OrdStatus
		public readonly static System.String OrdStatusNew_s = "0";
		public readonly static System.String OrdStatusPartiallyFilled_s = "1";
		public readonly static System.String OrdStatusFilled_s = "2";
		public readonly static System.String OrdStatusDoneForDay_s = "3";
		public readonly static System.String OrdStatusCanceled_s = "4";
		public readonly static System.String OrdStatusReplaced_s = "5";
		public readonly static System.String OrdStatusPendingCancel_s = "6";
		public readonly static System.String OrdStatusStopped_s = "7";
		public readonly static System.String OrdStatusRejected_s = "8";
		public readonly static System.String OrdStatusSuspended_s = "9";
		public readonly static System.String OrdStatusPendingNew_s = "A";
		public readonly static System.String OrdStatusCalculated_s = "B";
		public readonly static System.String OrdStatusExpired_s = "C";
		public readonly static System.String OrdStatusAcceptedForBidding_s = "D";
		public readonly static System.String OrdStatusPendingReplace_s = "E";
		public readonly static System.String OrdStatusRestated_s = "T";
		public readonly static System.String OrdStatusPendingFill_s = "U";
		public readonly static System.String OrdStatusAdminAck_s = "V";
		public readonly static System.String OrdStatusBuilding_s = "W";
		public readonly static System.String OrdStatusDeleted_s = "X";
		public readonly static System.String OrdStatusManualTrade_s = "Y";
		public readonly static System.String OrdStatusManualCancel_s = "Z";
		// WHEN YOU ADD TAGS MAKE SURE TO UPDATE THE EndTagIndex
		public readonly static System.String TAGDisplay = "9140";
		public readonly static System.String TAGLiquidityFlag = "9882";
		public readonly static short TAGDisplay_i = 9140;
		public readonly static short TAGLiquidityFlag_i = 9882;
		public readonly static int fixVersionID40 = 0;
		public readonly static int fixVersionID41 = 1;
		public readonly static int fixVersionID42 = 2;
		public readonly static int fixVersionID43 = 3;
		public readonly static int fixVersionID44 = 4;
		public readonly static System.String fixVersion40 = "4.0";
		public readonly static System.String fixVersion41 = "4.1";
		public readonly static System.String fixVersion42 = "4.2";
		public readonly static System.String fixVersion43 = "4.3";
		public readonly static System.String fixVersion44 = "4.4";
		public readonly static System.String beginString40 = "FIX.4.0";
		public readonly static System.String beginString41 = "FIX.4.1";
        public readonly static System.String beginString42 = "FIXT.1.1";//giriICAP
		public readonly static System.String beginString43 = "FIX.4.3";
		public readonly static System.String beginString44 = "FIX.4.4";
        public readonly static System.String beginString50 = "FIXT.1.1";
		public readonly static System.String[] legalBeginStrings;
		public readonly static System.String[] tags;
		/// <summary>This size is consistant with TCP/IP packet size. This data is collected from the RFC 879:
		/// The default IP Maximum Datagram Size is 576.
		/// The default TCP Maximum Segment Size is 536.
		/// </summary>
		public readonly static int PACKET_SIZE = 1024;
		/// <summary> Number of byte buffers to be used in inbound buffer pool.</summary>
		public readonly static int INBOUND_BUF_COUNT = 20;
		/// <summary> Maximum body length digits, for now its assumed 5 which will count for
		/// maximum of 99999.
		/// </summary>
		public readonly static int MAX_BODY_LENGTH_DIGITS = 5; //1 Megabyte
		/// <summary> FIX message should begin with this pattern.</summary>
		public readonly static System.String beginStringPattern = "8=";
		/// <summary> Begining of body length tag.</summary>
		public readonly static System.String bodyLengthPattern = "9=";
		/// <summary> FIX mesage type pattern.</summary>
		public readonly static System.String msgTypePattern = "35=";
		/// <summary> Temperory constants that tells how many bytes has to be skipped
		/// after begin string pattern to reach a delimiter pattern.
		/// </summary>
		public readonly static int MAX_SKIPS = 5;
		/// <summary> Begining of checksum tag.</summary>
		public readonly static System.String checksumPattern = "10=";
		/// <summary> Each tag-value pair is separated by this character.</summary>
		public readonly static System.String tagValueSeparator = "=";
		/// <summary> Each field ends with this delimiter.</summary>
		public readonly static char delimiter;
		/// <summary> Maximum checksum digits. Its should always be 3 unless FIX spec changes.</summary>
		public readonly static int MAX_CHECKSUM_DIGITS = 3;
		/// <summary> Fix message size that is good enough to hold
		/// Beginstring and bodylength tag/value.
		/// </summary>
		public readonly static int START_FIXMSG_SIZE;
		/// <summary> Fix message size that is good enough to hold
		/// Beginstring and bodylength tag/value.
		/// </summary>
		public readonly static int END_FIXMSG_SIZE;
		/// <summary> Session ramp up time, to make sure threads are initialezed started succesfully</summary>
		public readonly static int SESION_RAMPUP_TIME = 2000;
		/// <summary> Session ramp down time, to make sure the threads have finished their partial work.</summary>
		public readonly static int SESSION_RAMPDOWN_TIME = 2000;
		/// <summary> Time to wait/sleep before closing the socket,
		/// after the detection of junk message
		/// </summary>
		public readonly static int WAIT_AFTER_ATTACK = 5000;
		/// <summary> Transmission time(delta) for message to reach other end.
		/// for now its taken as 100ms.
		/// </summary>
		public readonly static int TRANSMISSION_TIME = 100;
		/// <summary> Default maximum size of inbound persistent storage.</summary>
		// @todo add to FixConstants
		public readonly static System.String TAGDropMessage = "20004";
		// on a login message in test mode, sets server's next sent msgSeqNum
		public readonly static System.String TAGLogonReceiveMsgSeqNum = "20005";
		// on a login message in test mode, sets server's next sent expected msgSeqNum (from client)
		public readonly static System.String TAGLogonSendMsgSeqNum = "20006";
		static FixConstants_Fields()
		{
			lclStr = Icap.com.scalper.util.FFillFixGlobals.CopyRightStr;
			ExecTypeNone = (char) SupportClass.Identity(- 1);
			OrdStatusNone = (char) SupportClass.Identity(- 1);
			legalBeginStrings = new System.String[]{Icap.com.scalper.fix.FixConstants_Fields.beginString40, Icap.com.scalper.fix.FixConstants_Fields.beginString41, Icap.com.scalper.fix.FixConstants_Fields.beginString42, Icap.com.scalper.fix.FixConstants_Fields.beginString43, Icap.com.scalper.fix.FixConstants_Fields.beginString44};
			tags = new System.String[500];
			delimiter = (char) 1;
			START_FIXMSG_SIZE = Icap.com.scalper.fix.FixConstants_Fields.beginStringPattern.Length + Icap.com.scalper.fix.FixConstants_Fields.MAX_SKIPS + 1 + Icap.com.scalper.fix.FixConstants_Fields.bodyLengthPattern.Length + Icap.com.scalper.fix.FixConstants_Fields.MAX_BODY_LENGTH_DIGITS + 1;
			END_FIXMSG_SIZE = Icap.com.scalper.fix.FixConstants_Fields.checksumPattern.Length + Icap.com.scalper.fix.FixConstants_Fields.MAX_CHECKSUM_DIGITS + 1;
		}
	}
	public interface FixConstants
	{
		
	}
}