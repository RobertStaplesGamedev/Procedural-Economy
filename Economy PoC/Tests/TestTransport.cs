using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Economy_PoC
{
    [TestFixture]
    public class TestTransport
    {
        //Test 1 Picking up a single item when there is only one item in the inventory
        [TestCase]
        public void Test_Pickup_Single()
        {
            Commodity test1 = new Commodity("Test1");
            int test1Int = 4;
            Transporter transporter = new Transporter("TestTrans");
         
            List<Stock> singleInventoryPickup = new List<Stock>();
            singleInventoryPickup.Add(new Stock(test1, test1Int));

            singleInventoryPickup = transporter.Pickup(test1, singleInventoryPickup);

            foreach (Stock stock in transporter.inventory)
            {
                Assert.AreEqual(test1.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }

            foreach (Stock stock in singleInventoryPickup)
            {
                Assert.AreEqual(test1.name, stock.commodity.name);
                Assert.AreEqual(0, stock.amount);
            }
        }
        //Test 2 Picking up a single item when there are many items in an inventory
        [TestCase]
        public void Test_Pickup_Multi()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 4;
            Commodity test2Comm = new Commodity("Test2");
            int test2Int = 3;
            Commodity test3Comm = new Commodity("Test3");
            int test3Int = 2;

            Transporter transporter = new Transporter("TestTrans");

            List<Stock> manyInventoryPickup = new List<Stock>();
            manyInventoryPickup.Add(new Stock(test1Comm, test1Int));
            manyInventoryPickup.Add(new Stock(test2Comm, test2Int));
            manyInventoryPickup.Add(new Stock(test3Comm, test3Int));

            List<Stock> leftoverStock = manyInventoryPickup;
            


            manyInventoryPickup = transporter.Pickup(test1Comm, manyInventoryPickup);

            foreach (Stock stock in transporter.inventory)
            {
                Assert.AreEqual(test1Comm.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }
            foreach (Stock stock in manyInventoryPickup)
            {
                if (test1Comm.name == stock.commodity.name)
                {
                    Assert.AreEqual(test1Comm.name, stock.commodity.name);
                    Assert.AreEqual(0, stock.amount);    
                }
                else
                {
                    foreach (Stock testStock in leftoverStock)
                    {
                        if (testStock.commodity.name == stock.commodity.name)
                        {
                            Assert.AreEqual(testStock.commodity.name, stock.commodity.name);
                            Assert.AreEqual(testStock.amount, stock.amount);
                        }
                    }
                }
            }
        }
        //Test 3 Test_Dropoff an item to an empty inventory
        [TestCase]
        public void Test_Dropoff_Single()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 4;

            Transporter transporter = new Transporter("TestTrans");
            List<Stock> singleInventoryTransport = new List<Stock>();
            List<Stock> singleInventoryDeposit = new List<Stock>();

            singleInventoryTransport.Add(new Stock(test1Comm, test1Int));

            transporter.inventory = singleInventoryTransport;

            singleInventoryDeposit = transporter.Dropoff(test1Comm, singleInventoryDeposit);
            foreach (Stock stock in singleInventoryDeposit)
            {
                Assert.AreEqual(test1Comm.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }

        }
        [TestCase]
        public void Test_Dropoff_Many()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 1;
            Commodity test2Comm = new Commodity("Test2");
            int test2Int = 2;
            Commodity test3Comm = new Commodity("Test3");
            int test3Int = 3;

            Transporter transporter = new Transporter("TestTrans");
            Population testPopulation = new Population(15, new List<Need>(), 15f);

            List<Stock> beforeTransport = new List<Stock>();
            

            beforeTransport.Add(new Stock(test1Comm, test1Int));
            beforeTransport.Add(new Stock(test2Comm, test2Int));
            beforeTransport.Add(new Stock(test3Comm, test3Int));

            transporter.inventory = beforeTransport;

            testPopulation.inventory = transporter.Dropoff(test1Comm, testPopulation.inventory);
            foreach (Stock popStock in testPopulation.inventory)
            { 
                foreach (Stock transStock in transporter.inventory)
                {
                    foreach (Stock testStock in beforeTransport)
                    {
                        //The item that has been removed it shoud be empty in the transporter but present in the popstock.
                        if (popStock.commodity.name == transStock.commodity.name)
                        {
                            //Stock has moved to the population
                            Assert.AreEqual(testStock.amount, popStock.amount);
                            //Stock has moved from the transporter
                            Assert.AreEqual(0, transStock.amount);
                        }
                        //if the item hasnt been removed it should be the same as before
                        else if (popStock.commodity.name != transStock.commodity.name && testStock.commodity.name == transStock.commodity.name)
                        {
                            Assert.AreEqual(testStock.amount, transStock.amount);
                        }
                    }
                }
            }
        }
    }
}
