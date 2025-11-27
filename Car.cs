using System;

namespace CarRentalSystem
{
    public class Car
    {
        public int CarID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string PlateNumber { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} ({Year}) - {PricePerDay} руб/день";
        }
    }
}