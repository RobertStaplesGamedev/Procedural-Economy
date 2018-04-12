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
        public void Test_1()
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
        //Test 1a Calculate Growth -0.25f
        [TestCase]
        public void Test_1a()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            Population testPop = new Population(12, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 10));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Assert outcome
            Assert.AreEqual(-0.25f, testPop.growth);
            Assert.AreEqual(0, testPop.inventory.Count);

        }
        //Test 2 Calculate Growth with two needs, and no inventory leftover
        [TestCase]
        public void Test_2()
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
        public void Test_3()
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
        public void Test_4()
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
            Assert.AreEqual(0.0f, testPop.growth);

            Assert.AreEqual(1, testPop.inventory.Count);
        }
        //Test 5 - Calculate Growth with two needs of different amount and with inventory leftover and a want
        [TestCase]
        public void Test_5()
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            Commodity test3 = new Commodity("test3");
            List<Need> testNeed = new List<Need>();
            List<Want> testWant = new List<Want>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            testWant.Add(new Want(test3, 0.25f));
            Population testPop = new Population(10, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, 15));
            testPop.inventory.Add(new Stock(test2, 10));
            testPop.inventory.Add(new Stock(test3, 10));

            //Call Function
            testPop.AddPopulationWants(testWant, 2);
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(0.0f, testPop.growth);

            Assert.AreEqual(1, testPop.inventory.Count);
        }
    }
}