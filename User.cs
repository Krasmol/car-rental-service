using System;

namespace CarRentalSystem
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DriverLicense { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Username})";
        }
    }
}