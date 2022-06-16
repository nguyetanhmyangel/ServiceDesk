using ServiceDesk.Utilities;
using System.Globalization;
using System.Web.UI;

namespace ServiceDesk.WebApp.Culture
{
    public class LanguagePage : Page
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            //If you would like to have DefaultLanguage changes to effect all users,
            // or when the session expires, the DefaultLanguage will be chosen, do this:
            // (better put in somewhere more GLOBAL so it will be called once)
            //LanguageManager.DefaultCulture = ...

            //Change language setting to user-chosen one
            //if (Claim.Session[Config.LanguageId] != null && string.Equals(Claim.Session[Config.LanguageId].ToString(), Config.RusLanguageId))
            //    ApplyNewLanguage(new CultureInfo("ru-Ru"));
            //else
            //    ApplyNewLanguage(new CultureInfo(Config.LanguageId));

            if (Claim.Session[Config.LanguageId] == null)
            {
                ApplyNewLanguage(new CultureInfo("vi-VN"));
            }
            else
            {
                ApplyNewLanguage(new CultureInfo(Session[Config.LanguageId].ToString()));
            }

            //ApplyNewLanguage(Session[SessionKeyLanguage] != null
            //    ? new CultureInfo(Session[SessionKeyLanguage].ToString())
            //    : new CultureInfo("vi-VN"));
        }

        private void ApplyNewLanguage(CultureInfo culture)
        {
            LanguageManager.CurrentCulture = culture;
            //Keep current language in session
            //Session.Add(Config.LanguageId, LanguageManager.CurrentCulture.Name);
            Claim.Session[Config.LanguageId] = LanguageManager.CurrentCulture.Name;
        }

        public void ApplyNewLanguageAndRefreshPage(CultureInfo culture)
        {
            ApplyNewLanguage(culture);
            //Refresh the current page to make all control-texts take effect
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}