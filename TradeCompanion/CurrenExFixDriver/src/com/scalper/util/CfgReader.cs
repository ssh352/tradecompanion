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
using log4net;
using System.IO;

namespace com.scalper.util
{

    /// <summary> INI format configuration file reader.</summary>
    public class CfgReader : ConfigurationProperties
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CfgReader));
        virtual public bool BlankLinesBwSections
        {
            set
            {
                putBlankLinesBwSections = value;
            }

        }


        /// <summary> set wether we should ignore case when getting and setting properties.
        /// 
        /// Default, caseSensitive is False
        /// 
        /// </summary>
        /// <returns>   caseSensitive
        /// </returns>
        /// <summary> set wether we should ignore case when getting and setting properties.
        /// 
        /// Default, caseSensitive is False
        /// 
        /// </summary>
        /// <param name="caseSensitive">
        /// </param>
        virtual public bool CaseSensitive
        {
            get
            {
                return caseSensitive;
            }

            set
            {
                this.caseSensitive = value;
            }

        }

        /// <summary> set wether This config reader can save changes to disk
        /// 
        /// Default, readOnly is False
        /// 
        /// </summary>
        /// <returns>   readOnly
        /// </returns>
        /// <summary> set wether This config reader can save changes to disk
        /// 
        /// Default, readOnly is False
        /// 
        /// </summary>
        /// <param name="readOnly">
        /// </param>
        virtual public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                this.readOnly = value;
            }

        }
        private System.Collections.IEnumerator IniHashValues
        {
            get
            {
                if (ini_hash_props != null)
                    return ini_hash_props.Values.GetEnumerator();
                else
                    return null;
            }

        }
        virtual public SupportClass.SetSupport SectionNames
        {
            get
            {
                if (ini_hash_props != null)
                {
                    return new SupportClass.HashSetSupport(ini_hash_props);
                }
                return null;
            }

        }

        internal System.String seperator = null;
        internal System.String cfgFiledir = null;
        private static System.Collections.Hashtable configReaders = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        private System.String configFilename;
        private System.Collections.Hashtable ini_hash_props;
        private System.Collections.ArrayList props = null;
        private System.Collections.ArrayList ini_props = null;
        internal bool modified = false;
        internal System.IO.Stream istrm = null;
        public bool caseSensitive = false;
        internal bool readOnly = false;
        internal bool valuesOnly = false;
        private System.Collections.ArrayList OrderedSectionNames = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        private bool putBlankLinesBwSections = false;
        private bool RemoveBlankProps = false;


        private CfgReader(System.String filename, bool valuesOnly)
        {
            this.valuesOnly = valuesOnly;
            configFilename = cleanFilename(filename);
            readProperties();
        }

        private CfgReader(System.String fileName, System.IO.Stream iStream)
        {
            this.istrm = iStream;
            configFilename = cleanFilename(fileName);
            readProperties();
        }

        public static CfgReader getInstance(System.String in_configFilename, System.IO.Stream istrm)
        {
            return get1(in_configFilename, istrm);
        }

        private static CfgReader get1(System.String in_configFilename, System.IO.Stream istrm)
        {
            CfgReader result = (CfgReader)CfgReader.configReaders[in_configFilename];
            if (result == null)
            {
                result = new CfgReader(in_configFilename, istrm);
                CfgReader.configReaders[in_configFilename] = result;
            }

            return result;
        }

        public static CfgReader getInstance(System.String in_configFilename, bool valuesOnly)
        {
            return get2(in_configFilename, valuesOnly);
        }

        private static CfgReader get2(System.String in_configFilename, bool valuesOnly)
        {
            CfgReader result = (CfgReader)CfgReader.configReaders[in_configFilename];
            if (result == null)
            {
                result = new CfgReader(in_configFilename, valuesOnly);
                CfgReader.configReaders[in_configFilename] = result;
            }

            return result;
        }

        public virtual System.Collections.IEnumerator list(System.String ini_section_name)
        {
            if (ini_hash_props != null)
            {
                System.Collections.ArrayList v = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
                if (v == null)
                    return null;

                System.Collections.ArrayList rc = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                for (int i = 0; i < v.Count; i++)
                {
                    CfgEntry entry = (CfgEntry)v[i];
                    if (valuesOnly && entry.value_Renamed != null && entry.value_Renamed.Trim().Length > 0)
                        rc.Add(entry.value_Renamed);
                    else if (entry.name != null)
                        rc.Add(entry.name);
                }
                return rc.GetEnumerator();
            }

            return null;
        }

        public virtual int listSize(System.String ini_section_name, bool isIni)
        {
            if (ini_hash_props != null)
            {
                System.Collections.ArrayList v = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
                if (v == null)
                    return 0;

                int count = 0;
                for (int i = 0; i < v.Count; i++)
                {
                    CfgEntry entry = (CfgEntry)v[i];
                    if (entry.name != null || entry.value_Renamed != null)
                        count++;
                }

                return count;
            }

            return 0;
        }

        /*public virtual System.Collections.IEnumerator list()
        {
            System.Collections.ArrayList rc = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int i = 0; i < props.Count; i++)
            {
                CfgEntry entry = (CfgEntry)props[i];
                rc.Add(entry.name);
            }
            return rc.GetEnumerator();
        }*/

        public virtual void clearProperties()
        {
            if (props != null)
                props.Clear();
            props = null;

            if (ini_props != null)
                ini_props.Clear();
            ini_props = null;

            if (ini_hash_props != null)
            {
                System.Collections.IEnumerator e = ini_hash_props.Values.GetEnumerator();
                while (e.MoveNext())
                {
                    System.Collections.ArrayList v = (System.Collections.ArrayList)e.Current;
                    v.Clear();
                }

                ini_hash_props.Clear();
            }

            ini_hash_props = null;
            if (OrderedSectionNames != null)
                OrderedSectionNames.Clear();
        }

        public virtual bool readProperties()
        {
            try
            {
                clearProperties();
                props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                System.IO.FileInfo configFile = null;
                configFile = new System.IO.FileInfo(configFilename);
                
                if (configFile.Exists == false && !readOnly)
                {
                    SupportClass.FileSupport.CreateNewFile(configFile);
                    //configFileFound = false;
                }
                else
                {
                    if (System.IO.File.Exists(configFile.ToString()) == false && readOnly)
                    {
                        //configFileFound = false;
                        return false;
                    }
                }

                System.IO.StreamReader in_Renamed;
                if (istrm != null)
                {
                    in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(istrm, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(istrm, System.Text.Encoding.Default).CurrentEncoding);
                }
                else
                {
                   //in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(configFile.FullName, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(configFile.FullName, System.Text.Encoding.Default).CurrentEncoding);
                    //in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(configFile.Name, System.Text.Encoding.UTF8).BaseStream);
                   // in_Renamed = new StreamReader(istrm);
                    in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(configFile.FullName, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(configFile.FullName, System.Text.Encoding.Default).CurrentEncoding);
                }

                System.Collections.ArrayList tmp_props = props;

                while (true)
                {
                    System.String nextLine = in_Renamed.ReadLine();
                    if (nextLine == null)
                        break;
                    if ((nextLine.Trim().StartsWith("[")) && (nextLine.Trim().IndexOf("]") > 0))
                    {
                        System.String ini_section = nextLine.Trim().Substring(1);
                        ini_section = ini_section.Substring(0, (ini_section.IndexOf("]")) - (0));
                        OrderedSectionNames.Add(ini_section);
                        if (ini_hash_props == null)
                            ini_hash_props = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

                        tmp_props = (System.Collections.ArrayList)ini_hash_props[ini_section];
                        if (tmp_props != null)
                        {
                            if (ini_props != null)
                                ini_props.Remove(tmp_props);

                            ini_hash_props.Remove(ini_section);
                            tmp_props.Clear();
                        }

                        tmp_props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                        //if (ini_hash_props is com.scalper.fix.driver.SessionProperties)
                        //    ((com.scalper.fix.driver.SessionProperties)ini_hash_props).put(ini_section, tmp_props);
                        //else
                        ini_hash_props[ini_section] = tmp_props;
                        if (ini_props == null)
                            ini_props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                        ini_props.Add(tmp_props);

                        CfgEntry cfgEntry = new CfgEntry(this);
                        cfgEntry.ini_section_name = ini_section;
                        cfgEntry.comment = nextLine;

                        tmp_props.Add(cfgEntry);
                    }
                    else
                    {
                        //int commentIndex = nextLine.indexOf("#");
                        int commentIndex = getCommentIdx(nextLine);
                        CfgEntry cfgEntry = new CfgEntry(this);
                        if (commentIndex >= 0)
                        {
                            System.String tmpLine = nextLine.Substring(0, (commentIndex) - (0));
                            int len1 = tmpLine.Length;
                            int len2 = tmpLine.Trim().Length;

                            commentIndex = commentIndex - (len1 - len2);

                            cfgEntry.comment = nextLine.Substring(commentIndex);
                            nextLine = nextLine.Substring(0, (commentIndex) - (0)).Trim();
                        }

                        if (valuesOnly)
                        {
                            cfgEntry.value_Renamed = nextLine;
                        }
                        else
                        {
                            int idx = nextLine.IndexOf("=");
                            if (idx >= 0)
                            {
                                cfgEntry.name = nextLine.Substring(0, (idx) - (0));
                                try
                                {
                                    cfgEntry.value_Renamed = nextLine.Substring(idx + 1);
                                }
                                catch 
                                {
                                    cfgEntry.value_Renamed = "";
                                }
                            }
                            //will be used to read the exported result file.
                            else if (nextLine.IndexOf(",") > 0)
                            {
                                idx = nextLine.IndexOf(",");
                                cfgEntry.name = nextLine.Substring(0, (idx) - (0)).Trim();
                                try
                                {
                                    cfgEntry.value_Renamed = nextLine.Substring(idx + 1).Trim();
                                }
                                catch 
                                {
                                    cfgEntry.value_Renamed = "";
                                }
                            }
                        }
                        if (cfgEntry.name != null && (cfgEntry.value_Renamed == null))
                            cfgEntry.value_Renamed = "";

                        tmp_props.Add(cfgEntry);
                    }
                }
                in_Renamed.Close();
            }
            catch (System.Exception e)
            {
                log.Error(e);
                log.Info("Could not load config file " + configFilename);
                //Logging.error(e);
                //Logging.info("Could not load config file " + configFilename);
                //configFileFound = false;
                return false;
            }
            return true;
        }

        // this function is to unescape the # character. It is used when you
        // need the # in your cfg entry and should not be processed as comment
        internal virtual System.String unescape(System.String line)
        {
            if (line != null)
            {
                int idx = line.IndexOf("\\#");
                if (idx < 0)
                    return line;

                while ((idx >= 0) && (line.Length > (idx + 1)))
                {
                    System.String line1 = line.Substring(0, (idx) - (0));
                    System.String line2 = line.Substring(idx + 1);

                    line = line1 + line2;
                    idx = line.IndexOf("\\#");
                }
            }

            return line;
        }

        internal virtual int getCommentIdx(System.String line)
        {
            if (line.Length == 0)
                return -1;

            //char[] dst = new char[line.length()];
            //line.getChars(0,line.length()-1, dst, 0);
            char[] dst = line.ToCharArray();
            int commentIndex = line.IndexOf("#");
            while ((commentIndex > 0) && (dst[commentIndex - 1]) == '\\')
            {
                commentIndex = line.IndexOf("#", commentIndex + 1);
            }

            return commentIndex;
        }

        public virtual System.String getProperty(System.String ini_section_name, System.String name)
        {
            if (ini_section_name == null)
                return getProperty(name);

            if (ini_hash_props != null)
            {
                System.Collections.ArrayList v = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
                if (v == null)
                    return null;

                CfgEntry entry = getCfgEntry(v, name);
                if (entry != null)
                    return unescape(entry.value_Renamed);
                //return entry.value;
            }

            return null;
        }

        public virtual System.String getProperty(System.String name)
        {
            if (props != null)
            {
                CfgEntry entry = getCfgEntry(props, name);
                if (entry != null)
                    return unescape(entry.value_Renamed);
            }
            return null;
        }

        public virtual System.String[] getProperties(System.String ini_section_name, System.String name)
        {
            if (ini_section_name == null)
                return getProperties(name);

            System.String[] values = new System.String[0];
            if (ini_hash_props != null)
            {
                System.Collections.ArrayList v = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
                if (v == null)
                    return null;

                System.Collections.ArrayList entries = getCfgEntries(v, name);
                values = new System.String[entries.Count];
                for (int i = 0; i < entries.Count; i++)
                {
                    CfgEntry cfgEntry = (CfgEntry)entries[i];
                    values[i] = unescape(cfgEntry.value_Renamed);
                }
            }

            return values;
        }

        public virtual System.String[] getProperties(System.String name)
        {
            System.String[] values = new System.String[0];
            if (props != null)
            {
                System.Collections.ArrayList entries = getCfgEntries(props, name);
                values = new System.String[entries.Count];
                for (int i = 0; i < entries.Count; i++)
                {
                    CfgEntry cfgEntry = (CfgEntry)entries[i];
                    values[i] = unescape(cfgEntry.value_Renamed);
                }
            }

            return values;
        }

        /// <summary> remove a property from the CfgReader</summary>
        /// <param name="name">the name of the property
        /// </param>
        public virtual void removeProperty(System.String name)
        {
            CfgEntry o = getCfgEntry(props, name);
            props.Remove(o);
        }

        /// <summary> remove a property from a particular section</summary>
        /// <param name="sectionName">the name of the section
        /// </param>
        /// <param name="prop_name">the name of the property to remove
        /// </param>
        public virtual void removeSectionProperty(System.String sectionName, System.String prop_name)
        {
            if (ini_hash_props == null)
                return;
            System.Collections.ArrayList sectionProps = (System.Collections.ArrayList)ini_hash_props[sectionName];
            if (sectionProps == null)
                return;
            for (int i = 0; i < sectionProps.Count; i++)
            {
                CfgEntry entry = (CfgEntry)sectionProps[i];
                if (entry.name != null && entry.name.Equals(prop_name))
                {
                    sectionProps.RemoveAt(i);
                    return;
                }
            }
        }

        private CfgEntry getCfgEntry(System.Collections.ArrayList props, System.String name)
        {
            if (name != null)
            {
                lock (props.SyncRoot)
                {
                    name = name.Trim();
                    System.Collections.IEnumerator e = props.GetEnumerator();
                    while (e.MoveNext())
                    {
                        CfgEntry cfgEntry = (CfgEntry)e.Current;
                        if ((cfgEntry.name != null) && (cfgEntry.equals(name)))
                            return cfgEntry;
                    }
                }
            }

            return null;
        }

        private System.Collections.ArrayList getCfgEntries(System.Collections.ArrayList props, System.String name)
        {
            System.Collections.ArrayList entries = new System.Collections.ArrayList();
            if (name != null)
            {
                lock (props.SyncRoot)
                {
                    name = name.Trim();
                    System.Collections.IEnumerator e = props.GetEnumerator();
                    while (e.MoveNext())
                    {
                        CfgEntry cfgEntry = (CfgEntry)e.Current;
                        if ((cfgEntry.name != null) && (cfgEntry.equals(name)))
                            entries.Add(cfgEntry);
                    }
                }
            }

            return entries;
        }

        internal virtual System.String insertPoundsBeforeSlashes(System.String value_Renamed)
        {
            if (value_Renamed == null || value_Renamed.Length == 0)
            {
                return value_Renamed;
            }
            int curIndex = 0;
            System.Text.StringBuilder tempBuffer = new System.Text.StringBuilder(value_Renamed.Length);
            while (true)
            {
                int nextIndex;
                // if there's already a slash there, don't add it
                bool endOfString = false;
                while (true)
                {
                    nextIndex = value_Renamed.IndexOf("#", curIndex);
                    if (nextIndex >= 0)
                    {
                        if (nextIndex == 0)
                        {
                            break;
                        }
                        else if (nextIndex > 0 && value_Renamed[(nextIndex - 1)] != '\\')
                        {
                            // only break if we've found a # with no \
                            break;
                        }
                    }
                    if (nextIndex < 0)
                    {
                        nextIndex = value_Renamed.Length;
                        endOfString = true;
                        break;
                    }
                }
                tempBuffer.Append(value_Renamed.Substring(curIndex, (nextIndex) - (curIndex)));
                if (endOfString)
                {
                    break;
                }
                tempBuffer.Append("\\#");
                curIndex = nextIndex + 1;
            }
            return tempBuffer.ToString();
        }

        public virtual bool addIniSection(System.String ini_section_name)
        {
            if (ini_section_name == null)
                return false;

            if (ini_hash_props == null)
                ini_hash_props = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

            if (ini_props == null)
                ini_props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            ini_section_name = ini_section_name.Trim();
            System.Collections.ArrayList tmp_props = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
            if (tmp_props == null)
            {
                tmp_props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                ini_props.Add(tmp_props);
                //if (ini_hash_props is com.scalper.fix.driver.SessionProperties)
                //    ((com.scalper.fix.driver.SessionProperties)ini_hash_props).put(ini_section_name, tmp_props);
                //else
                ini_hash_props[ini_section_name] = tmp_props;

                CfgEntry cfgEntry = new CfgEntry(this);
                cfgEntry.ini_section_name = ini_section_name;
                cfgEntry.comment = "[" + ini_section_name + "]";

                tmp_props.Add(cfgEntry);

                return false;
            }
            return true;
        }

        public virtual bool setProperty(System.String ini_section_name, System.String name, System.String value_Renamed)
        {
            if (ini_section_name == null)
                return setProperty(name, value_Renamed);

            value_Renamed = insertPoundsBeforeSlashes(value_Renamed);
            if (ini_hash_props == null)
                ini_hash_props = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

            if (ini_props == null)
                ini_props = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            ini_section_name = ini_section_name.Trim();
            System.Collections.ArrayList v = (System.Collections.ArrayList)ini_hash_props[ini_section_name];
            if (!doesSectionExist(ini_section_name))
                addIniSection(ini_section_name);
            if (v == null)
            {
                v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
                ini_props.Add(v);
                //if (ini_hash_props is com.scalper.fix.driver.SessionProperties)
                //    ((com.scalper.fix.driver.SessionProperties)ini_hash_props).put(ini_section_name, v);
                //else
                ini_hash_props[ini_section_name] = v;
            }

            CfgEntry entry = getCfgEntry(v, name);
            if (entry != null)
                entry.value_Renamed = value_Renamed;
            else
            {
                entry = new CfgEntry(this);
                entry.name = name;
                entry.value_Renamed = value_Renamed;
                int i = 0;
                if (v.Count > 0)
                {
                    for (i = (v.Count - 1); i >= 0; i--)
                    {
                        CfgEntry tmpEntry = (CfgEntry)v[i];
                        if (!tmpEntry.BlankLine)
                        {
                            i++;
                            break;
                        }
                    }
                }
                else if (name == null && value_Renamed == null && ini_section_name != null)
                {
                    // This allows the creation of an empty section
                    // in the config file by calling this func with
                    // null name and value, but non-null section name
                    entry.ini_section_name = ini_section_name;
                    entry.comment = "[" + ini_section_name + "]";
                }

                v.Insert(i, entry);
            }

            modified = true;
            //return save();
            return true;
        }

        public virtual bool setProperty(System.String name, System.String value_Renamed)
        {
            if (props == null)
            {
                log.Info("setProperty - props is null!");
                //Logging.info("setProperty - props is null!");
                return false;
            }
            value_Renamed = insertPoundsBeforeSlashes(value_Renamed);

            CfgEntry entry = getCfgEntry(props, name);
            if (entry != null)
                entry.value_Renamed = value_Renamed;
            else
            {
                entry = new CfgEntry(this);
                entry.name = name;
                entry.value_Renamed = value_Renamed;
                props.Add(entry);
            }

            modified = true;
            //return save();
            return true;
        }

        public virtual void flush()
        {
            if (modified)
                save();
        }


        public virtual bool save()
        {
            if (readOnly)
                return true;

            try
            {
                System.IO.StreamWriter out_Renamed = null;
                lock (props.SyncRoot)
                {
                    System.Collections.IEnumerator e = props.GetEnumerator();
                    while (e.MoveNext())
                    {
                        CfgEntry cfgEntry = (CfgEntry)e.Current;
                        System.String line = "";
                        if (cfgEntry.name != null)
                        {
                            System.String val = "";
                            if (cfgEntry.value_Renamed != null)
                                val = cfgEntry.value_Renamed;

                            line = line + cfgEntry.name + "=" + val;
                        }
                        else if (valuesOnly && (cfgEntry.value_Renamed != null))
                        {
                            line = line + cfgEntry.value_Renamed;
                        }

                        if (cfgEntry.comment != null)
                            line = line + cfgEntry.comment;

                        if (putBlankLinesBwSections && line.Equals(""))
                            continue;
                        out_Renamed.Write(line.ToCharArray(), 0, line.Length);
                        out_Renamed.WriteLine();
                    }
                }

                if (ini_props != null)
                {
                    lock (ini_props.SyncRoot)
                    {
                        System.Collections.IEnumerator e = ini_props.GetEnumerator();
                        while (e.MoveNext())
                        {
                            System.Collections.ArrayList ini_values = (System.Collections.ArrayList)e.Current;
                            System.Collections.IEnumerator e2 = ini_values.GetEnumerator();
                            while (e2.MoveNext())
                            {
                                System.String line = "";
                                CfgEntry cfgEntry = (CfgEntry)e2.Current;
                                bool isSectionStart = false;
                                if (cfgEntry.ini_section_name != null)
                                {
                                    //if (ini_values.size() > 1)
                                    // allow for the creation of empty ini sections
                                    isSectionStart = true;
                                    line = cfgEntry.comment;
                                }
                                else
                                {
                                    if ((cfgEntry.name != null) && (cfgEntry.value_Renamed != null))
                                    {
                                        line = line + cfgEntry.name + "=" + cfgEntry.value_Renamed;
                                    }
                                    else if (valuesOnly && (cfgEntry.value_Renamed != null))
                                    {
                                        line = line + cfgEntry.value_Renamed;
                                    }

                                    if (cfgEntry.comment != null)
                                        line = line + cfgEntry.comment;
                                }

                                line = line.Trim();
                                if (putBlankLinesBwSections)
                                    if (line.Equals(""))
                                        continue; //don't write blank lines
                                if (RemoveBlankProps && !valuesOnly && "".Equals(cfgEntry.value_Renamed))
                                    continue; //don't write empty prop
                                if (putBlankLinesBwSections && isSectionStart)
                                {
                                    out_Renamed.WriteLine();
                                }
                                out_Renamed.Write(line.ToCharArray(), 0, line.Length);
                                out_Renamed.WriteLine();
                            }
                        }
                    }
                }
                out_Renamed.Close();
            }
            catch (System.Exception e)
            {
                log.Error(e);
                log.Info("Could not load config file " + configFilename);
                //Logging.error(e);
                //Logging.info("Could not load config file " + configFilename);
                return false;
            }

            modified = false;
            return true;
        }

        private System.String cleanFilename(System.String filename)
        {
            //it has lot of bugs..
            if (cfgFiledir != null)
            {
                if (filename.StartsWith("./"))
                    filename = filename.Substring(2);

                if (cfgFiledir == null)
                {
                    seperator = System.IO.Path.DirectorySeparatorChar.ToString();
                    System.String userhome = System.Environment.GetEnvironmentVariable("userprofile");
                    cfgFiledir = userhome + seperator + ".xsim";
                }
                System.String cfgFilename = cfgFiledir + seperator + filename;
                System.IO.FileInfo file = new System.IO.FileInfo(cfgFiledir);
                bool tmpBool;
                if (System.IO.File.Exists(file.FullName))
                    tmpBool = true;
                else
                    tmpBool = System.IO.Directory.Exists(file.FullName);
                if (tmpBool && System.IO.File.Exists(file.FullName))
                {
                    bool tmpBool2;
                    if (System.IO.File.Exists(file.FullName))
                    {
                        System.IO.File.Delete(file.FullName);
                        tmpBool2 = true;
                    }
                    else if (System.IO.Directory.Exists(file.FullName))
                    {
                        System.IO.Directory.Delete(file.FullName);
                        tmpBool2 = true;
                    }
                    else
                        tmpBool2 = false;
                    bool generatedAux = tmpBool2;
                }
                bool tmpBool3;
                if (System.IO.File.Exists(file.FullName))
                    tmpBool3 = true;
                else
                    tmpBool3 = System.IO.Directory.Exists(file.FullName);
                if (!tmpBool3)
                {
                    System.IO.Directory.CreateDirectory(file.FullName);
                }

                filename = cfgFilename;
            }
            return filename;
        }

        public virtual bool doesSectionExist(System.String sectionName)
        {
            if (ini_hash_props == null || sectionName == null)
                return false;
            return ini_hash_props.ContainsKey(sectionName);
        }


        private CfgReader()
        {
            configFilename = null;
        }

        public class CfgEntry
        {
            private void InitBlock(CfgReader enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private CfgReader enclosingInstance;
            virtual public bool BlankLine
            {
                get
                {
                    return (((name == null) || (name.Trim().Length == 0)) && (value_Renamed == null) && (ini_section_name == null) && (comment == null));
                }

            }
            public CfgReader Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public System.String ini_section_name = null;

            public System.String name = null;

            public System.String value_Renamed = null;

            public System.String comment = null;

            public CfgEntry(CfgReader enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }

            public bool equals(System.String inName)
            {
                if (Enclosing_Instance.caseSensitive)
                    return ((inName != null) && (this.name != null) && (inName.Equals(this.name)));
                else
                    return ((inName != null) && (this.name != null) && (inName.ToUpper().Equals(this.name.ToUpper())));
            }

            public override System.String ToString()
            {
                return ini_section_name + "-" + name;
            }


        }

        /*[STAThread]
        public static void Main(System.String[] args)
        {
            System.String[] files = new System.String[] { };
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    CfgReader temp = CfgReader.getInstance(files[i]);
                    temp.save();
                    temp.flush();
                }
            }
            catch (System.Exception e)
            {
                SupportClass.WriteStackTrace(e, Console.Error);
            }
        }*/
    }
}