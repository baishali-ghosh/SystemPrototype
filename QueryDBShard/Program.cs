using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

class Program
{
    // Static dictionary to map userId to shard connection strings
    // Instead of creating 3 shards, assume each open connection is a new shard.
    // Map to show static mapping of user to shard, can make this logic as complex as needed, load from file, hash user_id it
    private static readonly Dictionary<int, string> userShardMap = new Dictionary<int, string>
    {
        { 1, "Server=localhost;Database=baishali_test;User ID=root;Port=3306;" },
        { 2, "Server=localhost;Database=baishali_test;User ID=root;Port=3306;" },
        { 3, "Server=localhost;Database=baishali_test;User ID=root;Port=3306;" }
    };

    

    // List to hold all open connections
    private static readonly List<MySqlConnection> openConnections = new List<MySqlConnection>();

    static void Main(string[] args)
    {
        // Initialize connections
        Init();

        int userId = 1;
        string query = "SELECT * FROM posts WHERE id = @userId";

        // Query the correct shard
        var result = RunQueryOnShard(userId, query);
        var result2 = RunQueryOnShard(5, query); // should comm from connection 2
        Console.WriteLine(result);
        Console.WriteLine(result2);
    }

    // Function to initialize connections for each shard
    private static void Init()
    {
        foreach (var shard in userShardMap)
        {
            string connectionString = shard.Value;

            // Open 3 connections for each shard
            for (int i = 0; i < 3; i++)
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                openConnections.Add(connection); // Add to the list of open connections
            }
        }
    }

    // Function to get the shard ID based on userId
    private static int GetShardIndex(int userId)
    {
        // Determine the shard based on the hash of the userId
        // Assuming here that the userId correspons to the the index of the equivalent connection in the openConnectionsObject
        Console.WriteLine($"Shard index for user ID {userId}: {(userId - 1) %  3}");
        return (userId - 1) %  3;
    }

    // Function to run any query on the target shard
    private static string RunQueryOnShard(int userId, string query)
    {
        int shardIndex = GetShardIndex(userId); // Fetch the index for the shard to get corresponding conn object
        var connection = openConnections[shardIndex];
        

        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@userId", userId);
            var reader = command.ExecuteReader();
            Console.WriteLine($"Using connection index: {shardIndex}");

            if (reader.Read())
            {
                // Assuming the user table has a column named "name"
                return $"Post found: {reader["content"]}";
            }
            else
            {
                return "User not found.";
            }
        }
    }
}
