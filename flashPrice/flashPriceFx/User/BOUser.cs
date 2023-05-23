using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flashPriceFx.User
{
    public class BOUser
    {
        public string userID { set; get; }
        public string userName{ set; get; }
        public string userPassword { set; get; }

        public DateTime entryDate { set; get; }
        public DateTime lastUpdate { set; get; }
    }
}
