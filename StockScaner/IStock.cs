using StockScaner.CustomerAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner
{
   public interface IStock
    {
       Info info { get; set; }

       List<Data> WeeklyDatas { get; set; }
       List<Data> Scan(SMALineType shortPeriod, SMALineType longPeriod, SMALineType bottomPeriod);        
    }
}
