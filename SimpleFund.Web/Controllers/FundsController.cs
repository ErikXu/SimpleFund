using System;
using System.Linq;
using System.Web.Http;
using SimpleFund.Domain;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Web.Models;

namespace SimpleFund.Web.Controllers
{
    [RoutePrefix("api/funds")]
    public class FundsController : ApiController
    {
        private readonly IFundRepository _fundRepository;
        private readonly IFundOperationRepository _fundOperationRepository;
        private readonly IFundPerformanceRepository _fundPerformanceRepository;
        private readonly IFundReturnRepository _fundReturnRepository;
        private readonly IFundWorstReturnRepository _fundWorstReturnRepository;
        private readonly IFundRiskRepository _fundRiskRepository;

        public FundsController(IFundRepository fundRepository,
                               IFundOperationRepository fundOperationRepository,
                               IFundPerformanceRepository fundPerformanceRepository,
                               IFundReturnRepository fundReturnRepository,
                               IFundWorstReturnRepository fundWorstReturnRepository,
                               IFundRiskRepository fundRiskRepository)
        {
            _fundRepository = fundRepository;
            _fundOperationRepository = fundOperationRepository;
            _fundPerformanceRepository = fundPerformanceRepository;
            _fundReturnRepository = fundReturnRepository;
            _fundWorstReturnRepository = fundWorstReturnRepository;
            _fundRiskRepository = fundRiskRepository;
        }

        [HttpGet]
        public PagedResult<Fund> List([FromUri]PagedFilter filter)
        {
            var query = _fundRepository.AsQueryable().OrderBy(n => n.Symbol);
            return PagedResult<Fund>.From(query, filter);
        }

        [HttpGet]
        [Route("{symbol}")]
        public FundDetail Detail(string symbol)
        {
            var detail = new FundDetail();

            var fund = _fundRepository.AsQueryable().SingleOrDefault(n => n.Symbol == symbol);
            if (fund != null)
            {
                detail.Symbol = fund.Symbol;
                detail.Name = fund.Name;
                detail.Price = fund.Price;
                detail.PriceDate = fund.PriceDate;
                detail.PriceChange = fund.PriceChange;
                if (fund.Price.HasValue && fund.PriceChange.HasValue)
                {
                    detail.PriceChangeRatio = fund.PriceChange.Value / fund.Price.Value * 100;
                }
            }

            var operation = _fundOperationRepository.AsQueryable().SingleOrDefault(n => n.Symbol == symbol);
            if (operation != null)
            {
                detail.Category = operation.Category;
                detail.InceptionDate = operation.InceptionDate;
                detail.OpenDate = operation.OpenDate;
                detail.TradingDate = operation.TradingDate;
                detail.Subscribe = operation.Subscribe;
                detail.Redeem = operation.Redeem;
                detail.Box = operation.Box;
                detail.FundTotalNetAsset = operation.FundTotalNetAsset;
                detail.MinimumInvestment = operation.MinimumInvestment;
                detail.Exchange = operation.Exchange;
                detail.FrontFee = operation.FrontFee;
                detail.DeferFee = operation.DeferFee;
            }

            detail.Performances = _fundPerformanceRepository.AsQueryable().Where(n => n.Symbol == symbol && n.Year != DateTime.Now.Year).OrderByDescending((n => n.Year));
            detail.CurrentReturn = _fundReturnRepository.AsQueryable().FirstOrDefault(n => n.Symbol == symbol && n.Type == ReturnType.Current);
            detail.MonthEndReturn = _fundReturnRepository.AsQueryable().FirstOrDefault(n => n.Symbol == symbol && n.Type == ReturnType.MonthEnd);
            detail.WorstReturn = _fundWorstReturnRepository.AsQueryable().FirstOrDefault(n => n.Symbol == symbol);
            detail.Risk = _fundRiskRepository.AsQueryable().FirstOrDefault(n => n.Symbol == symbol);

            return detail;
        }
    }
}