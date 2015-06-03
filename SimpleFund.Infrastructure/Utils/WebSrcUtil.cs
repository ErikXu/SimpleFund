using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;
using SimpleFund.Domain.Utils;

namespace SimpleFund.Infrastructure.Utils
{
    public class WebSrcUtil : IWebSrcUtil
    {
        private static readonly Regex FiltrateNamespaceRegex = new Regex("xmlns[^\"]*\"[^\"]*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string GetToString(string url, Encoding specifiedEncoding = null)
        {
            specifiedEncoding = specifiedEncoding ?? Encoding.UTF8;

            var webClient = new WebClient();

            var responseBytes = webClient.DownloadData(url);

            var response = specifiedEncoding.GetString(responseBytes);

            return response;
        }
         
        public XmlDocument GetToXml(string url, Encoding specifiedEncoding = null)
        {
            var sbXml = new StringBuilder();

            using (var stringWriter = new StringWriter(sbXml))
            {
                var xmlTextWriter = new XmlTextWriter(stringWriter);

                var htmlWeb = new HtmlWeb();

                if (specifiedEncoding != null)
                {
                    htmlWeb.OverrideEncoding = specifiedEncoding;
                }

                htmlWeb.UseCookies = true;

                try
                {
                    htmlWeb.LoadHtmlAsXml(@url, xmlTextWriter);
                }
                catch 
                {
                    htmlWeb.LoadHtmlAsXml(@url, xmlTextWriter);
                }
                
                var webSrc = FiltrateNamespaceRegex.Replace(sbXml.ToString(), string.Empty);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(webSrc);

                return xmlDoc;
            }
        }

        public XmlDocument PostToXml(string url, NameValueCollection fv = null, Encoding specifiedEncoding = null)
        {
            var sbXml = new StringBuilder();

            using (var stringWriter = new StringWriter(sbXml))
            {
                var xmlTextWriter = new XmlTextWriter(stringWriter);

                var htmlWeb = new HtmlWeb();

                if (specifiedEncoding != null)
                {
                    htmlWeb.OverrideEncoding = specifiedEncoding;
                }

                HtmlWeb.PreRequestHandler preRequestHandler = delegate(HttpWebRequest request)
                {
                    var str = AssemblePostPayload(fv);
                    var buffer = Encoding.ASCII.GetBytes(str.ToCharArray());
                    request.ContentLength = buffer.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    return true;
                };



                htmlWeb.PreRequest = (HtmlWeb.PreRequestHandler)Delegate.Combine(htmlWeb.PreRequest, preRequestHandler);

                try
                {
                    var document = htmlWeb.Load(url, "POST");
                    document.Save(xmlTextWriter);
                }
                catch
                {
                    var document = htmlWeb.Load(url, "POST");
                    document.Save(xmlTextWriter);
                }

                htmlWeb.PreRequest = (HtmlWeb.PreRequestHandler)Delegate.Remove(htmlWeb.PreRequest, preRequestHandler);

                var webSrc = FiltrateNamespaceRegex.Replace(sbXml.ToString(), string.Empty);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(webSrc);

                return xmlDoc;
            }
        }

        public string GetViewState(XmlNode doc)
        {
            var viewStateNode = doc.SelectSingleNode("//input[@id='__VIEWSTATE']/@value");

            string viewState = null;

            if (viewStateNode != null && !string.IsNullOrEmpty(viewStateNode.Value))
            {
                viewState = viewStateNode.Value;
            }

            return viewState;
        }

        private string AssemblePostPayload(NameValueCollection fv)
        {
            var builder = new StringBuilder();
            foreach (var str in fv.AllKeys)
            {
                builder.Append("&" + str + "=" + fv.Get(str).Replace("+", "%2B"));
            }
            return builder.ToString().Substring(1);
        }
    }
}
