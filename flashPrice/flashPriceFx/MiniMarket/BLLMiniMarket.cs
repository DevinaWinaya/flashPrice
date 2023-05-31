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

        #region getContent
        public static BOMiniMarket getIDMiniMarketByMiniMarketName(String miniMarketName)
        {
            return DBMiniMarket.getIDMiniMarketByMiniMarketName(miniMarketName);
        }

        public static BOMiniMarket getContentByID(String miniMarketID)
        {
            return DBMiniMarket.getContentByID(miniMarketID);
        }

        #endregion


        #region get list minimarket 
        public static BOMiniMarketList getListAllMiniMarket()
        {
            return DBMiniMarket.getListAllMiniMarket();
        }

        #region get list product for auto complete
        public static BOMiniMarketList getListMiniMarketForAutoComplete()
        {
            return DBMiniMarket.getListMiniMarketForAutoComplete();
        }

        #endregion
        #endregion
    }
}
