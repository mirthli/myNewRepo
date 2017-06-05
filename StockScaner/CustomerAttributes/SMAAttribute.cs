using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScaner.CustomerAttributes
{
  public  class SMAAttribute:Attribute
    {
        public SMALineType type;
        public SMAAttribute(SMALineType SMALineType)
        {
            type = SMALineType;
        }
    }

    public enum SMALineType
    {
        line5=5,
        line10=10,
        line20=20,
        line30=30,
        line60=60,
        line120=120,
        line240=240,
        line360=360
    }
}
