using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    public struct Need
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