using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;

public class DbHelper
{
    static string connectionString = ConfigurationManager.AppSettings["connectionString"];
    // Create database
    static public void StartDb()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Drop the database if it exists
            string dropTableQuery = $"DROP TABLE IF EXISTS people_info";
            connection.Execute(dropTableQuery);

            Console.WriteLine($"Table people_info deleted successfully.");

            Console.WriteLine("Creating new table...");
            
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS people_info (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT,
                    LastName TEXT,
                    Age INT
                )");    

            connection.Close();

            Console.WriteLine("Table successfuly created. Connection closed;");
        }
    }
}