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
        public bool Produce()
        {
            List<Stock> newProdList = new List<Stock>();
            int ingredientCount = 0;
            int ingredientPresent = 0;
            bool produced = false;

            bool foundinlist = false;

            //Check if there are ingredients
            if (commodity.ingredients.Count > 0 && inventory.Count > 0)
            { 
                //Check list for ingedients
                foreach (Ingredient ingredient in commodity.ingredients)
                {
                    ingredientCount++;
                    //Comparing this to the stock in the inventory
                    foreach (Stock stock in inventory)
                    {
                        if (ingredient.commodity.name == stock.commodity.name && stock.amount >= ingredient.amount)
                        {
                            //Remove items from stock and mark as incredients present
                            newProdList.Add(new Stock(stock.commodity, (stock.amount - ingredient.amount)));
                            ingredientPresent++;
                        }
                    }
                }
                foreach (Stock stock in inventory)
                {
                    //Produce
                    if (ingredientCount == ingredientPresent && commodity.name == stock.commodity.name)
                    {
                        newProdList.Add(new Stock(stock.commodity, stock.amount + 1));
                        produced = true;
                        foundinlist = true;
                        inventory = newProdList;
                        ingredientPresent = 0;
                    }
                }
                if (!foundinlist && ingredientCount == ingredientPresent)
                {
                    newProdList.Add(new Stock(commodity, 1));
                    produced = true;
                    inventory = newProdList;
                }
            }
            //Checking if there are no ingredients (resource)
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
                            produced = true;
                            inventory = newProdList;
                            itemFound = true;
                        }
                        
                    }
                    if (!itemFound)
                    {
                        newProdList.Add(new Stock(commodity, 1));
                        produced = true;
                        inventory = newProdList;
                    }
                }
                else
                {
                    newProdList.Add(new Stock(commodity, 1));
                    produced = true;
                    inventory = newProdList;
                }
            }
            return produced;
        }
        public int AmountProduced()
        {
            int produced = 0;
            foreach (Stock p in inventory)
            {
                if (commodity.name == p.commodity.name)
                {
                    produced = p.amount;
                }
            }

            return produced;
        }

    }

    class Transporter
    {
        public string name { get; private set; }
        public List<Stock> inventory;

        public Transporter(string _name)
        {
            name = _name;
            inventory = new List<Stock>();
        }
        public List<Stock> Pickup(Commodity PickupComm, List<Stock> custList)
        {
            List<Stock> newCustList = new List<Stock>();
            List<Stock> newInventoryList = new List<Stock>();
            bool foundInInventory = false;

            //Check if the customer list has been initialised
            if (custList.Count > 0)
            {
                //Start building the New customer list
                foreach (Stock custStock in custList)
                {
                    //Check if the Chosen Commodity is the current commdity being looked at
                    if (PickupComm.name == custStock.commodity.name)
                    {
                        //Checking if the inventory is not empty
                        if (inventory.Count > 0)
                        {
                            //Rewrite inventory
                            foreach (Stock transStock in inventory)
                            {
                                if (transStock.commodity.name != custStock.commodity.name)
                                {
                                    newInventoryList.Add(transStock);
                                }
                                //If it find the item in the CustList
                                else if (transStock.commodity.name == custStock.commodity.name)
                                {
                                    newInventoryList.Add(new Stock(transStock.commodity, (transStock.amount + custStock.amount)));
                                    newCustList.Add(new Stock(custStock.commodity, 0));
                                    foundInInventory = true;
                                }
                            }
                            if (!foundInInventory)
                            {
                                newInventoryList.Add(custStock);
                                newCustList.Add(new Stock(custStock.commodity, 0));
                            }
                        }
                        else
                        {
                            newInventoryList.Add(custStock);
                            newCustList.Add(new Stock(custStock.commodity, 0));
                        }
                    }
                    //if its not matching just add the item to the new customer list
                    else
                    {
                        newCustList.Add(custStock);
                    }
                }
                inventory = newInventoryList;
                custList = newCustList;
            }
            return custList;
        }
        public List<Stock> Dropoff(Commodity DropOffComm, List<Stock> custList)
        {
            List<Stock> newCustList = new List<Stock>();
            List<Stock> newInventoryList = new List<Stock>();
            bool foundInCustList = false;

            if (inventory.Count > 0)
            {
                //Start building the New customer list
                foreach (Stock ts in inventory)
                {
                    //Check if the Chosen Commodity is the current commdity being looked at
                    if (DropOffComm.name == ts.commodity.name)
                    {
                        //Checking if the inventory is not empty
                        if (custList.Count > 0)
                        {
                            //Rewrite inventory
                            foreach (Stock s in custList)
                            {
                                if (ts.commodity.name != s.commodity.name)
                                {
                                    //newInventoryList.Add(ts);
                                    newCustList.Add(s);
                                }
                                //If it find the item in the CustList
                                if (ts.commodity.name == s.commodity.name)
                                {
                                    newInventoryList.Add(new Stock(ts.commodity, (0)));
                                    newCustList.Add(new Stock(s.commodity, ts.amount + s.amount));
                                    foundInCustList = true;
                                }
                            }
                            if (!foundInCustList)
                            {
                                newInventoryList.Add(new Stock(ts.commodity, 0));
                                newCustList.Add(ts);
                            }
                        }
                        else
                        {
                            newInventoryList.Add(new Stock(ts.commodity, 0));
                            newCustList.Add(ts);
                        }
                    }
                    //if its not matching just add the item to the new customer list
                    else
                    {
                        newInventoryList.Add(ts);
                    }
                }
                inventory = newInventoryList;
                custList = newCustList;
            }
            return custList;
        }
    }
}