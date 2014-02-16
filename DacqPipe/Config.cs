using Latino;

namespace Dacq
{
    public static class Config
    {
        // general
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
        public static readonly string XmlDataRootDump
            = Utils.GetConfigValue<string>("xmlDataRootDump");
        public static readonly string HtmlDataRoot
            = Utils.GetConfigValue<string>("htmlDataRoot");
        public static readonly string HtmlDataRootDump
            = Utils.GetConfigValue<string>("htmlDataRootDump");
        public static readonly string HtmlViewRoot
            = Utils.GetConfigValue<string>("htmlViewRoot");
        public static readonly string Language
            = Utils.GetConfigValue<string>("language", "English"); 
        // TODO: make configurable
        public static readonly int NumPipes
            = 8;
        public static readonly int SleepBetweenPolls
            = 15 * 60000;
    }
}
