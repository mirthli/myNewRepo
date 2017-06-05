using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner
{
     public   class QLExcelStockReader:IStockReader
    {
        public IStock Read()
        {
            var fileName = "";
            var file = "";
            do
            {
                Console.WriteLine("input file name");
                file = Console.ReadLine().Trim();
                var xlsFileName = string.Format("{0}\\QLxlsFiles\\{1}.xls", Directory.GetCurrentDirectory(), file);
                bool xlsExists = File.Exists(xlsFileName);
                if (xlsExists)
                {
                    fileName = xlsFileName;
                }
                else
                {
                    var xlsxFileName = string.Format("{0}\\QLxlsFiles\\{1}.xlsx", Directory.GetCurrentDirectory(), file);
                    bool xlsxExists = File.Exists(xlsxFileName);
                    if (xlsxExists)
                    {
                        fileName = xlsxFileName;
                    }
                }
            } while (string.IsNullOrEmpty(fileName));


            DataTable ds = new DataTable();

            using (ExcelHelper helper = new ExcelHelper(fileName))
            {
                ds = helper.ExcelToDataTable(file, true);
            }

            QLStock stock = new QLStock();
            stock.info = new Info()
            {
                Name = file,
                Symbol = file
            };
            stock.WeeklyDatas = new List<Data>();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                stock.WeeklyDatas.Add(new Data()
                {
                    Date = DateTime.Parse(ds.Rows[i].ItemArray[0].ToString()),
                    Open = Convert.ToDouble(ds.Rows[i].ItemArray[1].ToString()),
                    High = Convert.ToDouble(ds.Rows[i].ItemArray[2].ToString()),
                    Low = Convert.ToDouble(ds.Rows[i].ItemArray[3].ToString()),
                    Close = Convert.ToDouble(ds.Rows[i].ItemArray[4].ToString()),
                    Volume = Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString()),
                    ValueOfVol = Convert.ToDouble(ds.Rows[i].ItemArray[6].ToString()),
                    MA5 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[7].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[7].ToString()),
                    MA10 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[8].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[8].ToString()),
                    MA20 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[9].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[9].ToString()),
                    MA30 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[10].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[10].ToString()),
                    MA60 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[11].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[11].ToString()),
                    MA120 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[12].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[12].ToString()),
                    MA240 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[13].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[13].ToString()),
                    MA360 = string.IsNullOrWhiteSpace(ds.Rows[i].ItemArray[14].ToString()) ? 0 : Convert.ToDouble(ds.Rows[i].ItemArray[14].ToString())
                });
            }
            return stock;
        }
    }
}
