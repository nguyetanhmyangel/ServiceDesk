using System;
using System.Web;

namespace ServiceDesk.Utilities
{
    /// <summary>
    ///     Summary description for cSessionCookie
    /// </summary>
    public class Claim 
    {
        // Cookie expiry time
        public static DateTime CookieExpiryTime = DateTime.Today.AddDays(365);

        // Get/set session values
        public static cSession Session = new cSession();

        //Get/set project cookie
        public static cCookie Cookie = new cCookie();

        // Random key

        // Get application root path (relative to domain)

        public class cSession
        {
            public object this[string name]
            {
                get => HttpContext.Current.Session[name];
                set => HttpContext.Current.Session[name] = value;
            }
        }

        public class cCookie
        {
            public string this[string name]
            {
                get => HttpContext.Current.Request.Cookies[Config.ProjectName] != null ? HttpContext.Current.Request.Cookies[Config.ProjectName][name] : "";
                set
                {
                    HttpCookie c;
                    if (HttpContext.Current.Request.Cookies[Config.ProjectName] != null)
                    {
                        c = HttpContext.Current.Request.Cookies[Config.ProjectName];
                        c.Values[name] = value;
                    }
                    else
                    {
                        c = new HttpCookie(Config.ProjectName);
                    }

                    c.Values[name] = value;
                    c.Path = Helper.AppPath();
                    c.Expires = CookieExpiryTime;
                    HttpContext.Current.Response.Cookies.Add(c);
                }
            }
        }
    }
}