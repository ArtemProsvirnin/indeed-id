using System;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Client
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();

        public static int TimeRange = 5000; //milliseconds
        public static int RequestCount = -1;

        static void Main(string[] args)
        {
            if (ApplyParameters(args))
                SendRequests();

            Console.ReadLine();
        }

        private static bool ApplyParameters(string[] args)
        {
            var c = new Configurator(args);
            return c.Config();
        }

        private static void SendRequests()
        {
            string host = ConfigurationManager.AppSettings["Host"];
            SendRequests(host, RequestCount == -1);
        }

        private static async void SendRequests(string host, bool infinite)
        {
            var loop = true;
            var random = new Random();

            while (loop)
            {
                await SendRequests(host);
                await Task.Delay(random.Next(TimeRange));

                if (!infinite)
                    RequestCount--;

                loop = RequestCount != 0;
            }

            Console.WriteLine("Запросы закончились");
        }

        private static async Task SendRequests(string host)
        {
            var json = JsonConvert.SerializeObject(new { description = "Запрос в службу поддержки" });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{host}/tasks/create", httpContent);
            json = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Запрос помещен в очередь, ответ сервера: {json}");
        }
    }
}
