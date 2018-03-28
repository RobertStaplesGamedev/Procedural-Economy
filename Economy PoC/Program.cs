using System;
using System.Collections.Generic;
using System.Timers;
using NUnit.Framework;

namespace Economy_PoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare Variables
            Population islandPopulation;
            List<Need> popNeeds;

            Commodity water;
            Commodity iron;
            Commodity ironbar;
            Commodity sword;

            List<Ingredient> ironbarRecipe;
            List<Ingredient> swordRecipe;

            Producer waterProd;
            Producer ironProd;
            Producer ironbarProd;
            Producer swordProd;

            Transporter trans;

            List<Producer> allProd;
            List<Commodity> allCom;

            float timer = 0;

            //Assign all variables
            water = new Commodity("Water");
            iron = new Commodity("Iron");
            ironbarRecipe = new List<Ingredient>();
            ironbarRecipe.Add(new Ingredient(iron, 4));

            ironbar = new Commodity("Iron Bar", ironbarRecipe);

            swordRecipe = new List<Ingredient>();
            swordRecipe.Add(new Ingredient(ironbar, 2));

            sword = new Commodity("Sword", swordRecipe);

            waterProd = new Producer("Well Cartel", water, 0.2f);
            ironProd = new Producer("Iron mines", iron, 0.5f);
            ironbarProd = new Producer("Iron bar makers r us", ironbar, 1.5f);
            swordProd = new Producer("Swords are us", sword, 1.5f);

            trans = new Transporter("IronLogistics");

            //List of Producers
            allProd = new List<Producer>();
            allProd.Add(waterProd);
            allProd.Add(ironProd);
            allProd.Add(ironbarProd);
            allProd.Add(swordProd);

            //List of Commodities
            allCom = new List<Commodity>();
            allCom.Add(water);
            allCom.Add(iron);
            allCom.Add(ironbar);
            allCom.Add(sword);

            popNeeds = new List<Need>();
            popNeeds.Add(new Need(sword, 1));
            popNeeds.Add(new Need(water, 10));

            islandPopulation = new Population(4, popNeeds, 15.00f);
            islandPopulation.inventory.Add(new Stock(sword, 10));

            //Sim Controller.
            string exit = "";

            while (exit != "x")
            {
                timer = timer + 0.1f;
                Console.WriteLine("Time: " + Math.Round(timer, 2));
                //Population
                Console.WriteLine("Population: " + islandPopulation.size);

                //GrowthRate
                Console.WriteLine("Growth Rate: " + islandPopulation.growth);
                foreach(Stock stock in islandPopulation.inventory)
                {
                    Console.WriteLine(stock.commodity.name + ": " + stock.amount);
                }
                Console.WriteLine();

                foreach (Producer p in allProd)
                {
                    Console.WriteLine(p.name);
                    //Console.WriteLine(p.commodity.name + ": " + p.AmountProduced());

                    foreach(Stock i in p.inventory)
                    {
                        Console.WriteLine(i.commodity.name + ": " + i.amount);
                    }
                    Console.WriteLine();
                }

                foreach (Commodity c in allCom)
                {
                    //Transporter Code
                    foreach (Producer p in allProd)
                    {
                        //Check for Pickup's
                        if (c.name == p.commodity.name)
                        {
                            p.inventory = trans.Pickup(c, p.inventory);
                        }
                    }
                    foreach (Producer p in allProd)
                    {
                        foreach (Ingredient i in p.commodity.ingredients)
                        {
                            //Check for Dropoff's
                            if (c.name == i.commodity.name)
                            {
                                //Check The producers inventory
                                p.inventory = trans.Dropoff(c, p.inventory);
                            }
                        }
                    }
                    //Dropoff to population
                    foreach (Need need in popNeeds)
                    {
                        if (need.commodity.name == c.name)
                        {
                            islandPopulation.inventory = trans.Dropoff(c, islandPopulation.inventory);
                        }
                    }

                    foreach (Producer p in allProd)
                    {
                        if (c.name == p.commodity.name)
                        {
                            p.ProduceCheck();
                        }
                    }
                }

                islandPopulation.CalculateGrowthAndPopulation();

                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }
        }
    }
}
