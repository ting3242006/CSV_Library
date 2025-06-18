using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class HasHeader : AWritePattern
    {
        public override void WriteDataLines<T>(string filePath, IEnumerable<T> records)
        {
            StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8);
            var props = typeof(T).GetProperties();
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
