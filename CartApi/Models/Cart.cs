using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartApi.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public string OrderStatus { get; set; }
        public int OrderID { get; set; }

        [NotMapped]
        public int CustomerID { get; set; }
        [NotMapped]
        public List<Product> Products { get; set; }
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int CartID { get; set; }
        public int CustomerID { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
        public double Total { get; set; }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }

    public class CartOrder
    {
        public int OrderID { get; set; }
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public List<Product> Products { get; set; }
    }
}
