using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TestTC
{
    [TestFixture]
    public class Broker : FIXAPITest  
    {
        [Test]
        public void TestLogonAndLogOut()
        {
            Assert.AreEqual(true, LogOnAndLogOut());
        }
        [Test]
        public void TestSubscribe()
        {
            Assert.AreEqual(true, SubscribeMarketData());
        }
        [Test]
        public void TestPlaceOrderBUY()
        {
             Assert.AreEqual(true,PlaceOrderBUY());
        }
        [Test]
        public void TestPlaceOrderSELL()
        {
            Assert.AreEqual(true, PlaceOrderSELL());
        }
    }
}
