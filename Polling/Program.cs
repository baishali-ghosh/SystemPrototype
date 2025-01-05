// Polling/ShortPollingExample.cs
using System;
using System.Threading.Tasks;

class ShortPollingExample
{
    private static readonly string connectionString = "Server=localhost;Database=baishali_test;User ID=root;Port=3306;"; // Replace with your actual connection string
    private static readonly ServerStatusDao serverStatusDao = new ServerStatusDao(connectionString);

    static async Task Main(string[] args)
    {
        await InitializeServerStatuses();

        Console.Write("Enter the server ID to poll: ");
        if (int.TryParse(Console.ReadLine(), out int serverId))
        {
        
                //await PollServer(serverId);
                //await Task.Delay(5000); // Wait for 5 seconds before the next poll
                // Start a task to update the server status after 5 seconds
                // To test long polling set the serverId as 1
                //string currentStatus = await serverStatusDao.GetStatusByIdAsync(serverId).Status;
                var targetStatus = "Created_" + new Random().Next(10,40);
                Task.Run(() => UpdateServerStatusAfterDelay(serverId, targetStatus, 5));
                await LongPollServerStatus(serverId, "Created", "Created", waitingFor);
                // Reset the status of the server back so sim works
                await UpdateServerStatusAfterDelay(serverId, "Created", 1);
            
        }
        else
        {
            Console.WriteLine("Invalid server ID.");
        }
    }

    private static async Task InitializeServerStatuses()
    {
        await serverStatusDao.CreateTableIfNotExists();
        for (int id = 1; id <= 4; id++)
        {
            var status = await serverStatusDao.GetStatusByIdAsync(id);
            if (status == null)
            {
                // Create new server statuses with specified states
                status = new ServerStatus
                (
                    id,
                    "Queued",
                    DateTime.Now
                );

                switch (id)
                {
                    case 1:
                    case 2:
                        status.Status = "In Progress";
                        break;
                    case 3:
                        status.Status = "Created";
                        break;
                    case 4:
                        status.Status = "Queued";
                        break;
                }

                await serverStatusDao.AddStatusAsync(status);
                Console.WriteLine($"Created Server ID: {status.Id}, Status: {status.Status}");
            }
        }
    }

    private static async Task PollServer(int serverId)
    {
        try
        {
            var status = await serverStatusDao.GetStatusByIdAsync(serverId);
            if (status != null)
            {
                Console.WriteLine($"Server ID: {status.Id}, Status: {status.Status}, Last Updated: {status.LastUpdated}");
            }
            else
            {
                Console.WriteLine($"No status found for Server ID: {serverId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task LongPollServerStatus(int serverId, string currentStatus, string targetStatus)
    {
        Console.WriteLine($"Starting long poll for Server ID: {serverId} till it reaches status: {targetStatus}");
        while (true)
        {
            {
                currentStatus = (await serverStatusDao.GetStatusByIdAsync(serverId)).Status;
                // Check if the status has changed
                if (targetStatus != currentStatus)
                {
                    Console.WriteLine($"Status changed for Server ID: {serverId}. New Status: {targetStatus}");
                    break; // Exit the loop if the status has changed
                }
                else {
                    Console.WriteLine("Status is unchanged, waiting for status changes");
                }
            }

            // Wait for a short period before checking again
            await Task.Delay(500); // Poll every half second
        }
    }


       private static async Task UpdateServerStatusAfterDelay(int serverId, string newStatus, int delayInSeconds)
      {
        Console.WriteLine("Updating post delay");
        await Task.Delay(delayInSeconds * 1000); // Wait for the specified delay

        // Update the server status in the database
        var status = new ServerStatus
        (
            serverId,
            newStatus,
            DateTime.Now
        );

        await serverStatusDao.UpdateStatusAsync(status);
        Console.WriteLine($"Updated Server ID: {serverId} to new status: {newStatus}");
    }

}