using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner
{
    public interface IStockReader
    {
        IStock Read();
    }
}
