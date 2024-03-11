using Spectre.Console;

public class OutputHelper
{
    static public void PrintTable(List<Person> people)
    {
        var table = new Table();
        table.AddColumn("First Name");
        table.AddColumn("Last Name");
        table.AddColumn("Age");

        foreach(var person in people)
            table.AddRow(person.FirstName, person.LastName, person.Age.ToString());
    
        AnsiConsole.Render(table);
    }
}