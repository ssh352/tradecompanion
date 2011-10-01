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
using CfgReader = Dukascopy.com.scalper.util.CfgReader;
using log4net;

namespace Dukascopy.com.scalper.fix
{
	
	public class FixDefinitions
	{
		public static CfgReader CfgReader
		{
			get
			{
				return FixDefinitions.curr_CfgReader;
			}
			
		}
		//static Vector customLoadTable;

        private static readonly ILog log = LogManager.GetLogger(typeof(FixDefinitions));

		protected internal static System.String FixVersion;
		protected internal static System.Int64 exchangeTypeID;
		protected internal static System.Boolean IsCMSExchange;
		
		protected internal static FixDefinitions fixDefinitions;
		protected internal static FixDefinitions fix40Definitions = null;
		protected internal static FixDefinitions fix41Definitions = null;
		protected internal static FixDefinitions fix42Definitions = null;
		protected internal static FixDefinitions fix43Definitions = null;
		protected internal static FixDefinitions fix44Definitions = null;
		protected internal static FixDefinitions fixCMSDefinitions = null;
		protected internal static CfgReader curr_CfgReader;
		public System.Collections.Hashtable fixFields = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		public System.Collections.Hashtable validFIXTags = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable(20));
		public System.Collections.Hashtable requiredFIXTags = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable(20));
		public System.Collections.Hashtable encryptedFIXTags = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable(20));
		public int FIXMaxTag = 0;
		
		protected internal FixDefinitions()
		{
		}
		
		public static bool isTagRequired(System.String sMsgType, System.String sTag)
		{
			lock (typeof(Dukascopy.com.scalper.fix.FixDefinitions))
			{
				SupportClass.HashSetSupport hs = (SupportClass.HashSetSupport) FixDefinitions.fixDefinitions.requiredFIXTags[sMsgType];
				if (hs == null)
				{
					return false;
				}
				return hs.Contains(sTag);
			}
		}
		
		public static FixDefinitions getFIXDefinition(System.String inFixVersion, ref System.Boolean inIsCMSExchange)
		{
			lock (typeof(Dukascopy.com.scalper.fix.FixDefinitions))
			{
				if (((FixDefinitions.FixVersion == null) || !FixDefinitions.FixVersion.Equals(inFixVersion)) && (inFixVersion != null))
				{
					FixDefinitions.FixVersion = inFixVersion;
					//FixDefinitions.exchangeTypeID = inExchangeTypeID;
					FixDefinitions.IsCMSExchange = inIsCMSExchange;
					FixDefinitions.fixDefinitions = FixDefinitions.loadFixData();
				}
				return FixDefinitions.fixDefinitions;
			}
		}
		
		public static FixDefinitions getFIXDefinition()
		{
			lock (typeof(Dukascopy.com.scalper.fix.FixDefinitions))
			{
				if (FixDefinitions.fixDefinitions == null)
				{
					System.Boolean tempAux = false;
					FixDefinitions.getFIXDefinition(Dukascopy.com.scalper.fix.FixConstants_Fields.beginString42, ref tempAux);
				}
				return FixDefinitions.fixDefinitions;
			}
		}
		
		public static void  clearFIXDefinition()
		{
			lock (typeof(Dukascopy.com.scalper.fix.FixDefinitions))
			{
				if (FixDefinitions.fixDefinitions != null)
					FixDefinitions.fixDefinitions = null;
			}
		}
		//("unchecked")
		protected internal static FixDefinitions loadFixData(System.String filename)
		{
			FixDefinitions curFixDefs = new FixDefinitions();
			if (filename == null)
			{
				log.Error("Couldn't find file for " + FixDefinitions.FixVersion);
			}
			
			FixDefinitions.curr_CfgReader = CfgReader.getInstance(filename, false);
			FixDefinitions.curr_CfgReader.CaseSensitive = true;
			FixDefinitions.curr_CfgReader.ReadOnly = true;
			
			
			System.Collections.IEnumerator e = FixDefinitions.curr_CfgReader.list("Definitions");
			while (e.MoveNext())
			{
				FieldInfo fieldInfo = new FieldInfo();
				fieldInfo.tagID = ((System.String) e.Current).Trim();
				System.String cfgValue = FixDefinitions.curr_CfgReader.getProperty("Definitions", fieldInfo.tagID);
				
				int idx1 = cfgValue.IndexOf(",");
				fieldInfo.tagName = cfgValue.Substring(0, (idx1) - (0)).Trim();
				cfgValue = cfgValue.Substring(idx1 + 1);
				
				int idx2 = cfgValue.IndexOf(",");
				fieldInfo.tagDataType = cfgValue.Substring(0, (idx2) - (0)).Trim();
				cfgValue = cfgValue.Substring(idx2);
				
				fieldInfo.tagDescription = cfgValue.Substring(1).Trim();
				int idx = fieldInfo.tagDescription.IndexOf("\\n");
				while (idx >= 0)
				{
					fieldInfo.tagDescription = fieldInfo.tagDescription.Substring(0, (idx) - (0)) + "\n" + fieldInfo.tagDescription.Substring(idx + 2);
					idx = fieldInfo.tagDescription.IndexOf("\\n");
				}
				
				System.String value_section = fieldInfo.tagID + "_VALUES";
				System.Collections.IEnumerator e_values = FixDefinitions.curr_CfgReader.list(value_section);
				if (e_values != null)
				{
					while (e_values.MoveNext())
					{
						System.String name = (System.String) e_values.Current;
						System.String value_Renamed = FixDefinitions.curr_CfgReader.getProperty(value_section, name);
						
						if ((name != null) && (value_Renamed != null))
						{
							System.String[] values = new System.String[2];
							values[0] = name.Trim();
							values[1] = value_Renamed.Trim();
							
							if (fieldInfo.tagValidValues == null)
								fieldInfo.tagValidValues = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
							
							fieldInfo.tagValidValues.Add(values);
						}
					}
				}
				//curFixDefs.fixFields.Add

					curFixDefs.fixFields[fieldInfo.tagID] = fieldInfo;
					curFixDefs.fixFields[fieldInfo.tagName] = fieldInfo;
					curFixDefs.fixFields[fieldInfo.tagName.ToUpper()] = fieldInfo;
			}
			
			try
			{
				curFixDefs.FIXMaxTag = System.Int32.Parse(FixDefinitions.curr_CfgReader.getProperty("Defaults", "FIXMaxTag"));
			}
			catch (System.Exception ex)
			{
				//ErrorLog.println(ErrorLog.ERROR, "Failed to load FIXMaxTag "+FixDefinitions.curr_CfgReader.getProperty("Defaults","FIXMaxTag")+" filename="+filename+" FixVersion="+FixDefinitions.FixVersion);
				log.Error("Failed to load FIXMaxTag " + FixDefinitions.curr_CfgReader.getProperty("Defaults", "FIXMaxTag") + " filename=" + filename + " FixVersion=" + FixDefinitions.FixVersion, ex);
			}
			
			FixDefinitions.buildTagsTable(FixDefinitions.curr_CfgReader, "Required_Tags", curFixDefs.requiredFIXTags);
			FixDefinitions.buildTagsTable(FixDefinitions.curr_CfgReader, "Valid_Tags", curFixDefs.validFIXTags);
			FixDefinitions.buildTagsTable(FixDefinitions.curr_CfgReader, "Encrypted_Tags", curFixDefs.encryptedFIXTags);
			return curFixDefs;
		}
		protected internal static FixDefinitions loadFixData()
		{
			FixDefinitions.fixDefinitions = FixDefinitions.loadFixDataToDefinitions();
			return FixDefinitions.fixDefinitions;
		}
		static public FixDefinitions getFixDefinitions(System.String FixVersion)
		{
			System.String filename;
			
			try
			{
				char minorVersion = BasicFixUtilities.getFixMinorVersion(FixVersion);
				if (minorVersion >= '0' && minorVersion <= '4')
				{
					//filename = "..\\config\\fix4" + minorVersion + ".dat";
                    filename = "fix4" + minorVersion + ".dat";
					switch (minorVersion)
					{
						
						case '0': 
							if (FixDefinitions.fix40Definitions != null)
								return FixDefinitions.fix40Definitions;
							FixDefinitions.fix40Definitions = FixDefinitions.loadFixData(filename);
							return FixDefinitions.fix40Definitions;
						
						case '1': 
							if (FixDefinitions.fix41Definitions != null)
								return FixDefinitions.fix41Definitions;
							FixDefinitions.fix41Definitions = FixDefinitions.loadFixData(filename);
							return FixDefinitions.fix41Definitions;
						
						case '2': 
							if (FixDefinitions.fix42Definitions != null)
								return FixDefinitions.fix42Definitions;
							FixDefinitions.fix42Definitions = FixDefinitions.loadFixData(filename);
							return FixDefinitions.fix42Definitions;
						
						case '3': 
							if (FixDefinitions.fix43Definitions != null)
								return FixDefinitions.fix43Definitions;
							FixDefinitions.fix43Definitions = FixDefinitions.loadFixData(filename);
							return FixDefinitions.fix43Definitions;
						
						case '4': 
							if (FixDefinitions.fix44Definitions != null)
								return FixDefinitions.fix44Definitions;
							FixDefinitions.fix44Definitions = FixDefinitions.loadFixData(filename);
							return FixDefinitions.fix44Definitions;
						
						
						default: 
							log.Error("unknown fix version  " + FixVersion);
							break;
						
					}
				}
			}
			catch (System.Exception e)
			{
                log.Error("Exception Occurred.", e);
			}
			return null;
		}
		protected internal static FixDefinitions loadFixDataToDefinitions()
		{
			if (FixDefinitions.IsCMSExchange)
			{
				try
				{
					FixDefinitions.fixCMSDefinitions = FixDefinitions.loadFixData("fix_cms.dat");
					return FixDefinitions.fixCMSDefinitions;
				}
				catch (System.Exception e)
				{
					SupportClass.WriteStackTrace(e, Console.Error);
                    log.Error("error in loading data from fix_cms.dat", e);
                    return null;
				}
			}
			char minorVersion = BasicFixUtilities.getFixMinorVersion(FixDefinitions.FixVersion);
			if (minorVersion >= '0' && minorVersion <= '4')
				return FixDefinitions.getFixDefinitions(FixDefinitions.FixVersion);
			return null;
		}
		
		//("unchecked")
		protected internal static void  buildTagsTable(CfgReader cfgReader, System.String value_section, System.Collections.Hashtable theTable)
		{
			System.Collections.IEnumerator e_values = cfgReader.list(value_section);
			if (e_values != null)
			{
				while (e_values.MoveNext())
				{
					System.String msgType = (System.String) e_values.Current;
					System.String value_Renamed = cfgReader.getProperty(value_section, msgType);
					if (value_Renamed != null)
					{
						SupportClass.HashSetSupport tokenSet = new SupportClass.HashSetSupport();
						SupportClass.Tokenizer st = new SupportClass.Tokenizer(value_Renamed, ",");
                        char[] limiters = { ',' }; 
                        String[] tokens ={ }; 
                        tokens = value_Renamed.Split(limiters);
                        for (int i = 0; i < tokens.Length; i++) 
                            tokenSet.Add(tokens[i]);
                        //while (st.HasMoreTokens())
						//{
						//	tokenSet.Add(st.NextToken().Trim());
						//}
                        
							theTable[msgType] = tokenSet;
					}
				}
			}
		}
	}
}