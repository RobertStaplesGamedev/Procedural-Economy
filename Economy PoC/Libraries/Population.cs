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
        private List<Want> popWants;
        private int wantCap;
        private bool wantsAdded;

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
            wantsAdded = false;
        }
        public float CalculateGrowthAndPopulation()
        {
            growth = 1.00f;
            float needsPercentage = 0;
            int[] populationNeedsCountArray = new int[2];

            //Starting populationgrowth is 100%
            //Then every x seconds it consumes x need for every popuation

            //Compare inventory to needs
            if (inventory.Count > 0)
            {
                //Calculate the needs are being met
                needsPercentage = CalculateNeeds(needsPercentage);

                //if over 50% of the needs are met calculate wants for a multiplyer
                if (needsPercentage >= 0.5f && wantsAdded)
                {
                    CalculateWants(needsPercentage);
                }

                //Calculate Growth Calculation
                growth = (needsPercentage / 100); 
            }
            //If the inventory is empty growth is just 0.
            else
            {
                growth = -1.00f;
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
        public void AddPopulationWants (List<Want> populationWants, int wantsCap)
        {
            popWants = populationWants;
            wantCap = wantsCap;
            wantsAdded = true;
        }
        private float CalculateNeeds(float needsPercentage)
        {
            int populationNeedsCount = 0;
            int populationNeedsMetCount = 0;

            
            string lowestNeed = "";
            int lowestNeedPercentge = -1;
            int currentNeedsPercentage = 1;

            tempInventory = new List<Stock>();
            List<string> usedInventory = new List<string>();
            bool hasfoundName = false;

            //Find the lowest met need (in percentage)
            foreach (Need need in popNeeds)
            {
                foreach (Stock stock in inventory)
                {
                    if (stock.commodity.name == need.commodity.name)
                    {
                        currentNeedsPercentage = (need.amount * size) / stock.amount;
                        if (lowestNeedPercentge == -1 || lowestNeedPercentge < currentNeedsPercentage)
                        {
                            lowestNeed = need.commodity.name;
                            lowestNeedPercentge = currentNeedsPercentage;
                        }

                    }
                }
            }
            //Only calculate for that need
            foreach (Need need in popNeeds)
            {
                foreach (Stock stock in inventory)
                {
                    if (stock.commodity.name == need.commodity.name)
                    {
                        //Remove needs from the inventory
                        if (stock.amount >= (need.amount * size))
                        {
                            tempInventory.Add(new Stock(stock.commodity, stock.amount - (need.amount * size)));
                        }
                        else if (stock.amount > 0)
                        {
                            tempInventory.Add(new Stock(stock.commodity, 0));
                        }
                        if (stock.commodity.name == lowestNeed)
                        {
                            populationNeedsCount += (need.amount * size);                            
                            populationNeedsMetCount += stock.amount;
                        }
                    }
                    else
                    {
                        foreach (string name in usedInventory)
                        {
                            if (stock.commodity.name == name)
                            {
                                hasfoundName = true;
                            }
                        }
                        if (hasfoundName)
                        {
                            tempInventory.Add(stock);
                            usedInventory.Add(stock.commodity.name);
                            hasfoundName = false;
                        }
                    }
                }
            }

            if (populationNeedsMetCount < populationNeedsCount)
            {
                if (populationNeedsMetCount >= (populationNeedsCount / 2))
                {
                    needsPercentage = ((populationNeedsMetCount - (populationNeedsCount / 2)) * 100) / (populationNeedsCount - (populationNeedsCount / 2));
                    needsPercentage = needsPercentage - 100;
                }
                else
                {
                    needsPercentage = -100;
                }
            }
            else if (populationNeedsMetCount >= populationNeedsCount && populationNeedsMetCount <= (populationNeedsCount * 2))
            {
                needsPercentage = ((populationNeedsMetCount - populationNeedsCount) * 100) / ((populationNeedsCount * 2) - populationNeedsCount);
                
            }
            return needsPercentage;
        }
        private float CalculateWants(float wantsPercentage)
        {
            float popWantsMet = 0;
            List<Stock> tempInventory2 =  new List<Stock>();


            foreach(Want want in popWants)
            {
                foreach(Stock stock in inventory)
                {
                    if (want.commodity.name == stock.commodity.name)
                    {
                        popWantsMet += (want.importance * stock.amount) / size;
                        //Remove wants from the inventory
                        if (stock.amount >= size)
                        {
                            tempInventory.Add(new Stock(stock.commodity, stock.amount - size));
                        }
                    }
                    else
                    {
                        tempInventory.Add(stock);
                    }
                }
            }

            //**Calculate wants percentage

            if (wantsPercentage > wantCap)
            {
                wantsPercentage = wantCap;
            }
            return wantsPercentage;
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