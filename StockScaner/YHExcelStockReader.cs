using StockScaner.CustomerAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace StockScaner
{
    public  class YHExcelStockReader:IStockReader
    {
        public IStock Read()
        {

            var fileName = "";
            var file = "";
            do
            {
                Console.WriteLine("input file name");
                file = Console.ReadLine().Trim();
                var CSVFileName = string.Format("{0}\\YHcsvFiles\\{1}.xls", Directory.GetCurrentDirectory(), file);
                bool CSCExists = File.Exists(CSVFileName);
                if (CSCExists)
                {
                    fileName = CSVFileName;
                }
                else
                {
                    var xlsxFileName = string.Format("{0}\\YHcsvFiles\\{1}.xlsx", Directory.GetCurrentDirectory(), file);
                    bool xlsxExists = File.Exists(xlsxFileName);
                    if (xlsxExists)
                    {
                        fileName = xlsxFileName;
                    }
                }
            } while (string.IsNullOrEmpty(fileName));

            YHStock stock = new YHStock();
            DataTable ds = new DataTable();
            
                using (ExcelHelper helper = new ExcelHelper(fileName))
                {
                    ds = helper.ExcelToDataTable(file, true);
                }
                
                stock.info = new Info()
                {
                    Name = file,
                    Symbol = file
                };
                stock.WeeklyDatas = new List<Data>();
                double sum = 0;
                double? sum5 = null;
                double? sum10 = null;
                double? sum20 = null;
                double? sum30 = null;
                double? sum60 = null;
                double? sum120 = null;
                double? sum240 = null;
                double? sum360 = null;
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (ds.Rows[i].ItemArray[5]==null||ds.Rows[i].ItemArray[5].ToString()=="null"||ds.Rows[i].ItemArray[5].ToString()=="0")
                    {
                        continue;
                    }

                    sum += Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());

                    if (i >= 4)
                    {
                        if (i == 4)
                        {
                            sum5 = sum;
                        }
                        else
                        {
                            sum5 = sum5 - Convert.ToDouble(ds.Rows[i - 5].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 9)
                    {
                        if (i == 9)
                        {
                            sum10 = sum;
                        }
                        else
                        {
                            sum10 = sum10 - Convert.ToDouble(ds.Rows[i - 10].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 19)
                    {
                        if (i == 19)
                        {
                            sum20 = sum;
                        }
                        else
                        {
                            sum20 = sum20 - Convert.ToDouble(ds.Rows[i - 20].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 29)
                    {
                        if (i == 29)
                        {
                            sum30 = sum;
                        }
                        else
                        {
                            sum30 = sum30 - Convert.ToDouble(ds.Rows[i - 30].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 59)
                    {
                        if (i == 59)
                        {
                            sum60 = sum;
                        }
                        else
                        {
                            sum60 = sum60 - Convert.ToDouble(ds.Rows[i - 60].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 119)
                    {
                        if (i == 119)
                        {
                            sum120 = sum;
                        }
                        else
                        {
                            sum120 = sum120 - Convert.ToDouble(ds.Rows[i - 120].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 239)
                    {
                        if (i == 239)
                        {
                            sum240 = sum;
                        }
                        else
                        {
                            sum240 = sum240 - Convert.ToDouble(ds.Rows[i - 240].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    if (i >= 359)
                    {
                        if (i == 359)
                        {
                            sum360 = sum;
                        }
                        else
                        {
                            sum360 = sum360 - Convert.ToDouble(ds.Rows[i - 360].ItemArray[5].ToString()) + Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString());
                        }
                    }
                    stock.WeeklyDatas.Add(new Data()
                    {
                        Date = DateTime.Parse(ds.Rows[i].ItemArray[0].ToString()),
                        Open = Convert.ToDouble(ds.Rows[i].ItemArray[1].ToString()),
                        High = Convert.ToDouble(ds.Rows[i].ItemArray[2].ToString()),
                        Low = Convert.ToDouble(ds.Rows[i].ItemArray[3].ToString()),
                        Close = Convert.ToDouble(ds.Rows[i].ItemArray[4].ToString()),
                        AdjClose = Convert.ToDouble(ds.Rows[i].ItemArray[5].ToString()),
                        Volume = Convert.ToDouble(ds.Rows[i].ItemArray[6].ToString()),

                        MA5 = sum5 / 5,
                        MA10 = sum10 / 10,
                        MA20 = sum20 / 20,
                        MA30 = sum30 / 30,
                        MA60 = sum60 / 60,
                        MA120 = sum120 / 120,
                        MA240 = sum240 / 240,
                        MA360 = sum360 / 360
                    });
                }    
            return stock;
        }  
    }
}
