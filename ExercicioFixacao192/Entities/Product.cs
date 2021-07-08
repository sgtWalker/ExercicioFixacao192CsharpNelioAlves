using System.Globalization;

namespace ExercicioFixacao192.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Product()
        {

        }
        public Product(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Name}, {TotalValue().ToString("F2", CultureInfo.InvariantCulture)}";
        }

        private double TotalValue()
        {
            return Quantity * Price;
        }
    }
}
