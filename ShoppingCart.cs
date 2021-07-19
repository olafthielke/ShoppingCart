using System.Collections.Generic;
using System.Linq;

namespace Ecommerce
{
    public class ShoppingCart
    {
        public decimal Total => LineItems.Sum(x => x.Subtotal);
        public List<LineItem> LineItems { get; } = new List<LineItem>();


        public ShoppingCart()
        { }

        public ShoppingCart(LineItem lineItem)
        {
            Add(lineItem);
        }


        public void Add(LineItem newLineItem)
        {
            Validate(newLineItem);

            foreach (var lineItem in LineItems)
                if (lineItem.Product.Id == newLineItem.Product.Id)
                {
                    lineItem.AddQuantity(newLineItem.Quantity);
                    return;
                }

            LineItems.Add(newLineItem);
        }


        private static void Validate(LineItem newLineItem)
        {
            if (newLineItem == null)
                throw new MissingLineItem();
            newLineItem.Validate();
        }
    }
}
