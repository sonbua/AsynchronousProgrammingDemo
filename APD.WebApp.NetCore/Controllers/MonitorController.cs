using System;
using System.Net.Http;
using System.Threading.Tasks;
using Library.NetCore;
using Microsoft.AspNetCore.Mvc;

namespace APD.WebApp.NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController
    {
        private readonly HttpClient _client;

        public MonitorController()
        {
            _client = new HttpClient();
        }

        [HttpGet]
        [Route("")]
        public async Task<string> Index()
        {
            Console.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = await GetIpAsync();
            Console.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            return ip;
        }

        /// <summary>
        /// Surprisingly there is NO deadlock.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Deadlock")]
        public string Deadlock()
        {
            Console.WriteLine($"---before HttpClient.GetStringAsync() - {Env.Current}");
            var ip = GetIpAsync().Result;
            Console.WriteLine($"--- after HttpClient.GetStringAsync() - {Env.Current}");

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