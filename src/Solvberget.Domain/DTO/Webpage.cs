using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Solvberget.Domain.DTO
{
    public class WebPage
    {
        WebClient client = new WebClient();

        public string Link { get; set; }
        public string Html { get; set; }

        public string GetHtml()
        {
            client.Encoding = Encoding.UTF8;
            string html = client.DownloadString(Link);
            //html = html.Replace("\n", "");
            html = html.Replace("\\", "");
            return html;
        }

        public static string StripHtmlTags(string strHtml)
        {
            return Regex.Replace(strHtml, @"<(?!\/?(a)(?=>|\s.*>))\/?.*?>", "");
        }

        public static HtmlNode GetDiv(string strHtml, string divName)
        {

            var expression = "//div[@class='"+divName+"']";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(strHtml);
            if (htmlDoc.DocumentNode != null)
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(expression);

                if (node != null)
                    return node;
            }
            return null;
        }

        public static string CleanHtml(string strHtml)
        {

            //Remove \n\t
            var cleanedHtml = strHtml.Replace("\n", "");
            cleanedHtml = cleanedHtml.Replace("\t", " ");
            
            //Create some \n
            cleanedHtml = cleanedHtml.Replace("<li>", "\n");
            cleanedHtml = cleanedHtml.Replace("&nbsp;", " ");

            //Remove/Replace specific <a> tags
            cleanedHtml = cleanedHtml.Replace("<a name=\'foaje\'></a>", "");
            cleanedHtml = cleanedHtml.Replace("<a name=\"eztoc2404_0_1\" id=\"eztoc2404_0_1\"></a>", "");
            cleanedHtml = cleanedHtml.Replace("<a href=\"#foaje\">kulturhusets foaje</a>", "kulturhusets foaje");
            
            //Remove all html tags
            cleanedHtml = StripHtmlTags(cleanedHtml);
            
            //Remove some characters
            cleanedHtml = cleanedHtml.Replace("» ", "\n");
            cleanedHtml = cleanedHtml.Replace("►", "");
            cleanedHtml = cleanedHtml.Replace(",", "");
          
            //Remove multipe whitespaces
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            cleanedHtml = regex.Replace(cleanedHtml, @" ");
          
            return cleanedHtml;
        }

        public static List<String> GetValue(HtmlNode node, string div)
        {
            var nodes = node.Descendants().Where(n => n.Name.StartsWith(div));
            var list = nodes.Select(nodex => nodex.InnerHtml).Select(CleanHtml).ToList();
            return list;
        }

    }
}
