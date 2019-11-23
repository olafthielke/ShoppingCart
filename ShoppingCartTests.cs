using System;
using System.Collections.Generic;
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
            Action add = () => cart.AddLineItem(new LineItem(null));
            add.Should().Throw<MissingProduct>();
        }
   }


    public class ShoppingCart
    {
        public decimal Total => 0;
        public List<LineItem> LineItems { get; private set; } = new List<LineItem>();

        public void AddLineItem(object lineItem)
        {
            if (lineItem == null)
                throw new MissingLineItem();
            throw new MissingProduct();
        }
    }

    public class LineItem
    {
        public LineItem(object product)
        {

        }
    }

    public class MissingLineItem : Exception
    {

    }

    public class MissingProduct : Exception
    {

    }
}
