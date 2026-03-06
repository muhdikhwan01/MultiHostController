using System;
using LHDN.ExcelToXML.Services;

namespace LHDN.ExcelToXML
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LHDN Excel to XML Converter ===");
            Console.Write("Enter Excel file path: ");
            string filePath = Console.ReadLine() ?? "";

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("❌ File not found.");
                return;
            }

            Console.WriteLine("Processing...");
            var (appType, instruments) = ExcelReader.LoadFromExcel(filePath);

            string outputName = appType == 43 ? "output_sekuriti.xml" : "output_am.xml";
            XmlGenerator.GenerateXml(appType, instruments, outputName);

            Console.WriteLine($"✅ XML generated successfully: {outputName}");
        }
    }
}
