using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Ecommerce
{
    public class ShoppingCartTests
    {
        // Construction
        [Fact]
        public void When_Construct_Then_Initialise_LineItems_To_Empty_Collection()
        {
            var cart = new ShoppingCart();
            cart.LineItems.Should().BeEmpty();
        }

        [Fact]
        public void When_Construct_Then_Total_Is_Zero()
        {
            var cart = new ShoppingCart();
            cart.Total.Should().Be(0);
        }

        // Add
        [Fact]
        public void Given_Null_LineItem_When_Call_AddLineItem_Then_Throw_MissingLineItem()
        {
            var cart = new ShoppingCart();
            Action add = () => cart.AddLineItem(null);
            add.Should().Throw<MissingLineItem>();
        }

        [Fact]
        public void Given_Null_Product_When_Call_AddLineItem_Then_Throw_MissingProduct()
        {
            var cart = new ShoppingCart();
            Action add = () => cart.AddLineItem(new LineItem(null, 3));
            add.Should().Throw<MissingProduct>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-3)]
        [InlineData(-5)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void Given_NonPositive_Quantity_When_Call_AddLineItem_Then_Throw_InvalidQuantity(int quantity)
        {
            var cart = new ShoppingCart();
            Action add = () => cart.AddLineItem(new LineItem(new Product("", 0), quantity));
            add.Should().Throw<InvalidQuantity>().WithMessage($"{quantity} is not a valid Quantity.");
        }

        [Theory]
        [InlineData("Apple", 0.35, 3, 1.05)]
        [InlineData("Banana", 0.59, 7, 4.13)]
        [InlineData("Cantaloupe", 4.50, 17, 76.5)]
        public void Given_Single_Valid_LineItem_When_Call_AddLineItem_Then_Add_LineItem_To_ShoppingCart(string productDesc, 
            decimal unitPrice, int quantity, decimal total)
        {
            var cart = new ShoppingCart();
            cart.AddLineItem(new LineItem(new Product(productDesc, unitPrice), quantity));
            VerifyLineItem(cart.LineItems[0], productDesc, unitPrice, quantity);
            cart.Total.Should().Be(total);
        }

        [Fact]
        public void Given_Multiple_LineItems_When_Call_AddLineItem_Then_Add_LineItems_To_ShoppingCart()
        {
            var cart = new ShoppingCart();
            FillCart(cart, TwentyNine_Apples, Nineteen_Bananas, Thirteen_Cantaloupes);
            VerifyLineItems(cart.LineItems, TwentyNine_Apples, Nineteen_Bananas, Thirteen_Cantaloupes);
            cart.Total.Should().Be(79.86m);
        }

        // Constants
        private static Product Apple = new Product("Apple", 0.35m);
        private static Product Banana = new Product("Banana", 0.59m);
        private static Product Cantaloupe = new Product("Cantaloupe", 4.50m);

        private static LineItem TwentyNine_Apples = new LineItem(Apple, 29);
        private static LineItem Nineteen_Bananas = new LineItem(Banana, 19);
        private static LineItem Thirteen_Cantaloupes = new LineItem(Cantaloupe, 13);

        // Setup
        private void FillCart(ShoppingCart cart, params LineItem[] lineItems)
        {
            foreach (var lineItem in lineItems)
                cart.AddLineItem(lineItem);
        }

        // Assertion
        private void VerifyLineItems(List<LineItem> expectedItems, params LineItem[] actualItems)
        {
            expectedItems.Count.Should().Be(actualItems.Count());
            for (var i = 0; i < expectedItems.Count; i++)
                VerifyLineItem(expectedItems[i], actualItems[i]);
        }

        private void VerifyLineItem(LineItem expectedItem, LineItem actualItem)
        {
            expectedItem.Should().BeEquivalentTo(actualItem);
        }

        private void VerifyLineItem(LineItem lineItem, string productDesc, decimal unitPrice, int quantity)
        {
            var product = lineItem.Product;
            product.Description.Should().Be(productDesc);
            product.UnitPrice.Should().Be(unitPrice);
            lineItem.Quantity.Should().Be(quantity);
            lineItem.Subtotal.Should().Be(quantity * unitPrice);
        }
   }


    public class ShoppingCart
    {
        public decimal Total => CalcTotal();
        public List<LineItem> LineItems { get; private set; } = new List<LineItem>();

        public void AddLineItem(LineItem lineItem)
        {
            Validate(lineItem);
            LineItems.Add(lineItem);
        }


        private void Validate(LineItem lineItem)
        {
            if (lineItem == null)
                throw new MissingLineItem();
            lineItem.Validate();
        }

        private decimal CalcTotal()
        {
            if (!LineItems.Any())
                return 0;
            return LineItems.Sum(x => x.Subtotal);
        }
    }

    public class LineItem
    {
        public Product Product { get; }
        public int Quantity { get; }
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
    }

    public class Product
    {
        public string Description { get; }
        public decimal UnitPrice { get; }


        public Product(string description, decimal unitPrice)
        {
            Description = description;
            UnitPrice = unitPrice;
        }
    }

    public class MissingLineItem : Exception
    {

    }

    public class MissingProduct : Exception
    {

    }

    public class InvalidQuantity : Exception
    {
        public InvalidQuantity(int quantity) 
            : base($"{quantity} is not a valid Quantity.")
        { }
    }
}
