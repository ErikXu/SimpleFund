using System.Collections.Generic;
using System.Linq;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundPerformanceTask : IFundPerformanceTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundPerformanceTask));

        private readonly IFundRepository _fundRepository;
        private readonly IFundPerformanceRepository _fundPerformanceRepository;
        private readonly IFundWorstReturnRepository _fundWorstReturnRepository;
        private readonly IWebSrcUtil _webSrcUtil;

        public FundPerformanceTask(IFundRepository fundRepository, 
                                   IFundPerformanceRepository fundPerformanceRepository, 
                                   IFundWorstReturnRepository fundWorstReturnRepository,
                                   IWebSrcUtil webSrcUtil)
        {
            _fundRepository = fundRepository;
            _fundPerformanceRepository = fundPerformanceRepository;
            _fundWorstReturnRepository = fundWorstReturnRepository;
            _webSrcUtil = webSrcUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Performance...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=performance&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var performances = new List<FundPerformance>();
            var worstReturns = new List<FundWorstReturn>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                var jsonObject = JsonConvert.DeserializeObject<FundPerformanceObject>(response);

                if (jsonObject != null)
                {
                    var performance = Fill(jsonObject.Year, fund.Symbol, fund.ShareClassId, fund.Name);

                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre1Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre2Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre3Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre4Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre5Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre6Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    performance = Fill(jsonObject.Pre7Year, fund.Symbol, fund.ShareClassId, fund.Name);
                    if (performance != null)
                    {
                        performances.Add(performance);
                    }

                    var worstReturn = new FundWorstReturn
                    {
                        Symbol = fund.Symbol,
                        ShareClassId = fund.ShareClassId,
                        Name = fund.Name,
                        WorstReturn3M = jsonObject.Worst3MonReturn,
                        WorstReturn6M = jsonObject.Worst6MonReturn
                    };

                    worstReturns.Add(worstReturn);
                }

                Logger.Info(string.Format("[{0}]Fund Performance已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (performances.Count > 0)
            {
                _fundPerformanceRepository.RemoveAll();
                _fundPerformanceRepository.InsertBatch(performances);
            }

            if (worstReturns.Count > 0)
            {
                _fundWorstReturnRepository.RemoveAll();
                _fundWorstReturnRepository.InsertBatch(worstReturns);
            }
        }

        private FundPerformance Fill(PerformanceItem item, string symbol, string shareClassId, string name)
        {
            if (item == null || !item.Year.HasValue)
            {
                return null;
            }

            var performance = new FundPerformance
            {
                Symbol = symbol,
                ShareClassId = shareClassId,
                Name = name,
                Year = item.Year.Value,
                ReturnYear = item.ReturnYear,
                ReturnYearToIndex = item.ReturnYear_Ind,
                ReturnYearToCategory = item.ReturnYear_Cat,
                ReturnQ1 = item.ReturnQ1,
                ReturnQ1ToIndex = item.ReturnQ1_Ind,
                ReturnQ1ToCategory = item.ReturnQ1_Cat,
                ReturnQ2 = item.ReturnQ2,
                ReturnQ2ToIndex = item.ReturnQ2_Ind,
                ReturnQ2ToCategory = item.ReturnQ2_Cat,
                ReturnQ3 = item.ReturnQ3,
                ReturnQ3ToIndex = item.ReturnQ3_Ind,
                ReturnQ3ToCategory = item.ReturnQ3_Cat,
                ReturnQ4 = item.ReturnQ4,
                ReturnQ4ToIndex = item.ReturnQ4_Ind,
                ReturnQ4ToCategory = item.ReturnQ4_Cat
            };

            return performance;
        }
    }

    public class FundPerformanceObject
    {
        public PerformanceItem Year { get; set; }
        public PerformanceItem Pre1Year { get; set; }
        public PerformanceItem Pre2Year { get; set; }
        public PerformanceItem Pre3Year { get; set; }
        public PerformanceItem Pre4Year { get; set; }
        public PerformanceItem Pre5Year { get; set; }
        public PerformanceItem Pre6Year { get; set; }
        public PerformanceItem Pre7Year { get; set; }
        public double? Worst3MonReturn { get; set; }
        public double? Worst6MonReturn { get; set; }
    }

    public class PerformanceItem
    {
        public int? Year { get; set; }

        public double? ReturnYear { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnYear_Ind { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnYear_Cat { get; set; }

        public double? ReturnQ1 { get; set; }

        public double? ReturnQ2 { get; set; }

        public double? ReturnQ3 { get; set; }

        public double? ReturnQ4 { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ1_Cat { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ2_Cat { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ3_Cat { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ4_Cat { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ1_Ind { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ2_Ind { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ3_Ind { get; set; }

        // ReSharper disable once InconsistentNaming
        public double? ReturnQ4_Ind { get; set; }
    }
}
