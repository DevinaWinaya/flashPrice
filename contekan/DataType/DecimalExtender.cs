using System.Text.RegularExpressions;
using System.Globalization;

namespace System
{
    public static partial class DecimalExtender
    {

        public static string convertDecimalToString(decimal inputD)
        {
            string Smntra, hasil="", input;
            input = inputD.ToString();
            String[]konversi={"","Satu ","Dua ","Tiga ","Empat ",
                      "Lima ","Enam ","Tujuh ","Delapan ",
                      "Sembilan "};
            String[] akhiran ={"","Ribu ","Juta ","Milyar ","Trilyun "};

            char[] angka   ={'0','0','0','0','0','0','0','0','0',
                      '0','0','0','0','0','0'};
            char[] temp = input.ToCharArray();

            int ratusan=0,puluhan=0,satuan=0,j=angka.Length-1,ulang;
            int panjang=temp.Length, bilangan=0;

            // pendefenisian bilangan/angka
            for(int i=0;i<temp.Length;i++)
            {
              angka[angka.Length-1-i]=temp[temp.Length-1-i]; 
            }
  
            // penentu banyaknya perulangan untuk menentukan akhiran
            if(panjang%3==0)ulang=panjang/3;
            else ulang=panjang/3+1;

            //aturan konversi desimal ke teks
            for(int i=0;i<ulang;i++)
            {
               Smntra="";
               satuan=int.Parse((angka[j]).ToString());
               puluhan=int.Parse((angka[--j]).ToString());
               ratusan=int.Parse((angka[--j]).ToString());
               j--;

               if(ratusan==1)
               {
                  Smntra="Seratus ";
                  if(satuan==0 && puluhan==0)
                     Smntra=Smntra+akhiran[i];
               }
               else if(ratusan!=0)
               {
                  Smntra=konversi[ratusan]+"Ratus ";
                  if(satuan==0 && puluhan==0)
                      Smntra=Smntra+akhiran[i];
               }

               if(puluhan==1)
               {
                    if(satuan==1)
                       Smntra=Smntra+"Sebelas "+akhiran[i];
                    else if(satuan!=0)
                       Smntra=Smntra+konversi[satuan]+"belas "
                              +akhiran[i];
                    else
                       Smntra=Smntra+" Sepuluh "+akhiran[i];
               }
               else if(puluhan!=0)
               {
                   Smntra=Smntra+konversi[puluhan]+"Puluh ";
                   if(satuan!=0)
                      Smntra=Smntra+konversi[satuan]+akhiran[i];
                   else
                      Smntra=Smntra+akhiran[i];
               }
               else if(satuan==1&& i==1)
                      Smntra=Smntra+"Seribu ";
               else if(satuan!=0)
                      Smntra=Smntra+konversi[satuan]+akhiran[i];

               hasil = Smntra + hasil;
            }
            return hasil;

        }


        //buang currency pada format uang
        public static string formatCurrency(string amount, string currencyType)
        {
            CultureInfo culture;
            if (currencyType == "IDR")
            {
                culture = new CultureInfo("id-ID");
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.NumberDecimalDigits = 0;
                culture.NumberFormat.CurrencyDecimalDigits = 0;
            }
            else
            {
                culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.NumberDecimalDigits = 2;
                culture.NumberFormat.CurrencyDecimalDigits = 2;
            }

            culture.NumberFormat.CurrencyDecimalSeparator = ",";
            culture.NumberFormat.CurrencyGroupSeparator = ".";

            culture.NumberFormat.NumberDecimalSeparator = ",";
            culture.NumberFormat.NumberGroupSeparator = ".";

            culture.NumberFormat.NegativeSign = "-";
            culture.NumberFormat.CurrencyNegativePattern = 1;

            amount = Decimal.Parse(amount).ToString("C", culture);
            return amount;
        }

        //buat dapetin brapa angka blakang koma nya. 
        public static int formatCurrencyDecimalPoint(string currencyType)
        {
            CultureInfo culture;
            if (currencyType == "IDR")
            {
                culture = new CultureInfo("id-ID");
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.NumberDecimalDigits = 0;
                culture.NumberFormat.CurrencyDecimalDigits = 0;

                return 0;
            }
            else
            {
                return 2;
            }
        }


        //buat baca angka yang sudah diformat thousand separator. 
        public static decimal unformatNumber(string currencyType, string xValue)
        {
            NumberStyles style;
            CultureInfo culture;
            decimal number;

            style = NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

            if (currencyType == "IDR")
            {
                culture = CultureInfo.CreateSpecificCulture("id-ID");
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.NumberDecimalDigits = 0;
                culture.NumberFormat.CurrencyDecimalDigits = 0;
                culture.NumberFormat.CurrencyDecimalSeparator = ",";
                culture.NumberFormat.CurrencyGroupSeparator = ".";

                culture.NumberFormat.NumberDecimalSeparator = ",";
                culture.NumberFormat.NumberGroupSeparator = ".";

                culture.NumberFormat.NegativeSign = "-";
                culture.NumberFormat.CurrencyNegativePattern = 1;
            }
            else
            {
                culture = CultureInfo.CreateSpecificCulture("en-US");
                culture.NumberFormat.CurrencySymbol = "";
                culture.NumberFormat.NumberDecimalDigits = 2;
                culture.NumberFormat.CurrencyDecimalDigits = 2;
                culture.NumberFormat.CurrencyDecimalSeparator = ",";
                culture.NumberFormat.CurrencyGroupSeparator = ".";

                culture.NumberFormat.NumberDecimalSeparator = ",";
                culture.NumberFormat.NumberGroupSeparator = ".";

                culture.NumberFormat.NegativeSign = "-";
                culture.NumberFormat.CurrencyNegativePattern = 1;
            }

            if (Decimal.TryParse(xValue, style, culture, out number))
                return number;
            else
                return 0;
        }



    }
}
