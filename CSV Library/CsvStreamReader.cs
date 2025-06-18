using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class CsvStreamReader
    {
        public IEnumerable<T> ReadCsv<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath)) yield break;

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new StreamReader(fs, Encoding.UTF8);

            string headerLine = reader.ReadLine() ?? HeaderManager.BuildHeaderLine<T>();
            var headerMap = HeaderManager.BuildHeaderIndexMap(headerLine);

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
                yield return record;
            }
        }
    }
}
