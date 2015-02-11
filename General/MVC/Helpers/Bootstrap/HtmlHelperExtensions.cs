using System.Web.Mvc;

namespace OP.General.MVC.Helpers.Bootstrap
{
    public static class HtmlHelperExtensions
    {
        public static Bootstrap<TModel> Bootstrap<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Bootstrap<TModel>(htmlHelper);
        }
    }
}
