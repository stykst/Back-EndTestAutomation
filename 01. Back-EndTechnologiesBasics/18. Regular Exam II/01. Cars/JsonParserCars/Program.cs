using System.Text.Json;
using JsonParser.Models;

namespace JsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseCars();
        }

        

        // Parses and displays cars data
        private static void ParseCars()
        {
            string jsonFilePath = Path.Combine("Datasets", "Cars.json");
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                List<Car>? cars = JsonSerializer.Deserialize<List<Car>>(json);
                DisplayCars(cars);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }



        // Displays the list of cars
        private static void DisplayCars(List<Car>? cars)
        {
            if (cars == null)
            {
                Console.WriteLine("No cars data found or data is not in the expected format.");
                return;
            }

            Console.WriteLine("Cars:");
            int carNumber = 1;
            foreach (var car in cars)
            {
                Console.WriteLine($"Cars #{carNumber}");
                Console.WriteLine($"ID: {car.ModelId} Name: {car.CarName}");
                Console.WriteLine($"Year Manufactured: {car.YearOfManufacture} Fuel Efficiency: {car.FuelEfficiency}");
                string features = String.Join(", ", car.Features);
                Console.WriteLine($"Features: {features}");

                carNumber++;
            }
        }

        // Handles errors that occur during JSON parsing
        private static void HandleError(Exception e)
        {
            if (e is JsonException)
            {
                Console.WriteLine($"JSON Parsing Error: {e.Message}");
            }
            else
            {
                Console.WriteLine($"An unexpected error occurred: {e.Message}");
            }
        }
    }
}