using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutKata
{
    public class SpecialPrice
    {
        public readonly string Item;
        public readonly int Count;
        public readonly int Price;

        public SpecialPrice(string item, int count, int price)
        {
            Item = item;
            Count = count;
            Price = price;
        }
    }
}
