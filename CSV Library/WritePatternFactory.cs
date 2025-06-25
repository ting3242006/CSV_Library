using CSV_Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Library
{
    public static class WritePatternFactory

    {
        public static AWritePattern WritePattern(HeaderStatus status, string filePath)
        {

            AWritePattern pattern = null;
            switch (status)
            {
                case HeaderStatus.NoFile:
                    {
                        pattern = new NoFile();
                        break;
                    }
                case HeaderStatus.HasHeader:
                    {
                        pattern = new HasHeader();
                        break;
                    }
                case HeaderStatus.NoHeader:
                    {
                        pattern = new NoHeader();
                        break;
                    }
            }
            return pattern;
        }
    }
}
