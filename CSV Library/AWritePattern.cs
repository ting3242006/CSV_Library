using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public abstract class AWritePattern
    {
        public abstract void WriteDataLines<T>(string filePath, IEnumerable<T> records);
    }
}
