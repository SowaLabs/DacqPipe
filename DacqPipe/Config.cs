using System;
using Latino;

namespace Dacq
{
    public static class Config
    {
        public static readonly string LogFileName
            = Utils.GetConfigValue<string>("logFileName");
        public static readonly string XmlDataRoot
            = Utils.GetConfigValue<string>("xmlDataRoot", "Data");
        public static readonly string XmlDataDumpRoot
            = Utils.GetConfigValue<string>("xmlDataDumpRoot");
        public static readonly string HtmlDataRoot
            = Utils.GetConfigValue<string>("htmlDataRoot", "DataHtml");
        public static readonly string HtmlDataDumpRoot
            = Utils.GetConfigValue<string>("htmlDataDumpRoot");
        public static readonly string HtmlViewRoot
            = Utils.GetConfigValue<string>("htmlViewRoot", "Previews");
        public static readonly string DataSourcesFileName
            = Utils.GetConfigValue<string>("dataSourcesFileName", "RssSources.txt");
        public static readonly string DbConnectionString
            = Utils.GetConfigValue<string>("dbConnectionString", "Server=(local);Database=DacqPipe;Trusted_Connection=Yes");
        public static readonly int MaxDocsPerCorpus
            = Utils.GetConfigValue<int>("maxDocsPerCorpus", "50");
        public static readonly bool RandomDelayAtStart
            = Utils.GetConfigValue<bool>("randomDelayAtStart", "no");
        public static readonly string Language
            = Utils.GetConfigValue<string>("language", "English"); 
        public static readonly int NumPipes
            = Utils.GetConfigValue<int>("numPipes", "2");
        public static readonly int SleepBetweenPolls
            = (int)Utils.GetConfigValue<TimeSpan>("sleepBetweenPolls", "00:15:00").TotalMilliseconds;
        // obsolete settings
        public static readonly string OfflineSource
            = Utils.GetConfigValue<string>("offlineSource"); 
        public static readonly string WebSiteId
            = Utils.GetConfigValue<string>("webSiteId", "dacq");
        public static readonly string ClientIp
            = Utils.GetConfigValue<string>("clientIp");
    }
}
