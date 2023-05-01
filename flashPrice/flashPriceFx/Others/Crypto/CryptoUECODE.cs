using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace othersFx.Crypto
{
    public class CryptoUECODE
    {   
        /// <summary>
        /// encode password dari stephen sutiono
        /// </summary>
        /// <param name="p_str">The text to encode.</param>        
        public static String EnCode_Pass(String p_str)
        {
            string strs = "";
            char[] chrA;
            char chrTmp;
            Int32 intTmp;
            if (p_str != "")
            {
                for (int i = 1; i <= p_str.Length; i++)
                {
                    chrA = StringExtender.Mid (p_str, i - 1, 1).ToCharArray();
                    chrTmp = chrA[0];
                    intTmp = System.Convert.ToInt32(chrTmp);
                    intTmp = intTmp * 2;
                    chrTmp = (char)intTmp;
                    strs += chrTmp.ToString();
                }
            }
            else
            {
                strs = p_str;
            }
            return strs;
        }

        /// <summary>
        /// decode password dari stephen sutiono
        /// </summary>
        /// <param name="p_str">The text to decode.</param>
        public static String UnCode_Pass(String p_str)
        {
            string strs = "";
            char[] chrA;
            char chrTmp;
            Int32 intTmp;

            if (p_str != "")
            {
                for (int i = 1; i <= p_str.Length; i++)
                {
                    chrA = StringExtender.Mid(p_str, i - 1, 1).ToCharArray();
                    chrTmp = chrA[0];
                    intTmp = System.Convert.ToInt32(chrTmp);
                    intTmp = intTmp / 2;
                    chrTmp = (char)intTmp;
                    strs += chrTmp.ToString();
                }
            }
            else
            {
                strs = p_str;
            }
            return strs;
        }



    }
}
