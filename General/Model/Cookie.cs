using System;
using System.Web;

namespace OP.General.Cookies
{
    /// <summary>
    /// Static class that handles browser cookies
    /// </summary>
    public static class Cookie
    {

        #region setCookie - Method that sets a cookie
        /// <summary>
        /// Method that sets a cookies
        /// </summary>
        /// <param name="cookieName">The name of the cookie</param>
        /// <param name="propertyName">The property name to assign a value to</param>
        /// <param name="cookieValue">the value to assign to the property</param>
        /// <param name="DaysToExpire">the number of days before the cookie expires</param>
        public static void SetCookie(string cookieName, string propertyName, string cookieValue, int DaysToExpire)
        {
            HttpCookie myCookie = new HttpCookie(cookieName.ToUpper());
            myCookie[propertyName.ToUpper()] = cookieValue;
            myCookie.Expires = DateTime.Now.AddDays(DaysToExpire);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        /// <summary>
        /// Method that sets a cookies
        /// </summary>
        /// <param name="cookieName">The name of the cookie</param>
        /// <param name="propertyName">a string of propertieso</param>
        /// <param name="cookieValue">a string of property values</param>
        /// <param name="DaysToExpire">the number of days before the cookie expires</param>
        public static void SetCookie(string cookieName, string[] propertyName, string[] cookieValue, int DaysToExpire)
        {
            HttpCookie myCookie = new HttpCookie(cookieName.ToUpper());

            for (int i = 0; i < propertyName.Length; i++)
            {
                myCookie[propertyName[i].ToUpper()] = cookieValue[i];
            }

            myCookie.Expires = DateTime.Now.AddDays(DaysToExpire);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        #endregion

        #region getCookie - Method that retrieves a cookie
        /// <summary>
        /// Methdo that retrieves a cookie
        /// </summary>
        /// <param name="cookieName">the name of the cookie</param>
        /// <param name="propertyName">the portpery to retrieve</param>
        /// <returns>the string avlue representation</returns>
        public static string GetCookie(string cookieName, string propertyName)
        {
            string retValue = null;
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[cookieName.ToUpper()];
            if (myCookie != null)
            {
                retValue = myCookie[propertyName.ToUpper()];
            }

            return retValue;

        }

        #endregion

        #region clearCookie - Method that clears a cookie
        /// <summary>
        /// Methdo that clears a cookie
        /// </summary>
        /// <param name="cookieName">the name of the cookie</param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie myCookie = new HttpCookie(cookieName.ToUpper());
            myCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        #endregion
    }
}
