using System;
using System.Xml;

namespace SimpleFund.Domain.Utils
{
    public interface IXpathUtil
    {
        string LocateString(XmlNode node, string xpath);
        DateTime? LocateDate(XmlNode node, string xpath);
        double? LocateDouble(XmlNode node, string xpath);
    }
}
