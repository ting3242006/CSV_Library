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
            // StreamWriter
            string filePath = "D:\\Coding\\家教123\\data.csv";  //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "records.csv");
            ICsvService service = WritePatternFactory.Create();
            //RecordModel recordModel = new RecordModel();
            var recordList = new List<RecordModel>
            {
                new RecordModel
                {
                    Date = "2025/06/18",
                    Price = "300",
                    Type = "餐飲",
                    Purpose = "午餐",
                    Target = "家裡",
                    Note = "",
                    Picture = ""
                }
            };
            service.AppendCsv(filePath, recordList);



            //string fileName = $"receipt_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.jpg";
            ////string targetPath = Path.Combine(appImageFolder, fileName);

            ////// 複製圖片到 App 專用資料夾
            ////if (File.Exists(selectedImagePath1))
            ////{
            ////    File.Copy(selectedImagePath1, targetPath);
            ////}

            //// 存相對路徑到 csv
            //RecordModel recordModel = new RecordModel();
            //List<RecordModel> recordList = new List<RecordModel>();
            //recordList.Add(recordModel);
            //List<RecordModel> values = CsvHelper.StreamWriter<RecordModel>(filePath, recordList);
            //Console.WriteLine(values);
            //Console.ReadLine();

            // StreamReader
            //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "records.csv");





            //var reader = new CsvStreamReader();

            //var lists = new List<RecordModel>(reader.ReadCsv<RecordModel>(filePath));

            //foreach (var item in lists)
            //{
            //    Console.WriteLine($"{item.Date} - {item.Price} - {item.Purpose}");
            //}
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