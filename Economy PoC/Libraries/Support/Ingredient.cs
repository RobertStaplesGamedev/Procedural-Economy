using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    public struct Ingredient
    {
        public Commodity commodity;
        public int amount;

        public Ingredient(Commodity _commodity, int _amount)
        {
            commodity = _commodity;
            amount = _amount;
        }
    }
}