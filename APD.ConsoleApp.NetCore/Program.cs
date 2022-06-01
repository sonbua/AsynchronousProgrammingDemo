using System;
using System.Net.Http;
using System.Threading.Tasks;
using Library.NetCore;

namespace APD.ConsoleApp.NetCore
{
    internal static class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
            // MainSync();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = await GetIpAsync();
            Console.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            Console.WriteLine($"My public IP: {ip}");
        }

        private static void MainSync()
        {
            Console.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = GetIpAsync().Result;
            Console.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            Console.WriteLine($"My public IP: {ip}");
        }

        private static async Task<string> GetIpAsync()
        {
            Console.WriteLine($"---before HttpClient.GetStringAsync() - {Env.Current}");
            var ip = await Client.GetStringAsync("https://api.ipify.org");
            Console.WriteLine($"--- after HttpClient.GetStringAsync() - {Env.Current}");

            return ip;
        }
    }
}