using System.Collections.Generic;

namespace CartApi.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public List<Product> Products { get; set; }
        public string OrderStatus { get; set; }
        public int OrderID { get; set; }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
