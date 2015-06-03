using Autofac;
using log4net;
using SimpleFund.Common;
using SimpleFund.Domain.Tasks.Fund;

namespace SimpleFund.Downloader
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            Logger.Info("开始下载数据...");
            var builder = new ContainerBuilder();
            Starter.Initialize(builder);

            var container = builder.Build();

            //var fundTask = container.Resolve<IFundTask>();
            //fundTask.Download();

            //var fundOperationTask = container.Resolve<IFundOperationTask>();
            //fundOperationTask.Download();

            //var fundFeeTask = container.Resolve<IFundFeeTask>();
            //fundFeeTask.Download();

            //var fundPerformanceTask = container.Resolve<IFundPerformanceTask>();
            //fundPerformanceTask.Download();

            //var fundReturnTask = container.Resolve<IFundReturnTask>();
            //fundReturnTask.Download();

            //var fundRiskTask = container.Resolve<IFundRiskTask>();
            //fundRiskTask.Download();

            //var fundManagementTask = container.Resolve<IFundManagementTask>();
            //fundManagementTask.Download();

            var fundDividendTask = container.Resolve<IFundDividendTask>();
            fundDividendTask.Download();

            Logger.Info("结束下载数据...");
        }
    }
}
