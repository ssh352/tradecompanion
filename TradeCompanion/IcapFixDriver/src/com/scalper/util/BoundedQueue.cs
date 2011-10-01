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
namespace Icap.com.scalper.util
{
    public sealed class BoundedQueue
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BoundedQueue));
        public bool NotifyOnPut
        {
            set
            {
                notifyOnPut = value;
            }

        }
        public System.String Name
        {
            set
            {
                this.name = value;
            }

        }
        public bool InstrumentationEnabled
        {
            get
            {
                return instrumentationEnabled;
            }

            set
            {
                instrumentationEnabled = value;
            }

        }
        public int MaxSize
        {
            get
            {
                return maxQueueSize;
            }

        }
        internal BoundedQueue doubleBufferedQueue;
        internal System.Object[] elmts = null;
        internal int elementCount = 0;
        internal int startIndex = 0;
        internal int endIndex = 0;
        public static bool aggressive_queue_cleanup = false;

        // allow queue to be temporarily unbounded
        private bool expand_forever = false;
        internal System.Object queueLock = new System.Object();
        internal bool isQueueLocked = false;
        public static int DefaultMaxQueueSize = 5000;
        internal int maxQueueSize = BoundedQueue.DefaultMaxQueueSize;
        private bool notifyOnPut = true;
        internal bool queueClosed = false;
        internal const bool debug = false;
        internal System.String name = null;
        private bool instrumentationEnabled = false;

        public BoundedQueue(System.String name)
        {
            init(name, false);
        }

        private BoundedQueue(System.String name, bool backup)
        {
            init(name, true);
        }

        private void init(System.String name, bool isbackup)
        {
            maxQueueSize = BoundedQueue.DefaultMaxQueueSize;
            elmts = new System.Object[maxQueueSize];
            this.name = name;
            if (!isbackup)
            {
                doubleBufferedQueue = new BoundedQueue("Backup: " + name, true);
            }
        }

        public void notifyPut()
        {
            lock (this)
            {
                System.Threading.Monitor.PulseAll(this);
            }
        }

        public void expandForever()
        {
            //System.Console.Out.WriteLine(System.Enviroment.StackTrace);
        }

        public static void setDefaultMaxQueueSize(int newMax)
        {
            BoundedQueue.DefaultMaxQueueSize = newMax;
            //Logging.info("Setting BoundedQueue, DefaultMaxQueueSize to " + BoundedQueue.DefaultMaxQueueSize);
            log.Info("Setting BoundedQueue, DefaultMaxQueueSize to " + BoundedQueue.DefaultMaxQueueSize);
        }

        public void close()
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

        public void removeAllElements()
        {
            clear();
        }

        public void clear()
        {
            lock (this)
            {
                elementCount = 0;
                startIndex = 0;
                endIndex = 0;
            }
            isQueueLocked = false;
            if (BoundedQueue.debug || FFillFixGlobals.show_QueuesFullAlert)
                log.Debug(name + ": removeAllElements unlocking cache size()=" + size());
                
            lock (queueLock)
            {
                System.Threading.Monitor.PulseAll(queueLock);
            }
        }

        public System.Object get_Renamed()
        {
            System.Object obj = getInternal(0, true);
            return obj;
        }

        public System.Object peek(int i)
        {
            return getInternal(i, false);
        }

        private System.Object getInternal(int idx, bool removeObj)
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && size() == 0)
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

            if (queueClosed && size() == 0)
                throw new System.IO.IOException("BoundedQueue Closed");

            System.Object elmt = null;
            lock (this)
            {
                if (size() > idx)
                {
                    if (removeObj)
                    {
                        if (idx > 0)
                        {
                            throw new System.ArgumentException("Can only remove from the start");
                        }
                        elmt = elmts[startIndex];
                        elmts[startIndex] = null; // let the gc clean it up
                        startIndex = ((startIndex + 1) % elmts.Length);
                        elementCount--;
                    }
                    else
                    {
                        int innerIndex = ((startIndex + idx) % elmts.Length);
                        elmt = elmts[innerIndex];
                    }
                }
            }

            if (isQueueLocked)
            {
                int qSize = findQSize();
                if (expand_forever || (maxQueueSize > 0) && (qSize < (maxQueueSize / 2)))
                {
                    isQueueLocked = false;
                    if (BoundedQueue.debug || FFillFixGlobals.show_QueuesFullAlert)
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

        public int size()
        {
            return elementCount;
        }

        public int findQSize()
        {
            return elementCount;
        }

        public void put(System.Object obj)
        {
            try
            {
                if (!expand_forever)
                {
                    lock (queueLock)
                    {
                        while (!queueClosed && (elementCount >= maxQueueSize))
                        {
                            // make sure there is enough room
                            System.Threading.Monitor.Wait(queueLock);
                        }

                        while (!queueClosed && isQueueLocked && (maxQueueSize > 0) && (findQSize() >= (maxQueueSize / 2)))
                        {
                            log.Debug(name + ": put cache locked wait to be released qSize=" + findQSize());
                                
                            System.Threading.Monitor.Wait(queueLock);
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
                elmts[endIndex] = obj;
                endIndex = ((endIndex + 1) % elmts.Length);
                elementCount++;

                if (notifyOnPut)
                    System.Threading.Monitor.PulseAll(this);
            }

            int qSize = findQSize();
            if (!expand_forever && (maxQueueSize > 0) && (qSize >= maxQueueSize))
            {
                if (BoundedQueue.debug || FFillFixGlobals.show_QueuesFullAlert)
                    log.Debug(name + ": put locking cache qSize=" + qSize);
                    
                isQueueLocked = true;
            }
        }

        public int getMessages(System.Object[] targetElmts)
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && size() == 0)
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                log.Error(e);
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            if (queueClosed && size() == 0)
                throw new System.IO.IOException("BoundedQueue Closed");

            int count = 0;
            lock (this)
            {
                count = elementCount;
                if (startIndex < endIndex)
                {
                    Array.Copy(elmts, startIndex, targetElmts, 0, elementCount);
                }
                else
                {
                    Array.Copy(elmts, startIndex, targetElmts, 0, elmts.Length - startIndex);
                    Array.Copy(elmts, 0, targetElmts, elmts.Length - startIndex, endIndex);
                }

                elementCount = 0;
                startIndex = 0;
                endIndex = 0;

                if (aggressive_queue_cleanup)
                    for (int i = elmts.Length; i-- > 0; elmts[i] = null)
                        ;
            }

            if (isQueueLocked)
            {
                int qSize = findQSize();
                if (expand_forever || (maxQueueSize > 0) && (qSize < (maxQueueSize / 2)))
                {
                    isQueueLocked = false;
                    if (BoundedQueue.debug || FFillFixGlobals.show_QueuesFullAlert)
                        log.Error(name + ": get unlocking cache qSize=" + qSize);
                        
                    lock (queueLock)
                    {
                        System.Threading.Monitor.PulseAll(queueLock);
                    }
                    log.Error(name + ": get unlocking cache done qSize=" + qSize);
                        
                }
            }

            return count;
        }

        public System.Object[] toArray()
        {
            lock (this)
            {
                System.Object[] targetElmts = new System.Object[elementCount];
                getMessages(targetElmts);
                return targetElmts;
            }
        }

        public BoundedQueue backupQueue()
        {
            try
            {
                lock (this)
                {
                    while (!queueClosed && size() == 0)
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                }
            }
            catch (System.Threading.ThreadInterruptedException e)
            {
                log.Error(e);
            }
            catch (System.Exception e)
            {
                //if ( S7FixLogger.IsError() )S7FixLogger.logger.printStackTrace(e);
                //Logging.error(e);
                log.Error(e);
            }

            if (queueClosed && size() == 0)
                throw new System.IO.IOException("BoundedQueue Closed");

            lock (this)
            {
                // first backup elements array
                System.Object[] tmpElmts = doubleBufferedQueue.elmts;

                // now copy in the elements into the double buffer
                doubleBufferedQueue.elmts = elmts;
                doubleBufferedQueue.elementCount = elementCount;
                doubleBufferedQueue.startIndex = startIndex;
                doubleBufferedQueue.endIndex = endIndex;

                // now reset the elements
                elmts = tmpElmts;
                elementCount = 0;
                startIndex = 0;
                endIndex = 0;
            }

            if (isQueueLocked)
            {
                int qSize = findQSize();
                if (expand_forever || (maxQueueSize > 0) && (qSize < (maxQueueSize / 2)))
                {
                    isQueueLocked = false;
                    if (BoundedQueue.debug || FFillFixGlobals.show_QueuesFullAlert)
                        log.Debug(name + ": get unlocking cache qSize=" + qSize);
                        
                    lock (queueLock)
                    {
                        System.Threading.Monitor.PulseAll(queueLock);
                    }
                    log.Debug(name + ": get unlocking cache done qSize=" + qSize);
                        
                }
            }

            return doubleBufferedQueue;
        }

        public override System.String ToString()
        {
            return name + " " + base.ToString();
        }
    }
}