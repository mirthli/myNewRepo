using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner
{
    public class YHStock : IStock
    {
        public Info info { get; set; }

        public List<Data> WeeklyDatas { get; set; }


        public List<Data> Scan(CustomerAttributes.SMALineType shortPeriod, CustomerAttributes.SMALineType longPeriod, CustomerAttributes.SMALineType bottomPeriod)
        {
            Data[] datas = WeeklyDatas.ToArray();
            List<Data> result = new List<Data>();
            for (int i = (int)longPeriod; i < datas.Length; i++)
            {
                var currentData = datas[i];
                var lastData = datas[i - 1];

                var short0PData = datas[i - (int)shortPeriod + 1];
                var short1PData = datas[i - (int)shortPeriod + 2];
                var short2PData = datas[i - (int)shortPeriod + 3];
                var long0Pdata = datas[i - (int)longPeriod + 1];
                var long1Pdata = datas[i - (int)longPeriod + 2];
                var long2Pdata = datas[i - (int)longPeriod + 3];

                bool isGreaterThanBefore = currentData.AdjClose >= short0PData.AdjClose &&
                                         currentData.AdjClose >= short1PData.AdjClose &&
                                         currentData.AdjClose >= short2PData.AdjClose &&
                                         currentData.AdjClose >= long0Pdata.AdjClose &&
                                         currentData.AdjClose >= long1Pdata.AdjClose &&
                                         currentData.AdjClose >= long2Pdata.AdjClose;


                var shortSMA = currentData.GetSMAData(shortPeriod);
                var lastShortSMA = lastData.GetSMAData(shortPeriod);
                var longSMA = currentData.GetSMAData(longPeriod);
                var lastLongSMA = lastData.GetSMAData(longPeriod);
                var bottomSMA = currentData.GetSMAData(bottomPeriod);
                var lastBottomSMA = lastData.GetSMAData(bottomPeriod);

                bool isDuotou = shortSMA > lastShortSMA
                                 && longSMA > lastLongSMA
                                 && shortSMA > bottomSMA
                                 && longSMA > bottomSMA
                                 && lastShortSMA > lastBottomSMA
                                 && lastLongSMA > lastBottomSMA
                                 && bottomSMA > lastBottomSMA;

                bool isCloseLowerThanSMA = currentData.AdjClose < shortSMA && currentData.AdjClose < longSMA;
                bool isLastCloseLowerThanSMA = lastData.AdjClose < lastShortSMA && lastData.AdjClose < lastLongSMA;
                if (isDuotou
                    && isCloseLowerThanSMA
                    && isLastCloseLowerThanSMA
                    && currentData.isDecrease()
                    && lastData.isDecrease()
                    && currentData.AdjClose < lastData.AdjClose
                    && isGreaterThanBefore
                    )
                {
                    result.Add(datas[i]);
                }

            }
            return result;

        }
    }
}
