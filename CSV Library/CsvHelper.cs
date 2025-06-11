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
    internal class CsvHelper
    {
        static bool hasHeader = false;
        static string datas = null;
        public static void AppendCsv<T>(string filePath, IEnumerable<T> records)
        {
            var headerLine = HeaderManager.BuildHeaderLine<T>();

            Dictionary<string, int> headerMap;
            bool needHeader;

            if (!File.Exists(headerLine) || new FileInfo(filePath).Length == 0)
            {
                // 檔案不存在或是空檔 → 要寫 Header
                needHeader = true;
                headerMap = HeaderManager.BuildHeaderIndexMap(headerLine);
            }
            else
            {
                // 讀第一行就能拿到舊 Header
                var streamReader = new StreamReader(filePath);
                string existedHeader = streamReader.ReadLine() ?? string.Empty;
                needHeader = string.IsNullOrWhiteSpace(existedHeader);
                headerMap = HeaderManager.BuildHeaderIndexMap(existedHeader);
            }
            var writer = new StreamWriter(filePath, append: true, Encoding.UTF8);
            if (needHeader)
                writer.WriteLine(headerLine);

            foreach (var record in records)
            {
                writer.WriteLine(HeaderManager.ConvertRecordToCsvLine(record, headerMap));
            }
        }

        public static List<T> StreamReader<T>(string filePath) where T : new()
        {
            var result = new List<T>();

            var streamReader = new StreamReader(filePath);
            string headerLine = streamReader.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(headerLine)) return result;

            var headerMap = HeaderManager.BuildHeaderIndexMap(headerLine);
            var props = typeof(T).GetProperties();

            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var fields = line.Split(',');
                var obj = new T();

                foreach (var prop in props)
                {
                    if (headerMap.TryGetValue(prop.Name, out int index) && index < fields.Length)
                        prop.SetValue(obj, fields[index]);
                }
                result.Add(obj);
            }
            return result;
        }
        //public static List<T> StreamReader<T>(string filePath) where T : new()
        //{
        //    //HW:
        //    //1.檢查資料夾路徑是否存在，如果寫入的目標資料夾路徑不存在，throw new FileNotFoundException()
        //    //2. 檢查寫入的資料副檔名格式是否為csv 如果不是則需要 throw new Exception()
        //    if (Path.GetExtension(filePath).ToLower() != ".csv")
        //    {
        //        throw new Exception("檔案必須要是 CSV 格式");
        //    }
        //    // DAO =　給資料庫寫入的資料
        //    // DTO =  實際上讀取的欄位

        //    List<T> records = new List<T>();
        //    using (StreamReader streamReader = new StreamReader(filePath))
        //    {
        //        var properties = typeof(T).GetProperties();
        //        string header = "";
        //        foreach (var prop in properties)
        //        {
        //            header += prop.Name + ",";
        //        }
        //        header = header.TrimEnd(',');

        //        while (!streamReader.EndOfStream)
        //        {
        //            string line = streamReader.ReadLine();
        //            // 略過標題列
        //            if (line == header) continue;
        //            string[] datas = line.Split(',');
        //            // 若欄位數不符，略過該行資料
        //            if (datas.Length != properties.Length)
        //            {
        //                Console.WriteLine($"資料欄位數與類型屬性數不符，略過此列: {line}");
        //                continue;
        //            }
        //            // 將每筆資料轉換成物件
        //            T record = new T();
        //            for (int i = 0; i < datas.Length; i++)
        //            {
        //                //利用propertyName 去跟HeaderManager要資料在哪一個欄位
        //                //再將欄位的index 丟到datas 反查 取回對應資料
        //                //最後將對應的資料值透過反射SetValue到指定的property上
        //                properties[i].SetValue(record, datas[i]);
        //            }
        //            records.Add(record);
        //        }
        //    }
        //    return records;
        //}

        public static void StreamWriter<T>(string filePath, List<T> records)
        {
            //HW: 1.檢查資料夾路徑是否存在，如果寫入的目標資料夾路徑不存在，則需要自動創建資料夾
            //Directory.Exsit() Directory.Create()
            //2. 檢查寫入的資料副檔名格式是否為csv 如果不是則需要 throw new Exception()
            // 下周 header 處理; 資料對位

            // 檢查資料夾路徑是否存在，如果不存在則自動建立資料夾
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // 檢查副檔名格式是否為csv
            if (Path.GetExtension(filePath).ToLower() != ".csv")
            {
                throw new Exception("只能傳入CSV格式的內容");
            }

            var properties = typeof(T).GetProperties();
            string headerLine = string.Join(",", properties.Select(p => p.Name));
            // 讀取現有檔案內容並判斷是否已有標題列
            bool fileExists = File.Exists(filePath);
            string[] existingLines = fileExists ? File.ReadAllLines(filePath) : new string[0];
            bool hasHeader = existingLines.Length > 0 && existingLines[0] == headerLine;
            // 改成 enum
            // 情境 2：無檔案或為空檔 → 寫入 Header + 新資料
            if (!fileExists || (existingLines.Length == 0))
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine(headerLine);
                    WriteDataLines(writer, records, properties);
                }
            }
            // 情境 3：有資料但無 Header → 補上 Header + 舊資料 + 新資料 → 覆寫檔案
            else if (!hasHeader)
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine(headerLine);
                    foreach (string line in existingLines)
                    {
                        writer.WriteLine(line);
                    }
                    WriteDataLines(writer, records, properties);
                }
            }
            // 情境 1：已有 Header → 直接附加資料
            else
            {
                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    WriteDataLines(writer, records, properties);
                }
            }
        }
        // 寫入多筆資料行內容
        private static void WriteDataLines<T>(StreamWriter writer, List<T> records, PropertyInfo[] properties)
        {
            foreach (T record in records)
            {
                string line = string.Join(",", properties.Select(p => (string)p.GetValue(record)));
                writer.WriteLine(line);
            }
        }
    }
}
