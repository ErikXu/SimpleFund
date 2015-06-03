using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using log4net;
using SimpleFund.Domain.Repositories.Fund;
using SimpleFund.Domain.Utils;
using Newtonsoft.Json;

namespace SimpleFund.Domain.Tasks.Fund.Impl
{
    public class FundManagementTask : IFundManagementTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FundManagementTask));
        private static readonly Regex ManagementYearsRegex = new Regex(@"[\d]*?(?=年)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ManagementDaysRegex = new Regex(@"(?<=年)[\d]*?(?=天)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IFundRepository _fundRepository;
        private readonly IFundManagementRepository _fundManagementRepository;
        private readonly IFundManagerRepository _fundManagerRepository;
        private readonly IWebSrcUtil _webSrcUtil;

        public FundManagementTask(IFundRepository fundRepository, 
                                  IFundManagementRepository fundManagementRepository, 
                                  IFundManagerRepository fundManagerRepository, 
                                  IWebSrcUtil webSrcUtil)
        {
            _fundRepository = fundRepository;
            _fundManagementRepository = fundManagementRepository;
            _fundManagerRepository = fundManagerRepository;
            _webSrcUtil = webSrcUtil;
        }

        public void Download()
        {
            Logger.Info("下载Fund Management...");

            const string urlTemplate = "http://cn.morningstar.com/handler/quicktake.ashx?command=manage&fcid={0}";

            var funds = _fundRepository.AsQueryable().ToList();

            var managements = new List<FundManagement>();
            var managers = new List<FundManager>();

            var current = 1;
            foreach (var fund in funds)
            {
                var url = string.Format(urlTemplate, fund.ShareClassId);

                var response = _webSrcUtil.GetToString(url);

                var jsonObject = JsonConvert.DeserializeObject<FundManagementObject>(response);

                if (jsonObject != null)
                {
                    var management = new FundManagement
                    {
                        Symbol = fund.Symbol,
                        ShareClassId = fund.ShareClassId,
                        Name = fund.Name,
                        InvestmentObjective = jsonObject.Profile,
                        CustodianBank = jsonObject.CustodianName,
                        ManagementCompany = jsonObject.CompanyName,
                        Tel = jsonObject.Tel,
                        Fax = jsonObject.Fax,
                        ManagementCompanyWebSite = jsonObject.CompanyHomepage,
                        CustodianBankWebSite = jsonObject.CustodianHomepage
                    };

                    managements.Add(management);

                    if (jsonObject.Managers != null && jsonObject.Managers.Count > 0)
                    {
                        foreach (var item in jsonObject.Managers)
                        {
                            var manager = new FundManager
                            {
                                Symbol = fund.Symbol,
                                ShareClassId = fund.ShareClassId,
                                Name = fund.Name,
                                ManagerId = item.ManagerId,
                                ManagerName = item.ManagerName,
                                IsLeave = item.Leave,
                                StartDate= item.StartDate,
                                EndDate = item.EndDate,
                                ManagementTime = item.ManagementTime
                            };

                            if (!string.IsNullOrEmpty(manager.ManagementTime))
                            {
                                int managementYears;
                                if (int.TryParse(ManagementYearsRegex.Match(manager.ManagementTime).Value, out managementYears))
                                {
                                    manager.ManagementYears = managementYears;
                                }

                                int managementDays;
                                if (int.TryParse(ManagementDaysRegex.Match(manager.ManagementTime).Value, out managementDays))
                                {
                                    manager.ManagementDays = managementDays;
                                }
                            }

                            managers.Add(manager);
                        }
                    }
                }

                Logger.Info(string.Format("[{0}]Fund Management已下载, {1}/{2}", fund.Symbol, current++, funds.Count));
            }

            if (managements.Count > 0)
            {
                _fundManagementRepository.RemoveAll();
                _fundManagementRepository.InsertBatch(managements);
            }

            if (managers.Count > 0)
            {
                _fundManagerRepository.RemoveAll();
                _fundManagerRepository.InsertBatch(managers);
            }
        }
    }

    public class FundManagementObject
    {
        public DateTime? InceptionDate { get; set; }
        public string Profile { get; set; }
        public string CustodianName { get; set; }
        public string CustodianHomepage { get; set; }
        public string CompanyName { get; set; }
        public string CompanyHomepage { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }

        public List<ManagerItem> Managers { get; set; }
    }

    public class ManagerItem
    {
        public bool? Leave { get; set; }
        public string ManagementRange { get; set; }
        public string ManagementTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Resume { get; set; }
    }
}
