using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.MiniMarket
{
    public class BOMiniMarket
    {
        public string miniMarketID { set; get; }
        public string miniMarketName { set; get; }
        public string miniMarketType { set; get; }
        public string miniMarketAddress { set; get; }
        public decimal miniMarketLattitude { set; get; }
        public decimal miniMarketLongitude { set; get; }
        public DateTime entryDate { set; get; }
        public DateTime lastUpdate { set; get; }

        public int distanceFromMe { set; get; } // in meter
        public string fromLocation { set; get; }
        public string toLocation { set; get; }
    
    }
}
