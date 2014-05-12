using System;
using Latino;

namespace Dacq
{
    public static class Config
    {
        public static readonly string LogFileName
            = Utils.GetConfigValue<string>("logFileName");
        public static readonly string DataSourcesFileName
            = Utils.GetConfigValue<string>("dataSourcesFileName");
        public static readonly string WebSiteId
            = Utils.GetConfigValue<string>("webSiteId", "dacq");
        public static readonly string dbConnectionString
            = Utils.GetConfigValue<string>("dbConnectionString"); 
        public static readonly string ClientIp
            = Utils.GetConfigValue<string>("clientIp");
        public static readonly string XmlDataRoot
            = Utils.GetConfigValue<string>("xmlDataRoot");
        public static readonly string XmlDataDumpRoot
            = Utils.GetConfigValue<string>("xmlDataDumpRoot");
        public static readonly string HtmlDataRoot
            = Utils.GetConfigValue<string>("htmlDataRoot");
        public static readonly string HtmlDataDumpRoot
            = Utils.GetConfigValue<string>("htmlDataDumpRoot");
        public static readonly string HtmlViewRoot
            = Utils.GetConfigValue<string>("htmlViewRoot");
        public static readonly string Language
            = Utils.GetConfigValue<string>("language", "English"); 
        public static readonly string OfflineSource
            = Utils.GetConfigValue<string>("offlineSource"); 
        public static readonly int NumPipes
            = Utils.GetConfigValue<int>("numPipes", "8");
        public static readonly int SleepBetweenPolls
            = (int)Utils.GetConfigValue<TimeSpan>("sleepBetweenPolls", "00:15:00").TotalMilliseconds;
    }
}
