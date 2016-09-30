using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class StockAnalyzer
    {
        public int GetMsftStockPrice(IStockFeed feed)
        {
            return feed.GetStockPrice("msft");
        }
    }
}
