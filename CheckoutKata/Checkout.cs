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
            var groupedItems = _items.GroupBy(x => x);
            foreach (var items in groupedItems)
            {
                var itemPrice = _dataRepository.GetItemPrice(items.Key);
                if(!itemPrice.HasValue)
                {
                    throw new ArgumentException();
                }
                result += itemPrice.Value * items.Count();
            }
            return result;
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
