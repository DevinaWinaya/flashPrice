using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.Product
{
    public class BOProduct
    {
        public string productID { set; get; }
        public string productName { set; get; }
        public string productDescription { set; get; }
        public string productCategoryID { set; get; }
        public int productPrice { set; get; }
        public string productImageUrl { set; get; }
        public DateTime entryDate { set; get; }
        public DateTime lastUpdate { set; get; }

        public string miniMarketName { set; get; }
        public string miniMarketAddress { set; get; }
        public string miniMarketType { set; get; }

    }
}
