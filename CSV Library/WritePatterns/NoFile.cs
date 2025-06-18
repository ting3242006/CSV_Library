using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class NoFile : AWritePattern
    {
        public override void WriteDataLines<T>(string filePath, IEnumerable<T> records)
        {
            StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8);
            var props = typeof(T).GetProperties();
            string header = String.Join(",", props.Select(x => x.Name));
            writer.WriteLine(header);
            foreach (T record in records)
            {
                string data = String.Join(",", props.Select(x => x.GetValue(record).ToString()));
                writer.WriteLine(data);
            }

            writer.Flush();
            writer.Close();
        }
    }
}
