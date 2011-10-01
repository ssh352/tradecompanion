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
/// File: FIXException.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
namespace Icap.com.scalper.fix
{
	
	/// <summary> <p>Description: This is generic S7 Software Solutions Pvt. Ltd. FIX exception.</p>
	/// <p>Copyright: Copyright (c) 2005</p>
	/// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
	/// </summary>
	/// <author>  Phaniraj Raghavendra
	/// </author>
	/// <version>  1.0
	/// </version>
	////("serial")
	[Serializable]
	public class FixException:System.Exception
	{
		/// <summary> Returns the code for the exception.</summary>
		/// <returns>
		/// </returns>
		/// <summary> Sets the code for the exception.</summary>
		/// <param name="code">
		/// </param>
		virtual public int Code
		{
			get
			{
				return code;
			}
			
			set
			{
				this.code = value;
			}
			
		}
		virtual public int RefTagID
		{
			get
			{
				return refTagID;
			}
			
			set
			{
				this.refTagID = value;
			}
			
		}
		virtual public bool ShouldLogOutUser
		{
			set
			{
				shouldLogOutUser_Renamed_Field = value;
			}
			
		}
		/// <summary> Returns the innermost exception in the chain that is still an instance of FIXException
		/// 
		/// </summary>
		/// <returns> the innermost exception
		/// </returns>
		
		/// <summary> Returns the innermost exception message in the chain
		/// 
		/// </summary>
		/// <returns> the innermost exception message
		/// </returns>
		virtual public System.String FirstMessage
		{
			get
			{
                return this.StackTrace;
				
			}
			
		}
		/// <summary> Returns all log messages from the exceptions in the chain,
		/// already converted from the message catalog
		/// 
		/// </summary>
		/// <returns> the converted log messages
		/// </returns>
		virtual public System.String[] LogMessages
		{
			//("unchecked")
			
			get
			{
				
				System.Collections.IList messages = new System.Collections.ArrayList();
				
				System.Exception t = this;
				
				while (t is FixException)
				{
					
					FixException ex = (FixException) t;
					if (ex != null)
					{
						messages.Add(ex.Message);
					}
					// move on to next cause
					//t = ex.getCause();
				}
				
				if (t != null)
				{
					messages.Add(t.Message);
				}
				
				System.String[] result = new System.String[messages.Count];
				SupportClass.ICollectionSupport.ToArray(messages, result);
				
				return result;
			}
			
		}
		
		// @todo most of these variables are never used.
		
		/// <summary> </summary>
		private const long serialVersionUID = 1L;
		
		// reason not set - not part of FIX spec
		public const int REJECT_REASON_NOT_SET = - 1;
		
		// these reasons (from Reject) are from 4.2 onwards
		public const int REJECT_REASON_INVALID_TAG_NUM = 0;
		public const int REJECT_REASON_REQD_TAG_MISSING = 1;
		public const int REJECT_REASON_TAG_UNDEFINED_FOR_MSGTYPE = 2;
		public const int REJECT_REASON_UNDEFINED_TAG = 3;
		public const int REJECT_REASON_NO_VALUE_FOR_TAG = 4;
		public const int REJECT_REASON_VALUE_OUT_OF_RANGE = 5;
		public const int REJECT_REASON_INCORRECT_DATA_FORMAT = 6;
		public const int REJECT_REASON_DECRYPTION_PROBLEM = 7;
		public const int REJECT_REASON_SIGNATURE_PROBLEM = 8;
		public const int REJECT_REASON_COMPID_PROBLEM = 9;
		public const int REJECT_REASON_SENDING_TIME_INACCURATE = 10;
		public const int REJECT_REASON_INVALID_MSGTYPE = 11;
		
		// from 4.3 onwards
		public const int REJECT_REASON_XML_VALIDATION_ERROR = 12;
		public const int REJECT_REASON_DUPLICATE_TAG = 13;
		public const int REJECT_REASON_TAG_OUT_OF_ORDER = 14;
		public const int REJECT_REASON_REPEATING_GROUP_FIELDS_OUT_OF_ORDER = 15;
		public const int REJECT_REASON_INCORRECT_NUMINGROUP_COUNT = 16;
		public const int REJECT_REASON_NON_DATA_HAS_SOH = 17;
		
		// from 4.4 onwards
		public const int REJECT_REASON_OTHER = 99;
		
		// ???
		// @todo remove
		public const int LOGOUT_MSGSEQNUM_TAG_MISSING_i = 200;
		public const System.String LOGOUT_MSGSEQNUM_TAG_MISSING_s = "MsgSeqNum field missing.";
		public const int INVALID_CHECKSUM_i = 201;
		public const System.String INVALID_CHECKSUM_s = "Message contains incorrect value for CheckSum.";
		
		/// <summary>reject reason code, as defined in the spec</summary>
		private int code;
		/// <summary> the tag with the problem</summary>
		private int refTagID;
		private bool shouldLogOutUser_Renamed_Field;
		
		/// <summary> Creates new <code>FIXException</code>.</summary>
		public FixException():this((System.String) null)
		{
		}
		
		/// <summary> Constructs an <code>FIXException</code> with the
		/// specified logging arguments.
		/// 
		/// </summary>
		/// <param name="args">the logging arguments
		/// </param>
		public FixException(System.String s):this(s, FixException.REJECT_REASON_NOT_SET)
		{
		}
		
		public FixException(System.String s, int code):base(s)
		{
			Code = code;
			RefTagID = - 1;
		}
		
		/// <summary> Constructs an <code>FIXException</code> with the specified
		/// embedded <CODE>Throwable</CODE>.
		/// 
		/// </summary>
		/// <param name="t">the exception to chain
		/// </param>
		public FixException(System.Exception t):base()
		{
			Code = FixException.REJECT_REASON_NOT_SET;
			RefTagID = - 1;
		}
		
		public virtual bool shouldLogOutUser()
		{
			return shouldLogOutUser_Renamed_Field;
		}
	}
}