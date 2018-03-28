using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    class Population
    {
        public int size;
        public float growth { get; private set; }
        public float speed { get; private set; }
        public float deltaSpeed;
        public float deltaPop;

        public List<Need> popNeeds;
        public List<Need> popWants;

        public List<Stock> inventory;
        private List<Stock> tempInventory;

        public Population(int _size, List<Need> _popNeeds, float _speed)
        {
            size = _size;
            popNeeds = _popNeeds;
            speed = _speed;
            growth = 1.00f;
            inventory = new List<Stock>();
            deltaPop = 0;
        }
        public float CalculateGrowthAndPopulation()
        {
            growth = 1.00f;
            int[] populationNeedsCountArray = new int[2];

            //Starting populationgrowth is 100%
            //Then every x seconds it consumes x need for every popuation

            //Compare inventory to needs
            if (inventory.Count > 0)
            {
                //Calculate the needs are being met
                populationNeedsCountArray = CalculateNeeds(populationNeedsCountArray);

                //Calculate Growth Calculation
                AdjustGrowth(populationNeedsCountArray[0], populationNeedsCountArray[1]); 
            }
            //If the inventory is empty growth is just 0.
            else
            {
                growth = -1.00f;
            }
            //if over 50% of the needs are met calculate wants for a multiplyer
            if (growth > 0f)
            {
                //Check if wants are null
                //Calculate wants
                //If wants are met than population gets a growth multiplyer as they are happier
            }
            
            if (Math.Round(deltaSpeed, 2) == Math.Round(speed, 2))
            {
                AdjustPopulation();
                inventory = tempInventory;
                deltaSpeed = 0;
            }
            else 
            {
                deltaSpeed = deltaSpeed + 0.1f;
            }
            Cleanup();
            return growth;
        }
        public void AddPopulationWants ()
        {

        }
        private int[] CalculateNeeds(int[] populationNeedsCountArray)
        {
            int populationNeedsCount = 0;
            int populationNeedsMetCount = 0;

            tempInventory = new List<Stock>();
            List<string> usedInventory = new List<string>();

            foreach (Need need in popNeeds)
            {
                populationNeedsCount += (need.amount * size);
                foreach (Stock stock in inventory)
                {
                    if (stock.commodity.name == need.commodity.name)
                    {
                        //Remove needs from the inventory
                        if (stock.amount >= (need.amount * size))
                        {
                            tempInventory.Add(new Stock(stock.commodity, stock.amount - (need.amount * size)));
                        }
                        populationNeedsMetCount += stock.amount;
                    }
                    // else if (stock.commodity.name != need.commodity.name && !usedInventory.Contains(stock.commodity.name))
                    // {
                    //     tempInventory.Add(stock);
                    //     usedInventory.Add(stock.commodity.name);
                    // }
                }

                foreach (Stock stock in inventory)
                {
                    foreach (Stock tempStock in tempInventory)
                    {
                        if (stock.commodity.name == tempStock.commodity.name && stock.commodity.name == need.commodity.name)
                        {

                        }
                    }
                }
            }

            populationNeedsCountArray[0] = populationNeedsCount;
            populationNeedsCountArray[1] = populationNeedsMetCount;

            return populationNeedsCountArray;
        }
        private void AdjustGrowth(int populationNeedsCount, int populationNeedsMetCount)
        {
            float needsPercentage = 0;

                //populationNeedsCount = 50;
                //populationNeedsMetCount = 26;

                //Calculate Growth against needs
                if (populationNeedsMetCount < populationNeedsCount)
                {
                    if (populationNeedsMetCount >= (populationNeedsCount / 2))
                    {
                        //needsPercentage = -(populationNeedsMetCount / populationNeedsCount);
                        needsPercentage = ((populationNeedsMetCount - (populationNeedsCount / 2)) * 100) / (populationNeedsCount - (populationNeedsCount / 2));
                        needsPercentage = (needsPercentage / 100);
                        if (needsPercentage == 0)
                        {
                            growth = -1.00f;
                        }
                        else
                        {
                            //growth = -(needsPercentage / 0.5f - 1);
                            growth = (needsPercentage - 1);
                        }
                    }
                    else
                    {
                        growth = -1.00f;
                    }
                    growth = (float) Math.Round(growth, 2);
                }
                else if (populationNeedsMetCount >= populationNeedsCount)
                {
                    if (populationNeedsMetCount <= (populationNeedsCount * 2))
                    {
                        needsPercentage = ((populationNeedsMetCount - populationNeedsCount) * 100) / ((populationNeedsCount * 2) - populationNeedsCount);
                        growth = needsPercentage / 100;
                    }
                    else 
                    {
                        growth = 1.00f;
                    }
                }
                else if (growth < -1.00)
                {
                    growth = -1.00f;
                }
        }
        private void AdjustPopulation()
        {
            deltaPop += growth;
            //deltapop is >= 1 or -1 add or remove population and minus out the deltapop
            if (deltaPop >= 1.00f || deltaPop <= -1.00f)
            {
                size += Convert.ToInt32(Math.Floor(deltaPop));
                if (deltaPop >=1)
                {
                    deltaPop = deltaPop - 1;
                }
                else if (deltaPop <= -1)
                {
                    deltaPop = deltaPop + 1;
                }
            }
            if (size < 0)
            {
                size = 0;
            }
        }
        private void Cleanup()
        {
            List<Stock> tempInv = new List<Stock>();
            if (inventory.Count > 0)
            {
                foreach (Stock stock in inventory)
                {
                    if (stock.amount > 0)
                    {
                        tempInv.Add(stock);
                    }
                }
                inventory = tempInv;
            }
        }
    }
}