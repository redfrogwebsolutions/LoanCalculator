using System;
namespace LoanCalculator.Model
{
    public class LoanSummary
    {        
        public DateTime DeliveryDate { get; set; }

        public int LoanLenght { get; set; }

        public decimal? ArrangmentFee { get; set; }

        public decimal? CompletionFee { get; set; }

        public decimal Deposit { get; set; }

        public decimal FullPrice { get; set; }

    }
}
