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
//import com.aegisoft.instrumentation.CodeInstrumentationMgr;
//import com.aegisoft.instrumentation.ICounter;
using log4net;
using System;
namespace Icap.com.scalper.util
{

    [Serializable]
    public class Queue : System.Collections.ArrayList
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Queue));
        virtual public bool NotifyOnPut
        {
            set
            {
                notifyOnPut = value;
            }

        }
        virtual public System.String Name
        {
            set
            {
                this.name = value;
            }

        }
        virtual public bool InstrumentationEnabled
        {
            get
            {
                return instrumentationEnabled;
            }

            set
            {
                instrumentationEnabled = value;
                /*	if ( counter == null )
                initInstrumentation();*/
            }

        }
        virtual public int MaxSize
        {
            get
            {
                return expand_forever ? System.Int32.MaxValue : maxQueueSize;
            }

            // set to 0 to shut off

            set
            {
                if (!restoring_queue_limit)
                {
                    this.maxQueueSize = value;
                }
                else
                {
                    saved_max = value;
                    shrink_limit();
                }
            }

        }
        virtual public System.Collections.ArrayList All
        {
            get
            {
                try
                {
                    lock (this)
                    {
                        while (!queueClosed && Count == 0)
                        {
                            System.Threading.Monitor.Wait(this);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                    //Logging.error(e);
                    log.Error(e);
                }

                if (queueClosed && Count == 0)
                    throw new System.IO.IOException("Queue Closed");

                System.Collections.ArrayList rc;
                lock (this)
                {
                    rc = (System.Collections.ArrayList)this.Clone();
                    innerCount = 0;
                    this.Clear();
                    shrink_limit();
                }

                if (isQueueLocked)
                {
                    isQueueLocked = false;
                    int qSize = findQSize();
                    if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                        log.Debug(name + ": getAll unlocking cache qSize=" + qSize);
                        
                    lock (queueLock)
                    {
                        System.Threading.Monitor.PulseAll(queueLock);
                    }
                    log.Debug(name + ": getAll unlocking cache done qSize=" + qSize);
                        
                }
                return rc;
            }

        }
        /// <summary> </summary>
        private const long serialVersionUID = 1L;
        // allow queue to be temporarily unbounded
        private bool expand_forever = false;
        private bool restoring_queue_limit = false;
        private int saved_max = 0;

        internal System.Object queueLock = new System.Object();
        internal bool isQueueLocked = false;
        internal static int DefaultMaxQueueSize = 5000;
        internal int maxQueueSize = Queue.DefaultMaxQueueSize;
        private bool notifyOnPut = true;

        internal int innerCount = 0;
        internal bool queueClosed = false;
        internal const bool debug = false;
        internal System.String name = null;

        // Instrumentation flag and counter.
        private bool instrumentationEnabled = false;
        //private ICounter counter = null;

        public Queue(System.String name)
            : base()
        {
            maxQueueSize = Queue.DefaultMaxQueueSize;
            this.name = name;
            initInstrumentation();
        }

        public virtual void notifyPut()
        {
            lock (this)
            {
                System.Threading.Monitor.PulseAll(this);
            }
        }

        private void initInstrumentation()
        {
            /*if ( instrumentationEnabled && CodeInstrumentationMgr.isEnabled() )
            {
            counter = CodeInstrumentationMgr.getInstance().createCounter(name+":Counter");
            counter.setEnabled(true);
            }*/
        }

        // ignore size limit
        public virtual void expandForever()
        {
            if (restoring_queue_limit)
            {
                maxQueueSize = saved_max;
                saved_max = 0;
                restoring_queue_limit = false;
            }
            expand_forever = true;
        } // expandForever

        // eventually, normal limit will be restored
        // in the meantime, operational limit will be adjusted to
        // track queue's size as it shrinks
        public virtual void restore_limit()
        {
            lock (this)
            {
                saved_max = maxQueueSize;
                int t = findQSize();
                if (t > 0)
                    maxQueueSize += t;
                isQueueLocked = false;
                restoring_queue_limit = true;
                expand_forever = false;
            }
        } // restore_limit

        // utility method; restoring a temporarily expanded limit
        private void shrink_limit()
        {
            if (!restoring_queue_limit)
                return;
            int newmax = findQSize() + saved_max;
            if (newmax <= saved_max)
            {
                // has queue emptied?
                // restore to normal
                restoring_queue_limit = false;
                maxQueueSize = saved_max;
                saved_max = 0;
            }
            // queue not empty; shrink maximum
            else if (maxQueueSize > newmax)
                maxQueueSize = newmax;
        } // shrink_limit

        public static void setDefaultMaxQueueSize(int newMax)
        {
            Queue.DefaultMaxQueueSize = newMax;
            log.Info("Setting Queue, DefaultMaxQueueSize to " + Queue.DefaultMaxQueueSize);
            //Logging.info("Setting Queue, DefaultMaxQueueSize to " + Queue.DefaultMaxQueueSize);
        }
        public virtual void close()
        {
            queueClosed = true;
            isQueueLocked = false;
            lock (queueLock)
            {
                System.Threading.Monitor.PulseAll(queueLock);
            }

            lock (this)
            {
                System.Threading.Monitor.PulseAll(this);
            }
        }
        public System.Object remove(int index)
        {
            lock (this)
            {
                
                System.Object tempObject;
                tempObject = base[index];
                base.RemoveAt(index);
                System.Object ret = tempObject;
                shrink_limit();
                return ret;
            }
        }

        public virtual void removeAllElements()
        {
            lock (this)
            {
                base.Clear();
                innerCount = 0;
                shrink_limit();
            }
            /*if ( instrumentationEnabled )
            counter.resetCount();*/
            isQueueLocked = false;
            if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                log.Debug(name + ": removeAllElements unlocking cache size()=" + Count);
                
            lock (queueLock)
            {
                System.Threading.Monitor.PulseAll(queueLock);
            }
        }

        public virtual System.Object get_Renamed()
        {
            System.Object obj = getInternal(0, true);
            return obj;
        }

        public virtual System.Object peekElement()
        {
            return getInternal(0, false);
        }

        public virtual System.Object peekElement(int i)
        {
            return getInternal(i, false);
        }

        public virtual void block(int i)
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && Count <= (i + 1))
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                log.Error(" ", e);
              //Do nothing
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }
        }

        private System.Object getInternal(int idx, bool removeObj)
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && Count == 0)
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                log.Error(" ", e);
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            if (queueClosed && Count == 0)
                throw new System.IO.IOException("Queue Closed");

            System.Object elmt = null;
            lock (this)
            {
                if (Count > idx)
                {
                    if (removeObj)
                    {
                        if (idx == 0)
                        {
                            System.Object tempObject;
                            tempObject = this[0];
                            RemoveAt(0);
                            elmt = tempObject;
                        }
                        else
                            elmt = this.remove(idx);

                        if (elmt is System.Collections.ArrayList)
                        {
                            int listSize = ((System.Collections.ArrayList)elmt).Count;
                            innerCount -= listSize;
                        }
                    }
                    else
                    {
                        if (idx == 0)
                            elmt = this[0];
                        else
                            elmt = this[idx];
                    }
                }
                shrink_limit();
            }

            if (isQueueLocked)
            {
                int qSize = findQSize();
                if (expand_forever || (maxQueueSize > 0) && (qSize < (maxQueueSize / 2)))
                {
                    isQueueLocked = false;
                    if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                        log.Debug(name + ": get unlocking cache qSize=" + qSize);
                        
                    lock (queueLock)
                    {
                        System.Threading.Monitor.PulseAll(queueLock);
                    }
                    log.Debug(name + ": get unlocking cache done qSize=" + qSize);
                        
                }
            }

            return elmt;
        }

        public virtual void addElement(System.Object obj)
        {
            //System.Console.Out.WriteLine(System.Enviroment.StackTrace);
            put(obj);
        }

        public virtual int findQSize()
        {
            return innerCount + Count;
            //    	return size();
        }

        public virtual void insertElementAt(System.Object obj, int index)
        {
            lock (this)
            {
                // System.Console.Out.WriteLine(System.Enviroment.StackTrace);
                base.Insert(index, obj);
                /*if ( instrumentationEnabled )
                counter.inc();*/
            }
        }

        //("unchecked")
        public virtual void forcePut(System.Object obj)
        {
            lock (this)
            {
                base.Add(obj);
                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }
        }

        //({"unchecked","unchecked"})
        public virtual void put(System.Object obj)
        {
            try
            {
                if (!expand_forever && isQueueLocked)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": put cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock, TimeSpan.FromMilliseconds(500));
                        }
                        log.Debug(name + ": put cache locked released reading next message qSize=" + findQSize());
                            
                    }
                }
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                log.Error(e);
                //Logging.error(e);
            }

            lock (this)
            {
                base.Add(obj);
                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }

            /*if ( instrumentationEnabled )
            counter.inc();*/

            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize > maxQueueSize))
            {
                if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": put locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }
        }

        //("unchecked")
        public virtual void putAt(System.Object obj, int index)
        {
            try
            {
                if (!expand_forever && isQueueLocked)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": putAt cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock, TimeSpan.FromMilliseconds(500));
                        }
                        log.Debug(name + ": putAt cache locked released reading next message qSize=" + findQSize());
                            
                    }
                }
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            lock (this)
            {
                base.Insert(index, obj);
                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }
            /*if ( instrumentationEnabled )
            counter.inc();*/

            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize > maxQueueSize))
            {
                if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": putAt locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }
        }

        //("unchecked")
        public virtual bool putAll(System.Object[] a, int offset, int count)
        {
            //Thread.dumpStack();
            try
            {
                if (!expand_forever && isQueueLocked)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": putAll cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock, TimeSpan.FromMilliseconds(500));
                        }
                        log.Debug(name + ": putAll cache locked released reading next message qSize=" + findQSize());
                            
                    }
                }
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            lock (this)
            {
                for (int i = offset; (i < a.Length) && (i - offset < count); i++)
                {
                    base.Add(a[i]);
                }
                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }

            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize > maxQueueSize))
            {
                if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": putAll locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }

            return count != 0;
        }

        public virtual void putAll(System.Collections.ArrayList msgs)
        {
            try
            {
                if (!expand_forever && isQueueLocked)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": putAll cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock, TimeSpan.FromMilliseconds(500));
                        }
                        log.Debug(name + ": putAll cache locked released reading next message qSize=" + findQSize());
                            
                    }
                }
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }


            lock (this)
            {
                base.AddRange(msgs);
                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }

            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize > maxQueueSize))
            {
                if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": putAll locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }

            return;
        }

        // if you use this you need to use the get() function because that will update the innerCount correctly
        public virtual void putList(System.Collections.ArrayList msgList)
        {
            try
            {
                if (!expand_forever && isQueueLocked)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": putAll cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock, TimeSpan.FromMilliseconds(500));
                        }
                        log.Debug(name + ": putAll cache locked released reading next message qSize=" + findQSize());
                            
                    }
                }
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
               //Logging.error(e);
                log.Error(e);
            }

            int msgSize;
            lock (this)
            {
                msgSize = msgList.Count;
                if (msgSize > 0)
                {
                    innerCount += msgSize;
                    base.Add(msgList);
                    if (notifyOnPut)
                        System.Threading.Monitor.PulseAll(this);
                }
            }
            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize > maxQueueSize))
            {
                if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": putAll locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }

            return;
        }

        public virtual int getMessages(System.Object[] msgs)
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && Count == 0)
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                log.Error(" ", e);
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            if (queueClosed && Count == 0)
                throw new System.IO.IOException("Queue Closed");

            int count;
            lock (this)
            {
                count = Count;
                if (msgs.Length < count)
                    count = msgs.Length;

                System.Collections.IEnumerator it = SupportClass.IListSupport.GetEnumerator(this, 0);

                for (int i = 0; i < count; i++)
                {
                    it.MoveNext();
                    msgs[i] = it.Current;

                    
                    this.RemoveAt(i);
                    //IEnumerator can not remove am element 
                }
                shrink_limit();
            }

            if (isQueueLocked)
            {
                int qSize = findQSize();
                if (expand_forever || (maxQueueSize > 0) && (qSize < (maxQueueSize / 2)))
                {
                    isQueueLocked = false;
                    if (Queue.debug || FFillFixGlobals.show_QueuesFullAlert)
                        log.Debug(name + ": getMessages unlocking cache qSize=" + qSize);
                        
                    lock (queueLock)
                    {
                        System.Threading.Monitor.PulseAll(queueLock);
                    }
                    log.Debug(name + ": getMessages unlocking cache done qSize=" + qSize);
                        
                }
            }
            return count;
        }

        public System.String toString()
        {
            return name + " " + SupportClass.CollectionToString((System.Collections.ArrayList)this);
        }
    }
}