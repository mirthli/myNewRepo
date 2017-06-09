using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner
{
    class Program
    {
        static void Main(string[] args)
        {

            var periodArray = new int[] { 5, 10, 20, 30, 60, 120, 240, 360 };
           
            do
            {
                 string scannerName;
                do
                {
                    Console.WriteLine("tell me what scanner you want to use: QL or YH");
                   scannerName=Console.ReadLine().Trim();
                } while (scannerName.ToUpper()!="QL"&&scannerName.ToUpper()!="YH");

                IStock stock = null;
                switch (scannerName.ToUpper())
                {
                    case "QL": 
                          stock = new QLExcelStockReader().Read();
                        break;
                    case "YH":                
                        stock = new YHExcelStockReader().Read();
                        break;
                    default:
                        break;
                }
          


                Console.WriteLine("data is ready");

            

                //string sp = "";
                //int shortP = 0;
                //do
                //{
                //    Console.WriteLine("type short period");
                //    sp = Console.ReadLine().Trim();

                //} while (!int.TryParse(sp, out shortP));

                //string lp = "";
                //int longP = 0;
                //do
                //{
                //    Console.WriteLine("type long period");
                //    lp = Console.ReadLine().Trim();

                //} while (!int.TryParse(lp, out longP));

                //CustomerAttributes.SMALineType shortPeriod = ConvertToSMA(shortP);
                //CustomerAttributes.SMALineType longPeriod = ConvertToSMA(longP);

                //CustomerAttributes.SMALineType bottomPeriod = ConvertToSMA(periodArray[Array.IndexOf(periodArray, longP) + 1]);

                //DefaultAnalyzer da = new DefaultAnalyzer(new AnalyzerMethod()
                //{

                //    ShortPeriod = shortPeriod,
                //    LongPeriod = longPeriod,
                //    BottomPeriod = bottomPeriod
                //});
                //da.Run(stock);


                DefaultAnalyzer da_20_30 = new DefaultAnalyzer(new AnalyzerMethod()
                {

                    ShortPeriod = CustomerAttributes.SMALineType.line20,
                    LongPeriod = CustomerAttributes.SMALineType.line30,
                    BottomPeriod = CustomerAttributes.SMALineType.line60

                });
                da_20_30.Run(stock);

                ShowResult(stock, da_20_30);


                DefaultAnalyzer da_30_60 = new DefaultAnalyzer(new AnalyzerMethod()
                {

                    ShortPeriod = CustomerAttributes.SMALineType.line30,
                    LongPeriod = CustomerAttributes.SMALineType.line60,
                    BottomPeriod = CustomerAttributes.SMALineType.line120

                });
                da_30_60.Run(stock);

                ShowResult(stock, da_30_60);

                DefaultAnalyzer da_60_120 = new DefaultAnalyzer(new AnalyzerMethod()
                {

                    ShortPeriod = CustomerAttributes.SMALineType.line60,
                    LongPeriod = CustomerAttributes.SMALineType.line120,
                    BottomPeriod = CustomerAttributes.SMALineType.line240

                });
                da_60_120.Run(stock);

                ShowResult(stock, da_60_120);

                DefaultAnalyzer da_120_240 = new DefaultAnalyzer(new AnalyzerMethod()
                {

                    ShortPeriod = CustomerAttributes.SMALineType.line120,
                    LongPeriod = CustomerAttributes.SMALineType.line240,
                    BottomPeriod = CustomerAttributes.SMALineType.line360

                });
                da_120_240.Run(stock);

                ShowResult(stock, da_120_240);
                
            } while (true);        
          
        }


        private static CustomerAttributes.SMALineType ConvertToSMA(int lineNUM)
        {
            switch (lineNUM)
            {
                case 5: return CustomerAttributes.SMALineType.line5;
                    break;
                case 10: return CustomerAttributes.SMALineType.line10;
                    break;
                case 20: return CustomerAttributes.SMALineType.line20;
                    break;
                case 30: return CustomerAttributes.SMALineType.line30;
                    break;
                case 60: return CustomerAttributes.SMALineType.line60;
                    break;
                case 120: return CustomerAttributes.SMALineType.line120;
                    break;
                case 240: return CustomerAttributes.SMALineType.line240;
                    break;
                case 360: return CustomerAttributes.SMALineType.line360;


                default: return default(CustomerAttributes.SMALineType);
                    break;
            }
        }


        public static void ShowResult(IStock stock,DefaultAnalyzer da)
        {
            Console.WriteLine("-----------------REPORT-------------------");
            Console.WriteLine(string.Format("stock symbol:{0}", stock.info.Symbol));
            Console.WriteLine(string.Format("Method {0}-{1}",(int)da.Method.ShortPeriod,(int)da.Method.LongPeriod));
            if (da.Results != null && da.Results.Count > 0)
            {
                foreach (var item in da.Results)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine(string.Format("start date : {0},     end date : {1}", item.StartDate, item.EndDate));
                    Console.WriteLine(string.Format("start price : {0},      end price:{1}", item.StartPrice, item.EndPrice));
                    Console.WriteLine(string.Format("Profit: {0}%", item.Profit));
                    Console.WriteLine("-----------------------------------");

                }
            }
        }
    }
}
