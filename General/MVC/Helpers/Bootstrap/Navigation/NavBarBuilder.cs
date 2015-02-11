using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OP.General.MVC.Helpers.Bootstrap
{
    public class NavBarBuilder<TModel> : BuilderBase<TModel, NavBar>
    {
        internal NavBarBuilder(HtmlHelper<TModel> htmlHelper, NavBar subNavBar)
            : base(htmlHelper, subNavBar)
        {
            base.textWriter.Write(@"<div class=""navbar-inner""><ul class=""nav"">");
        }

        public void Item(string text, string href, string cssClass = "", string roles = "")
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            StringBuilder sb = new StringBuilder();
            if (currentAction.ToString().ToUpper() == "INDEX")
            {
                sb.AppendFormat(@"/{0}", currentController.ToString());
            }
            else
            {

                sb.AppendFormat(@"/{0}/{1}", currentController.ToString(), currentAction.ToString());
            }
            if (href.ToUpper() == sb.ToString().ToUpper())
            {
                cssClass = cssClass + " active";
            }


            base.textWriter.Write(base.element.InternalItemTemplate
                .Replace("#{text}", text)
                .Replace("#{href}", href)
                .Replace("#{css}", cssClass));
        }

        public void DropDownItem(string text, IEnumerable<BootstrapListItem> items)
        {
            var builder = new TagBuilder("li");
            builder.AddCssClass("dropdown");

            var sb = new StringBuilder();
            sb.Append(@"<a class=""dropdown-toggle"" href=""#"" data-toggle=""dropdown"">");
            sb.Append(text);
            sb.Append(@"<b class=""caret""></b></a><ul class=""dropdown-menu"">");

            foreach (var item in items)
            {
                sb.AppendFormat(@"<li><a href=""{0}"">{1}</a></li>", item.Url, item.Text);
            }

            sb.Append("</ul>");

            builder.InnerHtml = sb.ToString();

            base.textWriter.Write(builder.ToString());
        }

        public override void Dispose()
        {
            base.textWriter.Write("</ul></div>");
            base.Dispose();
        }
    }
}
