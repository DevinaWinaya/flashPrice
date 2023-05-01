using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace HCFx.Extender.DataType
{
    public static class ListItemExtender
    {
        /// <summary>
        /// Returns enumeration of multiple selected ListItemCollection values
        /// </summary>
        /// <param name="listItemCollection">The ListItemCollection object to retrieve values from</param>
        /// <returns>Enumeration of selected values</returns>
        /// <remarks>
        /// Example usage: dropDownList.Items.GetMultiSelectedListItemsValue()
        /// </remarks>
        public static IEnumerable<string> GetMultiSelectedListItemsValue(this ListItemCollection listItemCollection)
        {
            return listItemCollection.GetMultiSelectedListItems()
                .Select(item => item.Value);
        }

        /// <summary>
        /// Returns enumeration of multiple selected ListItemCollection
        /// </summary>
        /// <param name="listItemCollection">The ListItemCollection object to retrieve values from</param>
        /// <returns>Enumeration of selected values</returns>
        /// <remarks>
        /// Example usage: dropDownList.Items.GetMultiSelectedListItems()
        /// </remarks>
        public static IEnumerable<ListItem> GetMultiSelectedListItems(this ListItemCollection listItemCollection)
        {
            return listItemCollection.Cast<ListItem>()
                .Where(item => item.Selected);
        }
    }
}
