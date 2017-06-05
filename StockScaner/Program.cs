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
                switch (scannerName)
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

            

                string sp = "";
                int shortP = 0;
                do
                {
                    Console.WriteLine("type short period");
                    sp = Console.ReadLine().Trim();

                } while (!int.TryParse(sp, out shortP));

                string lp = "";
                int longP = 0;
                do
                {
                    Console.WriteLine("type long period");
                    lp = Console.ReadLine().Trim();

                } while (!int.TryParse(lp, out longP));

                CustomerAttributes.SMALineType shortPeriod = ConvertToSMA(shortP);
                CustomerAttributes.SMALineType longPeriod = ConvertToSMA(longP);

                CustomerAttributes.SMALineType bottomPeriod = ConvertToSMA(periodArray[Array.IndexOf(periodArray, longP) + 1]);
                var res = stock.Scan(shortPeriod, longPeriod,bottomPeriod);
                Console.WriteLine("-----------------REPORT-------------------");
                Console.WriteLine(string.Format("stock symbol:{0}", stock.info.Symbol));
                //Console.WriteLine(string.Format("scan type:{0}__{1}"), ((int)CustomerAttributes.SMALineType.line20).ToString(), ((int)CustomerAttributes.SMALineType.line30).ToString());

                foreach (var data in res)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine(string.Format("date:{0}", data.Date));
                    Console.WriteLine("-----------------------------------");
                }
                
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
    }
}
