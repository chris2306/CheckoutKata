using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutKata
{
    public interface IDataRepository
    {
        int? GetItemPrice(string item);

        /// <summary>
        /// Gets the special price for an item.  If we pass in the count we can filter out the special price here, if we dont have enough items
        /// </summary>
        /// <param name="item">The item to get the special price for</param>
        /// <param name="count">Number of that item scanned</param>
        /// <returns></returns>
        SpecialPrice GetItemSpecialPrice(string item, int count);
    }
}
