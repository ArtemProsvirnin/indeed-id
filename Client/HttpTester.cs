using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Client
{
    internal class HttpTester
    {
        private static readonly HttpClient _client = new HttpClient();

        private string _host;

        public HttpTester(string host)
        {
            _host = host;
        }

        internal async void Run(int requestCount, int timeRange)
        {
            var infinite = requestCount == -1;
            var loop = true;
            var random = new Random();

            while (loop)
            {
                await SendRequests(_host);
                await Task.Delay(random.Next(timeRange));

                if (!infinite)
                    requestCount--;

                loop = requestCount != 0;
            }

            Program.WriteLine("Запросы закончились, нажмите Enter для выхода", ConsoleColor.Green);
        }

        private static async Task SendRequests(string host)
        {
            var json = JsonConvert.SerializeObject(new { description = "Запрос в службу поддержки" });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{host}/tasks/create", httpContent);
            json = await response.Content.ReadAsStringAsync();

            Program.WriteLine($"Запрос помещен в очередь, ответ сервера: {json}");
        }
    }
}
