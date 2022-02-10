namespace DiscountStore.Models
{
    public class Item
    {
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0m;

        public override int GetHashCode() => SKU.GetHashCode();

        public override bool Equals(object obj) => (obj as Item)?.SKU == SKU;
    }
}
