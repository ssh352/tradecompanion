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
/// File: AbstractMessageConnection.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using TagHelper = Dukascopy.com.scalper.fix.TagHelper;
namespace  Dukascopy.com.scalper.fix.driver
{
	
	/// <summary> has some convenience methods to make it easier to implement MessageConnection</summary>
	public abstract class AbstractMessageConnection : MessageConnection
	{
		public abstract Dukascopy.com.scalper.fix.driver.FIXConnection Connection{get;}
		public abstract System.Object MessageFirstPass{get;}
		protected internal const int HEADER = 0;
		protected internal const int BODY = 1;
		protected internal const int FOOTER = 2;
		
		public AbstractMessageConnection()
		{
			// no-op
		}
		
		/// <summary> returns 0, 1, or 2 depending on whether the tag is header, body, or footer.</summary>
		protected internal virtual int getHeaderBodyFooter(TagHelper tagHelper, int tag)
		{
			if (isTagHeader(tagHelper, tag))
			{
				return 0;
			}
			else
			{
				return isTagFooter(tagHelper, tag)?2:1;
			}
		}
		
		private bool isTagHeader(TagHelper tagHelper, int tag)
		{
			// shouldn't really be null, only if this is the server and client's logon message couldn't be parsed.
			return tagHelper.isTagHeader(tag);
		}
		
		private bool isTagFooter(TagHelper tagHelper, int tag)
		{
			// shouldn't really be null, only if this is the server and client's logon message couldn't be parsed.
			return tagHelper.isTagFooter(tag);
		}
		
		protected internal virtual System.String getString(sbyte[] buf)
		{
            return ScalperFixSession.getString(buf);
		}
		public abstract void  close();
		public abstract char getFixMinorVersion(System.Object param1);
        public abstract void getParsedMessage(Dukascopy.com.scalper.fix.Message param1, System.Object param2, Dukascopy.com.scalper.fix.TagHelper param3, bool param4);
		public abstract void  sendMessages(System.Object[] param1);
        public abstract void sendMessage(Dukascopy.com.scalper.fix.Message param1);
	}
}