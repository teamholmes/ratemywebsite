
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyApp.Web.Helpers
{
    public static class ButtonExtensions
    {
        public static IHtmlString SaveButton(this HtmlHelper helper)
        {
            return helper.Raw("<input type=\"submit\" id=\"SaveButton\" name=\"NavigationButton\" value=\"Save\" class=\"cancel\"/>");
        }


        public static IHtmlString SubmitButton(this HtmlHelper helper)
        {
            return helper.Raw(String.Format("<input type=\"submit\" class=\"btn btn-primary\" id=\"SubmitButton\" value=\"{0}\" name=\"submitbutton\" />", "Submit"));

        }
        /// <summary>
        /// General Submit button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="displaytext">Sets teh text for the button - please ntoe if you are testing aginst form subitssion buttons then the nmae really does count and if you change it you will nedd ot update your controller.</param>
        /// <returns></returns>
        public static IHtmlString SubmitButton(this HtmlHelper helper, string displaytext = "")
        {
            // set the default text programatically
            if (String.IsNullOrEmpty(displaytext))
            {
                displaytext = @WebApplication.Resources.GeneralResource.submit;
            }

            string disabledtextclass = string.Empty;

         

            // return helper.Raw("<input type=\"submit\" id=\"SubmitButton\" name=\"NavigationButton\" value=\"Submit\" />");
            return helper.Raw(String.Format("<input type=\"submit\" class=\"btn btn-primary\" id=\"SubmitButton\" value=\"{0}\" name=\"submitbutton\" />", displaytext));

        }

        public static IHtmlString SubmitButton(this HtmlHelper helper, string displaytext = "", string Id = "")
        {

            if (String.IsNullOrEmpty(displaytext))
            {
                displaytext = @WebApplication.Resources.GeneralResource.submit;
            }

            if (String.IsNullOrEmpty(Id))
            {
                Id = "SubmitButton";
            }

            return helper.Raw(String.Format("<input type=\"submit\" class=\"btn btn-primary\" id=\"{1}\" value=\"{0}\" name=\"submitbutton\" />", displaytext, Id));

        }

        public static IHtmlString SubmitButton(this HtmlHelper helper, string displaytext = "", string Id = "", string @class = "")
        {

            if (String.IsNullOrEmpty(displaytext))
            {
                displaytext = @WebApplication.Resources.GeneralResource.submit;
            }

            if (String.IsNullOrEmpty(Id))
            {
                Id = "SubmitButton";
            }

            return helper.Raw(String.Format("<input type=\"submit\" class=\"btn btn-primary {2}\" id=\"{1}\" value=\"{0}\" name=\"submitbutton\" />", displaytext, Id, @class));

        }

        public static IHtmlString BackButton(this HtmlHelper helper)
        {
            return helper.Raw("<input type=\"submit\" name=\"NavigationButton\" value=\"Back\" />");
        }

        public static IHtmlString JavaScriptButton(this HtmlHelper helper, string displaytext = "", string Id = "", string jsFunction = "")
        {
            if (String.IsNullOrEmpty(displaytext))
            {
                displaytext = @WebApplication.Resources.GeneralResource.submit;
            }

            if (String.IsNullOrEmpty(Id))
            {
                Id = "SubmitButton";
            }

            if (String.IsNullOrEmpty(jsFunction))
            {
                jsFunction = Id + "_Click";
            }

            return helper.Raw(String.Format("<input type=\"button\" class=\"btn btn-primary\" id=\"{1}\" value=\"{0}\" name=\"submitbutton\" onclick=\"{2}\" />", displaytext, Id, jsFunction));
        }

         /// <summary>
        /// General Submit button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="displaytext">Sets teh text for the button - please ntoe if you are testing aginst form subitssion buttons then the nmae really does count and if you change it you will nedd ot update your controller.</param>
        /// <returns></returns>
        public static IHtmlString SubmitButton(this HtmlHelper helper, string displaytext = "", Boolean isEnabled = true)
        {
            // set the default text programatically
            if (String.IsNullOrEmpty(displaytext))
            {
                displaytext = @WebApplication.Resources.GeneralResource.submit;
            }

            string disabledtextclass = string.Empty;

            if (!isEnabled)
            {
                disabledtextclass = " disabled";
            }

            // return helper.Raw("<input type=\"submit\" id=\"SubmitButton\" name=\"NavigationButton\" value=\"Submit\" />");
            return helper.Raw(String.Format("<input type=\"submit\" class=\"btn btn-primary{1}\" id=\"SubmitButton\" value=\"{0}\" name=\"submitbutton\" />", displaytext, disabledtextclass));

        }
    }
}