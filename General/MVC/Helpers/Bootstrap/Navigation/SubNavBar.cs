using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.General.MVC.Helpers.Bootstrap
{
    public class SubNavBar : HtmlElement
    {
        internal string InternalItemTemplate { get; private set; }

        public SubNavBar()
            : this(null)
        {
        }

        public SubNavBar(object htmlAttributes)
            : base("div", htmlAttributes)
        {
            this.InternalItemTemplate = @"<li class=""#{css}""><a href=""#{href}"">#{text}</a></li>";
            EnsureClass("subnav");
        }

    }
}
