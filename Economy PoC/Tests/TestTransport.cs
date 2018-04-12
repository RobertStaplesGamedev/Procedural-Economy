using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Economy_PoC
{
    [TestFixture]
    public class TestTransport
    {
        /*
        This is a 
        Test 1 - Picking up a single item when there is only one item in the inventory
        Test 2 - Picking up a single item when there are many items in an inventory
        Test 3 - Dropoff an item to an empty inventory
        Test 4 - Dropoff an item to an inventory with items in it
        Test 5 - Dropoff an item to an inventory with items in it that arent the intended item
        Test 6 - Pickup Items from the Designated Producer
        Test 7 - Dropoff items to the Designated Producer
        Test 8 - Dropoff items to the designated population
         */



        //Test 1 Picking up a single item when there is only one item in the inventory
        [TestCase]
        public void Test_1()
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
        public void Test_2()
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
        //Test 3 Dropoff an item to an empty inventory
        [TestCase]
        public void Test_3()
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
        //Test 4 - Dropoff an item to an inventory with items in it
        [TestCase]
        public void Test_4()
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
        //Test 5 - Dropoff an item to an inventory with items in it that arent the intended item
        [TestCase]
        public void Test_5()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 1;
            Commodity test2Comm = new Commodity("Test2");
            int test2Int = 2;
            Commodity test3Comm = new Commodity("Test3");
            int test3Int = 3;

            Transporter transporter = new Transporter("TestTrans");
            Producer testProducer = new Producer("TestProducer", test2Comm, 0.5f);

            List<Stock> beforeTransport = new List<Stock>();
            

            beforeTransport.Add(new Stock(test1Comm, test1Int));
            beforeTransport.Add(new Stock(test2Comm, test2Int));
            
            testProducer.inventory.Add(new Stock(test3Comm, test3Int));

            transporter.inventory = beforeTransport;

            testProducer.inventory = transporter.Dropoff(test2Comm, testProducer.inventory);
            foreach (Stock popStock in testProducer.inventory)
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
        //Test 6 - Pickup Items from the Designated Producer
        [TestCase]
        public void Test_6()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 4;
            Commodity test2Comm = new Commodity("Test2");

            Producer testProducer1 = new Producer("TestProducer1", test1Comm, 0.5f);
            Producer testProducer2 = new Producer("TestProducer2", test2Comm, 0.5f);

            List<Producer> allProd = new List<Producer>();
            allProd.Add(testProducer1);
            allProd.Add(testProducer2);

            Transporter transporter = new Transporter("TestTrans", 0.5f, testProducer1, testProducer2);

            List<Stock> manyInventoryPickup = new List<Stock>();
            
            testProducer1.inventory.Add(new Stock(test1Comm, test1Int));


            foreach (Producer producer in allProd)
            {
                if (producer.name == transporter.pickupProducer.name)
                {
                    manyInventoryPickup = transporter.Pickup(test1Comm, producer.inventory);
                }
            }

            foreach (Stock stock in transporter.inventory)
            {
                Assert.AreEqual(test1Comm.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }
        }
        //Test 7 - Dropoff items to the Designated Producer
        [TestCase]
        public void Test_7()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 4;
            Commodity test2Comm = new Commodity("Test2");

            Producer testProducer1 = new Producer("TestProducer1", test1Comm, 0.5f);
            Producer testProducer2 = new Producer("TestProducer2", test2Comm, 0.5f);

            List<Producer> allProd = new List<Producer>();
            allProd.Add(testProducer1);
            allProd.Add(testProducer2);

            Transporter transporter = new Transporter("TestTrans", 0.5f, testProducer1, testProducer2);

            List<Stock> manyInventoryPickup = new List<Stock>();
            
            testProducer1.inventory.Add(new Stock(test1Comm, test1Int));


            foreach (Producer producer in allProd)
            {
                if (producer.name == transporter.dropoffProducer.name)
                {
                    manyInventoryPickup = transporter.Dropoff(test1Comm, producer.inventory);
                }
            }
            foreach (Stock stock in testProducer2.inventory)
            {
                Assert.AreEqual(test1Comm.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }
            Assert.AreEqual(0, transporter.inventory.Count);

        }
        //Test 8 - Dropoff items to the designated population
        [TestCase]
        public void Test_8()
        {
            Commodity test1Comm = new Commodity("Test1");
            int test1Int = 4;
            Commodity test2Comm = new Commodity("Test2");

            Producer testProducer1 = new Producer("TestProducer1", test1Comm, 0.5f);
            Population testPopulation = new Population(34, new List<Need>(), 50);

            Transporter transporter = new Transporter("TestTrans", 0.5f, testProducer1, testPopulation);
            
            testProducer1.inventory.Add(new Stock(test1Comm, test1Int));

            transporter.Dropoff(test1Comm, testPopulation.inventory);
            
            foreach (Stock stock in testPopulation.inventory)
            {
                Assert.AreEqual(test1Comm.name, stock.commodity.name);
                Assert.AreEqual(test1Int, stock.amount);
            }
            Assert.AreEqual(0, transporter.inventory.Count);
        }
    }
}
