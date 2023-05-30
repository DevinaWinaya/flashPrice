using flashPriceFx.MiniMarket;
using flashPriceFx.Product;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Services;

namespace flashPrice.webService
{
    /// <summary>
    /// Summary description for wsvProduct
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class wsvMiniMarket : System.Web.Services.WebService
    {
        /// Fill Minimarket LIST
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string[] getListMiniMarketCached(string prefixText, int count)
        {
            string cacheName = "miniMarketListCache";
            BOMiniMarketList xBO = new BOMiniMarketList();

            xBO = BLLMiniMarket.getListMiniMarketForAutoComplete();

            if (xBO != null)
            {
                string[] list = new string[xBO.Count];
                int x = 0;
                foreach (BOMiniMarket data in xBO)
                {
                    list[x] = data.miniMarketName;
                    x++;
                    if (x == xBO.Count)
                    {
                        break;
                    }
                }
                try
                {
                    HttpContext.Current.Cache.Add(cacheName, list, null, Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(60), CacheItemPriority.AboveNormal, null);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                HttpContext.Current.Cache[cacheName] = null;
            }

            //return (from data in (string[])Session[sessName] where data.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select data).Take(count).ToArray();

            return (from data in (string[])HttpContext.Current.Cache[cacheName]
                    where (data.IndexOf(prefixText, StringComparison.CurrentCultureIgnoreCase) > -1)
                    select data).Take(count).ToArray();
        }


    }
}
