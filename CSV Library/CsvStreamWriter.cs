using CSV_Library.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class CsvStreamWriter : AWritePattern
    {
        //protected override void WriteDataLines<T>(StreamWriter writer,
        //                                        IEnumerable<T> records,
        //                                        string headerLine)
        //{
        //    if (headerLine != null)
        //    {
        //        writer.WriteLine(headerLine);
        //    }

        //    var headerMap = HeaderManager.BuildHeaderIndexMap(headerLine ?? HeaderManager.BuildHeaderLine<T>());

        //    foreach (var record in records)
        //    {
        //        writer.WriteLine(HeaderManager.ConvertRecordToCsvLine(record, headerMap));
        //        writer.Flush();
        //        writer.Close();
        //    }
        //}

        public void AppendCsv<T>(string filePath, IEnumerable<T> records)
        {

            // 用一個 function 寫在 CsvHelper
            FileUtils.EnsureCsvExtention(filePath);
            FileUtils.EnsureDirectory(filePath);
            // get HeaderEnum
            string propertyHeader = String.Join(",", typeof(T).GetProperties().Select(x => x.Name));
            HeaderStatus status = DetectHeaderStatus(filePath, propertyHeader);

            // call factory 拿到 AWritePattern
            AWritePattern pattern = WritePatternFactory.WritePattern(status, filePath);

            pattern.WriteDataLines(filePath, records);
        }

        private HeaderStatus DetectHeaderStatus(string filePath, string headerLine)
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
        public override void WriteDataLines<T>(string filePath, IEnumerable<T> records)
        {
            throw new NotImplementedException();
        }
    }
}
