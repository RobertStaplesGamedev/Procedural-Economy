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

        public Population(int _size, List<Need> _popNeeds, float _speed)
        {
            size = _size;
            popNeeds = _popNeeds;
            speed = _speed;
            growth = 1.00f;
            inventory = new List<Stock>();
            deltaPop = 0;
        }
        public float CalculateGrowth()
        {
            growth = 1.00f;
            float populationNeedsCount = 0;
            float populationNeedsMetCount = 0;
            float needsPercentage;
            List<Stock> tempInventory = new List<Stock>();
            List<string> usedInventory = new List<string>();

            //Starting populationgrowth is 100%
            //Then every x seconds it consumes x need for every popuation

            //Check if needs are being met
            //Compare inventory to needs
            if (inventory.Count > 0)
            {
                foreach (Need need in popNeeds)
                {
                    populationNeedsCount += (need.amount * size);
                    foreach (Stock stock in inventory)
                    {
                        if (stock.commodity.name == need.commodity.name && stock.amount >= (need.amount * size))
                        {
                            //Remove needs from the inventory
                            tempInventory.Add(new Stock(stock.commodity, stock.amount - (need.amount * size)));
                            populationNeedsMetCount += (need.amount * size);
                        }
                        else if (stock.commodity.name != need.commodity.name && !usedInventory.Contains(stock.commodity.name))
                        {
                            tempInventory.Add(stock);
                            usedInventory.Add(stock.commodity.name);
                        }
                    }
                }
                //Calculate Growth against needs
                needsPercentage = (populationNeedsMetCount / populationNeedsCount);
                growth = needsPercentage / 0.5f - 1;
                Math.Round(growth, 2);
            }
            else
            {
                growth = -1.00f;
            }

            //if over 50% of the needs are met calculate wants for a multiplyer
            if (growth >= 0f)
            {
                //Check if wants are null
                //Calculate wants
                //If wants are met than population gets a growth multiplyer as they are happier
            }
            inventory = tempInventory;
            AdjustPopulation();

            return growth;
        }
        public void AddPopulationWants ()
        {

        }
        public void AdjustPopulation()
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
    }
    struct Need
    {
        public Commodity commodity;
        public int amount;

        public Need(Commodity _commodity, int _amount)
        {
            if (_amount <= 0)
            {
                new NullReferenceException();
            }

            commodity = _commodity;
            amount = _amount;
        }
    }
}