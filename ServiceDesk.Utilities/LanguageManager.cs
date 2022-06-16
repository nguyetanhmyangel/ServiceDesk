using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ServiceDesk.Utilities
{
    /// <summary>
    ///     Summary description for LanguageManager
    /// </summary>
    public class LanguageManager
    {
        /// <summary>
        ///     Default CultureInfo
        /// </summary>
        public static readonly CultureInfo DefaultCulture = new CultureInfo("vi-VN");

        /// <summary>
        ///     Available CultureInfo that according resources can be found
        /// </summary>
        public static readonly CultureInfo[] AvailableCultures;

        static LanguageManager()
        {
            //
            // Available Cultures
            //
            var resourcespath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_GlobalResources");
            var dirInfo = new DirectoryInfo(resourcespath);
            var availableResources = (from fi in dirInfo.GetFiles("*.*.resx", SearchOption.AllDirectories) select Path.GetFileNameWithoutExtension(fi.Name) into cultureName where cultureName.LastIndexOf(".", StringComparison.Ordinal) != cultureName.Length - 1 select cultureName.Substring(cultureName.LastIndexOf(".", StringComparison.Ordinal) + 1)).ToList();
            var result = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(culture => availableResources.Contains(culture.ToString())).ToList();
            AvailableCultures = result.ToArray();
            //
            // Current Culture
            //
            CurrentCulture = DefaultCulture;
            // If default culture is not available, take another available one to use
            if (!result.Contains(DefaultCulture) && result.Count > 0) CurrentCulture = result[0];
        }

        /// <summary>
        ///     Current selected culture
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get => Thread.CurrentThread.CurrentCulture;
            set
            {
                //NOTE:
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-A"); //correct
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr"); //correct
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-A"); //correct as we have given locale
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr"); //wrong, will not work
                Thread.CurrentThread.CurrentUICulture = value;
                Thread.CurrentThread.CurrentCulture = value;
            }
        }
    }
}