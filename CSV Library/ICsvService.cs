using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public interface ICsvService
    {
        void AppendCsv<T>(string filePath, IEnumerable<T> records);
    }

    public class CsvService : ICsvService
    {
        private readonly CsvStreamWriter _writer = new CsvStreamWriter();

        public void AppendCsv<T>(string filePath, IEnumerable<T> records) =>
            _writer.AppendCsv(filePath, records);
    }
}
