using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Web.Model
{

    // Even it is simple project I have added viewmodel classes as it is easy separate services from web project. 

    public class LoanDetailsVm
    {
        public LoanSummaryVm Summary { get; set; }
        public List<PaymentScheduleItemVm> Payments { get; set; }
    }
}
