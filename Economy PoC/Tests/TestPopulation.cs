using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Economy_PoC
{
    [TestFixture]
    public class TestPopulation
    {
        //Test 1 Calculate Growth with one need of 5
        [TestCase]
        public void Test_One_Need()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            Population testPop = new Population(10, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 10));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Assert outcome
            Assert.AreEqual(0, testPop.growth);
            Assert.AreEqual(0, testPop.inventory.Count);

        }
        //Test 2 Calculate Growth with two needs, and no inventory leftover
        [TestCase]
        public void Test_Two_Needs()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            Population testPop = new Population(10, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 10));
            testPop.inventory.Add(new Stock(test2, 10));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(0, testPop.growth);

            Assert.AreEqual(0, testPop.inventory.Count);

        }

        //Test 3 - Calculate Growth with two needs, and with inventory leftover
        [TestCase]
        public void Test_Two_Needs_Leftover()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            Population testPop = new Population(10, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 15));
            testPop.inventory.Add(new Stock(test2, 15));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(0.5, testPop.growth);

            Assert.AreEqual(2, testPop.inventory.Count);


        }
        //Test 4 - Calculate Growth with two needs of different amount and with inventory leftover
        [TestCase]
        public void Test_Two_Needs_Multi()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            Population testPop = new Population(10, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 15));
            testPop.inventory.Add(new Stock(test2, 10));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(0.25f, testPop.growth);

            Assert.AreEqual(1, testPop.inventory.Count);
        }
    }
}