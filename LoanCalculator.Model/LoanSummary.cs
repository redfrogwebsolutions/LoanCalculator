using System;
namespace LoanCalculator.Model
{
    public class LoanSummary
    {
        //[JsonProperty(PropertyName = "deliveryDate")]
        public DateTime DeliveryDate { get; set; }

        //[JsonProperty(PropertyName = "loanLenght")]
        public int LoanLenght { get; set; }

        //[JsonProperty(PropertyName = "arrangmentFee")]
        public decimal? ArrangmentFee { get; set; }

        //[JsonProperty(PropertyName = "completionFee")]
        public decimal? CompletionFee { get; set; }

        //[JsonProperty(PropertyName = "deposit")]
        public decimal Deposit { get; set; }

        //[JsonProperty(PropertyName = "fullPrice")]
        public decimal FullPrice { get; set; }


    }
}
