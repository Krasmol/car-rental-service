using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CarRentalSystem
{
    public class DatabaseHelper
    {
        private string dataFolder;
        private User currentUser;

        public DatabaseHelper()
        {
            dataFolder = Path.Combine(Application.StartupPath, "Data");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            CreateEmptyJsonFiles();
            AddSampleDataIfEmpty();
        }

        public User CurrentUser => currentUser;

        private void CreateEmptyJsonFiles()
        {
            string[] files = { "Cars.json", "Clients.json", "Rentals.json", "Users.json" };

            foreach (string file in files)
            {
                string filePath = Path.Combine(dataFolder, file);
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "[]", Encoding.UTF8);
                }
            }
        }

        private void AddSampleDataIfEmpty()
        {
            var cars = GetCars();
            var users = GetUsers();

            if (cars.Count == 0 && users.Count == 0)
            {
                // Тестовые автомобили
                var sampleCars = new List<Car>
                {
                    new Car { CarID = 1, Brand = "Toyota", Model = "Camry", Year = 2022, Color = "Черный", PricePerDay = 2500, PlateNumber = "А123ВС77" },
                    new Car { CarID = 2, Brand = "Honda", Model = "Civic", Year = 2021, Color = "Белый", PricePerDay = 2000, PlateNumber = "В456ОР77" },
                    new Car { CarID = 3, Brand = "BMW", Model = "X5", Year = 2023, Color = "Синий", PricePerDay = 5000, PlateNumber = "С789ТТ77" }
                };
                SaveCars(sampleCars);

                // Тестовый пользователь
                var sampleUsers = new List<User>
                {
                    new User { UserID = 1, Username = "user1", Password = "123", FirstName = "Иван", LastName = "Петров", Phone = "+79161234567", DriverLicense = "77АВ123456" },
                    new User { UserID = 2, Username = "user2", Password = "123", FirstName = "Мария", LastName = "Сидорова", Phone = "+79167654321", DriverLicense = "77СD654321" }
                };
                SaveUsers(sampleUsers);
            }
        }

        // ========== USERS ==========
        public List<User> GetUsers()
        {
            string filePath = GetFilePath("Users");
            if (!File.Exists(filePath)) return new List<User>();

            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
                return new List<User>();
            }
        }

        public void SaveUsers(List<User> users)
        {
            try
            {
                string filePath = GetFilePath("Users");
                string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения пользователей: {ex.Message}");
            }
        }

        public bool RegisterUser(User user)
        {
            try
            {
                var users = GetUsers();

                // Проверка уникальности логина
                if (users.Any(u => u.Username == user.Username))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return false;
                }
                user.UserID = users.Count > 0 ? users.Max(u => u.UserID) + 1 : 1;
                users.Add(user);
                SaveUsers(users);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
                return false;
            }
        }

        public bool Login(string username, string password)
        {
            var users = GetUsers();
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                currentUser = user;
                return true;
            }

            return false;
        }

        public void Logout()
        {
            currentUser = null;
        }

        // ========== CARS ==========
        public List<Car> GetCars()
        {
            string filePath = GetFilePath("Cars");
            if (!File.Exists(filePath)) return new List<Car>();

            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<List<Car>>(json) ?? new List<Car>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки автомобилей: {ex.Message}");
                return new List<Car>();
            }
        }

        public void SaveCars(List<Car> cars)
        {
            try
            {
                string filePath = GetFilePath("Cars");
                string json = JsonConvert.SerializeObject(cars, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения автомобилей: {ex.Message}");
            }
        }

        public bool AddCar(Car car)
        {
            try
            {
                var cars = GetCars();
                car.CarID = cars.Count > 0 ? cars.Max(c => c.CarID) + 1 : 1;
                cars.Add(car);
                SaveCars(cars);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления автомобиля: {ex.Message}");
                return false;
            }
        }

        // ========== CLIENTS ==========
        public List<Client> GetClients()
        {
            string filePath = GetFilePath("Clients");
            if (!File.Exists(filePath)) return new List<Client>();

            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<List<Client>>(json) ?? new List<Client>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
                return new List<Client>();
            }
        }

        public void SaveClients(List<Client> clients)
        {
            try
            {
                string filePath = GetFilePath("Clients");
                string json = JsonConvert.SerializeObject(clients, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения клиентов: {ex.Message}");
            }
        }

        public bool AddClient(Client client)
        {
            try
            {
                var clients = GetClients();
                client.ClientID = clients.Count > 0 ? clients.Max(c => c.ClientID) + 1 : 1;
                clients.Add(client);
                SaveClients(clients);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}");
                return false;
            }
        }
        // ========== RENTALS ==========
        public List<Rental> GetRentals()
        {
            string filePath = GetFilePath("Rentals");
            if (!File.Exists(filePath)) return new List<Rental>();

            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<List<Rental>>(json) ?? new List<Rental>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки аренд: {ex.Message}");
                return new List<Rental>();
            }
        }

        public void SaveRentals(List<Rental> rentals)
        {
            try
            {
                string filePath = GetFilePath("Rentals");
                string json = JsonConvert.SerializeObject(rentals, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения аренд: {ex.Message}");
            }
        }

        public bool AddRental(Rental rental)
        {
            try
            {
                var rentals = GetRentals();
                rental.RentalID = rentals.Count > 0 ? rentals.Max(r => r.RentalID) + 1 : 1;
                rentals.Add(rental);
                SaveRentals(rentals);

                // Помечаем автомобиль как занятый
                var cars = GetCars();
                var car = cars.FirstOrDefault(c => c.CarID == rental.CarID);
                if (car != null)
                {
                    car.IsAvailable = false;
                    SaveCars(cars);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления аренды: {ex.Message}");
                return false;
            }
        }

        public bool CompleteRental(int rentalId)
        {
            try
            {
                var rentals = GetRentals();
                var rental = rentals.FirstOrDefault(r => r.RentalID == rentalId);
                if (rental != null && rental.Status == "Активна")
                {
                    rental.Status = "Завершена";
                    rental.ReturnDate = DateTime.Now;

                    // Освобождаем автомобиль
                    var cars = GetCars();
                    var car = cars.FirstOrDefault(c => c.CarID == rental.CarID);
                    if (car != null)
                    {
                        car.IsAvailable = true;
                        SaveCars(cars);
                    }

                    SaveRentals(rentals);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка завершения аренды: {ex.Message}");
                return false;
            }
        }

        public List<Rental> GetUserRentals(int userId)
        {
            return GetRentals().Where(r => r.ClientID == userId).ToList();
        }

        // ========== DATA TABLE FOR GRID ==========
        public DataTable GetData(string tableName)
        {
            var dt = new DataTable();

            switch (tableName)
            {
                case "Cars":
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Марка", typeof(string));
                    dt.Columns.Add("Модель", typeof(string));
                    dt.Columns.Add("Год", typeof(int));
                    dt.Columns.Add("Цвет", typeof(string));
                    dt.Columns.Add("Цена/день", typeof(decimal));
                    dt.Columns.Add("Госномер", typeof(string));
                    dt.Columns.Add("Доступен", typeof(bool));
                    foreach (var car in GetCars())
                    {
                        dt.Rows.Add(car.CarID, car.Brand, car.Model, car.Year, car.Color,
                                   car.PricePerDay, car.PlateNumber, car.IsAvailable);
                    }
                    break;

                case "Clients":
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Имя", typeof(string));
                    dt.Columns.Add("Фамилия", typeof(string));
                    dt.Columns.Add("Телефон", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Водительские права", typeof(string));
                    dt.Columns.Add("Дата регистрации", typeof(string));

                    foreach (var client in GetClients())
                    {
                        dt.Rows.Add(client.ClientID, client.FirstName, client.LastName, client.Phone,
                                   client.Email, client.DriverLicense, client.RegistrationDate.ToString("dd.MM.yyyy"));
                    }
                    break;

                case "Rentals":
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("ID автомобиля", typeof(int));
                    dt.Columns.Add("ID клиента", typeof(int));
                    dt.Columns.Add("Дата аренды", typeof(string));
                    dt.Columns.Add("Дней", typeof(int));
                    dt.Columns.Add("Стоимость", typeof(decimal));
                    dt.Columns.Add("Статус", typeof(string));

                    foreach (var rental in GetRentals())
                    {
                        dt.Rows.Add(rental.RentalID, rental.CarID, rental.ClientID,
                                   rental.RentalDate.ToString("dd.MM.yyyy HH:mm"),
                                   rental.Days, rental.TotalPrice, rental.Status);
                    }
                    break;
            }

            return dt;
        }

        public DataTable GetAvailableCarsData()
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Марка", typeof(string));
            dt.Columns.Add("Модель", typeof(string));
            dt.Columns.Add("Год", typeof(int));
            dt.Columns.Add("Цвет", typeof(string));
            dt.Columns.Add("Цена/день", typeof(decimal));
            dt.Columns.Add("Госномер", typeof(string));

            foreach (var car in GetCars().Where(c => c.IsAvailable))
            {
                dt.Rows.Add(car.CarID, car.Brand, car.Model, car.Year, car.Color,
                           car.PricePerDay, car.PlateNumber);
            }

            return dt;
        }

        public DataTable GetUserRentalsData(int userId)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Автомобиль", typeof(string));
            dt.Columns.Add("Дата начала", typeof(string));
            dt.Columns.Add("Дней", typeof(int));
            dt.Columns.Add("Стоимость", typeof(decimal));
            dt.Columns.Add("Статус", typeof(string));

            var userRentals = GetUserRentals(userId);
            var cars = GetCars();

            foreach (var rental in userRentals)
            {
                var car = cars.FirstOrDefault(c => c.CarID == rental.CarID);
                string carInfo = car != null ? $"{car.Brand} {car.Model}" : "Неизвестно";

                dt.Rows.Add(rental.RentalID, carInfo,
                           rental.RentalDate.ToString("dd.MM.yyyy HH:mm"),
                           rental.Days, rental.TotalPrice, rental.Status);
            }

            return dt;
        }

        private string GetFilePath(string tableName)
        {
            return Path.Combine(dataFolder, $"{tableName}.json");
        }
    }
}