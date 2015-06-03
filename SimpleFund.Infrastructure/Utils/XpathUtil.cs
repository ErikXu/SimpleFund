using System;
using System.Xml;
using SimpleFund.Domain.Utils;

namespace SimpleFund.Infrastructure.Utils
{
    public class XpathUtil : IXpathUtil
    {
        public string LocateString(XmlNode node, string xpath)
        {
            var nodeValue = node.SelectSingleNode(xpath);
            return nodeValue != null ? nodeValue.Value : null;
        }

        public DateTime? LocateDate(XmlNode node, string xpath)
        {
            var nodeValue = node.SelectSingleNode(xpath);
            DateTime value;

            if (nodeValue != null && DateTime.TryParse(nodeValue.Value, out value))
            {
                return value;
            }

            return null;
        }

        public double? LocateDouble(XmlNode node, string xpath)
        {
            var nodeValue = node.SelectSingleNode(xpath);
            double value;

            if (nodeValue != null && double.TryParse(nodeValue.Value, out value))
            {
                return value;
            }

            return null;
        }
    }
}
