using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    class Transporter
    {
        public string name { get; private set; }
        public List<Stock> inventory;
        public float speed { get; private set; }
        public float deltaSpeed;
        private bool isTravellingToDestination;

        public Producer pickupProducer  { get; private set; }
        public Producer dropoffProducer  { get; private set; }

        public Population dropoffPopulation  { get; private set; }

        public Transporter(string _name)
        {
            name = _name;
            inventory = new List<Stock>();
            speed = 0;
            isTravellingToDestination = true;
        }
        public Transporter(string _name, float _speed)
        {
            name = _name;
            inventory = new List<Stock>();
            speed = _speed;
            deltaSpeed = 0;
            isTravellingToDestination = true;
        }
        public Transporter(string _name, float _speed, Producer _pickupProducer, Producer _dropoffProducer)
        {
            name = _name;
            inventory = new List<Stock>();
            speed = _speed;
            deltaSpeed = 0;
            isTravellingToDestination = true;
            pickupProducer = _pickupProducer;
            dropoffProducer = _dropoffProducer;
        }
        public Transporter(string _name, float _speed, Producer _pickupProducer, Population _dropoffPopulation)
        {
            name = _name;
            inventory = new List<Stock>();
            speed = _speed;
            deltaSpeed = 0;
            isTravellingToDestination = true;
            pickupProducer = _pickupProducer;
            dropoffPopulation = _dropoffPopulation;
        }
        
        public List<Stock> Pickup(Commodity PickupComm, List<Stock> custList)
        {
            List<Stock> newCustList = new List<Stock>();
            List<Stock> newInventoryList = new List<Stock>();
            bool foundInInventory = false;

            
            //Check if the customer list has been initialised
            if (custList.Count > 0 && TransporterLoctaion(true))
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
                    //if inventoryStock not matching just add the item to the new customer list
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

            if (inventory.Count > 0 && TransporterLoctaion(false))
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
        private bool TransporterLoctaion(bool isPickup)
        {
            //if speed = 0 delivery is instant and the transaction just happens
            if (speed == 0)
            {
                return true;
            }
            //Starting position is 0 deltaspeed (at pickup)
            else if(isPickup && Math.Round(deltaSpeed, 2) == 0)
            {
                isTravellingToDestination = true;
                deltaSpeed += 0.01f;
                return true;
            }
            //Traveling to dropoff
            else if (isTravellingToDestination && Math.Round(deltaSpeed, 2) < Math.Round(speed, 2))
            {
                deltaSpeed += 0.01f;
                return false;
            }
            //Ending posittion is deltaspeed = speed (dropoff)
            else if (!isPickup && isTravellingToDestination && Math.Round(deltaSpeed, 2) == Math.Round(speed, 2))
            {
                deltaSpeed -= 0.01f;
                isTravellingToDestination = false;
                return true;
            }
            //Traveling from dropoff
            else if (!isTravellingToDestination && Math.Round(deltaSpeed, 2) < Math.Round(speed, 2))
            {
                deltaSpeed -= 0.01f;
                return false;
            }
            else
            {
                //Throw error
                return false;
            }
        }
    }
}
