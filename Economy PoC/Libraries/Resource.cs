using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    class Resource
    {
        //Variables
        public string name;
        public Commodity commodity;
        public int amount;
        
        public Resource(string _name, Commodity _commodity, int _amount)
        {
            name = _name;
            commodity = _commodity;
            amount = _amount;
        }
    }
}