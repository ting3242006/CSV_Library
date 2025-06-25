using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    // 資料欄位對應，Header建立，三種狀況判斷
    internal static class HeaderManager
    {
        // dic 去反查資料對應 => 儲存完整的資料欄位，能提供給CSVHelper去做資料對位(Mapping)
        // 將資料轉換為 CSV 字串行
        public static string BuildHeaderLine<T>()
        {
            return string.Join(",", typeof(T).GetProperties().Select(p => p.Name));
        }

        // 將 Header 字串轉成 {欄位名稱, Index} 的對應表
        public static Dictionary<string, int> BuildHeaderIndexMap(string headerLine)
        {
            return headerLine.Split(',')
                             .Select((name, idx) => new { name, idx })
                             .ToDictionary(x => x.name, x => x.idx);
        }

        public static HeaderStatus DetectHeaderStatus(string filePath, string headerLine)
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return HeaderStatus.NoFile;

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var reader = new StreamReader(fs, Encoding.UTF8);
            string firstLine = reader.ReadLine();

            return firstLine != null && firstLine.Equals(headerLine, StringComparison.Ordinal)
                   ? HeaderStatus.HasHeader
                   : HeaderStatus.NoHeader;
        }

        public static string ConvertRecordToCsvLine<T>(T record, Dictionary<string, int> headerIndexMap)
        {
            string[] values = new string[headerIndexMap.Count];
            Type type = typeof(T);

            foreach (var kvp in headerIndexMap)
            {
                PropertyInfo prop = type.GetProperty(kvp.Key);
                if (prop == null) continue;

                object raw = prop.GetValue(record);
                string value = raw.ToString() ?? string.Empty;

                value = value.Replace("\r", string.Empty).Replace("\n", string.Empty);
                values[kvp.Value] = value;
            }
            return string.Join(",", values);
        }
    }
}
