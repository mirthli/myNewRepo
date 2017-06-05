using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockScaner.CustomerAttributes;

namespace StockScaner
{
    public class QLStock:IStock
    {
        public Info info { get; set; }
        public List<Data> WeeklyDatas { get; set; }

        //public int[] periodArray=new int[]{5,10,20,30,60,120,240,360};
     

        //public Data PredictedBuyPoint { get; set; }
        //public Data PredictedSellPoint { get; set; }
        public List<Data> Scan(SMALineType shortPeriod, SMALineType longPeriod,SMALineType bottomPeriod)
        {
            Data[] datas = WeeklyDatas.ToArray();
            List<Data> result = new List<Data>();
            for (int  i= (int)longPeriod; i < datas.Length; i++)
            {
                var currentData=datas[i];
                var lastData=datas[i-1];

                var short0PData = datas[i - (int)shortPeriod+1];
                var short1PData = datas[i - (int)shortPeriod+2];
                var short2PData = datas[i - (int)shortPeriod+3];
                var long0Pdata = datas[i - (int)longPeriod+1];
                var long1Pdata = datas[i - (int)longPeriod + 2];
                var long2Pdata = datas[i - (int)longPeriod + 3];

                bool isGreaterThanBefore = currentData.Close >= short0PData.Close &&
                                         currentData.Close >= short1PData.Close &&
                                         currentData.Close >= short2PData.Close &&
                                         currentData.Close >= long0Pdata.Close &&
                                         currentData.Close >= long1Pdata.Close &&
                                         currentData.Close >= long2Pdata.Close;


                var shortSMA=currentData.GetSMAData(shortPeriod);
                var lastShortSMA=lastData.GetSMAData(shortPeriod);
                var longSMA=currentData.GetSMAData(longPeriod);
                var lastLongSMA=lastData.GetSMAData(longPeriod);
                var bottomSMA = currentData.GetSMAData(bottomPeriod);
                var lastBottomSMA = lastData.GetSMAData(bottomPeriod);

                bool isDuotou = shortSMA > lastShortSMA
                                 && longSMA > lastLongSMA
                                 && shortSMA > bottomSMA
                                 && longSMA > bottomSMA
                                 && lastShortSMA > lastBottomSMA
                                 && lastLongSMA > lastBottomSMA
                                 && bottomSMA > lastBottomSMA;

                bool isCloseLowerThanSMA = currentData.Close < shortSMA && currentData.Close < longSMA;
                bool isLastCloseLowerThanSMA=lastData.Close<lastShortSMA&&lastData.Close<lastLongSMA;
                if (isDuotou 
                    && isCloseLowerThanSMA 
                    && isLastCloseLowerThanSMA 
                    && currentData.isDecrease() 
                    && lastData.isDecrease() 
                    && currentData.Close<lastData.Close
                    && isGreaterThanBefore
                    )
                {
                    result.Add(datas[i]);
                }

                //var smaAttr= typeof(WeeklyData).GetCustomAttributes(typeof(SMAAttribute), true).FirstOrDefault() as SMAAttribute;

                //bool isDuotou = datas[i].MA30 - datas[i - 1].MA30 > 0 &&
                //              datas[i].MA20 - datas[i - 1].MA20 > 0;
            }
            return result;
        }

        public List<Data> calSMA(SMALineType smaType,List<Data> datas)
        {
            int lineNum=(int)smaType;
            if (datas.Count>lineNum)
            {
                int i = 0;
                foreach (Data item in datas.Skip(lineNum - 1))
                {
                    var totalData = datas.Skip(i).Take(lineNum);
                    i++;
                    double result = totalData.Sum(d => d.Close) / lineNum;
                    item.SetSMAData(smaType, result);
                }    
            }
            return datas;
        }
       
        
    }
    public class Info
    {
        public string Name { get; set; }
        public string Symbol { get; set; }


    }
    public class Data
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        public double ValueOfVol { get; set; }
        [SMA(SMALineType.line5)]
        public double? MA5 { get; set; }
        [SMA(SMALineType.line10)]
        public double? MA10 { get; set; }
        [SMA(SMALineType.line20)]
        public double? MA20 { get; set; }
        [SMA(SMALineType.line30)]
        public double? MA30 { get; set; }
        [SMA(SMALineType.line60)]
        public double? MA60 { get; set; }
        [SMA(SMALineType.line120)]
        public double? MA120 { get; set; }
        [SMA(SMALineType.line240)]
        public double? MA240 { get; set; }
        [SMA(SMALineType.line360)]
        public double? MA360 { get; set; }

        #region check duotou

        public bool CheckIfDuotou(SMALineType shortPeriod,SMALineType longPeriod)
        {
            switch (longPeriod)
            {
                case SMALineType.line5:
                 
                case SMALineType.line10: 
                  
                case SMALineType.line20:
                   
                case SMALineType.line30: return CheckIfDuotou20_30() && CheckIfDuotou30_60();

                case SMALineType.line60: return CheckIfDuotou30_60() && CheckIfDuotou60_120();
                  
                case SMALineType.line120: return CheckIfDuotou60_120() && CheckIfDuotou120_240();
            
                case SMALineType.line240: 
              
                case SMALineType.line360:return CheckIfDuotou120_240() && CheckIfDuotou240_360();
                default: return CheckIfDuotou20_30() && CheckIfDuotou30_60();
            }
        }
        public bool CheckIfDuotou5_10()
        {
            return MA5 >= MA10;
        }
        public bool CheckIfDuotou20_30()
        {
            return MA20 >= MA30;
        }
        public bool CheckIfDuotou30_60()
        {
            return MA30 >= MA60;
        }
        public bool CheckIfDuotou60_120()
        {
            return MA60 >= MA120;
        }
        public bool CheckIfDuotou120_240()
        {
            return MA120 >= MA240;
        }
        public bool CheckIfDuotou240_360()
        {
            return MA240 >= MA360;
        }
        #endregion

        public bool isDecrease()
        {
            return Close < Open;
        }

        public double? GetSMAData(SMALineType Period)
        {
            var props = typeof(Data).GetProperties();
            double? result = null;
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(SMAAttribute), true);
                if (attrs.Count()>0)
                {
                    if ((attrs.FirstOrDefault() as SMAAttribute).type== Period)
                    {
                         result =(double?) prop.GetValue(this, null);
                        break;
                    } 
                }
            }
            return result;
        }

        public void SetSMAData( SMALineType Period, double value)
        {
            var props = typeof(Data).GetProperties();
     
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(SMAAttribute), true);
                if (attrs.Count() > 0)
                {
                    if ((attrs.FirstOrDefault() as SMAAttribute).type == Period)
                    {
                        
                        prop.SetValue(this, value);
                        break;
                    }
                }
            }        
 
        }
    }
 
   
}
