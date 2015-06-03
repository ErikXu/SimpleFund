using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundTask : IFundTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundPurchaseInfoRepository _fundPurchaseInfoRepository;
        private readonly IWebSrcUtil _webSrcUtil;
        private readonly IXpathUtil _xpathUtil;

        public FundTask(IFundRepository fundRepository, 
                        IFundPurchaseInfoRepository fundPurchaseInfoRepository,
                        IWebSrcUtil webSrcUtil,
                        IXpathUtil xpathUtil)
        {
            _fundRepository = fundRepository;
            _fundPurchaseInfoRepository = fundPurchaseInfoRepository;
            _webSrcUtil = webSrcUtil;
            _xpathUtil = xpathUtil;
        }

        public void Download()
        {
            const string url = "http://cn.morningstar.com/fundselect/default.aspx";

            var viewstate = FirstAccess(url);
            viewstate = DownloadAllFundList(url, viewstate);
            viewstate = DownloadFundPurchaseInfos(url, viewstate);
            viewstate = SetOpenFundsFlag(url, viewstate);
            viewstate = SetCloseFundsFlag(url, viewstate);
            viewstate = SetQDIIFundsFlag(url, viewstate);
            viewstate = SetETFFundsFlag(url, viewstate);
            viewstate = SetLOFFundsFlag(url, viewstate);
            viewstate = SetActiveManagementFundsFlag(url, viewstate);
            viewstate = SetIndexFundsFlag(url, viewstate);
            viewstate = SetIndexEnhancedFundsFlag(url, viewstate);
            SetSmallCapFundsFlag(url, viewstate);
        }

        private string FirstAccess(string url)
        {
            Logger.Info("访问晨星基金筛选器...");

            var doc = _webSrcUtil.GetToXml(url, Encoding.UTF8);
            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string DownloadAllFundList(string url, string preViewstate)
        {
            Logger.Info("下载基金列表...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", "ctl00$cphMain$ddlPageSite"}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$ddlPageSite", "10000"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var nodes = doc.SelectNodes("//table[@id='ctl00_cphMain_gridResult']/tr[position()>1 and td[position()=2]/a/text() != '']");

            var fundList = new List<Domain.Fund>();

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var fund = GetFund(node);
                    fundList.Add(fund);
                }
            }

            if (fundList.Count > 0)
            {
                _fundRepository.RemoveAll();
                _fundRepository.InsertBatch(fundList);
            }

            Logger.Info(string.Format("基金数：{0}", fundList.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private Domain.Fund GetFund(XmlNode node)
        {
            var fund = new Domain.Fund
            {
                Symbol = _xpathUtil.LocateString(node, "td[position()=2]/a/text()"),
                ShareClassId = _xpathUtil.LocateString(node, "td[position()=3]/a/@href"),
                Name = _xpathUtil.LocateString(node, "td[position()=3]/a/text()"),
                Category = _xpathUtil.LocateString(node, "td[position()=4]/text()"),
                PriceDate = _xpathUtil.LocateDate(node, "td[position()=7]/text()"),
                Price = _xpathUtil.LocateDouble(node, "td[position()=8]/text()"),
                PriceChange = _xpathUtil.LocateDouble(node, "td[position()=9]/text()"),
                ReturnYtd = _xpathUtil.LocateDouble(node, "td[position()=10]/text()")
            };

            if (!string.IsNullOrEmpty(fund.ShareClassId))
            {
                fund.ShareClassId = fund.ShareClassId.Replace("/quicktake/", string.Empty);
            }

            return fund;
        }

        private string DownloadFundPurchaseInfos(string url, string preViewstate)
        {
            Logger.Info("下载基金购买信息...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", "ctl00$cphMain$lbOperations"}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$ddlPageSite", "10000"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var nodes = doc.SelectNodes("//table[@id='ctl00_cphMain_gridResult']/tr[position()>1 and td[position()=2]/a/text() != '']");

            var infos = new List<FundPurchaseInfo>();

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var info = GetPurchaseInfo(node);
                    infos.Add(info);
                }
            }

            if (infos.Count > 0)
            {
                _fundPurchaseInfoRepository.RemoveAll();
                _fundPurchaseInfoRepository.InsertBatch(infos);
            }

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private FundPurchaseInfo GetPurchaseInfo(XmlNode node)
        {
            var info = new FundPurchaseInfo
            {
                Symbol = _xpathUtil.LocateString(node, "td[position()=2]/a/text()"),
                ShareClassId = _xpathUtil.LocateString(node, "td[position()=3]/a/@href"),
                Name = _xpathUtil.LocateString(node, "td[position()=3]/a/text()"),
                InceptionDate = _xpathUtil.LocateDate(node, "td[position()=4]/text()"),
                PurchaseStatus = _xpathUtil.LocateString(node, "td[position()=5]/a/text()"),
                RedemptionStatus = _xpathUtil.LocateString(node, "td[position()=6]/a/text()"),
                FrontFee = _xpathUtil.LocateDouble(node, "td[position()=8]/text()"),
                DeferFee = _xpathUtil.LocateDouble(node, "td[position()=9]/text()"),
                RedemptionFee = _xpathUtil.LocateDouble(node, "td[position()=10]/text()"),
                ManagementFee = _xpathUtil.LocateDouble(node, "td[position()=11]/text()"),
                CustodianFee = _xpathUtil.LocateDouble(node, "td[position()=12]/text()"),
                DistributionFee = _xpathUtil.LocateDouble(node, "td[position()=13]/text()")
            };

            if (!string.IsNullOrEmpty(info.ShareClassId))
            {
                info.ShareClassId = info.ShareClassId.Replace("/quicktake/", string.Empty);
            }

            return info;
        }

        private string SetOpenFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置开放式基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$0", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetOpenFundsFlag(symbols);

            Logger.Info(string.Format("开放式基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetCloseFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置封闭式基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$1", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetCloseFundsFlag(symbols);

            Logger.Info(string.Format("封闭式基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetQDIIFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置QDII基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$2", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetQDIIFundsFlag(symbols);

            Logger.Info(string.Format("QDII基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetETFFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置ETF基金...");

            var postVars = new NameValueCollection
            {
                {"__EVENTTARGET", ""},
                {"__EVENTARGUMENT", ""},
                {"__LASTFOCUS", ""},
                {"__VIEWSTATE", preViewstate},
                {"ctl00$cphMain$cblGroup$3", "on"},
                {"ctl00$cphMain$ddlPageSite", "10000"},
                {"ctl00$cphMain$btnGo", "查询"}
            };

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetETFFundsFlag(symbols);

            Logger.Info(string.Format("ETF基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetLOFFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置LOF基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$4", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetLOFFundsFlag(symbols);

            Logger.Info(string.Format("LOF基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetActiveManagementFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置主动管理型基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$5", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetActiveManagementFundsFlag(symbols);

            Logger.Info(string.Format("主动管理型基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetIndexFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置指数型基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$6", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetIndexFundsFlag(symbols);

            Logger.Info(string.Format("主动指数型基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private string SetIndexEnhancedFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置指数增强型基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$7", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};

            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetIndexEnhancedFundsFlag(symbols);

            Logger.Info(string.Format("指数增强型基金数：{0}", symbols.Count));

            var viewstate = _webSrcUtil.GetViewState(doc);
            return viewstate;
        }

        private void SetSmallCapFundsFlag(string url, string preViewstate)
        {
            Logger.Info("设置中小盘基金...");

            var postVars = new NameValueCollection
			               	{
			               		{"__EVENTTARGET", ""}, 
								{"__EVENTARGUMENT", ""},
								{"__LASTFOCUS", ""},
								{"__VIEWSTATE", preViewstate},
								{"ctl00$cphMain$cblGroup$8", "on"},
								{"ctl00$cphMain$ddlPageSite", "10000"},
								{"ctl00$cphMain$btnGo", "查询"}
			               	};


            var doc = _webSrcUtil.PostToXml(url, postVars, Encoding.UTF8);

            var symbols = GetSymbols(doc);

            _fundRepository.SetSmallCapFundsFlag(symbols);

            Logger.Info(string.Format("中小盘基金数：{0}", symbols.Count));
        }

        private static List<string> GetSymbols(XmlNode doc)
        {
            var nodes = doc.SelectNodes("//table[@id='ctl00_cphMain_gridResult']/tr[position()>1 and td[position()=2]/a/text() != '']");

            var symbols = new List<string>();

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var nodeValue = node.SelectSingleNode("td[position()=2]/a/text()");
                    var symbol = nodeValue != null ? nodeValue.Value : null;
                    symbols.Add(symbol);
                }
            }
            return symbols;
        }
    }
}
