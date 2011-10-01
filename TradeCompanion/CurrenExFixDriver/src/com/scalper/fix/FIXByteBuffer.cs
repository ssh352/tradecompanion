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
namespace com.scalper.fix
{
	
	public class FIXByteBuffer
	{
		virtual public sbyte[] Bytes
		{
			get
			{
				return theBytes;
			}
			
		}
		virtual public com.scalper.util.ByteBuffer ByteBuffer
		{
			get
			{
				return com.scalper.util.ByteBuffer.wrap(theBytes, 0, count);
			}
			
		}
		
		internal static readonly char[] DigitTens = new char[]{'0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '2', '2', '2', '2', '2', '2', '2', '2', '2', '2', '3', '3', '3', '3', '3', '3', '3', '3', '3', '3', '4', '4', '4', '4', '4', '4', '4', '4', '4', '4', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '6', '6', '6', '6', '6', '6', '6', '6', '6', '6', '7', '7', '7', '7', '7', '7', '7', '7', '7', '7', '8', '8', '8', '8', '8', '8', '8', '8', '8', '8', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9'};
		
		internal static readonly char[] DigitOnes = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
		
		/// <summary> All possible chars for representing a number as a String</summary>
		internal static readonly char[] digits = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
		
		public static sbyte fixByteSeparator = (sbyte) 1;
		public static sbyte fixByteEqual = (sbyte) '=';
		
		public sbyte[] theBytes;
		public int count;
		
		public FIXByteBuffer():this(500)
		{
		}
		
		public FIXByteBuffer(int length)
		{
			if (length > 0)
			{
				theBytes = new sbyte[length];
			}
		}
		
		public void  clear()
		{
			count = 0;
		}
		
		private void  expandCapacity(int minimumCapacity)
		{
			int newCapacity = (theBytes.Length + 1) * 2;
			if (newCapacity < 0)
			{
				newCapacity = System.Int32.MaxValue;
			}
			else if (minimumCapacity > newCapacity)
			{
				newCapacity = minimumCapacity;
			}
			
			sbyte[] newValue = new sbyte[newCapacity];
			Array.Copy(theBytes, 0, newValue, 0, count);
			theBytes = newValue;
		}
		
		public virtual FIXByteBuffer append(Message msg)
		{
			for (int i = 0; i < msg.elementCount; i++)
			{
				append(msg.tagIDs_s[i]).append(FIXByteBuffer.fixByteEqual).append(msg.values[i]).append(FIXByteBuffer.fixByteSeparator);
			}
			
			return this;
		}
		
		public FIXByteBuffer append(System.String str)
		{
			if (str != null)
			{
				int len = str.Length;
				int newcount = count + len;
				if (newcount > theBytes.Length)
					expandCapacity(newcount);
				sbyte[] strBytes = SupportClass.ToSByteArray(SupportClass.ToByteArray(str));
				Array.Copy(strBytes, 0, theBytes, count, len);
				count = newcount;
			}
			return this;
		}
		
		public FIXByteBuffer append(sbyte c)
		{
			int newcount = count + 1;
			if (newcount > theBytes.Length)
				expandCapacity(newcount);
			theBytes[count++] = c;
			return this;
		}
		
		public FIXByteBuffer append(int i)
		{
			appendTo(i, this);
			return this;
		}
		
		public FIXByteBuffer append(long l)
		{
			appendTo(l, this);
			return this;
		}
		
		public FIXByteBuffer append(sbyte[] str, int offset, int len)
		{
			int newcount = count + len;
			if (newcount > theBytes.Length)
				expandCapacity(newcount);
			Array.Copy(str, offset, theBytes, count, len);
			count = newcount;
			return this;
		}
		
		public FIXByteBuffer append(sbyte[] str)
		{
			int newcount = count + str.Length;
			if (newcount > theBytes.Length)
				expandCapacity(newcount);
			Array.Copy(str, 0, theBytes, count, str.Length);
			count = newcount;
			return this;
		}
		
