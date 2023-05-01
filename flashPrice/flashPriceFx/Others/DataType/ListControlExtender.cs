using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI.WebControls;
using HCFx.Other;

namespace HCFx.Extender.DataType
{
    public static class ListControlExtender
    {
        /// <summary>
        /// Sets <see cref="ListControl"/> data source to specified <paramref name="source"/> with strongly set DataValueField and DataTextField.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="source"></param>
        /// <param name="valueExpression"></param>
        public static void SetDataSourceLambda<TSource, TValue>(this ListControl listControl,
            IEnumerable<TSource> source, Expression<Func<TSource, TValue>> valueExpression)
        {
            if (valueExpression == null)
                throw new ArgumentNullException("valueExpression");

            listControl.DataSource = source;
            listControl.DataValueField = ParseMemberExpressionToString(valueExpression);
        }

        /// <summary>
        /// Sets <see cref="ListControl"/> data source to specified <paramref name="source"/> with strongly set DataValueField and DataTextField.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TDisplay"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="source"></param>
        /// <param name="valueExpression"></param>
        /// <param name="displayExpression"></param>
        public static void SetDataSourceLambda<TSource, TValue, TDisplay>(this ListControl listControl,
            IEnumerable<TSource> source, Expression<Func<TSource, TValue>> valueExpression,
            Expression<Func<TSource, TDisplay>> displayExpression)
        {
            if (valueExpression == null) throw new ArgumentNullException("valueExpression");
            if (displayExpression == null) throw new ArgumentNullException("displayExpression");

            listControl.DataSource = source;
            listControl.DataValueField = ParseMemberExpressionToString(valueExpression);
            listControl.DataTextField = ParseMemberExpressionToString(displayExpression);
        }

        /// <summary>
        /// Binds <see cref="ListControl"/> data source to specified <paramref name="source"/> with strongly set DataValueField.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="source"></param>
        /// <param name="valueExpression"></param>
        public static void BindDataSourceLambda<TSource, TValue>(this ListControl listControl,
            IEnumerable<TSource> source, Expression<Func<TSource, TValue>> valueExpression)
        {
            SetDataSourceLambda(listControl, source, valueExpression);
            listControl.DataBind();
        }

        /// <summary>
        /// Binds <see cref="ListControl"/> data source to specified <paramref name="source"/> with optional strongly set DataValueField and DataTextField.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TDisplay"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="source"></param>
        /// <param name="valueExpression"></param>
        /// <param name="displayExpression"></param>
        public static void BindDataSourceLambda<TSource, TValue, TDisplay>(this ListControl listControl,
            IEnumerable<TSource> source, Expression<Func<TSource, TValue>> valueExpression,
            Expression<Func<TSource, TDisplay>> displayExpression)
        {
            SetDataSourceLambda(listControl, source, valueExpression, displayExpression);
            listControl.DataBind();
        }

        /// <summary>
        /// Strongly set the <see cref="ListControl"/> DataValueField with specified expression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="memberExpression">The member expression to be used as DataValueField</param>
        public static void SetDataValueField<TModel>(this ListControl listControl,
            Expression<Func<TModel, object>> memberExpression)
        {
            listControl.DataValueField = ParseMemberExpressionToString(memberExpression);
        }

        /// <summary>
        /// Strongly set the <see cref="ListControl"/> DataTextField with specified expression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="memberExpression">The member expression to be used as DataTextField</param>
        public static void SetDataTextField<TModel>(this ListControl listControl,
            Expression<Func<TModel, object>> memberExpression)
        {
            listControl.DataTextField = ParseMemberExpressionToString(memberExpression);
        }

        private static string ParseMemberExpressionToString<TModel, TMember>(
            Expression<Func<TModel, TMember>> memberExpression)
        {
            return new MemberExpressionEvaluator(memberExpression).GetMemberExpression();
        }


        /// <summary>
        /// Sets <see cref="ListControl"/> selected value to specified <paramref name="value"/> or set to default of <see cref="String"/> if not found
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Returns true if value exists and set, otherwise false</returns>
        public static bool TrySetListControlSelectedValue(this ListControl listControl, string value,
            string defaultValue = default(string))
        {
            if (!listControl.Items.Cast<ListItem>().Select(item => item.Value).Contains(value))
            {
                listControl.SelectedValue = defaultValue;
                return false;
            }

            listControl.SelectedValue = value;
            return true;
        }

        public static void AddItemsAsListItem<TSource>(this ListControl listControl, IEnumerable<TSource> source,
            Func<TSource, string> valueSelector, Func<TSource, string> textSelector)
        {
            foreach (var item in source)
            {
                listControl.Items.Add(new ListItem(textSelector(item), valueSelector(item)));
            }
        }
    }
}
