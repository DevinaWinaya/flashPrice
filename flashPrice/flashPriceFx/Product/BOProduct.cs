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
        public string productCategory { set; get; }
        public int productPrice { set; get; }
        public byte[] productImageContent { set; get; }
        public DateTime entryDate { set; get; }
        public DateTime lastUpdate { set; get; }

        public string miniMarketName { set; get; }
        public string miniMarketAddress { set; get; }


    }
}
