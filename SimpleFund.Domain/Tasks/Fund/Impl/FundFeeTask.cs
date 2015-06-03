using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundFeeTask : IFundFeeTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundFeeTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundFeeRepository _fundFeeRepository;
        private readonly IWebSrcUtil _webSrcUtil;
        private readonly IParseUtil _parseUtil;

        public FundFeeTask(IFundRepository fundRepository, IFundFeeRepository fundFeeRepository, IWebSrcUtil webSrcUtil, IParseUtil parseUtil)
        {
            _fundRepository = fundRepository;
            _fundFeeRepository = fundFeeRepository;
            _webSrcUtil = webSrcUtil;
            _parseUtil = parseUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Fee...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=fee&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var fees = new List<FundFee>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                var jsonObject = JsonConvert.DeserializeObject<FundFeeObject>(response);

                if (jsonObject != null)
                {
                    var fee = new FundFee
                    {
                        Symbol = fund.Symbol,
                        ShareClassId = fund.ShareClassId,
                        Name = fund.Name,
                        ManagementFee = _parseUtil.ToDoubleNullable(jsonObject.Management),
                        CustodianFee = _parseUtil.ToDoubleNullable(jsonObject.Custodial),
                        DistributionFee = _parseUtil.ToDoubleNullable(jsonObject.Distribution)
                    };

                    fees.Add(fee);
                }

                Logger.Info(string.Format("[{0}]Fund Fee已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (fees.Count > 0)
            {
                _fundFeeRepository.RemoveAll();
                _fundFeeRepository.InsertBatch(fees);
            }
        }
    }

    public class FundFeeObject
    {
        public string Management { get; set; }
        public string Custodial { get; set; }
        public string Distribution { get; set; }
        public string MinInvestment { get; set; }
        public string AdditionalPurchase { get; set; }
        public string MinInvestmentUnit { get; set; }

        public List<FeeItem> Front { get; set; }
        public List<FeeItem> Defer { get; set; }
        public List<FeeItem> Redemption { get; set; }
    }

    public class FeeItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
