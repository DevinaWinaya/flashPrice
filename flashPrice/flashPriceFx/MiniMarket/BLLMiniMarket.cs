using flashPriceFx.MiniMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.Product
{
    public class BLLMiniMarket
    {
        #region get list minimarket 
        public static BOMiniMarketList getListAllMiniMarket()
        {
            return DBMiniMarket.getListAllMiniMarket();
        }

        #endregion
    }
}
