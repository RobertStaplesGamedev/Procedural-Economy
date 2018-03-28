using System;
using System.Collections.Generic;
using System.Text;

namespace Economy_PoC
{
    public class Commodity
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
}
