namespace HangfireClient
{
    public interface ITestService
    {
        void Run(string message);
        void CallApi();
    }

    public class TestService : ITestService
    {
        public void CallApi()
        {
            Console.WriteLine("----------------Start---------------");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7275/");
            var data = client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast").GetAwaiter().GetResult();
            foreach (var item in data ?? new List<WeatherForecast>())
            {
                Console.WriteLine($"Date: {item.Date} - TemperatureC: {item.TemperatureF}");
            }
            Console.WriteLine("----------------End---------------");
        }

        public void Run(string message)
        {
            Console.WriteLine($"{message}-{DateTime.Now}");
        }
    }


    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}