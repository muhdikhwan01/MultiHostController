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

                    // Execute command locally on this machine
                    ExecuteCommand(task.Command);

                    //---------------------------------------------------
                    // STEP 5: Notify MasterController task completed
                    //---------------------------------------------------
                    await _httpClient.PostAsync(
                        $"http://localhost:5200/api/tasks/complete/{task.Id}",
                        null);

                    Console.WriteLine($"Task {task.Id} completed");
                }
            }

            //-------------------------------------------------------
            // Wait 5 seconds before next heartbeat and task poll
            //-------------------------------------------------------
            await Task.Delay(5000, stoppingToken);
        }
    }

    // This method executes commands received from the MasterController
    private void ExecuteCommand(string command)
    {
        // Check if the command is to install MinIO
        if (command == "install-minio")
        {
            Console.WriteLine("Starting MinIO installation...");

            // Build absolute path to the script
            string scriptPath = Path.Combine(
                AppContext.BaseDirectory,
                "scripts",
                "install-minio.ps1");

            // Configure PowerShell process
            var process = new Process();
            process.StartInfo.FileName = "powershell";
            process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"";

            // Allow reading script output
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Start script
            process.Start();

            // Capture output
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait until script completes
            process.WaitForExit();

            // Print logs
            Console.WriteLine("Script Output:");
            Console.WriteLine(output);

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("Script Error:");
                Console.WriteLine(error);
            }

            Console.WriteLine("MinIO installation finished.");
        }
        else
        {
            Console.WriteLine($"Unknown command received: {command}");
        }
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