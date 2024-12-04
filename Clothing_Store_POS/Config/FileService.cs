using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Clothing_Store_POS.Config
{
    public class FileService
    {
        public void ExportCsv<T>(List<T> data, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream)) 
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                // Write data to csv file
                csvWriter.WriteRecords(data);
                streamWriter.Flush();

                byte[] csvData = memoryStream.ToArray();

                // Create file path
                string baseDirectory = AppContext.BaseDirectory;
                string rootProjectPath = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
                string subFolderPath = Path.Combine(rootProjectPath, "Files", "CSV");

                if (!Directory.Exists(subFolderPath))
                {
                    Directory.CreateDirectory(subFolderPath);
                }

                string filePath = Path.Combine(subFolderPath, fileName);

                File.WriteAllBytes(filePath, csvData);
            }
        }
    }
}
