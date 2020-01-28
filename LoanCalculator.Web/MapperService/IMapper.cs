using LoanCalculator.Model;
using LoanCalculator.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Web.MapperService
{
    public interface IMapper
    {
        LoanSummary MapLoanSummaryVmToLoanSummary(LoanSummaryVm vm);
        LoanSummaryVm MapLoanSummaryToLoanSummaryVm(LoanSummary vm);
        List<PaymentScheduleItemVm> MapPaymentScheduleItemListToPaymentScheduleItemVmList(List<PaymentScheduleItem> payments);
        LoanDetailsVm MapConvertLoanDetailsToLoanDetailsVm(LoanDetails loanDetails);
    }
}
