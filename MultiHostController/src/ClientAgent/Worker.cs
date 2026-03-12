using System.Net.Http.Json;
using System.Diagnostics; // Used to run scripts or system commands

namespace ClientAgent;

public class Worker : BackgroundService
{
    // HttpClient used to communicate with MasterController API
    private readonly HttpClient _httpClient = new HttpClient();

    // This will store the ID assigned by MasterController when the agent registers
    private int hostId;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //-----------------------------------------------------------
        // STEP 1: Prepare host information for registration
        //-----------------------------------------------------------
        var host = new
        {
            hostname = Environment.MachineName,   // Current machine name
            ipAddress = "127.0.0.1",              // Local IP (can improve later)
            os = Environment.OSVersion.ToString() // OS information
        };

        //-----------------------------------------------------------
        // STEP 2: Register this agent with the MasterController
        //-----------------------------------------------------------
        var response = await _httpClient.PostAsJsonAsync(
            "http://localhost:5200/api/hosts/register",
            host);

        // Read the response from API and extract assigned hostId
        var registeredHost = await response.Content.ReadFromJsonAsync<HostResponse>();

        hostId = registeredHost.Id;

        Console.WriteLine($"Registered with ID {hostId}");

        //-----------------------------------------------------------
        // MAIN LOOP: Agent lifecycle
        //-----------------------------------------------------------
        while (!stoppingToken.IsCancellationRequested)
        {
            //-------------------------------------------------------
            // STEP 3: Send heartbeat to MasterController
            // This tells the server that this host is still alive
            //-------------------------------------------------------
            await _httpClient.PostAsync(
                $"http://localhost:5200/api/hosts/heartbeat/{hostId}",
                null);

            Console.WriteLine("Heartbeat sent");


            //-------------------------------------------------------
            // STEP 4: Ask MasterController if there are tasks
            // assigned to this host
            //-------------------------------------------------------
            var tasks = await _httpClient.GetFromJsonAsync<List<DeploymentTask>>(
                $"http://localhost:5200/api/tasks/{hostId}");

            // If tasks exist, execute them
            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Executing task: {task.Command}");

                    var result = ExecuteCommand(task.Command);

                    await _httpClient.PostAsJsonAsync(
                        "http://localhost:5200/api/tasks/result",
                        new
                        {
                            taskId = task.Id,
                            status = result.Success ? "Success" : "Failed",
                            output = result.Output,
                            completedAt = DateTime.UtcNow
                        });

                    Console.WriteLine($"Task {task.Id} completed");
                }
            }

            //-------------------------------------------------------
            // Wait 5 seconds before next heartbeat and task poll
            //-------------------------------------------------------
            await Task.Delay(5000, stoppingToken);
        }
    }

    private (bool Success, string Output) ExecuteCommand(string command)
    {
        if (command == "install-minio")
        {
            Console.WriteLine("Starting MinIO installation...");

            string scriptPath = Path.Combine(
                AppContext.BaseDirectory,
                "scripts",
                "install-minio.ps1");

            var process = new Process();

            process.StartInfo.FileName = "powershell";
            process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"";

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            string finalOutput = output + error;

            bool success = process.ExitCode == 0;

            Console.WriteLine(finalOutput);

            return (success, finalOutput);
        }

        return (false, "Unknown command");
    }
}

//////////////////////////////////////////////////////////////////////////
// RESPONSE MODEL
// Used to read the host ID returned from the MasterController API
//////////////////////////////////////////////////////////////////////////

public class HostResponse
{
    public int Id { get; set; }
}

//////////////////////////////////////////////////////////////////////////
// TASK MODEL
// Represents a deployment task sent from MasterController
//////////////////////////////////////////////////////////////////////////

public class DeploymentTask
{
    public int Id { get; set; }

    public int HostId { get; set; }

    public string Command { get; set; }

    public string Status { get; set; }
}