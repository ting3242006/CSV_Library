using CSV_Library.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class CSVHelper
    {
        // 多筆資料
        public static void Write<T>(string filePath, IEnumerable<T> records)
        {
            // 用一個 function 寫在 CsvHelper
            FileUtils.EnsureCsvExtention(filePath);
            FileUtils.EnsureDirectory(filePath);
            // get HeaderEnum
            string propertyHeader = String.Join(",", typeof(T).GetProperties().Select(x => x.Name));
            HeaderStatus status = HeaderManager.DetectHeaderStatus(filePath, propertyHeader);

            // call factory 拿到 AWritePattern
            AWritePattern pattern = WritePatternFactory.WritePattern(status, filePath);

            pattern.WriteDataLines(filePath, records);
        }
        // 一筆資料
        public static void Write<T>(string filePath, T record)
        {
            // 用一個 function 寫在 CsvHelper
            FileUtils.EnsureCsvExtention(filePath);
            FileUtils.EnsureDirectory(filePath);
            // get HeaderEnum
            string propertyHeader = String.Join(",", typeof(T).GetProperties().Select(x => x.Name));
            HeaderStatus status = HeaderManager.DetectHeaderStatus(filePath, propertyHeader);

            // call factory 拿到 AWritePattern
            AWritePattern pattern = WritePatternFactory.WritePattern(status, filePath);

            pattern.WriteDataLines(filePath, new List<T>() { record });
        }

        public static IEnumerable<T> Read<T>(string filePath) where T : new()
        {
            FileUtils.EnsureCsvExtention(filePath);
            string headerLine = HeaderManager.BuildHeaderLine<T>();
            var headerMap = HeaderManager.BuildHeaderIndexMap(headerLine);

            var reader = new StreamReader(filePath, Encoding.UTF8);
            List<T> records = new List<T>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;

                string[] values = line.Split(',');
                T record = new T();

                foreach (var kvp in headerMap)
                {
                    if (kvp.Value >= values.Length) continue;

                    PropertyInfo prop = typeof(T).GetProperty(kvp.Key);
                    if (prop == null) continue;

                    object converted = Convert.ChangeType(values[kvp.Value], prop.PropertyType);
                    prop.SetValue(record, converted);

                }
                records.Add(record);
            }
            return records;
        }
    }

}
