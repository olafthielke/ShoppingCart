using System;

namespace Ecommerce
{
    public class InvalidQuantity : Exception
    {
        public InvalidQuantity(int quantity)
            : base($"{quantity} is not a valid Quantity.")
        { }
    }
}
