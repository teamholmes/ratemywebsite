using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OP.General.MVC.Helpers.Bootstrap
{
    public class Modal : HtmlElement
    {
        public Modal()
            : this(null)
        {
        }

        public Modal(object htmlAttributes)
            : base("div", htmlAttributes)
        {
            EnsureClass("modal hide");
        }
    }
}
