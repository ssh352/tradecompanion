/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd. Software Inc. ("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
using FxIntegral.com.scalper.fix;
using FxIntegral.com.scalper.fix.driver;
using log4net;
namespace FxIntegral.com.scalper.fix.driver.client
{
	
	
	public class CommonTest
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(CommonTest));
		public static SessionProperties DefaultProperties
		{
			/* package */
			
			get
			{
				SessionProperties properties = new SessionProperties();
				properties.setProperty(SessionProperties.Prop_SuppressHeartbeats, false);
				properties.setProperty(SessionProperties.Prop_AnswerTestRequests, true);
				properties.setProperty(SessionProperties.Prop_PrintFixBefore, false);
				properties.setProperty(SessionProperties.Prop_PrintFixAfter, true);
				properties.setProperty(SessionProperties.Prop_PrintFixHumanReadable, true);
                properties.setProperty(SessionProperties.Prop_SendDatesWithMilliseconds, true);
				// this is problematic..
				properties.setProperty(SessionProperties.Prop_FixVersion, FxIntegral.com.scalper.fix.FixConstants_Fields.beginString43);
				return properties;
			}
			
		}
		protected internal const System.String TEST_GROUP_EXTENSION = "txt";
		protected internal const System.String DIRECTORY = "directory";
		protected internal const System.String RUN_TEST_FILE = "run_test_file";
		protected internal const System.String SET = "set";
		protected internal const System.String EXIT = "exit";
		
		protected internal const char COMMAND_CHAR = '\'';
		
		// for OptimizeIt
		private const bool SLEEP_FOREVER_AFTER_DONE = false;
		
		public CommonTest():this(3337) 
		{
		}
		
		public CommonTest(int port)
		{
            //directory = "";
            //testFileParser = new TestFileParser(port); 
		}
		protected internal class TestGroupProperties
		{
			virtual public SessionProperties SessionProperties
			{
				get
				{
					return properties;
				}
				
			}
			virtual public bool Locked
			{
				get
				{
					return locked;
				}
				
			}
			// private final TestGroupProperties parent;
			private SessionProperties properties = null;
			private bool locked = false;
			
			public virtual void  addSessionProperty(System.String property, System.String value_Renamed)
			{
				checkNotLocked();
				properties.setProperty(property, value_Renamed);
			}
			
			protected internal virtual void  checkNotLocked()
			{
				if (locked)
				{
					throw new System.SystemException();
				}
			}
		}
	}

}