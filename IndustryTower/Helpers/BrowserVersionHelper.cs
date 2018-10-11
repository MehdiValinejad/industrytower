using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace IndustryTower.Helpers
{


    static public class BrowserVersionHelper
    {
        private static IDictionary<string, int> _BlockedBrowseList = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"InternetExplorer",8},
            {"IE",8},
            {"Firefox",4},
            {"Chrome",10},
            {"Safari",4}
        };

        public static bool isSupported()
        {
            var Request = HttpContext.Current.Request;
            if (Request.Browser.Browser == "IE"
                || Request.Browser.Browser == "InternetExplorer"
                || Request.Browser.Browser == "Mozilla")
            {
                if (double.Parse(Request.UserAgent.Substring(Request.UserAgent.LastIndexOf("Trident/") + 8, 3)) >= 4)
                {
                    return true;
                }
            }

            return _BlockedBrowseList.Where(d => d.Key == Request.Browser.Browser)
                              .Any(v => v.Value < HttpContext.Current.Request.Browser.MajorVersion);

        }

        public static bool isBlocked()
        {
            var Request = HttpContext.Current.Request;
            return _BlockedBrowseList.Where(d => d.Key == Request.Browser.Browser)
                              .Any(v => v.Value > Request.Browser.MajorVersion);
        }
    }
    //public enum SupportStatus
    //{
    //    Unknown = 0,
    //    Supported = 1,
    //    Warning = 2,
    //    Blocked = 3
    //}

    //public class SupportedBrowsers
    //{
    //    readonly string _path = HttpContext.Current.Server.MapPath("~/SupportedBrowsers.xml"); // ConfigurationManager.AppSettings["supportedbrowsers"];
 
    //    [DefaultValue("")]
    //    public string BrowserName { get; set; }
    //    [DefaultValue("")]
    //    public string MachineType { get; set; }
    //    [DefaultValue(0)]
    //    public double Version { get; set; }
    //    [DefaultValue(false)]
    //    public bool IsValid { get; set; }
 
    //    /// <summary>
    //    /// Retrieve the message to display to the users based on the browser settings.
    //    /// </summary>
    //    public string GetMessage()
    //    {
    //        var msg = string.Empty;
    //        switch (SupportStatus)
    //        {
    //            case SupportStatus.Warning:
    //                msg =
    //                    String.Format(
    //                        "Your web browser, {0} v{1} for {2}, is not fully supported.  Please consider downloading one of the supported browsers listed for full functionality within CampusConnect.",
    //                        BrowserName,
    //                        Version,
    //                        MachineType
    //                        );
    //                break;
    //            case SupportStatus.Blocked:
    //                msg =
    //                    String.Format(
    //                        "Your web browser, {0} v{1} for {2}, is not supported at all.  Please download one of the supported browsers listed to use CampusConnect.",
    //                        BrowserName,
    //                        Version,
    //                        MachineType
    //                        );
    //                break;
    //        }
    //        return msg;
    //    }
 
    //    public SupportStatus SupportStatus { get; set; }
 
    //    /// <summary>
    //    /// Constructor to seed the class with the relevant information
    //    /// </summary>
    //    public SupportedBrowsers(HttpBrowserCapabilitiesBase browser)
    //    {
    //        if (browser == null) return;
 
    //        BrowserName = browser.Browser;
    //        if (!GetMachineType()) return;
    //        if (!GetVersion(browser)) return;
    //        IsValid = GetSupportStatus();
 
    //    }
 
    //    /// <summary>
    //    /// Generate a list of supported browsers based on the machine type.
    //    /// </summary>
    //    /// <param name="machineType">
    //    ///Windows, Mac</param>
    //    /// <returns></returns>
    //    public List<string> GetList(string machineType)
    //    {
    //        const string li = "{0} {1} <a href='{2}' target='_blank'>Get {3}</a>";  
    //        // {0}: browserName
    //        // {1}: minVersion
    //        // {2}: link
    //        var list = new List<string>();
 
    //        var supported = XDocument.Load(_path).Descendants("supported").ToList();
 
    //        foreach (var el in supported.Descendants())
    //        {
    //            var elname = el.Name;
    //            var minVersionAtt = el.Attribute("MinVersion");
    //            var maxVersionAtt = el.Attribute("MaxVersion");
    //            var machineTypeAtt = el.Attribute("MachineType");
    //            var displayNameAtt = el.Attribute("displayName");
    //            var linkAtt = el.Attribute("link");
 
    //            if (minVersionAtt == null || maxVersionAtt == null || machineTypeAtt == null || displayNameAtt == null || linkAtt == null)
    //                throw new Exception("Error in SupportedBrowsers.xml file.");
 
    //            try
    //            {
    //                var version = minVersionAtt.Value == maxVersionAtt.Value
    //                                  ? "v" + maxVersionAtt.Value
    //                                  : "v" + minVersionAtt.Value + " - v" + maxVersionAtt.Value;
    //                if (machineTypeAtt.Value.ToUpper().Contains(machineType.ToUpper()))
    //                    list.Add(string.Format(li,
    //                                           displayNameAtt.Value,
    //                                           version,
    //                                           linkAtt.Value,
    //                                           elname
    //                                           ));
    //            }
    //            catch (Exception)
    //            {
    //                throw new Exception("Error in SupportedBrowsers.xml file.");
 
    //            }
    //        }
 
    //        return list;
    //    }
 
    //    /// <summary>
    //    /// Parse the machine type out of the UserAgent string.
    //    /// </summary>
    //    public bool GetMachineType()
    //    {
    //        var platform = HttpContext.Current.Request.UserAgent;
    //        if (platform == null)
    //            return false;
    //        if (platform.ToUpper().Contains("WINDOWS"))
    //        {
    //            MachineType = "Windows";
    //            return true;
    //        }
    //        if (platform.ToUpper().Contains("MACINTOSH"))
    //        {
    //            MachineType = "Mac";
    //            return true;
    //        }
    //        return false;
    //    }
 
    //    /// <summary>
    //    /// Get the browser version.  
    //    /// For IE we have temporarily hardcoded the version based on the Trident version in the UserAgent.
    //    /// ToDo: clean this up and make it work properly in the future.
    //    /// </summary>
    //    public bool GetVersion(HttpBrowserCapabilitiesBase browser)
    //    {
    //        double version;

    //        if (BrowserName == "IE" || BrowserName == "InternetExplorer")
    //        {
    //            var platform = HttpContext.Current.Request.UserAgent;
    //            if (platform != null && platform.Contains("Trident/5.0"))
    //                Version = 9.0;
    //            else if (platform != null && platform.Contains("Trident/4.0"))
    //                Version = 8.0;
    //            else if (double.TryParse(browser.Version, out version))
    //                Version = version;
    //        } 
    //        else if (double.TryParse(browser.Version, out version))
    //            Version = version;
    //        else
    //            return false;
    //        return true;
    //    }
 
    //    /// <summary>
    //    /// Determine the level of support we have for this browser
    //    /// </summary>
    //    public bool GetSupportStatus()
    //    {
    //        var doc = XDocument.Load(_path);
 
    //        var supported = doc.Descendants("supported").Descendants(BrowserName).ToList();
    //        var warning = doc.Descendants("warning").Descendants(BrowserName).ToList();
    //        var blocked = doc.Descendants("blocked").Descendants(BrowserName).ToList();
 
    //        if (IsSupportedStatusInData(supported)) SupportStatus = SupportStatus.Supported;
    //        else if (IsSupportedStatusInData(warning)) SupportStatus = SupportStatus.Warning;
    //        else if (IsSupportedStatusInData(blocked)) SupportStatus = SupportStatus.Blocked;
    //        else
    //        {
    //            SupportStatus = SupportStatus.Blocked;
    //            return false;
    //        }
    //        return true;
    //    }
 
    //    /// <summary>
    //    /// Parses the actual XML Elements from the file and determines if it is supported.
    //    /// </summary>
    //    public bool IsSupportedStatusInData(IEnumerable<XElement> data)
    //    {
    //        foreach (var el in data)
    //        {
    //            var minVersionAtt = el.Attribute("MinVersion");
    //            var maxVersionAtt = el.Attribute("MaxVersion");
    //            var machineTypeAtt = el.Attribute("MachineType");
 
    //            if (minVersionAtt == null || maxVersionAtt == null || machineTypeAtt == null)
    //                throw new Exception("Error in SupportedBrowsers.xml file.");
 
    //            try
    //            {
    //                if (Version >= double.Parse(minVersionAtt.Value)
    //                    && Version <= double.Parse(maxVersionAtt.Value)
    //                    && machineTypeAtt.Value.ToUpper().Contains(MachineType.ToUpper()))
    //                    return true;
    //            }
    //            catch (Exception)
    //            {
    //                throw new Exception("Error in SupportedBrowsers.xml file.");
 
    //            }
    //        }
    //        return false;
    //    }
    //}


    

    //static public class BrowserVersionHelper
    //{
    //    public static bool IsSupported()
    //    {
    //        SupportedBrowsers BSupport = new SupportedBrowsers(HttpContext.Current.Request.Browser);
    //        if (BSupport.GetSupportStatus() == true)
    //        {
    //            return true;
    //        }
    //        else return false;
    //    }
    //}

}