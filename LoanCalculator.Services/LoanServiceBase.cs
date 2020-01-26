using System;
using System.Collections.Generic;
using LoanCalculator.Model;

namespace LoanCalculator.Services
{
    public abstract class LoanServiceBase
    {

        public virtual IEnumerable<DateTime> GetPaymentSchedulerDates(DateTime loanStartDate, int numberOfMonth, DayOfWeek paymentDay, int paymentStartDelay = 1)
        {
            const int firstDayOfMonth = 1;

            var listOfPaymentDays = new List<DateTime>();

            var firstPaymentMonth = new DateTime(loanStartDate.AddMonths(paymentStartDelay).Year, loanStartDate.AddMonths(paymentStartDelay).Month, firstDayOfMonth);
            var lastPaymentMonth = firstPaymentMonth.AddMonths(numberOfMonth - 1);

            var counterDate = firstPaymentMonth;

            while (counterDate <= lastPaymentMonth)
            {

                DateTime date = new DateTime(counterDate.Year, counterDate.Month, firstDayOfMonth);

                while (date.DayOfWeek != paymentDay)
                {
                    date = date.AddDays(1);
                }

                listOfPaymentDays.Add(date);

                counterDate = counterDate.AddMonths(1);
            }

            return listOfPaymentDays;
        }
    }
}
