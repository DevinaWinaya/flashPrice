using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flashPriceFX;
using othersFx;

namespace flashPriceFx.User
{
    public class BLLUser
    {
        public static BOProcessResult tryLogin(String username, String password)
        {
            BOUser xLogin = DBUser.doLogin(username);
            BOProcessResult retVal = new BOProcessResult();

            if (username.Equals("") && password.Equals(""))
            {
                retVal.isSuccess = false;
                retVal.xMessage = "Dimohon untuk mengisi username dan password";
            }

            else
            {
                if (xLogin != null)
                {
                    if (xLogin.userName.Equals(username) && xLogin.userPassword.Equals(password))
                    {
                        retVal.isSuccess = true;
                        retVal.xDataObject = xLogin;
                        retVal.xMessage = "Login Berhasil";
                    }
                    else
                    {
                        retVal.isSuccess = false;
                        retVal.xMessage = "Login Gagal";
                    }
                }
                else
                {
                    retVal.isSuccess = false;
                    retVal.xMessage = "User Tidak Ditemukan";
                }
            }

            return retVal;
        }

    }
}
