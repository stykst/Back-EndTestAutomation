using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlToHtml.Models;
using System.Text;


namespace YamlToHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Which YAML file would you like to parse? (orders/reservations)");
                string choice = Console.ReadLine()?.ToLower() ?? string.Empty;

                switch (choice)
                {
                    case "orders":
                        var orders = ParseYamlFile<List<Order>>("Datasets/Orders.yaml");
                        DisplayOrders(orders);
                        return; // Exit the loop after successful operation

                    case "reservations":
                        var reservations = ParseYamlFile<List<Reservation>>("Datasets/Reservations.yaml");
                        DisplayReservations(reservations);
                        return; // Exit the loop after successful operation

                    default:
                        Console.WriteLine("Invalid choice. Please enter 'orders' or 'reservations'.");
                        break; // Continue the loop, prompting the user again
                }
            }
        }

        private static T ParseYamlFile<T>(string filePath)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlContent = File.ReadAllText(filePath);
            return deserializer.Deserialize<T>(yamlContent);
        }

        private static void DisplayOrders(List<Order> orders)
        {
            if (orders == null)
            {
                Console.WriteLine("No orders data found or data is not in the expected format.");
                return;
            }

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<html><body>");
            //htmlBuilder.AppendLine("<h1>Orders</h1>");
            htmlBuilder.AppendLine("<ul>");

            foreach (var order in orders)
            {
                htmlBuilder.AppendLine("<li>");
                htmlBuilder.AppendFormat("Order ID: {0}, Customer: {1}, Item: {2}, Quantity: {3}, Total Amount: {4}<br>",
                                         order.OrderId, order.Customer, order.Item, order.Quantity, order.TotalAmount);
                htmlBuilder.AppendLine("</li>");
            }

            htmlBuilder.AppendLine("</ul>");
            htmlBuilder.AppendLine("</body></html>");

            File.WriteAllText("Orders.html", htmlBuilder.ToString());

            string filePath = "Orders.html";
            File.WriteAllText(filePath, htmlBuilder.ToString());
            OpenInBrowser(filePath);
        }
        private static void DisplayReservations(List<Reservation> reservations)
        {
            if (reservations == null)
            {
                Console.WriteLine("No reservations data found or data is not in the expected format.");
                return;
            }

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<html><body>");
            //htmlBuilder.AppendLine("<h1>Reservations</h1>");
            htmlBuilder.AppendLine("<ul>");

            foreach (var reservation in reservations)
            {
                htmlBuilder.AppendLine("<li>");
                htmlBuilder.AppendFormat("Reservation ID: {0}, Guest: {1}<br>", reservation.ReservationId, reservation.GuestName);
                htmlBuilder.AppendLine("<ul>");

                foreach (var service in reservation.Services)
                {
                    htmlBuilder.AppendLine("<li>");
                    htmlBuilder.AppendFormat("Service: {0}, Date: {1}, Time: {2}", service.Type, service.Date, service.Time);
                    htmlBuilder.AppendLine("</li>");
                }

                htmlBuilder.AppendLine("</ul>");
                htmlBuilder.AppendLine("</li>");
            }

            htmlBuilder.AppendLine("</ul>");
            htmlBuilder.AppendLine("</body></html>");

            File.WriteAllText("Reservations.html", htmlBuilder.ToString());

            string filePath = "Reservations.html";
            File.WriteAllText(filePath, htmlBuilder.ToString());
            OpenInBrowser(filePath);
        }

        private static void OpenInBrowser(string filePath)
        {
            try
            {
                var fullPath = Path.GetFullPath(filePath);
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true // This is required to open the file in the default browser
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open the file in browser. Error: {ex.Message}");
            }
        }
    }
    }

