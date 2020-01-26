using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanCalculator.Model;

namespace LoanCalculator.Services
{
    public class LoanNoFeeService : LoanServiceBase, ILoanNoFeeService
    {
        public async Task<LoanDetails> CalculateLoan(LoanSummary requestSummary)
        {
            var paymentDates = GetPaymentSchedulerDates(requestSummary.DeliveryDate, requestSummary.LoanLenght, DayOfWeek.Monday);

            return new LoanDetails
            {
                Payments = await Task.Run<List<PaymentScheduleItem>>(() => CalculatePayments(requestSummary, paymentDates)),
                Summary = requestSummary

            };
        }

        private List<PaymentScheduleItem> CalculatePayments(LoanSummary summary, IEnumerable<DateTime> paymentDates)
        {
            List<PaymentScheduleItem> scheduler = new List<PaymentScheduleItem>();

            var creditAmount = summary.FullPrice - summary.Deposit;

            var paymentRate = Math.Round((creditAmount / summary.LoanLenght), 2);

            foreach (DateTime date in paymentDates)
            {
                scheduler.Add(new PaymentScheduleItem
                {
                    PaymentDate = date,
                    PaymentAmount = paymentRate
                });
            }

            var mathRoundDifference = creditAmount - scheduler.Sum(x => x.PaymentAmount);

            scheduler[summary.LoanLenght - 1].PaymentAmount += (mathRoundDifference);

            scheduler[0].PaymentAmount += (summary.ArrangmentFee == null ? 0 : Convert.ToDecimal(summary.ArrangmentFee));
            scheduler[summary.LoanLenght - 1].PaymentAmount += (summary.CompletionFee == null ? 0 : Convert.ToDecimal(summary.CompletionFee));

            return scheduler;
        }

    }
}
