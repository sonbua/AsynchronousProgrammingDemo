using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library.NetFramework;

namespace APD.WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
        }

        private void synchronousWaitButton_Click(object sender, EventArgs e)
        {
            Trace.WriteLine($"---before Thread.Sleep() - {Env.Current}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Trace.WriteLine($"--- after Thread.Sleep() - {Env.Current}");

            myIpLabel.Text = "Still on UI thread";
        }

        private async void asynchronousWaitButton_Click(object sender, EventArgs e)
        {
            myIpLabel.Text = "Aynchronous wait 5s...";

            Trace.WriteLine($"---before Task.Delay() - {Env.Current}");
            await Task.Delay(TimeSpan.FromSeconds(5));
            Trace.WriteLine($"--- after Task.Delay() - {Env.Current}");

            myIpLabel.Text = "Resume on UI thread!";
        }

        private async void getIpButton_Click(object sender, EventArgs e)
        {
            myIpLabel.Text = "Checking IP...";

            Trace.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = await GetIpAsync();
            Trace.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            myIpLabel.Text = $"My public IP is {ip}";
        }

        private void deadlockButton_Click(object sender, EventArgs e)
        {
            myIpLabel.Text = "...";

            Trace.WriteLine($"---before GetIpAsync() - {Env.Current}");
            var ip = GetIpAsync().Result;
            Trace.WriteLine($"--- after GetIpAsync() - {Env.Current}");

            myIpLabel.Text = $"My public IP is {ip}";
        }

        private async Task<string> GetIpAsync()
        {
            Trace.WriteLine($"---before HttpClient.GetStringAsync() - {Env.Current}");
            var ip = await _httpClient.GetStringAsync("https://api.ipify.org");
            Trace.WriteLine($"--- after HttpClient.GetStringAsync() - {Env.Current}");

            return ip;
        }
    }
}