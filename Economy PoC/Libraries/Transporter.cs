using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
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
                foreach (Stock cusinventoryStocktock in custList)
                {
                    //Check if the Chosen Commodity is the current commdity being looked at
                    if (PickupComm.name == cusinventoryStocktock.commodity.name)
                    {
                        //Checking if the inventory is not empty
                        if (inventory.Count > 0)
                        {
                            //Rewrite inventory
                            foreach (Stock transStock in inventory)
                            {
                                if (transStock.commodity.name != cusinventoryStocktock.commodity.name)
                                {
                                    newInventoryList.Add(transStock);
                                }
                                //If it find the item in the CustList
                                else if (transStock.commodity.name == cusinventoryStocktock.commodity.name)
                                {
                                    newInventoryList.Add(new Stock(transStock.commodity, (transStock.amount + cusinventoryStocktock.amount)));
                                    newCustList.Add(new Stock(cusinventoryStocktock.commodity, 0));
                                    foundInInventory = true;
                                }
                            }
                            if (!foundInInventory)
                            {
                                newInventoryList.Add(cusinventoryStocktock);
                                newCustList.Add(new Stock(cusinventoryStocktock.commodity, 0));
                            }
                        }
                        else
                        {
                            newInventoryList.Add(cusinventoryStocktock);
                            newCustList.Add(new Stock(cusinventoryStocktock.commodity, 0));
                        }
                    }
                    //if iinventoryStock not matching just add the item to the new customer list
                    else
                    {
                        newCustList.Add(cusinventoryStocktock);
                    }
                }
                inventory = newInventoryList;
                custList = newCustList;
            }
            Cleanup();
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
                foreach (Stock inventoryStock in inventory)
                {
                    //Check if the Chosen Commodity is the current commdity being looked at
                    if (DropOffComm.name == inventoryStock.commodity.name)
                    {
                        //Checking if the inventory is not empty
                        if (custList.Count > 0)
                        {
                            //Rewrite inventory
                            foreach (Stock customerStock in custList)
                            {
                                if (inventoryStock.commodity.name != customerStock.commodity.name)
                                {
                                    //newInventoryList.Add(inventoryStock);
                                    newCustList.Add(customerStock);
                                }
                                //If it find the item in the CustList
                                else if (inventoryStock.commodity.name == customerStock.commodity.name)
                                {
                                    newInventoryList.Add(new Stock(inventoryStock.commodity, (0)));
                                    newCustList.Add(new Stock(customerStock.commodity, inventoryStock.amount + customerStock.amount));
                                    foundInCustList = true;
                                }
                            }
                            if (!foundInCustList)
                            {
                                newInventoryList.Add(new Stock(inventoryStock.commodity, 0));
                                newCustList.Add(inventoryStock);
                            }
                        }
                        else
                        {
                            newInventoryList.Add(new Stock(inventoryStock.commodity, 0));
                            newCustList.Add(inventoryStock);
                        }
                    }
                    //if iinventoryStock not matching just add the item to the new customer list
                    else
                    {
                        newInventoryList.Add(inventoryStock);
                    }
                }
                inventory = newInventoryList;
                custList = newCustList;
            }
            Cleanup();
            return custList;
        }

        //Cleans up Empty inventory in the Transporter
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
