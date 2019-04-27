using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutKata
{
    interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }
}
