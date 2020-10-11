namespace APIProxy1.Models
{
    public class ProductModel
    {
        public string name { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
    }

    public enum SortType
    {
        Low = 1,
        High = 2,
        Ascending = 3,
        Descending = 4,
        Recommended = 5
    }
}