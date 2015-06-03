using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundDividendTask : IFundDividendTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundDividendTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundDividendRepository _fundDividendRepository;
        private readonly IFundSplitRepository _fundSplitRepository;
        private readonly IWebSrcUtil _webSrcUtil;

        public FundDividendTask(IFundRepository fundRepository,
                                IFundDividendRepository fundDividendRepository,
                                IFundSplitRepository fundSplitRepository,
                                IWebSrcUtil webSrcUtil)
        {
            _fundRepository = fundRepository;
            _fundDividendRepository = fundDividendRepository;
            _fundSplitRepository = fundSplitRepository;
            _webSrcUtil = webSrcUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Dividend & Split...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=dividend&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var dividends = new List<FundDividend>();
            var splits = new List<FundSplit>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                FundDividendObject jsonObject = null;

                try
                {
                    jsonObject = JsonConvert.DeserializeObject<FundDividendObject>(response);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("Can not Deserialize URL:{0}", url), ex);
                }

                if (jsonObject != null)
                {
                    if (jsonObject.Dividend != null && jsonObject.Dividend.Count > 0)
                    {
                        foreach (var item in jsonObject.Dividend)
                        {
                            var dividend = new FundDividend
                            {
                                Symbol = fund.Symbol,
                                ShareClassId = fund.ShareClassId,
                                Name = fund.Name,
                                ExcludingDate = item.ExcludingDate,
                                ReinvestDate = item.ReinvestDate,
                                TotalDistribution = item.TotalDistribution,
                                ReinvestPrice = item.ReinvestPrice
                            };

                            dividends.Add(dividend);
                        }
                    }

                    if (jsonObject.Split != null && jsonObject.Split.Count > 0)
                    {
                        foreach (var item in jsonObject.Split)
                        {
                            var split = new FundSplit
                            {
                                Symbol = fund.Symbol,
                                ShareClassId = fund.ShareClassId,
                                Name = fund.Name,
                                EffectiveDate = item.EffectiveDate,
                                Ratio = item.Ratio
                            };

                            splits.Add(split);
                        }
                    }
                }

                Logger.Info(string.Format("[{0}]Fund Dividend & Split已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (dividends.Count > 0)
            {
                _fundDividendRepository.RemoveAll();
                _fundDividendRepository.InsertBatch(dividends);
            }

            if (splits.Count > 0)
            {
                _fundSplitRepository.RemoveAll();
                _fundSplitRepository.InsertBatch(splits);
            }
        }
    }

    public class FundDividendObject
    {
        public List<DividendItem> Dividend { get; set; }
        public List<SplitItem> Split { get; set; }
    }

    public class DividendItem
    {
        public string FundClassId { get; set; }
        public DateTime ExcludingDate { get; set; }
        public DateTime ReinvestDate { get; set; }
        public double TotalDistribution { get; set; }
        public double ReinvestPrice { get; set; }
    }

    public class SplitItem
    {
        public string FundClassId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public double Ratio { get; set; }
    }
}
