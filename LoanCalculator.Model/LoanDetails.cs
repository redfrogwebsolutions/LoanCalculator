using System;
using System.Collections.Generic;

namespace LoanCalculator.Model
{
    public class LoanDetails
    {
        public LoanSummary Summary { get; set; }
        public List<PaymentScheduleItem> Payments { get; set; }
    }
}
