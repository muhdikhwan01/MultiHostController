using System.Net.Http.Json;

namespace ClientAgent;

public class Worker : BackgroundService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private int hostId;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var host = new
        {
            hostname = Environment.MachineName,
            ipAddress = "127.0.0.1",
            os = Environment.OSVersion.ToString()
        };

        // Register host
        var response = await _httpClient.PostAsJsonAsync(
            "http://localhost:5200/api/hosts/register",
            host);

        var registeredHost = await response.Content.ReadFromJsonAsync<HostResponse>();
        hostId = registeredHost.Id;

        Console.WriteLine($"Registered with ID {hostId}");

        // Heartbeat loop
        while (!stoppingToken.IsCancellationRequested)
        {
            await _httpClient.PostAsync(
                $"http://localhost:5200/api/hosts/heartbeat/{hostId}",
                null);

            Console.WriteLine("Heartbeat sent");

            await Task.Delay(5000, stoppingToken);
        }
    }
}

public class HostResponse
{
    public int Id { get; set; }
}