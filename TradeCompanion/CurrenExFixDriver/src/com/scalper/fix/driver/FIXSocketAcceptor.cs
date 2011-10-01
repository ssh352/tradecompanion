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
/// File: FIXSocketAcceptor.cs
/// Created on Apr 6, 2006
/// ****************************************************
/// </summary>
using System;
using FixException = com.scalper.fix.FixException;
using Logging = com.scalper.util.Logging;
using System.Net.Sockets;
using log4net;
namespace com.scalper.fix.driver
{


    /// <summary> <p>Description:This class represents socket based FIX acceptor.
    /// </p>
    /// <p>Copyright: Copyright (c) 2005</p>
    /// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
    /// </summary>
    /// <author>  Phaniraj Raghavendra
    /// </author>
    /// <version>  1.0
    /// </version>
    public class FIXSocketAcceptor : FIXAcceptor
    {
        
        virtual public bool BlockingMode
        {
            set
            {
                this.blockingMode = value;
            }

        }
        
        private static readonly ILog log = LogManager.GetLogger(typeof(FIXSocketAcceptor));
        internal bool blockingMode = true;
        // used as the value when new connections are put into the hashmap
        private System.Object VAL = new System.Object();
        private System.Net.Sockets.TcpListener sockAcceptor;
        private int port;
        /// <summary> a map of all connections so that they can be closed later.
        /// This is logically a set, not a map, but there's no WeakHashSet in C#.
        /// </summary>
        private System.Collections.Hashtable connections;
        private volatile bool stopListen;
        private ServerThread serverThread;
        private int sendBufferSize = 0;

        /// <summary> Constructs the socket based acceptor.</summary>
        /// <param name="port	Port">where acceptor needs to start listening.
        /// </param>
        /// <throws>  FixException </throws>
        public FIXSocketAcceptor(int port)
        {
            this.port = port;

            sockAcceptor = new TcpListener(port);
            connections = new System.Collections.Hashtable();
        }
        /// <summary> <p>Description: Server thread that keeps listening for any incoming
        /// connections. For any new incoming connection, builds the FIXNotice
        /// object and calls the corresponding listeners.</p>
        /// <p>Copyright: Copyright (c) 2005</p>
        /// <p>Company: S7 Software Solutions Pvt. Ltd. Software Inc</p>
        /// </summary>
        /// <author>  Phaniraj Raghavendra
        /// </author>
        /// <version>  1.0
        /// </version>
        private class ServerThread : SupportClass.ThreadClass
        {
            private void InitBlock(FIXSocketAcceptor enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private FIXSocketAcceptor enclosingInstance;
            public FIXSocketAcceptor Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public ServerThread(FIXSocketAcceptor enclosingInstance, System.String name)
                : base(name)
            {
                InitBlock(enclosingInstance);
            }

            override public void Run()
            {
                try
                {

                    //Loop forever waiting for connections
                    Enclosing_Instance.sockAcceptor.Start();
                    Enclosing_Instance.sockAcceptor.AcceptSocket();

            
                    while (!Enclosing_Instance.stopListen)
                    {
                        Enclosing_Instance.waitForNextConncetion(Enclosing_Instance.sockAcceptor);
                    }
                }
                catch (System.IO.IOException e)
                {
                    //Logging.error(e);
                    log.Error(e);
                }
                catch (System.Exception e)
                {
                    log.Error(e);
                    //Logging.error(e);
                }
            }
        }

        internal virtual void waitForNextConncetion(System.Net.Sockets.TcpListener nextServer)
        {
            System.Net.Sockets.TcpClient channel = nextServer.AcceptTcpClient();
            FIXConnection connection = new FIXSocketConnection(channel);
            if (sendBufferSize > 0)
                connection.SendBufferSize = sendBufferSize;
            lock (connections.SyncRoot)
            {
                connections[connection] = VAL;
            }
            //Call listeners here -->
            FIXNotice notice = new FIXNotice(this, connection, FIXNotice.FIX_CONNECTION_ESTABLISHED, "connection established");
            if ((listener != null) && listenerInterests[FIXNotice.FIX_CONNECTION_ESTABLISHED])
                listener.takeNote(notice);
            else
            {
                //Logging.error("No listener is registered for the event 'connected' ");
                log.Error("No listener is registered for the event 'connected' ");
                throw new FixException("No listener is registered");
            }
        }
        /// <summary> Starts the thread that will keep listening on specific port for
        /// incoming connections and on succesfull establishment of connection,
        /// it willl flags the notice.
        /// </summary>
        public override void start()
        {
            serverThread = new ServerThread(this, "Server Thread");
            serverThread.Start();
        }

        /// <summary> Stops listening for incoming connections, flags the notice.</summary>
        public override void stop()
        {
            stopListen = true;
            try
            {
                System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64)10000 * 2100));
            }
            catch (System.Exception e)
            {
                log.Error(e);
                SupportClass.WriteStackTrace(e, Console.Error);
            }
            serverThread.Interrupt();
            try
            {
               // while (!sockAcceptor.isClosed())
                    sockAcceptor.Stop();
            }
            catch (System.IO.IOException ioe)
            {
                log.Error(ioe);
                SupportClass.WriteStackTrace(ioe, Console.Error);
            }
            lock (connections.SyncRoot)
            {
                System.Collections.IEnumerator it = new SupportClass.HashSetSupport(connections.Keys).GetEnumerator();
                while (it.MoveNext())
                {
                    FIXSocketConnection conn = (FIXSocketConnection)it.Current;
                    conn.close();
                    
                }
                connections.Clear();
            }
        }
    }
}