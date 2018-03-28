using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Economy_PoC
{
    [TestFixture]
    public class TestProducer
    {
        [TestCase("Iron")]        
        public void Should_Produce(string input)
        {
            Commodity iron = new Commodity("Iron");
            Producer ironProd = new Producer("Iron mines", iron, 0.5f);
            ironProd.deltaSpeed = 0.5f;
            ironProd.ProduceCheck();
            Assert.AreEqual(input, ironProd.commodity.name);
        }
    }
}