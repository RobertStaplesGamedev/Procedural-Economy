using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    class Commodity
    {
        public string name { get; private set; }
        public List<Ingredient> ingredients { get; private set; }
        public int count;

        public Commodity(string _name)
        {
            name = _name;
            ingredients = new List<Ingredient>();
            count = 0;
        }

        public Commodity(string _name, List<Ingredient> _ingredients)
        {
            name = _name;
            ingredients = _ingredients;
        }
    }

    struct Ingredient
    {
        public Commodity commodity;
        public int amount;

        public Ingredient(Commodity _commodity, int _amount)
        {
            commodity = _commodity;
            amount = _amount;
        }
    }

    struct Stock
    {
        public Commodity commodity;
        public int amount;

        public Stock(Commodity _commodity, int _amount)
        {
            commodity = _commodity;
            amount = _amount;
        }
    }
}
