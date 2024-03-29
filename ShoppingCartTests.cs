﻿using System;
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
        public void Given_Null_LineItem_When_Call_Add_LineItem_Then_Throw_MissingLineItem()
        {
            var cart = new ShoppingCart();
            Action add = () => cart.Add(null);
            add.Should().Throw<MissingLineItem>();
        }

        [Fact]
        public void Given_Null_Product_When_Call_Add_LineItem_Then_Throw_MissingProduct()
        {
            var cart = new ShoppingCart();
            Action add = () => cart.Add(new LineItem(null, 3));
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
        public void Given_NonPositive_Quantity_When_Call_Add_LineItem_Then_Throw_InvalidQuantity(int quantity)
        {
            var cart = new ShoppingCart();
            Action add = () => cart.Add(new LineItem(new Product(1, "", 0), quantity));
            add.Should().Throw<InvalidQuantity>().WithMessage($"{quantity} is not a valid Quantity.");
        }

        [Theory]
        [InlineData(123, "Apple", 0.35, 3, 1.05)]
        [InlineData(456, "Banana", 0.59, 7, 4.13)]
        [InlineData(789, "Cantaloupe", 4.50, 17, 76.5)]
        public void Given_Single_Valid_LineItem_When_Call_Add_LineItem_Then_Add_LineItem_To_ShoppingCart(int productId,
            string productDesc,
            decimal unitPrice,
            int quantity,
            decimal total)
        {
            var cart = new ShoppingCart();
            cart.Add(new LineItem(new Product(productId, productDesc, unitPrice), quantity));
            VerifyLineItem(cart.LineItems[0], productId, productDesc, unitPrice, quantity);
            cart.Total.Should().Be(total);
        }

        [Fact]
        public void Given_Multiple_LineItems_When_Call_Add_LineItem_Then_Add_LineItems_To_ShoppingCart()
        {
            var cart = new ShoppingCart();
            FillCart(cart, TwentyNine_Apples, Nineteen_Bananas, Thirteen_Cantaloupes);
            VerifyLineItems(cart.LineItems, TwentyNine_Apples, Nineteen_Bananas, Thirteen_Cantaloupes);
            cart.Total.Should()
                .Be(TwentyNine_Apples.Subtotal + Nineteen_Bananas.Subtotal + Thirteen_Cantaloupes.Subtotal);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(3, 5, 8)]
        [InlineData(5, 1, 6)]
        [InlineData(6, 4, 10)]
        public void Given_Product_Already_Exists_In_A_LineItem_When_Call_Add_LineItem_Then_Merge_LineItem_Quantities(
            int initQuantity,
            int moreQuantity,
            int totalQuantity)
        {
            var cart = new ShoppingCart(new LineItem(Apple, initQuantity));
            cart.Add(new LineItem(Apple, moreQuantity));
            cart.LineItems.Count.Should().Be(1);
            VerifyLineItem(new LineItem(Apple, totalQuantity), cart.LineItems[0]);
        }


        // Constants
        private static Product Apple = new Product(123, "Apple", 0.35m);
        private static Product Banana = new Product(456, "Banana", 0.59m);
        private static Product Cantaloupe = new Product(789, "Cantaloupe", 4.50m);

        private static LineItem TwentyNine_Apples = new LineItem(Apple, 29);
        private static LineItem Nineteen_Bananas = new LineItem(Banana, 19);
        private static LineItem Thirteen_Cantaloupes = new LineItem(Cantaloupe, 13);

        // Setup
        private void FillCart(ShoppingCart cart, params LineItem[] lineItems)
        {
            foreach (var lineItem in lineItems)
                cart.Add(lineItem);
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

        private void VerifyLineItem(LineItem lineItem, int productId, string productDesc, decimal unitPrice,
            int quantity)
        {
            VerifyProduct(lineItem.Product, productId, productDesc, unitPrice);
            lineItem.Quantity.Should().Be(quantity);
            lineItem.Subtotal.Should().Be(quantity * unitPrice);
        }

        private void VerifyProduct(Product product, int productId, string productDesc, decimal unitPrice)
        {
            product.Id.Should().Be(productId);
            product.Description.Should().Be(productDesc);
            product.UnitPrice.Should().Be(unitPrice);
        }
    }
}
