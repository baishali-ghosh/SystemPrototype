using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public class ServerStatusDao
{
    private readonly string _connectionString;

    public ServerStatusDao(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task CreateTableIfNotExists()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand(@"
                CREATE TABLE IF NOT EXISTS server_status (
                    id INT PRIMARY KEY,
                    status VARCHAR(50),
                    last_updated DATETIME
                );", connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<ServerStatus>> GetAllStatusesAsync()
    {
        var statuses = new List<ServerStatus>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM server_status", connection);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {                    
                    int serverId =  Convert.ToInt32(reader["id"]);
                    ServerStatus stat = new ServerStatus(serverId, reader["status"].ToString(), null);
                    statuses.Add(stat);
                }
            }
        }
        return statuses;
    }

    public async Task<ServerStatus> GetStatusByIdAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM server_status WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    int serverId =  Convert.ToInt32(reader["id"]);
                    ServerStatus stat = new ServerStatus(serverId, reader["status"].ToString(), null);   
                    return stat;     
                }
            }
        }

        return null; // Not found
    }

    public async Task AddStatusAsync(ServerStatus status)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("INSERT INTO server_status (id, status, last_updated) VALUES (@id, @status, @lastUpdated)", connection);
             command.Parameters.AddWithValue("@id", status.Id);
            command.Parameters.AddWithValue("@status", status.Status);
            command.Parameters.AddWithValue("@lastUpdated", status.LastUpdated);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateStatusAsync(ServerStatus status)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("UPDATE server_status SET status = @status, last_updated = @lastUpdated WHERE id = @id", connection);
            command.Parameters.AddWithValue("@status", status.Status);
            command.Parameters.AddWithValue("@lastUpdated", status.LastUpdated);
            command.Parameters.AddWithValue("@id", status.Id);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteStatusAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("DELETE FROM server_status WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}

public class ServerStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public DateTime? LastUpdated { get; set; }

    public ServerStatus(int id, string status, DateTime? time)
    {
        this.Id = id;
        this.Status = status;
        this.LastUpdated = time;
    }
} 