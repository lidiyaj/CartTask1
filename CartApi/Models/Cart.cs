namespace CartApi.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public Product Products { get; set; }
        public string OrderStatus { get; set; }
        public int OrderID { get; set; }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public double ProductPrice { get; set; }
        public double Total { get; set; }
    }
}
