using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce
{
    public class Product
    {
        public int Id { get; }
        public string Description { get; }
        public decimal UnitPrice { get; }


        public Product(int id, string description, decimal unitPrice)
        {
            Id = id;
            Description = description;
            UnitPrice = unitPrice;
        }
    }
}
