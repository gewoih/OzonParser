namespace OzonParser.Models
{
    public sealed class Product
    {
        public ulong Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int FeebacksCount { get; set; }

        public override string? ToString()
        {
            return $"{Id};{Brand};{Title};{Price};{FeebacksCount}";
        }
    }
}
