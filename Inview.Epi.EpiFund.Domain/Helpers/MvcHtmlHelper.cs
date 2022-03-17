using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.Helpers
{
    public static class MvcHtmlHelper
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name,
                                            IEnumerable<SelectListItem> items)
        {
            var output = new StringBuilder();
            output.Append(@"<div class=""checkboxList"">");

            foreach (var item in items)
            {
                output.Append(@"<input type=""checkbox"" name=""");
                output.Append(name);
                output.Append("\" value=\"");
                output.Append(item.Value);
                output.Append("\"");

                if (item.Selected)
                    output.Append(@" checked=""checked""");

                output.Append(" />");
                output.Append(item.Text);
                output.Append("<br />");
            }

            output.Append("</div>");

            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString CheckBoxTable(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            // coding for static 4 cells per row for now INCOMPLETE
            var output = new StringBuilder();
            string cells = string.Empty;
            output.Append(@"<table class=""checkboxTable"">");
            int cellCount = 0;
            int itemCount = items.Count(i => !string.IsNullOrEmpty(i.Value));
            foreach (var item in items)
            {
                if (itemCount >= 4)
                {
                    if (cellCount < 5)
                    {
                        cells += @"<td><input type=""checkbox"" name=""" +
                                        name +
                                        "\" value=\"" +
                                        item.Value +
                                        "\"";
                        if (item.Selected)
                            cells += @" checked=""checked"" /></td>";
                        cellCount++;
                        itemCount--;
                    }
                    else
                    {
                        output.Append("<row>" + cells + "</row>");
                        cells = string.Empty;
                        cellCount = 0;
                        itemCount--;

                        cells += @"<td><input type=""checkbox"" name=""" +
                                        name +
                                        "\" value=\"" +
                                        item.Value +
                                        "\"";
                        if (item.Selected)
                            cells += @" checked=""checked"" /></td>";
                    }
                }
                else
                {
                    if (itemCount == 1)
                    {
                        switch (cellCount)
                        {
                            case 0:
                                //cells += 
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                        }
                    }
                    else
                    {

                    }
                }
            }
            return new MvcHtmlString(output.ToString());
        }

        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;
            if (controller == currentController && action == currentAction)
                return cssClass;
            else if (currentController == "Home" && controller == "DataPortal" && currentAction == "InvestorOpportunities" && action == "AssetList")
                return cssClass;
            else
                return String.Empty;
            

        }
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue, string defaultOption = "", bool isRequired = false, string validationMsg = "")
        {
            
            IEnumerable<TEnum> values = System.Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();
            IEnumerable<SelectListItem> items = Enumerable.Empty<SelectListItem>();
            if (name == "AssetDocumentType")
            {
                items =
                from value in values
                select new SelectListItem
                {
                    Text = EnumHelper.GetEnumDescription(System.Enum.Parse(typeof(TEnum), value.ToString()) as System.Enum),
                    Value = value.ToString(),
                };
            }
            else
            {
                items =
                     from value in values 
                     select new SelectListItem
                     {
                         Text = EnumHelper.GetEnumDescription(System.Enum.Parse(typeof(TEnum), value.ToString()) as System.Enum),
                         Value = value.ToString(),
                         Selected = (value.Equals(selectedValue))
                     };
            }
            var output = new StringBuilder();
            output.Append(@"<select ");
            if (isRequired)
            {
                output.Append("data-val='true' data-val-required='" + validationMsg + "'");
            }
            output.Append("id=" + name + @" name=" + name + @">");

            if (!string.IsNullOrEmpty(defaultOption))
            {
                output.Append("<option " + (!items.Any(i => i.Selected) ? "selected='selected'" : "") + ">" + defaultOption + "</option>");
            }

            foreach (var item in items)
            {
                output.Append("<option value='" + item.Value + "' ");
                if (item.Selected)
                {
                    output.Append("selected='selected' ");
                }
                output.Append(">" + item.Text + "</option>");
            }

            output.Append(@"</select>");

            return new MvcHtmlString(output.ToString());
        }
    }
}
