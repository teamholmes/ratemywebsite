using System.Web.Mvc;
using System.Linq;
using System.Text;
using System;
using System.Web;

namespace MyApp.Web.Helpers
{
    public static class HtmlExtensionMethods
    {
        /// <summary>
        /// Returns an error alert that lists each model error, much like the standard ValidationSummary only with
        /// altered markup for the Twitter bootstrap styles.
        /// </summary>
        public static MvcHtmlString ValidationSummaryBootstrap(this HtmlHelper helper, bool closeable)
        {


            var errors = helper.ViewContext.ViewData.ModelState.SelectMany(state => state.Value.Errors.Select(error => error.ErrorMessage));

            int errorCount = errors.Count();

            if (errorCount == 0)
            {
                return new MvcHtmlString(string.Empty);
            }


            var div = new TagBuilder("div");
           // div.AddCssClass("alert");
           // div.AddCssClass("alert-error");
            div.AddCssClass("validation-summary-errors");
            

            string message;

            if (errorCount == 1)
            {
                message = errors.First();
            }
            else
            {
                message = "Please fix the errors listed below and try again.";
                div.AddCssClass("alert-block");
            }

            if (closeable)
            {
                //var button = new TagBuilder("button");
                //button.AddCssClass("close");
                //button.MergeAttribute("type", "button");
                //button.MergeAttribute("data-dismiss", "alert");
                //button.SetInnerText("x");
                //div.InnerHtml += button.ToString();
            }

            div.InnerHtml += String.Format("<strong>{0}</strong> - {1}", @Resources.GeneralResource.warning, message );

            if (errorCount > 1)
            {
                var ul = new TagBuilder("ul");

                foreach (var error in errors)
                {
                    var li = new TagBuilder("li");
                    li.AddCssClass("text-error");
                    li.SetInnerText(error);
                    ul.InnerHtml += li.ToString();
                }

                div.InnerHtml += ul.ToString();
            }


            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"row\">");
            sb.Append("<div class=\"span12\">");
            sb.Append("<div class=\"validation-summary-errors\">");
            sb.Append(div.InnerHtml.ToString());
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");


            return new MvcHtmlString(sb.ToString());
          //  return new MvcHtmlString(div.ToString());
        }

        /// <summary>
        /// Overload allowing no arguments.
        /// </summary>
        public static MvcHtmlString ValidationSummaryBootstrap(this HtmlHelper helper)
        {
            return ValidationSummaryBootstrap(helper, true);
        }


        public static IHtmlString InformationPanel(this HtmlHelper helper, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return helper.Raw("");
            }
            else
            {
                return helper.Raw(String.Format("<div class=\"validation-summary-errors\"></strong>{0}</div>", message));
            }
        }
    }
}