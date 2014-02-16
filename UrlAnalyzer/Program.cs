using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using Latino;
using Latino.Workflows.TextMining;

namespace UrlAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            MultiSet<string> urlCount
                = new MultiSet<string>();
            MultiSet<string> domainCount
                = new MultiSet<string>();
            Dictionary<string, Set<string>> domainToUrlMapping
                = new Dictionary<string, Set<string>>();
            Dictionary<string, Dictionary<string, Set<string>>> data
                = new Dictionary<string, Dictionary<string, Set<string>>>();
            Dictionary<string, Dictionary<string, Set<string>>> domainData
                = new Dictionary<string, Dictionary<string, Set<string>>>();

            foreach (string fileName in Directory.GetFiles(@"C:\Work\DacqPipe\Data", "*.xml.gz", SearchOption.AllDirectories))
            {
                //Console.WriteLine(fileName);
                Document doc = new Document("", "");
                doc.ReadXmlCompressed(fileName);
                Console.WriteLine(doc.Name);
                string url = doc.Features.GetFeatureValue("responseUrl");
                //Console.WriteLine(url);
                string left;
                ArrayList<string> path;
                ArrayList<KeyDat<string, string>> qParsed;
                ParseUrl(url, out left, out path, out qParsed);
                string urlKey = UrlAsString(left, path, qParsed, new Set<string>());
                urlCount.Add(urlKey);
                domainCount.Add(left);
                if (!domainToUrlMapping.ContainsKey(left)) { domainToUrlMapping.Add(left, new Set<string>()); }
                domainToUrlMapping[left].Add(urlKey);
                if (!data.ContainsKey(urlKey)) { data.Add(urlKey, new Dictionary<string, Set<string>>()); }
                if (!domainData.ContainsKey(left)) { domainData.Add(left, new Dictionary<string, Set<string>>()); }
                Dictionary<string, Set<string>> urlInfo = data[urlKey];
                Dictionary<string, Set<string>> domainInfo = domainData[left];
                foreach (KeyDat<string, string> item in qParsed)
                {
                    //Console.WriteLine(item.Key + "=" + item.Dat);
                    if (!urlInfo.ContainsKey(item.Key)) { urlInfo.Add(item.Key, new Set<string>()); }
                    urlInfo[item.Key].Add(item.Dat);
                    if (!domainInfo.ContainsKey(item.Key)) { domainInfo.Add(item.Key, new Set<string>()); }
                    domainInfo[item.Key].Add(item.Dat);
                }
            }

            Set<string> paramShitList 
                = new Set<string>("utm_campaign,feedName,mod,rss_id,comment,commentid,partner".Split(','));

            StreamWriter w = new StreamWriter(@"C:\Users\Administrator\Desktop\reportDomains.txt");

            foreach (KeyValuePair<string, Dictionary<string, Set<string>>> item in domainData)
            {
                bool found = false;
                foreach (KeyValuePair<string, Set<string>> paramInfo in item.Value)
                {
                    if (paramInfo.Value.Count > 1 && !paramShitList.Contains(paramInfo.Key.ToLower()))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    bool __found = false;
                    StringBuilder s = new StringBuilder();
                    s.AppendLine("********************** Domain Info **********************");
                    s.AppendLine();
                    s.AppendLine(item.Key + " (" + domainCount.GetCount(item.Key) + ")");
                    foreach (KeyValuePair<string, Set<string>> paramInfo in item.Value)
                    {
                        if (!paramShitList.Contains(paramInfo.Key) && paramInfo.Value.Count > 1)
                        {
                            s.AppendLine("\t" + paramInfo.Key + "\t" + paramInfo.Value.Count + "\t" + paramInfo.Value);
                        }
                    }
                    s.AppendLine();
                    s.AppendLine("*** Details ***");
                    s.AppendLine();
                    foreach (string url in domainToUrlMapping[item.Key])
                    {
                        bool _found = false;
                        foreach (KeyValuePair<string, Set<string>> paramInfo in data[url])
                        {
                            if (paramInfo.Value.Count > 1 && !paramShitList.Contains(paramInfo.Key))
                            {
                                _found = true;
                                break;
                            }
                        }
                        if (_found)
                        {
                            __found = true;
                            s.AppendLine(url + " (" + urlCount.GetCount(url) + ")");
                            foreach (KeyValuePair<string, Set<string>> paramInfo in data[url])
                            {
                                if (paramInfo.Value.Count > 1)
                                {
                                    s.AppendLine("\t" + paramInfo.Key + "\t" + paramInfo.Value.Count + "\t" + paramInfo.Value);
                                }
                            }
                            s.AppendLine();
                        }
                    }
                    s.AppendLine();
                    if (__found) { w.Write(s.ToString()); }
                }
            }
            w.Close();
        }

        // *** everything after # (document fragment) is discarded
        static void ParseUrl(string url, out string left, out ArrayList<string> path, out ArrayList<KeyDat<string, string>> queryParsed)
        {
            Uri u = new Uri(url);
            left = string.Format("{0}://{1}:{2}", u.Scheme, u.Host, u.Port);
            path = new ArrayList<string>();
            for (int i = 1; i < u.Segments.Length; i++)
            {
                string seg = HttpUtility.UrlDecode(u.Segments[i].Trim('/'));
                if (seg != "") { path.Add(seg); }
            }
            NameValueCollection query = HttpUtility.ParseQueryString(u.Query);
            queryParsed = new ArrayList<KeyDat<string, string>>();
            for (int i = 0; i < query.Count; i++)
            {
                string key = query.Keys[i];
                if (key == null) { key = "null"; }
                string val = GetValuesAsStr(query, i);
                queryParsed.Add(new KeyDat<string, string>(key, val));
            }
            queryParsed.Sort();
        }

        static string UrlEncode(string txt, params char[] chars)
        {
            foreach (char ch in chars)
            {
                txt = txt.Replace(ch.ToString(), string.Format("%{0:X2}", (int)ch));
            }
            return txt;
        }

        static string GetValuesAsStr(NameValueCollection query, int idx)
        {
            string[] values = query.GetValues(idx);
            Array.Sort(values);
            string valuesStr = "";
            foreach (string value in values)
            {
                valuesStr += UrlEncode(value, '%', ',') + ",";
            }
            return valuesStr.TrimEnd(',');
        }

        static string UrlAsString(string left, IEnumerable<string> path, ArrayList<KeyDat<string, string>> query, Set<string> queryFilter)
        {
            string url = left;
            foreach (string seg in path)
            {
                url += "/" + UrlEncode(seg, '%', '/', '?');
            }
            ArrayList<KeyDat<string, string>> tmp = query;
            if (queryFilter != null)
            {
                tmp = new ArrayList<KeyDat<string, string>>();
                foreach (KeyDat<string, string> item in query)
                {
                    if (queryFilter.Contains(item.Key)) { tmp.Add(item); }
                }
            }
            if (tmp.Count > 0)
            {
                url += "?";
                foreach (KeyDat<string, string> item in tmp)
                {
                    url += UrlEncode(item.Key, '%', '&', '=') + "=" + UrlEncode(item.Dat, '&', '=') + "&";
                }
                url = url.TrimEnd('&');
            }
            return url;
        }
    }
}
