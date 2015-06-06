using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundPriceTask : IFundPriceTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundPriceTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundPriceRepository _fundPriceRepository;
        private readonly IWebSrcUtil _webSrcUtil;
        private readonly IXpathUtil _xpathUtil;

        public FundPriceTask(IFundRepository fundRepository, IFundPriceRepository fundPriceRepository, IWebSrcUtil webSrcUtil, IXpathUtil xpathUtil)
        {
            _fundRepository = fundRepository;
            _fundPriceRepository = fundPriceRepository;
            _webSrcUtil = webSrcUtil;
            _xpathUtil = xpathUtil;
        }

        public void Download()
        {
            Logger.Info("下载基金净值...");

            const string urlTemplate = "http://fund.eastmoney.com/f10/F10DataApi.aspx?type=lsjz&code={0}&page=1&per=100000";

            var funds = _fundRepository.AsQueryable().Where(n => n.Category != "货币市场基金" && !n.Category.Contains("短债基金")).ToList();

            var prices = new List<FundPrice>();

            var current = 1;

            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.Symbol);

                List<FundPrice> pricesOfFund;

                try
                {
                    pricesOfFund = DownloadPrices(url, fund.Symbol, fund.ShareClassId, fund.Name);
                }
                catch
                {
                    Thread.Sleep(200);
                    pricesOfFund = DownloadPrices(url, fund.Symbol, fund.ShareClassId, fund.Name);
                }

                if (pricesOfFund != null && pricesOfFund.Count > 0)
                {
                    prices.AddRange(pricesOfFund);
                }

                Logger.Info(string.Format("[{0}]净值已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (prices.Count > 0)
            {
                Logger.Info("开始清空原有数据...");
                _fundPriceRepository.RemoveAll();
                Logger.Info("结束清空原有数据...");

                Logger.Info("开始插入净值数据...");
                _fundPriceRepository.InsertBatch(prices);
                Logger.Info("结束插入净值数据...");
            }

        }

        private List<FundPrice> DownloadPrices(string url, string symbol, string shareClassId, string name)
        {
            var html = _webSrcUtil.GetToString(url, Encoding.GetEncoding("GB2312"));

            if (html.Contains("暂无数据"))
            {
                return null;
            }

            html = "<Root>" + html + "</Root>";
            var doc = new XmlDocument();
            doc.LoadXml(html);
            var nodes = doc.SelectNodes("//table/tbody/tr");

            var prices = new List<FundPrice>();

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var price = new FundPrice
                    {
                        Symbol = symbol,
                        ShareClassId = shareClassId,
                        Name = name
                    };

                    var effectiveDate = _xpathUtil.LocateDate(node, "td[position()=1]/text()");
                    if (!effectiveDate.HasValue)
                    {
                        Logger.Error(string.Format("[{0}]缺少生效日期！", symbol));
                        continue;
                    }
                    price.EffectiveDate = effectiveDate.Value;

                    var closePrice = _xpathUtil.LocateDouble(node, "td[position()=2]/text()");
                    if (!closePrice.HasValue)
                    {
                        Logger.Error(string.Format("[{0}]缺少净值！", symbol));
                        continue;
                    }
                    price.ClosePrice = closePrice.Value;

                    price.AccNav = _xpathUtil.LocateDouble(node, "td[position()=3]/text()");

                    prices.Add(price);
                }
            }

            return prices;
        }
    }
}
