using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSV_Library
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //CSVHelper.Write(filepath,data);


            // StreamWriter
            string filePath = "D:\\Coding\\家教123\\data.csv";  //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "records.csv");

            //var recordList = new List<RecordModel>
            //{
            //    new RecordModel
            //    {
            //        Date = "2025/06/18",
            //        Price = "100",
            //        Type = "餐飲",
            //        Purpose = "午餐",
            //        Target = "家裡",
            //        Note = "",
            //        Picture = ""
            //    }
            //};



            CSVHelper.Write(filePath, new RecordModel
            {
                Date = "2025/06/18",
                Price = "150",
                Type = "餐飲",
                Purpose = "午餐",
                Target = "家裡",
                Note = "",
                Picture = ""
            });
            //writer.AppendCsv(filePath, recordList);
            var list = CSVHelper.Read<RecordModel>(filePath);
            foreach (var item in list)
            {
                Console.WriteLine(item.Purpose + item.Price);
            }

            Console.ReadLine();
        }
    }
}

internal class RecordModel
{
    // Date,Price,Type,Proose,Target,Note,Picute
    // 2025/05/06,100,食物,晚餐,家裡,"",""
    public String Date { get; set; } = "2025/05/06";
    public String Price { get; set; } = "110";
    public String Type { get; set; } = "食物";
    public String Purpose { get; set; } = "晚餐";
    public String Target { get; set; } = "家裡";
    public String Note { get; set; } = "";
    public String Picture { get; set; } = "";
}
internal class DataModel
{
    public String Price { get; set; }
    public String Date { get; set; }
    public String Purpose { get; set; }
}