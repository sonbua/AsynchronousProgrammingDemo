using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Library.NetFramework;

namespace APD.WebApp.NetFramework.Controllers
{
    public class MonitorController : Controller
    {
        private readonly HttpClient _client;

        public MonitorController()
        {
            _client = new HttpClient();
        }

        public async Task<string> Index()
        {
            Console.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = await GetIpAsync();
            Console.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            return ip;
        }

        public string Deadlock()
        {
            Console.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = GetIpAsync().Result; // .GetAwaiter().GetResult();
            Console.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            return ip;
        }

        private async Task<string> GetIpAsync()
        {
            Console.WriteLine($"---before HttpClient.GetStringAsync() - {Env.Current}");
            var ip = await _client.GetStringAsync("https://api.ipify.org");
            Console.WriteLine($"--- after HttpClient.GetStringAsync() - {Env.Current}");

            return ip;
        }
    }
}