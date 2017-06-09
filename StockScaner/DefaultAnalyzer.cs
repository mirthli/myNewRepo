using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScaner.CustomerAttributes;

namespace StockScaner
{
    public class DefaultAnalyzer
    {
        public AnalyzerMethod Method { get; set; }
        public List<AnalyzerResult> Results { get; set; }
        public DefaultAnalyzer(AnalyzerMethod method)
        {
            this.Method = method;
        }
        public void Run(IStock stock)
        {
            List<Data> datas = stock.Scan(Method.ShortPeriod, Method.LongPeriod, Method.BottomPeriod);
     
            if (datas.Count>0)
            {
                       List<AnalyzerResult> res = new List<AnalyzerResult>();
                foreach (Data item in datas)
                {
                    AnalyzerResult ar = new AnalyzerResult();
                    ar.Symbol = stock.info.Symbol;
                    int index = stock.WeeklyDatas.IndexOf(item);

                    int j = 0;
                    while ((stock.WeeklyDatas[index - 1 - j].AdjClose == 0 ? stock.WeeklyDatas[index - 1 - j].Close : stock.WeeklyDatas[index - 1 - j].AdjClose) >= (stock.WeeklyDatas[index  - j].AdjClose == 0 ? stock.WeeklyDatas[index  - j].Close : stock.WeeklyDatas[index  - j].AdjClose) &&
                        (stock.WeeklyDatas[index - 1 - j].AdjClose == 0 ? stock.WeeklyDatas[index - 1 - j].Close : stock.WeeklyDatas[index - 1 - j].AdjClose) <= (stock.WeeklyDatas[index - 2 - j].AdjClose == 0 ? stock.WeeklyDatas[index - 2 - j].Close : stock.WeeklyDatas[index - 2 - j].AdjClose))
                    {
                        j++;
                    }
                    

                    ar.StartDate = item.Date;
                    ar.StartPrice = item.AdjClose == 0 ? item.Close : item.AdjClose;


               
                    int i = 0;
                    while ((stock.WeeklyDatas[index + 1 + i].AdjClose == 0 ?stock.WeeklyDatas[index + 1 + i].Close : stock.WeeklyDatas[index + 1 + i] .AdjClose)<= (item.AdjClose==0?item.Close:item.AdjClose))
                    {
                        i++;
                    }
                    Data end = new Data();
                    if (index + 2 * i + j+1<=stock.WeeklyDatas.Count)
                    {
                        end = stock.WeeklyDatas[index + 2 * i + j + 1];
                    }

                 
                    ar.EndDate = end.Date;
                    ar.EndPrice = end.AdjClose==0?end.Close:end.AdjClose;
                    ar.Profit =Math.Round( ar.EndPrice / ar.StartPrice,4);

                    res.Add(ar);
                }
                Results = res;
            }
        }

    }

    public class AnalyzerMethod
    {
        public bool IsDuotou { get; set; }
        public SMALineType  ShortPeriod { get; set; }

        public SMALineType LongPeriod { get; set; }

        public SMALineType BottomPeriod { get; set; }
    }

    public class AnalyzerResult
    {
        public int StockId { get; set; }
        public string Symbol { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StartPrice { get; set; }
        public double EndPrice { get; set; }
        public double Profit { get; set; }
        
    }
}
