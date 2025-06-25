using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSV_Library
{
    public class NoHeader : AWritePattern
    {
        public override void WriteDataLines<T>(string filePath, IEnumerable<T> records)
        {
            string oldData = File.ReadAllText(filePath);
            var props = typeof(T).GetProperties();
            string header = String.Join(",", props.Select(x => x.Name));


            StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);
            writer.WriteLine(header);
            writer.Write(oldData);

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
