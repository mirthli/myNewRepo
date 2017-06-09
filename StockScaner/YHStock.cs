using StockScaner.CustomerAttributes;
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


        //public List<Data> Scan(SMALineType shortPeriod, SMALineType longPeriod, SMALineType bottomPeriod)
        //{
        //    Data[] datas = WeeklyDatas.ToArray();
        //    List<Data> result = new List<Data>();
        //    for (int i = (int)longPeriod; i < datas.Length; i++)
        //    {
        //        var currentData = datas[i];
        //        var lastData = datas[i - 1];
        //        var last2Data = datas[i - 2];

        //        var short0PData = datas[i - (int)shortPeriod + 1];
        //        var short1PData = datas[i - (int)shortPeriod + 2];
        //        var short2PData = datas[i - (int)shortPeriod + 3];
        //        var long0Pdata = datas[i - (int)longPeriod + 1];
        //        var long1Pdata = datas[i - (int)longPeriod + 2];
        //        var long2Pdata = datas[i - (int)longPeriod + 3];

        //        bool isGreaterThanBefore = currentData.Close >= short0PData.Close &&
        //                                 currentData.Close >= short1PData.Close &&
        //                                 currentData.Close >= short2PData.Close &&
        //                                 currentData.Close >= long0Pdata.Close &&
        //                                 currentData.Close >= long1Pdata.Close &&
        //                                 currentData.Close >= long2Pdata.Close;


        //        var shortSMA = currentData.GetSMAData(shortPeriod);
        //        var lastShortSMA = lastData.GetSMAData(shortPeriod);
        //        var last2ShortSMA = last2Data.GetSMAData(shortPeriod);
        //        var longSMA = currentData.GetSMAData(longPeriod);
        //        var lastLongSMA = lastData.GetSMAData(longPeriod);
        //        var last2LongSMA = last2Data.GetSMAData(longPeriod);
        //        var bottomSMA = currentData.GetSMAData(bottomPeriod);
        //        var lastBottomSMA = lastData.GetSMAData(bottomPeriod);
        //        var last2BottomSMA = last2Data.GetSMAData(bottomPeriod);

        //        bool isDuotou = shortSMA > lastShortSMA
        //                         && longSMA > lastLongSMA
        //                         && bottomSMA > lastBottomSMA
        //                         && shortSMA > bottomSMA
        //                         && longSMA > bottomSMA
        //                         && shortSMA > longSMA

        //                         && lastShortSMA > last2ShortSMA
        //                         && lastLongSMA > last2LongSMA
        //                         && lastBottomSMA > last2BottomSMA
        //                         && lastShortSMA > lastBottomSMA
        //                         && lastLongSMA > lastBottomSMA
        //                         && lastShortSMA > lastLongSMA;
        //        bool isCurrentCloseLowerThanLastClose = currentData.Close < lastData.Close;
        //        bool isLastCloseLowerThanLastClose = lastData.Close < last2Data.Close;
        //        bool isCurrentCloseLowerThanOpen = currentData.Close < currentData.Open;
        //        bool isLastCloseLowerThanOpen = lastData.Close < lastData.Open;

        //        bool isCloseLowerThanSMA = currentData.Close < shortSMA && currentData.Close < longSMA;
        //        bool isLastCloseLowerThanSMA = lastData.Close < lastShortSMA && lastData.Close < lastLongSMA;
        //        if (isDuotou
        //            && isCloseLowerThanSMA
        //            && isLastCloseLowerThanSMA
        //            && currentData.isDecrease()
        //            && lastData.isDecrease()
        //            && isCurrentCloseLowerThanLastClose
        //            && isLastCloseLowerThanLastClose
        //            && isGreaterThanBefore
        //            && isCurrentCloseLowerThanOpen
        //            && isLastCloseLowerThanOpen
        //            )
        //        {
        //            result.Add(datas[i]);
        //        }
        //    }
        //    return result;
        //}



        public List<Data> Scan(SMALineType shortPeriod, SMALineType longPeriod, SMALineType bottomPeriod)
        {
            Data[] datas = WeeklyDatas.ToArray();
            List<Data> result = new List<Data>();
            for (int i = (int)longPeriod; i < datas.Length; i++)
            {
                var currentData = datas[i];
                var lastData = datas[i - 1];
                var last2Data = datas[i - 2];

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
                var last2ShortSMA = last2Data.GetSMAData(shortPeriod);
                var longSMA = currentData.GetSMAData(longPeriod);
                var lastLongSMA = lastData.GetSMAData(longPeriod);
                var last2LongSMA = last2Data.GetSMAData(longPeriod);
                var bottomSMA = currentData.GetSMAData(bottomPeriod);
                var lastBottomSMA = lastData.GetSMAData(bottomPeriod);
                var last2BottomSMA = last2Data.GetSMAData(bottomPeriod);

                bool isDuotou = shortSMA > lastShortSMA
                                 && longSMA > lastLongSMA
                                 && bottomSMA > lastBottomSMA
                                 && shortSMA > bottomSMA
                                 && longSMA > bottomSMA
                                 && shortSMA > longSMA

                                 && lastShortSMA > last2ShortSMA
                                 && lastLongSMA > last2LongSMA
                                 && lastBottomSMA > last2BottomSMA
                                 && lastShortSMA > lastBottomSMA
                                 && lastLongSMA > lastBottomSMA
                                 && lastShortSMA > lastLongSMA;
                bool isCurrentCloseLowerThanLastClose = currentData.Close < lastData.Close;
                bool isLastCloseLowerThanLastClose = lastData.Close < last2Data.Close;
                bool isCurrentCloseLowerThanOpen = currentData.Close < currentData.Open;
                bool isLastCloseLowerThanOpen = lastData.Close < lastData.Open;

                bool isCloseLowerThanSMA = currentData.AdjClose < shortSMA && currentData.AdjClose < longSMA;
                bool isLastCloseLowerThanSMA = lastData.AdjClose < lastShortSMA && lastData.AdjClose < lastLongSMA;
                if (isDuotou
                    && isCloseLowerThanSMA
                    && isLastCloseLowerThanSMA
                    && currentData.isDecrease()
                    && lastData.isDecrease()
                    && isCurrentCloseLowerThanLastClose
                    && isLastCloseLowerThanLastClose
                    && isGreaterThanBefore
                    && isCurrentCloseLowerThanOpen
                    && isLastCloseLowerThanOpen
                    )
                {
                    result.Add(datas[i]);
                }
            }
            return result;
        }
    }
}
