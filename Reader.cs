using System;
using OfficeOpenXml;
using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;

class Reader
{
    static string filepath = ConfigurationManager.AppSettings["filepath"];
    static string connectionString = ConfigurationManager.AppSettings["connectionString"];
    static void Main(string[] args)
    {
        var people = readXLS(filepath);
        DbHelper.StartDb();
        PopulateDb(people);
    }
    // Create a list of classes from the excel sheet data
    static public List<Person> readXLS(string FilePath)
    {
        FileInfo existingFile = new FileInfo(FilePath);
        ExcelPackage package = new ExcelPackage(existingFile);
        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

        List<Person> people = new List<Person>();

        int lastRow = worksheet.Dimension.End.Row;     

        for (int row = 1; row <= lastRow; row++)
            {
                string FirstName = worksheet.Cells[row, 1].Value?.ToString();
                string LastName = worksheet.Cells[row, 2].Value?.ToString();
                int Age = int.Parse(worksheet.Cells[row, 3].Value?.ToString());

                people.Add(new Person{FirstName=FirstName, LastName=LastName, Age=Age});
            }
        
        return people;
    }   
    // MAKE THIS INDEPENDENT OF NUMBER OF COLUMNS.
    static void PopulateDb(List<Person> people)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            foreach (var person in people)
            {
                connection.Execute(@"
                    INSERT INTO people_info (FirstName, LastName, Age)
                    VALUES (@FirstName, @LastName, @Age)", person);
            }

                connection.Close();
        }
    }
}