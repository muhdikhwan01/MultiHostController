using System.Net.Http.Json;

namespace ClientAgent;

public class Worker : BackgroundService
{
    private readonly HttpClient _httpClient = new HttpClient();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var host = new
        {
            hostname = Environment.MachineName,
            ipAddress = "127.0.0.1",
            os = Environment.OSVersion.ToString()
        };

        bool registered = false;

        while (!registered && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _httpClient.PostAsJsonAsync(
                    "http://localhost:5200/api/hosts/register",
                    host);

                registered = true;
            }
            catch
            {
                Console.WriteLine("Master not ready. Retrying...");
                await Task.Delay(3000);
            }
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(10000, stoppingToken);
        }
    }
}