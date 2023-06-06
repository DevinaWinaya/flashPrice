using flashPriceFx.MiniMarket;
using flashPriceFX;
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

        #region get list mini market queries
        public static BOMiniMarketList getListMinimarketQueries(String searchText, String sortBy, String sortDir)
        {
            return DBMiniMarket.getListMinimarketQueries(searchText, sortBy, sortDir);
        }
        #endregion
        #endregion

        #region manage minimarket 
        public static BOProcessResult manageMinimarket(BOMiniMarket xMinimarket, String flag)
        {
            return DBMiniMarket.manageMinimarket(xMinimarket, flag);
        }

        #endregion
    }
}
