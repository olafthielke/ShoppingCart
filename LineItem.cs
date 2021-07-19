namespace Ecommerce
{
    public class LineItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }
        public decimal Subtotal => Product.UnitPrice * Quantity;

        public LineItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public void Validate()
        {
            if (Product == null)
                throw new MissingProduct();
            if (Quantity <= 0)
                throw new InvalidQuantity(Quantity);
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}
