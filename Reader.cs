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
        OutputHelper.PrintTable(people);
    }

    static public List<Person> readXLS(string FilePath)
    {   
        Console.WriteLine("Reading in data...\n");

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

        Console.Write("Converting to classes...\n");
        return people;
    }   

    // MAKE THIS INDEPENDENT OF NUMBER OF COLUMNS.
    static void PopulateDb(List<Person> people)
    {   
        Console.WriteLine("Populating database...\n");

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
        Console.WriteLine("Data successfully inserted into database.\n");
    }
}