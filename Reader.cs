using System;
using OfficeOpenXml;
using System.Configuration;

class Reader
{

    static void Main(string[] args)
    {
        Reader.readXLS(ConfigurationManager.AppSettings["filepath"]);
    }
    static public void readXLS(string FilePath)
    {
        FileInfo existingFile = new FileInfo(FilePath);
        using (ExcelPackage package = new ExcelPackage(existingFile))
        {
            //get the first worksheet in the workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int colCount = worksheet.Dimension.End.Column;  //get Column Count
            int rowCount = worksheet.Dimension.End.Row;     //get row count
            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Console.WriteLine(" Row:" + row + " column:" + col + " Value:" + worksheet.Cells[row, col].Value?.ToString().Trim());
                }
            }
        }
    }
}