		internal sbyte[] tmpByteToIntBuf = null;
		internal void  appendTo(int i, FIXByteBuffer sb)
		{
			switch (i)
			{
				
				case System.Int32.MinValue: 
					sb.append("-2147483648");
					return ;
				
				case - 3: 
					sb.append("-3");
					return ;
				
				case - 2: 
					sb.append("-2");
					return ;
				
				case - 1: 
					sb.append("-1");
					return ;
				
				case 0: 
					sb.append("0");
					return ;
				
				case 1: 
					sb.append("1");
					return ;
				
				case 2: 
					sb.append("2");
					return ;
				
				case 3: 
					sb.append("3");
					return ;
				
				case 4: 
					sb.append("4");
					return ;
				
				case 5: 
					sb.append("5");
					return ;
				
				case 6: 
					sb.append("6");
					return ;
				
				case 7: 
					sb.append("7");
					return ;
				
				case 8: 
					sb.append("8");
					return ;
				
				case 9: 
					sb.append("9");
					return ;
				
				case 10: 
					sb.append("10");
					return ;
				}
			
			if (tmpByteToIntBuf == null)
				tmpByteToIntBuf = new sbyte[12];
			int charPos = FIXByteBuffer.convertIntToBytes(i, tmpByteToIntBuf);
			sb.append(tmpByteToIntBuf, charPos, 12 - charPos);
		}
		
		internal sbyte[] tmpByteToLongBuf = null;
		internal void  appendTo(long i, FIXByteBuffer sb)
		{
			if (i == System.Int64.MinValue)
			{
				sb.append("-9223372036854775808");
				return ;
			}
			if (tmpByteToLongBuf == null)
				tmpByteToLongBuf = new sbyte[20];
			int charPos = FIXByteBuffer.convertLongToBytes(i, tmpByteToLongBuf);
			sb.append(tmpByteToLongBuf, charPos, (20 - charPos));
			return ;
		}
		
		private static int convertLongToBytes(long i, sbyte[] buf)
		{
			long q;
			int r;
			int charPos = 20;
			char sign = (char) (0);
			
			if (i < 0)
			{
				sign = '-';
				i = - i;
			}
			
			// Get 2 digits/iteration using longs until quotient fits into an int
			while (i > System.Int32.MaxValue)
			{
				q = i / 100;
				// really: r = i - (q * 100);
				r = (int) (i - ((q << 6) + (q << 5) + (q << 2)));
				i = q;
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitOnes[r];
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitTens[r];
			}
			
			// Get 2 digits/iteration using ints
			int q2;
			int i2 = (int) i;
			while (i2 >= 65536)
			{
				q2 = i2 / 100;
				// really: r = i2 - (q * 100);
				r = i2 - ((q2 << 6) + (q2 << 5) + (q2 << 2));
				i2 = q2;
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitOnes[r];
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitTens[r];
			}
			
			// Fall thru to fast mode for smaller numbers
			// assert(i2 <= 65536, i2);
			for (; ; )
			{
				q2 = SupportClass.URShift((i2 * 52429), (16 + 3));
				r = i2 - ((q2 << 3) + (q2 << 1)); // r = i2-(q2*10) ...
				buf[--charPos] = (sbyte) FIXByteBuffer.digits[r];
				i2 = q2;
				if (i2 == 0)
					break;
			}
			if (sign != 0)
			{
				buf[--charPos] = (sbyte) sign;
			}
			return charPos;
		}
		
		private static int convertIntToBytes(int i, sbyte[] buf)
		{
			int q, r;
			int charPos = 12;
			char sign = (char) (0);
			
			if (i < 0)
			{
				sign = '-';
				i = - i;
			}
			
			// Generate two digits per iteration
			while (i >= 65536)
			{
				q = i / 100;
				// really: r = i - (q * 100);
				r = i - ((q << 6) + (q << 5) + (q << 2));
				i = q;
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitOnes[r];
				buf[--charPos] = (sbyte) FIXByteBuffer.DigitTens[r];
			}
			
			// Fall thru to fast mode for smaller numbers
			// assert(i <= 65536, i);
			for (; ; )
			{
				q = SupportClass.URShift((i * 52429), (16 + 3));
				r = i - ((q << 3) + (q << 1)); // r = i-(q*10) ...
				buf[--charPos] = (sbyte) FIXByteBuffer.digits[r];
				i = q;
				if (i == 0)
					break;
			}
			if (sign != 0)
			{
				buf[--charPos] = (sbyte) sign;
			}
			return charPos;
		}
		
		public System.String toMsg()
		{
			return new System.String(SupportClass.ToCharArray(theBytes), 0, count);
		}
		
		public FIXByteBuffer mirror()
		{
			FIXByteBuffer mirrorBuffer = new FIXByteBuffer(- 1);
			mirrorBuffer.theBytes = theBytes;
			mirrorBuffer.count = count;
			theBytes = new sbyte[count];
			count = 0;
			return mirrorBuffer;
		}
	}
}