using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CheckoutKata
{
    public class Checkout : ICheckout
    {
        private IDataRepository _dataRepository;
        private List<string> _items;

        /// <summary>
        /// Constructor.  Use dependency injection to get these parameters
        /// </summary>
        /// <param name="dataRepository">Repository for data needed to calculate the total price</param>
        public Checkout(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            _items = new List<string>();
        }

        public int GetTotalPrice()
        {
            int result = 0;
            //Group the items so we only need to get the price once for each item type
            var groupedItems = _items.GroupBy(x => x);
            foreach (var items in groupedItems)
            {
                result += GetItemsPrice(items.Key, items.Count());
            }
            return result;
        }

        public int GetItemsPrice(string item, int count)
        {
            var itemPrice = _dataRepository.GetItemPrice(item);
            if (!itemPrice.HasValue)
            {
                throw new ArgumentException();
            }
            return itemPrice.Value * count;
        }

        public void Scan(string item)
        {
            if(string.IsNullOrEmpty(item))
            {
                //log also
                throw new ArgumentNullException();
            }
            _items.Add(item);
        }
    }
}
