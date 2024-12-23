using CsvHelper;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Clothing_Store_POS.Config
{
    public class FileService
    {
        private string _rootPath { get; set; }

        public FileService()
        {
            string baseDirectory = AppContext.BaseDirectory;
            _rootPath = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
        }

        public string GetRootPath()
        {
            return _rootPath;
        }

        public void ExportCsv<T>(List<T> data, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream)) 
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                // Create file path
                
                string subFolderPath = Path.Combine(_rootPath, "Files", "CSV");

                if (!Directory.Exists(subFolderPath))
                {
                    Directory.CreateDirectory(subFolderPath);
                }

                string filePath = Path.Combine(subFolderPath, fileName);

                // Write data to csv file
                csvWriter.WriteRecords(data);
                streamWriter.Flush();

                byte[] csvData = memoryStream.ToArray();

                File.WriteAllBytes(filePath, csvData);
            }
        }

        public void ExportPdf<T>(List<T> data, string filename)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Init writer
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Add title
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                document.Add(new Paragraph("Exported data").SetFont(boldFont).SetFontSize(16));

                // create table
                var table = new Table(typeof(T).GetProperties().Length);

                // add name columns
                foreach (var property in typeof(T).GetProperties())
                {
                    table.AddCell(new Cell().Add(new Paragraph(property.Name).SetFont(boldFont).SetFontSize(8)));
                }    

                // Add data
                foreach (var item in data)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var value = property.GetValue(item)?.ToString() ?? string.Empty;
                        table.AddCell(new Cell().Add(new Paragraph(value).SetFontSize(8)));
                    }
                }

                document.Add(table);
                document.Close();

                var pdfData = memoryStream.ToArray();

                // Create file path
                string subFolderPath = Path.Combine(_rootPath, "Files", "PDF");

                if (!Directory.Exists(subFolderPath))
                {
                    Directory.CreateDirectory(subFolderPath);
                }

                string filePath = Path.Combine(subFolderPath, filename);

                File.WriteAllBytes(filePath, pdfData);
            }
        }
    
        public List<T> ImportCsv<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read records
                var records = csvReader.GetRecords<T>().ToList();

                return records;
            }
        }
    }
}
