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
            List<Need> popWants;

            Commodity water;
            Commodity iron;
            Commodity ironbar;
            Commodity sword;

            List<Ingredient> ironbarRecipe;
            List<Ingredient> swordRecipe;

            Resource well;
            Resource ironMountain;

            Producer waterProd;
            Producer ironProd;
            Producer ironbarProd;
            Producer swordProd;

            Transporter waterTrans;
            Transporter ironTrans;
            Transporter ironbarTrans;
            Transporter swordTrans;

            List<Resource> allRes;
            List<Producer> allProd;
            List<Commodity> allCom;
            List<Transporter> allTrans;

            float timer = 0;

            //Assign all variables
            water = new Commodity("Water");
            iron = new Commodity("Iron");
            ironbarRecipe = new List<Ingredient>();
            ironbarRecipe.Add(new Ingredient(iron, 4));

            well = new Resource("Well", water, 50000);
            ironMountain = new Resource("Iron Mountain", iron, 5000);

            ironbar = new Commodity("Iron Bar", ironbarRecipe);

            swordRecipe = new List<Ingredient>();
            swordRecipe.Add(new Ingredient(ironbar, 2));

            sword = new Commodity("Sword", swordRecipe);

            waterProd = new Producer("Well Cartel", water, 0.2f, well);
            ironProd = new Producer("Iron mines", iron, 0.5f, ironMountain);
            ironbarProd = new Producer("Iron bar makers r us", ironbar, 1.5f);
            swordProd = new Producer("Swords are us", sword, 1.5f);

            popNeeds = new List<Need>();
            popNeeds.Add(new Need(sword, 1));
            popNeeds.Add(new Need(water, 10));

            islandPopulation = new Population(4, popNeeds, 15.00f);
            islandPopulation.inventory.Add(new Stock(sword, 10));

            waterTrans = new Transporter("Water Cart", 0, waterProd, islandPopulation);
            ironTrans = new Transporter("IronLogistics", 0, ironProd, ironbarProd);
            ironbarTrans = new Transporter("Bar Movers", 0, ironbarProd, swordProd);
            swordTrans = new Transporter("Sword Truckers", 0, swordProd, islandPopulation);

            //List of Resources
            allRes = new List<Resource>();
            allRes.Add(well);
            allRes.Add(ironMountain);


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
            
            //List of all the Transporters
            allTrans = new List<Transporter>();
            allTrans.Add(waterTrans);
            allTrans.Add(ironTrans);
            allTrans.Add(ironbarTrans);
            allTrans.Add(swordTrans);

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

                    foreach(Stock i in p.inventory)
                    {
                        Console.WriteLine(i.commodity.name + ": " + i.amount);
                    }
                    if (p.resource != null)
                    {
                        //Console.WriteLine(p.resource.name + ": " + p.resource.amount);
                    }
                    Console.WriteLine();
                }

                foreach (Commodity c in allCom)
                {
                    //Transporter Code
                    foreach (Producer producer in allProd)
                    {
                        foreach (Transporter transporter in allTrans)
                        {
                            //Check for Pickup's
                            if (c.name == producer.commodity.name && producer.name ==  transporter.pickupProducer.name)
                            {
                                producer.inventory = transporter.Pickup(c, producer.inventory);
                            }
                            foreach (Ingredient i in producer.commodity.ingredients)
                            {
                                //Check for Dropoff's
                                if (c.name == i.commodity.name && transporter.dropoffProducer != null && producer.name ==  transporter.dropoffProducer.name)
                                {
                                    //Check The producers inventory
                                    producer.inventory = transporter.Dropoff(c, producer.inventory);
                                }
                            }
                            //Dropoff to population
                            foreach (Need need in popNeeds)
                            {
                                if (need.commodity.name == c.name && transporter.dropoffPopulation != null)
                                {
                                    islandPopulation.inventory = transporter.Dropoff(c, islandPopulation.inventory);
                                }
                            }
                        }
                    }
                    //Production code                  
                    foreach (Producer p in allProd)
                    {
                        if (c.name == p.commodity.name)
                        {
                            p.ProduceCheck();
                        }
                    }
                }
                //Calculate growth and consume population
                islandPopulation.CalculateGrowthAndPopulation();

                //Add timer
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }
        }
    }
}
