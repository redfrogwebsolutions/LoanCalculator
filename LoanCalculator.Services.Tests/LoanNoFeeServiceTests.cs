using System;
using System.Collections.Generic;
using System.Linq;
using LoanCalculator.Model;
using LoanCalculator.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarLoanCalculator.Services.Tests
{
    [TestClass]
    public class LoanNoFeeServicesTests
    {
        private LoanNoFeeService services;
        private DateTime startDate;

        [TestInitialize]
        public void TestSetUp()
        {
            services = new LoanNoFeeService();
            startDate = new DateTime(2020, 2, 15);
        }

        [TestMethod]
        public void DoesGetPaymentSchedulerReturnRightAmountOfRecordsTest()
        {
            var daysYear = services.GetPaymentSchedulerDates(startDate, 12, DayOfWeek.Monday);
            var daysTwoYear = services.GetPaymentSchedulerDates(startDate, 24, DayOfWeek.Monday);
            var daysThreeYear = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Monday);
            var daysMonth = services.GetPaymentSchedulerDates(startDate, 1, DayOfWeek.Monday);
            var daysThreeMonths = services.GetPaymentSchedulerDates(startDate, 3, DayOfWeek.Monday);

            Assert.AreEqual(12, daysYear.Count());
            Assert.AreEqual(24, daysTwoYear.Count());
            Assert.AreEqual(36, daysThreeYear.Count());
            Assert.AreEqual(1, daysMonth.Count());
            Assert.AreEqual(3, daysThreeMonths.Count());

        }

        [TestMethod]
        public void DoesShedulerReturnsProperDayOfWeek()
        {
            var daysMonday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Monday);
            var daysTuesday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Tuesday);
            var daysWednesday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Wednesday);
            var daysThursday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Thursday);
            var daysFriday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Friday);
            var daysSaturday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Saturday);
            var daysSunday = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Sunday);

            Assert.IsFalse(daysMonday.Any(x => x.DayOfWeek != DayOfWeek.Monday));
            Assert.IsFalse(daysTuesday.Any(x => x.DayOfWeek != DayOfWeek.Tuesday));
            Assert.IsFalse(daysWednesday.Any(x => x.DayOfWeek != DayOfWeek.Wednesday));
            Assert.IsFalse(daysThursday.Any(x => x.DayOfWeek != DayOfWeek.Thursday));
            Assert.IsFalse(daysFriday.Any(x => x.DayOfWeek != DayOfWeek.Friday));
            Assert.IsFalse(daysSaturday.Any(x => x.DayOfWeek != DayOfWeek.Saturday));
            Assert.IsFalse(daysSunday.Any(x => x.DayOfWeek != DayOfWeek.Sunday));
        }

        [TestMethod]
        public void DoesTheMethodReturnsProperDatesForDefaultPaymentStartDelayParameter()
        {
            //todo: create assert for all 36 months

            var d1 = new DateTime(2020, 3, 2);
            var d2 = new DateTime(2020, 4, 6);
            var d3 = new DateTime(2020, 5, 4);

            var days = services.GetPaymentSchedulerDates(startDate, 36, DayOfWeek.Monday).ToList();

            Assert.AreEqual(d1, days[0]);
            Assert.AreEqual(d2, days[1]);
            Assert.AreEqual(d3, days[2]);
        }

        [TestMethod]
        public void DoesCalculeteLoanReturnsTheSameServiceDetailsAsPassedToTheMethodTest()
        {
            var summary = new LoanSummary()
            {
                ArrangmentFee = 88,
                CompletionFee = 20,
                Deposit = 150,
                FullPrice = 2000,
                LoanLenght = 12,
                DeliveryDate = new DateTime(2020, 1, 1)
            };

            var loanDetails = services.CalculateLoan(summary).Result;

            Assert.AreEqual(summary, loanDetails.Summary);
        }

        [TestMethod]
        public void DoesCalculateLoanAddingProperSetUpFeeTest()
        {
            var summary = GetSummary();

            var loanDetails = services.CalculateLoan(summary).Result;

            Assert.AreEqual(Convert.ToDecimal(188), loanDetails.Payments[0].PaymentAmount);
        }

        [TestMethod]
        public void DoesCalculateLoanAddingProperCompletionFeeTest()
        {
            var summary = GetSummary();

            var loanDetails = services.CalculateLoan(summary).Result;

            Assert.AreEqual(Convert.ToDecimal(120), loanDetails.Payments[summary.LoanLenght - 1].PaymentAmount);
        }

        [TestMethod]
        public void DoesCalculateLoanCalculetePaymentsProperlyTest()
        {
            var summary = GetSummary();

            var loanDetails = services.CalculateLoan(summary).Result;

            Assert.AreEqual(summary.LoanLenght - 2, loanDetails.Payments.Count(x => x.PaymentAmount == 100));
        }

        [TestMethod]
        public void DoesCalculateLoanCalculeteAllPaymentsAmountProperlyForDecimalTest()
        {
            var summary = GetSummary();

            //2223.27/12 = 185.2725 so must be rounded
            summary.FullPrice = Convert.ToDecimal(3223.27);

            var loanDetails = services.CalculateLoan(summary).Result;

            var expectedAmount = (summary.FullPrice - summary.Deposit) + summary.ArrangmentFee + summary.CompletionFee;

            Assert.AreEqual(expectedAmount, loanDetails.Payments.Sum(x => x.PaymentAmount));
        }

        private LoanSummary GetSummary()
        {
            return new LoanSummary()
            {
                ArrangmentFee = 88,
                CompletionFee = 20,
                Deposit = 1000,
                FullPrice = 2200,
                LoanLenght = 12,
                DeliveryDate = new DateTime(2020, 1, 1)
            };
        }
    }
}
