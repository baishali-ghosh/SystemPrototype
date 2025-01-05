# Server Status Polling Application

This application demonstrates a simple polling mechanism to check the status of servers stored in a MySQL database. It uses a Data Access Object (DAO) pattern to interact with the database and allows users to poll the status of specific servers.

## Features

- Initializes server statuses in the database if they do not exist.
- Polls the status of a server at regular intervals (Short Polling Imple)
- Supports CRUD operations on the `server_status` table.

## Technologies Used

- C#
- MySQL
- MySql.Data.MySqlClient (for direct MySQL interaction)

## Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/server-status-polling.git
   cd server-status-polling
   ```

2. **Install Dependencies**
   Make sure you have the necessary NuGet packages installed. You can do this via the Package Manager Console:
   ```bash
   Install-Package MySql.Data
   ```

3. **Configure Database**
   - Create a MySQL database and a table named `server_status` with the following structure:
     ```sql
     CREATE TABLE server_status (
         id INT PRIMARY KEY,
         status VARCHAR(50),
         last_updated DATETIME
     );
     ```
   - Update the connection string in `Polling/Program.cs`:
     ```csharp
     private static readonly string connectionString = "your_connection_string_here";
     ```

4. **Run the Application**
   - Build and run the application using your preferred IDE or command line.
   - Follow the prompts to enter a server ID to poll.

## Usage

- Upon running the application, it will check for the existence of server IDs 1, 2, 3, and 4. If they do not exist, it will create them with the following statuses:
  - ID 1: In Progress
  - ID 2: In Progress
  - ID 3: Created
  - ID 4: Queued

- You will be prompted to enter a server ID to poll. The application will then display the current status of the specified server every 5 seconds.

## Code Structure

- **DataAccess/ServerStatusDao.cs**: Contains the DAO implementation for interacting with the `server_status` table.
- **Polling/Program