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

        internal void Run(int requestCount, int timeRange)
        {            
            if (requestCount == -1)
                InfiniteLoop(timeRange);
            else
                Loop(requestCount, timeRange);
        }

        private async void InfiniteLoop(int timeRange)
        {
            while (true)
                await SendRequest(timeRange);
        }

        private async void Loop(int requestCount, int timeRange)
        {
            while (0 < requestCount--)
                await SendRequest(timeRange);

            Program.WriteLine("Запросы закончились, нажмите Enter для выхода", ConsoleColor.Green);
        }

        private async Task SendRequest(int timeRange)
        {
            var random = new Random();

            await SendRequest();
            await Task.Delay(random.Next(timeRange));
        }

        private async Task SendRequest()
        {
            var json = JsonConvert.SerializeObject(new { description = "Запрос в службу поддержки" });
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_host}/tasks/create", httpContent);
            json = await response.Content.ReadAsStringAsync();

            Program.WriteLine($"Запрос помещен в очередь, ответ сервера: {json}");
        }
    }
}
