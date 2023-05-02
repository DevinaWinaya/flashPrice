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
        public static BOProductList getListProductQuery(String searchText)
        {
            return DBProduct.getListProductQuery(searchText);
        }
        #endregion
    }
}
