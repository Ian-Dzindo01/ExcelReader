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

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS people_info (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT,
                    LastName TEXT,
                    Age INT
                )");    

            connection.Close();
        }
    }
}