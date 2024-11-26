using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wypozyczalnia_samochodow.src.Shered.Entities;

namespace Wypozyczalnia_samochodow.src.Shered.Database
{
    internal class CarDatabase
    {
        private const string FilePath = "..\\..\\..\\cars.json";

        public List<Car> LoadCars()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Car>();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Car>>(json) ?? new List<Car>();
        }

        public void SaveCars(List<Car> cars)
        {
            try
            {
                var json = JsonSerializer.Serialize(cars, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd przy zapisie do pliku: {ex.Message}");
            }
        }

        public void ReserveCar(int carId, string renterSurname, int rentalDays)
        {
            var cars = LoadCars();
            var car = cars.Find(c => c.Id == carId);

            if (car != null && car.IsAvailable)
            {
                car.IsAvailable = false;
                car.RenterSurname = renterSurname;
                car.RentalDays = rentalDays;
                SaveCars(cars);
                Console.WriteLine($"Samochód o ID {carId} został pomyślnie zarezerwowany na {rentalDays} dni dla {renterSurname}.");
            }
            else
            {
                Console.WriteLine($"Samochód o ID {carId} jest niedostępny lub nie istnieje.");
            }
        }
    }
}
