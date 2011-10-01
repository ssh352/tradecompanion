/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd.("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;

namespace Icap.com.scalper.util
{

    /// <summary> A configuration file which supports both section-level settings and default settings at the top.  Section settings
    /// always override default settings.  Example:
    /// 
    /// <BR>
    /// <BR># Value1 is the default for Setting1
    /// <BR>Setting1=Value1
    /// <BR>
    /// <BR>[Section1]
    /// <BR> # Section1 does not use the default
    /// <BR>Setting1=Value2
    /// <BR>
    /// <BR>[Section2]
    /// <BR> # Section2 implicitly uses the default
    /// <BR>Setting2=Value3
    /// 
    /// <P>This example yields the following configuration:
    /// <BR>Section1 { Setting1 = Value2 }
    /// <BR>Section2 { Setting1 = Value1, Setting2 = Value3 }
    /// 
    /// </summary>
    public class SectionProperties : Icap.com.scalper.util.ConfigurationProperties
    {
        virtual public System.String SectionName
        {
            get
            {
                return sectionName;
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public CfgReader Reader
        {


            get
            {
                return cfgReader;
            }

        }
        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        virtual public System.Collections.Hashtable Defaults
        {
            get
            {
                return defaults;
            }

        }
        internal System.Collections.Hashtable defaults;
        protected internal CfgReader cfgReader;
        protected internal System.String sectionName;

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public SectionProperties(System.Collections.Hashtable defaults, CfgReader cfgReader)
        {
            this.defaults = defaults;
            this.cfgReader = cfgReader;
            sectionName = null;
            // this will read only high level parameters in the cfg file
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public SectionProperties(System.Collections.Hashtable defaults, CfgReader cfgReader, System.String sectionName)
        {
            this.defaults = defaults;
            this.cfgReader = cfgReader;
            this.sectionName = sectionName;
        }

        public virtual System.String[] getProperties(System.String prop)
        {
            return cfgReader.getProperties(sectionName, prop);
        }

        public virtual System.String[] getProperties(System.String sLine, System.String prop)
        {
            return getProperties(prop);
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual System.String getProperty(System.String prop)
        {
            System.String value_Renamed = cfgReader.getProperty(sectionName, prop);
            if (value_Renamed == null)
            {
                value_Renamed = cfgReader.getProperty(prop);
            }

            if (value_Renamed == null)
            {
                value_Renamed = ((System.String)defaults[prop]);
            }
            return value_Renamed;
        }

        public virtual System.String getProperty(System.String sLine, System.String prop)
        {
            return getProperty(prop);
        }
        public virtual void setProperty(System.String name, System.String value_Renamed)
        {
            cfgReader.setProperty(sectionName, name, value_Renamed);
        }

        public virtual void flush()
        {
            cfgReader.flush();
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual bool getBoolProperty(System.String prop)
        {
            return BasicUtilities.isTrue(getProperty(prop));
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        //("unchecked")
        public virtual void setDefaultProperty(System.String prop, System.String name)
        {
            defaults[prop] = name;
        }

        /// <y.exclude>  FOR INTERNAL USE ONLY </y.exclude>
        public virtual int getIntProperty(System.String prop)
        {
            try
            {
                return System.Int32.Parse(getProperty(prop));
            }
            catch 
            {
                return -1;
            }
        }
    }
}