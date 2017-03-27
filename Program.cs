using System;
using System.Collections.Generic;


namespace parser_suppliers
{
    internal class Program
    {
        private static string Db = "tenders_test";
        private static string sfx = "";
        private static DateTime localDate = DateTime.Now;

        public static void Main(string[] args)
        {

            string _fileLog = String.Format("./log_suppliers/suppliers_{0}.log", localDate.ToString("dd_MM_yyyy"));
            int period = 0;
            int downCount = 10;
            string archive = "";
            while (downCount >=0)
            {
                --downCount;

            }

        }
    }
}