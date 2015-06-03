using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundRiskTask : IFundRiskTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (FundRiskTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundRiskRepository _fundRiskRepository;
        private readonly IWebSrcUtil _webSrcUtil;

        public FundRiskTask(IFundRepository fundRepository, IFundRiskRepository fundRiskRepository,
            IWebSrcUtil webSrcUtil)
        {
            _fundRepository = fundRepository;
            _fundRiskRepository = fundRiskRepository;
            _webSrcUtil = webSrcUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Risk...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=rating&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var risks = new List<FundRisk>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                var jsonObject = JsonConvert.DeserializeObject<FundRiskObject>(response);

                if (jsonObject != null)
                {
                    var risk = new FundRisk
                    {
                        Symbol = fund.Symbol,
                        ShareClassId = fund.ShareClassId,
                        Name = fund.Name,
                        EffectiveDate = jsonObject.EffectiveDate,
                        RatingDate = jsonObject.RatingDate,
                        Rating1Y = jsonObject.Rating1Year,
                        Rating2Y = jsonObject.Rating2Year,
                        Rating3Y = jsonObject.Rating3Year,
                        Rating5Y = jsonObject.Rating5Year,
                        Rating10Y = jsonObject.Rating10Year
                    };

                    if (jsonObject.RiskAssessment != null)
                    {
                        var averageReturn = jsonObject.RiskAssessment.FirstOrDefault(n => n.Name == "平均回报（%）");
                        if (averageReturn != null)
                        {
                            risk.AverageReturn3Y = averageReturn.Year3;
                            risk.AverageReturn5Y = averageReturn.Year5;
                            risk.AverageReturn10Y = averageReturn.Year10;
                        }

                        var standardDeviation = jsonObject.RiskAssessment.FirstOrDefault(n => n.Name == "标准差（%）");
                        if (standardDeviation != null)
                        {
                            risk.StandardDeviation3Y = standardDeviation.Year3;
                            risk.StandardDeviation5Y = standardDeviation.Year5;
                            risk.StandardDeviation10Y = standardDeviation.Year10;
                        }

                        var msRiskFactor = jsonObject.RiskAssessment.FirstOrDefault(n => n.Name == "晨星风险系数");
                        if (msRiskFactor != null)
                        {
                            risk.MSRiskFactor3Y = msRiskFactor.Year3;
                            risk.MSRiskFactor5Y = msRiskFactor.Year5;
                            risk.MSRiskFactor10Y = msRiskFactor.Year10;
                        }

                        var sharpeRatio = jsonObject.RiskAssessment.FirstOrDefault(n => n.Name == "夏普比率");
                        if (sharpeRatio != null)
                        {
                            risk.SharpeRatio3Y = sharpeRatio.Year3;
                            risk.SharpeRatio5Y = sharpeRatio.Year5;
                            risk.SharpeRatio10Y = sharpeRatio.Year10;
                        }
                    }

                    if (jsonObject.RiskStats != null)
                    {
                        var alpha = jsonObject.RiskStats.FirstOrDefault(n => n.Name == "阿尔法系数（%）");
                        if (alpha != null)
                        {
                            risk.AlphaToIndex3Y = alpha.ToInd;
                            risk.AlphaToCategory3Y = alpha.ToCat;
                        }

                        var beta = jsonObject.RiskStats.FirstOrDefault(n => n.Name == "贝塔系数");
                        if (beta != null)
                        {
                            risk.BetaToIndex3Y = beta.ToInd;
                            risk.BetaToCategory3Y = beta.ToCat;
                        }

                        var rSquare = jsonObject.RiskStats.FirstOrDefault(n => n.Name == "R平方");
                        if (rSquare != null)
                        {
                            risk.RSquareToIndex3Y = rSquare.ToInd;
                            risk.RSquareToCategory3Y = rSquare.ToCat;
                        }
                    }

                    risks.Add(risk);
                }

                Logger.Info(string.Format("[{0}]Fund Risk已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (risks.Count > 0)
            {
                _fundRiskRepository.RemoveAll();
                _fundRiskRepository.InsertBatch(risks);
            }
        }
    }

    public class FundRiskObject
    {
        public string FundClassId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime? RatingDate { get; set; }

        public string RiskProfile3Year { get; set; }

        public string RiskProfile3YearCatToMkt { get; set; }

        public int? Rating1Year { get; set; }

        public int? Rating2Year { get; set; }

        public int? Rating3Year { get; set; }

        public int? Rating5Year { get; set; }

        public int? Rating10Year { get; set; }

        // ReSharper disable once InconsistentNaming
        public int? Rating2Year_Risk { get; set; }

        // ReSharper disable once InconsistentNaming
        public int? Rating3Year_Risk { get; set; }

        // ReSharper disable once InconsistentNaming
        public int? Rating5Year_Risk { get; set; }

        // ReSharper disable once InconsistentNaming
        public int? Rating10Year_Risk { get; set; }

        public List<RiskAssessment> RiskAssessment { get; set; }

        public List<RiskStats> RiskStats { get; set; }
    }

    public class RiskAssessment
    {
        public string Name { get; set; }

        public double? Year3 { get; set; }

        public string Year3Rank { get; set; }

        public double? Year5 { get; set; }

        public string Year5Rank { get; set; }

        public double? Year10 { get; set; }

        public string Year10Rank { get; set; }
    }

    public class RiskStats
    {
        public string Name { get; set; }

        public double? ToInd { get; set; }

        public double? ToCat { get; set; }
    }   
}
