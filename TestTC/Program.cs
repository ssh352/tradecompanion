using System;
using System.Collections.Generic;
using System.Text;

namespace TestTC
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Broker testBroker = new Broker();
           // testBroker.TestLogonAndLogOut();
            testBroker.TestSubscribe();
            //testBroker.TestPlaceOrderBUY();
            //testBroker.TestPlaceOrderSELL();

        }
    }
}
