using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace CSV_Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // StreamWriter
            string filePath = "D:\\Coding\\家教123\\data.csv";  //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "records.csv");
            RecordModel recordModel = new RecordModel();
            List<RecordModel> recordList = new List<RecordModel>();
            recordList.Add(recordModel);
            CsvHelper.StreamWriter<RecordModel>(filePath, recordList);



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
            List<DataModel> lists = CsvHelper.StreamReader<DataModel>(filePath);

            foreach (DataModel list in lists)
            {
                var properties = typeof(DataModel).GetProperties();
                Console.WriteLine(list.Purpose);
                //Console.WriteLine(list.Target);
                Console.WriteLine(list.Price);
                Console.WriteLine(list.Date);
                //Console.WriteLine(list.Type);
                //Console.WriteLine(list.Note);
                //Console.WriteLine(list.Picture);
                Console.WriteLine($"---------------------------------");

            }

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