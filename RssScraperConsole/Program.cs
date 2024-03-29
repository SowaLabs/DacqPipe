﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using Latino;
using Latino.Web;

namespace RssScraperConsole
{
    class Program
    {
        static string[] mExcludeList = new string[] { 
            "fusion.google.com",
            "add.my.yahoo.com",
            "my.yahoo.com/add", 
            "www.bloglines.com",
            "www.newsgator.com",
            "www.netvibes.com",
            "www.google.com/ig/add",
            "my.msn.com/addtomymsn",
            "www.google.com/reader",
            "www.live.com",
            "www.addthis.com"
            };

        static string[] mRegexList = new string[] { 
            @"href=[""'](?<rssUrl>[^""']*\.rss)[""']",
            @"href=[""'](?<rssUrl>[^""']*\.xml)[""']",
            @"href=[""'](?<rssUrl>[^""']*\.rdf)[""']",
            @"href=[""'](?<rssUrl>[^""']*rss[^""']*)[""']",
            @"href=[""'](?<rssUrl>[^""']*feed[^""']*)[""']",
            @"type=""application/rss[^>]*?href=[""'](?<rssUrl>[^""']*)[""']",
            @"href=[""'](?<rssUrl>[^""']*)[""'][^>]*?type=""application/rss"
            };

        // TODO: make configurable
        static bool miTestLinks
            = true;
        static bool miConvertAtomToRss
            = true;
        static bool miFeedburnerFormat
            = true;
        static bool miRemoveNonRss
            = true;
        static bool miOutputTestResult
            = false;

        static string mInputFileName
            = Utils.GetConfigValue("InputFileName");
        static string mOutputFileName
            = Utils.GetConfigValue("OutputFileName");

        static void OutputLine(StreamWriter w, string str, params object[] args)
        {
            Console.WriteLine(str, args);
            w.WriteLine(str, args);
            w.Flush();
        }

        static void Output(StreamWriter w, string str, params object[] args)
        {
            Console.Write(str, args);
            w.Write(str, args);
        }

        static bool TestRssXml(string rssXml)
        {
            return rssXml.Contains("<item") || rssXml.Contains("<channel");
        }

        static bool TestAtomXml(string atomXml)
        {
            return atomXml.Contains("<entry") || atomXml.Contains("<feed");
        }

        static IEnumerable<string> ReadInputFile(string fileName)
        { 
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
            ArrayList<string> taggedLines = new ArrayList<string>();
            bool skipMode = false;
            foreach (string _line in lines)
            {
                string line = _line.Trim();
                if (line.StartsWith("Site:"))
                {
                    taggedLines.Add("C " + line); // copy
                    skipMode = false; 
                }
                else if (line.StartsWith("# RSS list:"))
                {
                    taggedLines.Add("P " + line.Substring(11).Trim()); // process
                    skipMode = true;
                }
                else if (skipMode && line != "")
                {
                    taggedLines.Add("S " + line); // skip
                }
                else
                {
                    taggedLines.Add("C " + line);
                }
            }
            return taggedLines;
        }

        static void Main(string[] args)
        {
            // read input file
            IEnumerable<string> taggedLines = ReadInputFile(mInputFileName);
            // read settings
            ArrayList<string> includeList = new ArrayList<string>();
            int i = 0;
            foreach (char flag in Utils.GetConfigValue("IncludeRules", "yyyyyyy"))
            {
                if (flag == 'y') { includeList.Add(mRegexList[i]); }
                i++;
            }
            ArrayList<string> excludeList = new ArrayList<string>();
            i = 0;
            foreach (char flag in Utils.GetConfigValue("ExcludeRules", "yyyyyyyyyyy"))
            {
                if (flag == 'y') { excludeList.Add(mExcludeList[i]); }
                i++;
            }
            using (StreamWriter w = new StreamWriter(mOutputFileName, /*append=*/false, Encoding.UTF8))
            {
                // for each index page, fetch all RSS links
                foreach (string _line in taggedLines)
                {
                    string line = _line.Substring(2);
                    if (_line.StartsWith("C")) { OutputLine(w, line); } // copy
                    else if (_line.StartsWith("P")) // process
                    {
                        try
                        {
                            OutputLine(w, "# P # RSS list: {0}", line);
                            Set<string> links = new Set<string>();
                            Uri baseUrl = new Uri(line);
                            string html = WebUtils.GetWebPageDetectEncoding(line);
                            foreach (string regex in includeList)
                            {
                                try
                                {
                                    Regex r = new Regex(regex, RegexOptions.IgnoreCase);
                                    Match m = r.Match(html);
                                    while (m.Success)
                                    {
                                        string message = "RSS feed NOT detected.";
                                        string url = m.Result("${rssUrl}").Trim();
                                        url = new Uri(baseUrl, url).ToString();
                                        string urlLower = url.ToLower();
                                        // test whether to include link
                                        bool ok = true;
                                        foreach (string substr in excludeList)
                                        {
                                            if (urlLower.Contains(substr)) { ok = false; break; }
                                        }
                                        if (ok && !links.Contains(urlLower))
                                        {
                                            // test RSS file
                                            if (miTestLinks)
                                            {
                                                string xml = null;
                                                try { xml = WebUtils.GetWebPageDetectEncoding(url); }
                                                catch { }
                                                bool rssXmlFound = xml != null && TestRssXml(xml);
                                                if (rssXmlFound) { message = "RSS feed detected."; }
                                                // convert Atom to RSS
                                                if (xml != null && miConvertAtomToRss && !rssXmlFound && TestAtomXml(xml))
                                                {
                                                    url = "http://www.devtacular.com/utilities/atomtorss/?url=" + HttpUtility.HtmlEncode(url);
                                                    xml = null;
                                                    try { xml = WebUtils.GetWebPageDetectEncoding(url); }
                                                    catch { }
                                                    rssXmlFound = xml != null && TestRssXml(xml);
                                                    if (rssXmlFound) { message = "RSS feed detected after converting from Atom."; }
                                                }
                                                else // try the format=xml trick
                                                {
                                                    if (miFeedburnerFormat && !rssXmlFound)
                                                    {
                                                        string newUrl = url + (url.Contains("?") ? "&" : "?") + "format=xml";
                                                        try { xml = WebUtils.GetWebPageDetectEncoding(newUrl); }
                                                        catch { }
                                                        rssXmlFound = xml != null && TestRssXml(xml);
                                                        if (rssXmlFound)
                                                        {
                                                            message = "RSS feed detected after applying the format=xml trick.";
                                                            url = newUrl;
                                                        }
                                                    }
                                                }
                                                if (miRemoveNonRss && !rssXmlFound) { Output(w, "#"); }
                                                OutputLine(w, Utils.ToOneLine(url));
                                                if (miOutputTestResult)
                                                {
                                                    OutputLine(w, "# " + message);
                                                }
                                            }
                                            else
                                            {
                                                OutputLine(w, Utils.ToOneLine(url));
                                            }
                                            links.Add(urlLower);
                                        }
                                        m = m.NextMatch();
                                    } // m.Success
                                }
                                catch (Exception e)
                                {
                                    OutputLine(w, e.Message + "\r\n" + e.StackTrace);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            OutputLine(w, e.Message + "\r\n" + e.StackTrace);
                        }
                    } 
                }
            }
        }
    }
}
