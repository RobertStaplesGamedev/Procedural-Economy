using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    class Producer
    {
        public string name { get; private set; }
        public Commodity commodity { get; private set; }
        public float speed { get; private set; }
        public float deltaSpeed;

        public List<Stock> inventory;

        public Producer(string _name, Commodity _commodity, float _speed)
        {
            name = _name;
            commodity = _commodity;
            speed = _speed;
            deltaSpeed = 0;
            inventory = new List<Stock>();
        }
        public void ProduceCheck()
        {
            List<Stock> newProdList = new List<Stock>();
            int ingredientCount = 0;
            int ingredientPresent = 0;

            bool foundinlist = false;

            //Check if there are ingredieninventoryStock
            if (commodity.ingredients.Count > 0 && inventory.Count > 0)
            { 
                //Check list for ingedieninventoryStock
                foreach (Ingredient ingredient in commodity.ingredients)
                {
                    ingredientCount++;
                    //Comparing this to the stock in the inventory
                    foreach (Stock stock in inventory)
                    {
                        if (ingredient.commodity.name == stock.commodity.name && stock.amount >= ingredient.amount)
                        {
                            //Remove items from stock and mark as incredieninventoryStock present
                            newProdList.Add(new Stock(stock.commodity, (stock.amount - ingredient.amount)));
                            ingredientPresent++;
                        }
                    }
                }
                //Produce
                foreach (Stock stock in inventory)
                {
                    if (ingredientCount == ingredientPresent && commodity.name == stock.commodity.name)
                    {
                        newProdList.Add(new Stock(stock.commodity, stock.amount + 1));
                        foundinlist = true;
                        Produce(newProdList);
                        ingredientPresent = 0;
                    }
                }
                if (!foundinlist && ingredientCount == ingredientPresent)
                {
                    newProdList.Add(new Stock(commodity, 1));
                    Produce(newProdList);
                }
            }
            //Checking if there are no ingredieninventoryStock (resource)
            else if (commodity.ingredients.Count == 0)
            {
                bool itemFound = false;
                if (inventory.Count > 0)
                {
                    foreach (Stock stock in inventory)
                    {
                        if (stock.commodity.name == commodity.name)
                        {
                            newProdList.Add(new Stock(commodity, stock.amount + 1));
                            Produce(newProdList);
                            itemFound = true;
                        }
                    }
                    if (!itemFound)
                    {
                        newProdList.Add(new Stock(commodity, 1));
                        Produce(newProdList);
                    }
                }
                else
                {
                    newProdList.Add(new Stock(commodity, 1));
                    Produce(newProdList);
                }
            }
        }
        public int AmountProduced()
        {
            int produced = 0;
            foreach (Stock stock in inventory)
            {
                if (commodity.name == stock.commodity.name)
                {
                    produced = stock.amount;
                }
            }
            return produced;
        }
        private void Produce(List<Stock> tempInventory)
        {
           if (Math.Round(deltaSpeed, 2) < Math.Round(speed, 2))
            {
                deltaSpeed = deltaSpeed + 0.1f;
            }
            else
            {
                inventory = tempInventory;
                deltaSpeed = 0;
            } 
        }

    }
}