using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace HCFx.Extender.DataType
{
    public static class BaseDataBindControlExtender
    {
        public static void ClearResetDataBoundControl(this BaseDataBoundControl baseDataBoundControl)
        {
            baseDataBoundControl.DataSource = null;
            baseDataBoundControl.DataBind();
        }

        public static void ClearResetDataBoundControl(this ListControl listControl)
        {
            ClearResetDataBoundControl((BaseDataBoundControl) listControl);
            listControl.Items.Clear();
        }
    }
}
