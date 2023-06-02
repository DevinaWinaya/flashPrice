using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.Product
{
    public class BLLProduct
    {
        #region getContent
        public static BOProduct getContent(String productID)
        {
            return DBProduct.getContent(productID);
        }
        #endregion

        #region getList

        #region get list product 

        public static BOProductList getListProduct(String searchText, String categoryProduct, bool isViewSponsorship, String sortBy, String sortDir, int startRow, int maxRow)
        {
            return DBProduct.getListProductQuery(searchText, categoryProduct, isViewSponsorship, sortBy, sortDir, startRow, maxRow);
        }

        public static decimal getCountListProduct(String searchText, String categoryProduct, bool isViewSponsorship, String sortBy, String sortDir, int startRow, int maxRow)
        {
            return DBProduct.getCountLisProductQuery(searchText, categoryProduct, isViewSponsorship, sortBy, sortDir,  startRow, maxRow);
        }

        #endregion

        #region get list product for auto complete
        public static BOProductList getListProductForAutoComplete()
        {
            return DBProduct.getListProductForAutoComplete();
        }

        #endregion
        #endregion
    }
}
