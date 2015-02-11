using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace OP.General.MVC.Helpers.Bootstrap
{
    public class Bootstrap<TModel>
    {
        private readonly HtmlHelper<TModel> helper;

        internal Bootstrap(HtmlHelper<TModel> helper)
        {
            this.helper = helper;
        }

        #region Modal (Dialog)

        public ModalBuilder<TModel> Begin(Modal modal)
        {
            if (modal == null)
            {
                throw new ArgumentNullException("modal");
            }

            return new ModalBuilder<TModel>(this.helper, modal);
        }

        #endregion Modal (Dialog)

        #region SubNavBar

        //TODO: The styling for subnav on the bootstrap demo site is not included in Bootstrap CSS file..
        // It comes from docs.css. See this link:
        // http://stackoverflow.com/questions/11661559/bootstrap-subnav-does-not-have-the-same-style-as-on-demo-site
        // We can add it later if needed

        public SubNavBarBuilder<TModel> Begin(SubNavBar subNav)
        {
            if (subNav == null)
            {
                throw new ArgumentNullException("subNav");
            }

            return new SubNavBarBuilder<TModel>(this.helper, subNav);
        }

        #endregion SubNavBar


        #region NavBar


        public NavBarBuilder<TModel> Begin(NavBar Nav)
        {
            if (Nav == null)
            {
                throw new ArgumentNullException("Nav");
            }

            return new NavBarBuilder<TModel>(this.helper, Nav);
        }

        #endregion NavBar

    }
}
