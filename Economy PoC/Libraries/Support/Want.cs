using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    public struct Want
    {
        public Commodity commodity { get; private set;}
        public float importance { get; private set;}

        public Want(Commodity _commodity, float _importance)
        {
            commodity = _commodity;
            importance = _importance;
        }
    }
}