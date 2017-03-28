using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net;
using System.IO.Compression;


namespace parser_suppliers
{
    internal class Program
    {
        private static string Db = "tenders_test";
        private static string sfx = "";
        private static DateTime localDate = DateTime.Now;

        public static void Main(string[] args)
        {
            string _fileLog = $"./log_suppliers/suppliers_{localDate:dd_MM_yyyy}.log";
            using (StreamWriter sw = new StreamWriter(_fileLog, true, System.Text.Encoding.Default))
            {
                sw.WriteLine($"Время начала парсинга: {localDate}\n");

            }
            double period = 0;
            int downCount = 10;
            string archive = "";
            string nameArch = "suppliers-20170326.json.zip";
            string namef = "suppliers-20170326.json";
            string extractPath = "./";
            while (downCount >=-10)
            {
                try
                {
                    DateTime lastDate = localDate.AddDays(period);
//                    Console.WriteLine(lastDate.ToString());
                    string dateArch = String.Format("{0:yyyyMMd}", lastDate);
//                    Console.WriteLine(dateArch);
                    string url = $"https://clearspending.ru/download/opendata/suppliers-{dateArch}.json.zip";
                    nameArch = $"suppliers-{dateArch}.json.zip";
                    namef = $"suppliers-{dateArch}.json";
                    WebClient wc = new WebClient();
                    wc.DownloadFile(url, nameArch);
                    break;



                }
                catch (Exception ex)
                {
                    using (StreamWriter sw = new StreamWriter(_fileLog, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(ex.ToString());
                    }
//                    Console.WriteLine(ex.ToString());
                    --period;
                }
                --downCount;
            }
            FileInfo fileInf = new FileInfo(nameArch);
            if (fileInf.Exists)
            {
               ZipFile.ExtractToDirectory(nameArch, extractPath);
                using (StreamReader sr = new StreamReader(namef, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        try
                        {
                            /*Console.WriteLine(line);*/
                            Parser Sup = new Parser(line);
                            Sup.pars();
                        }
                        catch (Exception e)
                        {
                            using (StreamWriter sw = new StreamWriter(_fileLog, true, System.Text.Encoding.Default))
                            {
                                sw.WriteLine(e.ToString());
                            }
                        }
                    }

                }
                using (StreamWriter sw = new StreamWriter(_fileLog, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"Добавлено поставщиков: {Parser.log_insert}\n");
                    sw.WriteLine($"Обновлено поставщиков: {Parser.log_update}\n");
                    sw.WriteLine($"Поставщиков без inn: {Parser.inn_null}\n");
                    sw.WriteLine($"Время окончания парсинга: {DateTime.Now}\n");
                }
            }
        }
    }
}