using FluentAssertions;
using Xunit;

namespace Ecommerce
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Can_Create()
        {
            var cart = new ShoppingCart();
        }

        [Fact]
        public void When_Construct_Then_Initialise_LineItems_To_Empty_Collection()
        {
            var cart = new ShoppingCart();
            cart.LineItems.Should().BeEmpty();
        }
    }

    public class ShoppingCart
    {

    }
}
