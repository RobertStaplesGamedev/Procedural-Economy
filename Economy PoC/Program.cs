using System;
using System.Collections.Generic;
using System.Timers;

namespace Economy_PoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare Variables
            Population islandPopulation;
            List<Need> popNeeds;

            Commodity iron;
            Commodity ironbar;
            Commodity sword;

            List<Ingredient> ironbarRecipe;
            List<Ingredient> swordRecipe;

            Producer ironProd;
            Producer ironbarProd;
            Producer swordProd;

            Transporter trans;

            List<Producer> allProd;
            List<Commodity> allCom;

            float timer = 0;

            //Assign all variables
            iron = new Commodity("Iron");
            ironbarRecipe = new List<Ingredient>();
            ironbarRecipe.Add(new Ingredient(iron, 4));

            ironbar = new Commodity("Iron Bar", ironbarRecipe);

            swordRecipe = new List<Ingredient>();
            swordRecipe.Add(new Ingredient(ironbar, 2));

            sword = new Commodity("Sword", swordRecipe);

            ironProd = new Producer("Iron mines", iron, 0.5f);
            ironbarProd = new Producer("Iron bar makers r us", ironbar, 1.5f);
            swordProd = new Producer("Swords are us", sword, 1.5f);

            trans = new Transporter("IronLogistics");

            //TestCode
            
            //ironbarProd.inventory.Add(new Stock(iron, 1000));
            //trans.inventory.Add(new Stock(iron, 4));
            //trans.inventory.Add(new Stock(ironbar, 4));

            //List of Producers
            allProd = new List<Producer>();
            allProd.Add(ironProd);
            allProd.Add(ironbarProd);
            allProd.Add(swordProd);

            //List of Commodities
            allCom = new List<Commodity>();
            allCom.Add(iron);
            allCom.Add(ironbar);
            allCom.Add(sword);

            popNeeds = new List<Need>();
            popNeeds.Add(new Need(sword, 1));

            islandPopulation = new Population(4, popNeeds, 10.00f);
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
                    Console.WriteLine(stock.commodity.name + ": " + stock.amount + "\n");
                }
                //Console.WriteLine("Population: " + islandPopulation.speed);
                //Console.WriteLine("Population: " + Math.Round(islandPopulation.deltaSpeed, 2));
                //Producers
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
                        bool produced = false;
                        if (c.name == p.commodity.name)
                        {
                            if (Math.Round(p.deltaSpeed, 2) == Math.Round(p.speed, 2))
                            {
                                //Producer Code
                                produced = p.Produce();
                                if (produced)
                                {
                                    p.deltaSpeed = 0;
                                }
                            }
                            else
                            {
                                p.deltaSpeed = p.deltaSpeed + 0.1f;
                            }

                        }

                    }
                }
                if (Math.Round(islandPopulation.deltaSpeed, 2) == Math.Round(islandPopulation.speed, 2))
                {
                    islandPopulation.CalculateGrowth();
                    islandPopulation.deltaSpeed = 0;
                }
                else
                {
                    islandPopulation.deltaSpeed = islandPopulation.deltaSpeed + 0.1f;
                }
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }
        }
    }
}
