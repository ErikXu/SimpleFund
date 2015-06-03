using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundReturnTask : IFundReturnTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundReturnTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundReturnRepository _fundReturnRepository;
        private readonly IWebSrcUtil _webSrcUtil;

        public FundReturnTask(IFundRepository fundRepository, IFundReturnRepository fundReturnRepository, IWebSrcUtil webSrcUtil)
        {
            _fundRepository = fundRepository;
            _fundReturnRepository = fundReturnRepository;
            _webSrcUtil = webSrcUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Return...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=return&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var returns = new List<FundReturn>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                var jsonObject = JsonConvert.DeserializeObject<FundReturnObject>(response);

                if (jsonObject != null)
                {
                    if (jsonObject.CurrentReturn != null)
                    {
                        var fundReturn = Fill(jsonObject.CurrentReturn, fund.Symbol, fund.ShareClassId, fund.Name, ReturnType.Current);
                        if (fundReturn != null)
                        {
                            returns.Add(fundReturn);
                        }

                        fundReturn = Fill(jsonObject.MonthEndReturn, fund.Symbol, fund.ShareClassId, fund.Name, ReturnType.MonthEnd);
                        if (fundReturn != null)
                        {
                            returns.Add(fundReturn);
                        } 
                    }
                }

                Logger.Info(string.Format("[{0}]Fund Return已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (returns.Count > 0)
            {
                _fundReturnRepository.RemoveAll();
                _fundReturnRepository.InsertBatch(returns);
            }
        }

        private FundReturn Fill(ReturnGroup group, string symbol, string shareClassId, string name, ReturnType type)
        {
            if (group == null || !group.EffectiveDate.HasValue)
            {
                return null;
            }

            var fundReturn = new FundReturn
            {
                Symbol = symbol,
                ShareClassId = shareClassId,
                Name = name,
                Type = type,
                EffectiveDate = group.EffectiveDate.Value
            };

            var returnYTD = group.Return.FirstOrDefault(n => n.Name == "今年以来回报");
            if (returnYTD != null)
            {
                fundReturn.ReturnYTD = returnYTD.Return;
                fundReturn.ReturnToIndYTD = returnYTD.ReturnToInd;
                fundReturn.ReturnToCatYTD = returnYTD.ReturnToCat;
            }

            var return1M = group.Return.FirstOrDefault(n => n.Name == "一个月回报");
            if (return1M != null)
            {
                fundReturn.Return1M = return1M.Return;
                fundReturn.ReturnToInd1M = return1M.ReturnToInd;
                fundReturn.ReturnToCat1M = return1M.ReturnToCat;
            }

            var return3M = group.Return.FirstOrDefault(n => n.Name == "三个月回报");
            if (return3M != null)
            {
                fundReturn.Return3M = return3M.Return;
                fundReturn.ReturnToInd3M = return3M.ReturnToInd;
                fundReturn.ReturnToCat3M = return3M.ReturnToCat;
            }

            var return6M = group.Return.FirstOrDefault(n => n.Name == "六个月回报");
            if (return6M != null)
            {
                fundReturn.Return6M = return6M.Return;
                fundReturn.ReturnToInd6M = return6M.ReturnToInd;
                fundReturn.ReturnToCat6M = return6M.ReturnToCat;
            }

            var return1Y = group.Return.FirstOrDefault(n => n.Name == "一年回报");
            if (return1Y != null)
            {
                fundReturn.Return1Y = return1Y.Return;
                fundReturn.ReturnToInd1Y = return1Y.ReturnToInd;
                fundReturn.ReturnToCat1Y = return1Y.ReturnToCat;
            }

            var return2Y = group.Return.FirstOrDefault(n => n.Name == "二年回报（年化）");
            if (return2Y != null)
            {
                fundReturn.Return2Y = return2Y.Return;
                fundReturn.ReturnToInd2Y = return2Y.ReturnToInd;
                fundReturn.ReturnToCat2Y = return2Y.ReturnToCat;
            }

            var return3Y = group.Return.FirstOrDefault(n => n.Name == "三年回报（年化）");
            if (return3Y != null)
            {
                fundReturn.Return3Y = return3Y.Return;
                fundReturn.ReturnToInd3Y = return3Y.ReturnToInd;
                fundReturn.ReturnToCat3Y = return3Y.ReturnToCat;
            }

            var return5Y = group.Return.FirstOrDefault(n => n.Name == "五年回报（年化）");
            if (return5Y != null)
            {
                fundReturn.Return5Y = return5Y.Return;
                fundReturn.ReturnToInd5Y = return5Y.ReturnToInd;
                fundReturn.ReturnToCat5Y = return5Y.ReturnToCat;
            }

            var return10Y = group.Return.FirstOrDefault(n => n.Name == "十年回报（年化）");
            if (return10Y != null)
            {
                fundReturn.Return10Y = return10Y.Return;
                fundReturn.ReturnToInd10Y = return10Y.ReturnToInd;
                fundReturn.ReturnToCat10Y = return10Y.ReturnToCat;
            }

            return fundReturn;
        }
    }

    public class FundReturnObject
    {
        public ReturnGroup CurrentReturn { get; set; }
        public ReturnGroup MonthEndReturn { get; set; }
    }

    public class ReturnGroup
    {
        public DateTime? EffectiveDate { get; set; }
        public List<ReturnItem> Return { get; set; }
    }

    public class ReturnItem
    {
        public string Name { get; set; }
        public double? Return { get; set; }
        public double? ReturnToInd { get; set; }
        public double? ReturnToCat { get; set; }
        public int? ReturnRank { get; set; }
        public int? ReturnCatSize { get; set; }
        public double? ReturnBenchmark { get; set; }
        public double? ReturnCategory { get; set; }
    }
}
