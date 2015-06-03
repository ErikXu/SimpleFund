using System.Collections.Specialized;
using System.Text;
using System.Xml;

namespace SimpleFund.Domain.Utils
{
    public interface IWebSrcUtil
    {
        string GetToString(string url, Encoding specifiedEncoding = null);
        XmlDocument GetToXml(string url, Encoding specifiedEncoding = null);
        XmlDocument PostToXml(string url, NameValueCollection fv = null, Encoding specifiedEncoding = null);
        string GetViewState(XmlNode doc);
    }
}
