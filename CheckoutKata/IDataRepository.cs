using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutKata
{
    public interface IDataRepository
    {
        int? GetItemPrice(string item);
    }
}
