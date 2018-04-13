using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Economy_PoC
{
    [TestFixture]
    public class TestPopulation
    {
        //Growth with one need
        [TestCase(16, 14, -0.25f, 0)]
        [TestCase(16, 16, 0.0f, 0)]
        [TestCase(16, 20, 0.25f, 1)]
        public void Test_1(int popSize, int inventory, float growth, int inventoryCount)
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            Population testPop = new Population(popSize, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, inventory));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Assert outcome
            Assert.AreEqual(growth, testPop.growth);
            Assert.AreEqual(inventoryCount, testPop.inventory.Count);

        }
        //Growth with two needs
        [TestCase(16, 10, 8, -1f, 0)]
        [TestCase(16, 14, 10, -0.75f, 0)]
        [TestCase(10, 10, 10, 0.0f, 0)]
        [TestCase(16, 25, 20, 0.25f, 2)]
        [TestCase(16, 20, 25, 0.25f, 2)]
        [TestCase(10, 20, 25, 1, 2)]
        [TestCase(10, 25, 20, 1, 2)]
        public void Test_2(int popSize, int inventoryItem1, int inventoryItem2, float growth, int inventoryCount)
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            List<Need> testNeed = new List<Need>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            Population testPop = new Population(popSize, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, inventoryItem1));
            testPop.inventory.Add(new Stock(test2, inventoryItem2));

            //Call Function
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(growth, testPop.growth);
            Assert.AreEqual(inventoryCount, testPop.inventory.Count);

        }
        //Growth with one need and one want
        [TestCase(16, 20, 16, 0.5f, 0.75f, 1)] //Positive from needs below want cap
        [TestCase(16, 20, 17, 0.5f, 0.75f, 2)] //Positive from needs below want cap
        [TestCase(16, 20, 16, 2.5f, 2f, 1)] //Positive from needs above want cap
        [TestCase(16, 14, 10, 0.5f, -0.25f, 1)] //negative from needs means wants doesnt affect cap
        public void Test_3(int popSize, int needInventory, int wantInventory, float wantImportance, float growth, int inventoryCount)
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            List<Need> testNeed = new List<Need>();
            List<Want> testWant = new List<Want>();
            testNeed.Add(new Need(test1, 1));
            testWant.Add(new Want(test2, wantImportance));
            Population testPop = new Population(popSize, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, needInventory));
            testPop.inventory.Add(new Stock(test2, wantInventory));

            //Call Function
            testPop.AddPopulationWants(testWant, 2);
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(growth, testPop.growth);

            Assert.AreEqual(inventoryCount, testPop.inventory.Count);
        }
        //Two needs one want
        [TestCase(16, 20, 20, 16, 0.5f, 0.75f, 2)] //Positive growth
        [TestCase(16, 25, 25, 17, 2.5f, 2f, 3)] //Positive growth from needs
        [TestCase(16, 14, 10, 16, 0.5f, -0.75f, 1)] //Negative growth from needs
        [TestCase(16, 10, 20, 16, 0.5f, -0.75f, 2)] //Negative growth from needs
        public void Test_4(int popSize, int inventoryItem1, int inventoryItem2, int wantInventory, float wantImportance, float growth, int inventoryCount)
        {
            //Setup
            Commodity test1 = new Commodity("test1");
            Commodity test2 = new Commodity("test2");
            Commodity test3 = new Commodity("test3");
            List<Need> testNeed = new List<Need>();
            List<Want> testWant = new List<Want>();
            testNeed.Add(new Need(test1, 1));
            testNeed.Add(new Need(test2, 1));
            testWant.Add(new Want(test3, wantImportance));
            Population testPop = new Population(popSize, testNeed, 5);
            testPop.deltaSpeed = 5;
            testPop.inventory.Add(new Stock(test1, inventoryItem1));
            testPop.inventory.Add(new Stock(test2, inventoryItem2));
            testPop.inventory.Add(new Stock(test3, wantInventory));

            //Call Function
            testPop.AddPopulationWants(testWant, 2);
            testPop.CalculateGrowthAndPopulation();

            //Asset Outcome
            Assert.AreEqual(growth, testPop.growth);

            Assert.AreEqual(inventoryCount, testPop.inventory.Count);
        }
    }
}