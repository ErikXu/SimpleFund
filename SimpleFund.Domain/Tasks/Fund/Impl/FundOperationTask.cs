using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundOperationTask : IFundOperationTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundOperationTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundOperationRepository _fundOperationRepository;
        private readonly IWebSrcUtil _webSrcUtil;
        private readonly IXpathUtil _xpathUtil;
        private readonly IParseUtil _parseUtil;
        private readonly IRegexTask _regexTask;

        public FundOperationTask(IFundRepository fundRepository, 
                                 IFundOperationRepository fundOperationRepository, 
                                 IWebSrcUtil webSrcUtil, 
                                 IXpathUtil xpathUtil,
                                 IParseUtil parseUtil,
                                 IRegexTask regexTask)
        {
            _fundRepository = fundRepository;
            _fundOperationRepository = fundOperationRepository;
            _webSrcUtil = webSrcUtil;
            _xpathUtil = xpathUtil;
            _parseUtil = parseUtil;
            _regexTask = regexTask;
        }

        public void Download()
        {
            Logger.Info("下载基金营运数据...");

            const string urlTemplate = "http://cn.morningstar.com/quicktake/{0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var operations = new List<FundOperation>();

            var current = 1;
            foreach (var fund in funds)
            {
                var operation = new FundOperation
                {
                    Symbol = fund.Symbol,
                    ShareClassId = fund.ShareClassId,
                    Name = fund.Name
                };

                var url = string.Format(urlTemplate, fund.ShareClassId);
                FillOperation(url, operation);

                operations.Add(operation);

                Logger.Info(string.Format("[{0}]运营信息已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (operations.Count > 0)
            {
                _fundOperationRepository.RemoveAll();
                _fundOperationRepository.InsertBatch(operations);
            }
        }

        private void FillOperation(string url, FundOperation operation)
        {
            var doc = _webSrcUtil.GetToXml(url);

            operation.CategoryId = _xpathUtil.LocateString(doc, "//input[@class='categoryid']/@value");
            operation.Category = _xpathUtil.LocateString(doc, "//span[@class='category']/text()");
            operation.InceptionDate = _xpathUtil.LocateDate(doc, "//span[@class='inception']/text()");
            operation.OpenDate = _xpathUtil.LocateDate(doc, "//span[@class='start']/text()"); 
            operation.TradingDate = _xpathUtil.LocateDate(doc, "//span[@class='tradingdate']/text()");
            operation.Subscribe = _xpathUtil.LocateString(doc, "//span[@class='subscribe']/text()");
            operation.Redeem = _xpathUtil.LocateString(doc, "//span[@class='redeem']/text()");
            operation.Box = _xpathUtil.LocateString(doc, "//span[@class='sbdesc']/text()");
            operation.FundTotalNetAsset = _xpathUtil.LocateDouble(doc, "//span[@class='asset']/text()");
            operation.MinimumInvestment = _xpathUtil.LocateDouble(doc, "//span[@class='min']/text()");
            operation.Exchange = _xpathUtil.LocateString(doc, "//ul[@class='info']/li[10]/span/text()");
            operation.FrontFee = _xpathUtil.LocateString(doc, "//ul[@class='info']/li[11]/span/text()");
            operation.DeferFee = _xpathUtil.LocateString(doc, "//ul[@class='info']/li[12]/span/text()");

            if (operation.FrontFee != null)
            {
                operation.FrontFeeUnit = _regexTask.MatchUnit(operation.FrontFee);
                var value = _regexTask.MatchNumber(operation.FrontFee);
                operation.FrontFeeValue = _parseUtil.ToDoubleNullable(value);
            }

            if (operation.DeferFee != null)
            {
                operation.DeferFeeUnit = _regexTask.MatchUnit(operation.DeferFee);
                var value = _regexTask.MatchNumber(operation.DeferFee);
                operation.DeferFeeValue = _parseUtil.ToDoubleNullable(value);
            }
        }

    }
}
