using System.Collections.Generic;

namespace SimpleFund.Domain.Repositories.Fund
{
    public interface IFundRepository : IRepository<Domain.Fund>
    {
        void RemoveAll();
        void SetOpenFundsFlag(IEnumerable<string> symbols);
        void SetCloseFundsFlag(IEnumerable<string> symbols);
        void SetQDIIFundsFlag(IEnumerable<string> symbols);
        void SetETFFundsFlag(IEnumerable<string> symbols);
        void SetLOFFundsFlag(IEnumerable<string> symbols);
        void SetActiveManagementFundsFlag(IEnumerable<string> symbols);
        void SetIndexFundsFlag(IEnumerable<string> symbols);
        void SetIndexEnhancedFundsFlag(IEnumerable<string> symbols);
        void SetSmallCapFundsFlag(IEnumerable<string> symbols);
    }
}

