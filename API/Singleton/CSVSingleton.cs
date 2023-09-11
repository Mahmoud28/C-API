using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Singleton
{
    public class CSVSingleton
    {
        private CSVSingleton() { }
        public int make_id { get; set; }
        public string make_name { get; set; }

        private static readonly object _lock = new object ();
        private static List<CSVSingleton> list = null;
        public static List<CSVSingleton> GetCarMakeList
        {
            get
            {
                lock (_lock)
                {
                    if (list != null && list.Count() > 0)
                    {
                        return list;
                    }
                    else
                    {
                        list = new List<CSVSingleton>();

                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };

                        using (TextReader fileReader = File.OpenText(Path.Combine(Startup._ev.WebRootPath, "files/CarMake.csv")))
                        {
                            var csv = new CsvReader(fileReader, config);
                            csv.Read();
                            list.AddRange(csv.GetRecords<CSVSingleton>().ToList());
                        }
                        return list;
                    }
                }
            }
        }

    }
}
