using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Web.Model
{
    public class LoanSummaryVm
    {
        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public int LoanLenght { get; set; }

        public decimal? ArrangmentFee { get; set; }

        public decimal? CompletionFee { get; set; }

        [Required]
        public decimal Deposit { get; set; }

        [Required]
        public decimal FullPrice { get; set; }
    }
}
