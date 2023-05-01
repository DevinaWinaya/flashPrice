using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HCFx.Crypto
{
    public class CryptoKLHC
    {
        #region "KLHC CRYPTO"
        public static string PassCrypt(string Pass, string typ)
        {
            char ch;
            int length = 1;
            string str2 = "";
            int num2 = Pass.Trim().Length;

            //Pass = Pass.ToUpper();


            if (StringExtender.Left(Pass, Pass.Length) == "")
            {
                ch = '\0';
            }
            else
            {
                ch = (char)((int)Math.Round((double)((char.Parse(StringExtender.Left(Pass, length))) + (double.Parse(typ) + ((length * 2) - 13)))));
            }

            while ((ch != '\0') && (length <= num2) && (ch.ToString() != ""))
            {
                str2 = str2 + ch.ToString();
                length++;


                if (length <= num2 && StringExtender.Mid(Pass, length, 1) == "")
                {
                    ch = '\0';
                }
                else if (length <= num2)
                {
                    ch = (char)((int)Math.Round((double)((char.Parse(StringExtender.Mid(Pass, length , 1))) + (double.Parse(typ) + ((length * 2) - 13)))));
                }
            }

            return str2;
        }

        public static string Crypt(string In)
        {
            int i = 0;
            Random randnum = new Random();
            float num = randnum.Next(0, 100);
            float num2 = randnum.Next(0, 100);
            float numx = randnum.Next(101, 200);
            float numy = randnum.Next(101, 200);
            float flt = num / numx;
            float flt2 = num2 / numy;

            double idx1 = Math.Floor((double)flt * 5 + 5);
            double idx2 = Math.Floor((double)flt2 * 5);
            int idx3 = (int)(idx1 - idx2);

            string wordrev = StringExtender.ReverseString(In);
            int len = wordrev.Length;

            string hasil = idx1.ToString();
         

            Encoding encoder = ASCIIEncoding.GetEncoding("us-ascii", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());

            while (i <= len - 1)
            {

                byte[] bAsciiString = encoder.GetBytes(wordrev.Substring(i, 1));
                int ascii = int.Parse(bAsciiString[0].ToString());
                string cleanString = (ascii + idx3).ToString();
                string right3digit = StringExtender.Right(("0" + cleanString), 3);
                hasil = hasil + right3digit;
                i++;
            }
            hasil = hasil + idx2.ToString();

            return hasil;
        }
        #endregion

        private static readonly String EncryptionKey = "Rfc2898DeriveBytesEncryptionKey";
        private static readonly Char[] padding = { '=' };
        public static String Encrypt(String clearText)
        {
            if (!String.IsNullOrWhiteSpace(clearText))
            {
                try
                {
                    Byte[] clearBytes = Encoding.Unicode.GetBytes(clearText.Trim());
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new Byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes, 0, clearBytes.Length);
                                cs.Close();
                            }
                            clearText = Convert.ToBase64String(ms.ToArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
                        }
                    }

                    return clearText;
                }
                catch
                {

                }
            }

            return "";
        }

        public static String Decrypt(String cipherText)
        {
            if (!String.IsNullOrWhiteSpace(cipherText))
            {
                cipherText = cipherText.Trim();
                string incoming = cipherText.Replace('_', '/').Replace('-', '+');
                switch (cipherText.Length % 4)
                {
                    case 2: incoming += "=="; break;
                    case 3: incoming += "="; break;
                }

                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(incoming);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new Byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }

                    return cipherText;
                }
                catch
                {

                }
            }

            return "";
        }

        public static String DecryptQueryString(String queryStringName, String parameterName)
        {
            Uri uri;
            String value = "";

            try
            {
                value = Decrypt(queryStringName);
                uri = new Uri("http://localhost/Default.aspx?" + value);
                value = HttpUtility.ParseQueryString(uri.Query).Get(parameterName) ?? "";
            }
            catch
            {

            }

            return value.Trim();
        }
        
        public static String DecryptQueryString(HttpRequest request, String queryStringName, String parameterName)
        {
            Uri uri;
            String value = "";

            try
            {
                uri = new Uri(request.Url.AbsoluteUri);
                value = HttpUtility.ParseQueryString(uri.Query).Get(queryStringName);
                value = Decrypt(value);
                uri = new Uri("http://localhost/Default.aspx?" + value);
                value = HttpUtility.ParseQueryString(uri.Query).Get(parameterName) ?? "";
            }
            catch
            {
                
            }

            return value.Trim() ?? "";
        }
    }
}
