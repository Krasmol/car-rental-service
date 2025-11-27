
using System;

namespace CarRentalSystem
{
    public class Client
    {
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DriverLicense { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({DriverLicense})";
        }
    }
}