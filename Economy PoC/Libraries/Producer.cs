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
        public Resource resource { get; private set; }

        public Producer(string _name, Commodity _commodity, float _speed)
        {
            name = _name;
            commodity = _commodity;
            speed = _speed;
            deltaSpeed = 0;
            inventory = new List<Stock>();
        }
        public Producer(string _name, Commodity _commodity, float _speed, Resource _resource)
        {
            name = _name;
            commodity = _commodity;
            speed = _speed;
            deltaSpeed = 0;
            inventory = new List<Stock>();
            resource = _resource;
        }
        public void ProduceCheck()
        {
            bool foundinlist = false;

            //Check if there are ingredieninventoryStock
            if (commodity.ingredients.Count > 0 && inventory.Count > 0)
            { 
                List<Stock> newProdList = new List<Stock>();
                int ingredientCount = 0;
                int ingredientPresent = 0;

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
            //If there is no Comodity and has resources it can only create if the resources are present
            else if (commodity.ingredients.Count == 0 && resource != null)
            {
                if (resource.amount > 0)
                {
                    NoInventoryProduceCheck();
                    resource.amount = resource.amount - 1;
                }
            }
            //If there is no Comodity ingredients and no Resource it is a virtual comodity
            else if (commodity.ingredients.Count == 0)
            {
                NoInventoryProduceCheck();
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
        private void NoInventoryProduceCheck()
        {
            List<Stock> newProdList = new List<Stock>();
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