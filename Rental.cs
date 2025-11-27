
using System;

namespace CarRentalSystem
{
    public class Rental
    {
        public int RentalID { get; set; }
        public int CarID { get; set; }
        public int ClientID { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }
        public int Days { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Активна";

        public override string ToString()
        {
            return $"Аренда #{RentalID} - {Status}";
        }
    }
}