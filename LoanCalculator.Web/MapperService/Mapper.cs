using LoanCalculator.Model;
using LoanCalculator.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Web.MapperService
{
    //just in this simple project. Usually I will use AutoMapper
    public class Mapper: IMapper
    {
        public LoanSummary MapLoanSummaryVmToLoanSummary(Model.LoanSummaryVm vm)
        {
            return new LoanSummary
            {
                ArrangmentFee = vm.ArrangmentFee,
                CompletionFee = vm.CompletionFee,
                DeliveryDate = vm.DeliveryDate,
                Deposit = vm.Deposit,
                FullPrice = vm.FullPrice,
                LoanLenght = vm.LoanLenght
            };
        }

        public LoanSummaryVm MapLoanSummaryToLoanSummaryVm(LoanCalculator.Model.LoanSummary vm)
        {
            return new Model.LoanSummaryVm
            {
                ArrangmentFee = vm.ArrangmentFee,
                CompletionFee = vm.CompletionFee,
                DeliveryDate = vm.DeliveryDate,
                Deposit = vm.Deposit,
                FullPrice = vm.FullPrice,
                LoanLenght = vm.LoanLenght
            };
        }

        public List<PaymentScheduleItemVm> MapPaymentScheduleItemListToPaymentScheduleItemVmList(List<PaymentScheduleItem> payments)
        {
            var paymentsVm = new List<PaymentScheduleItemVm>();
            foreach(PaymentScheduleItem payment in payments)
            {
                paymentsVm.Add(new PaymentScheduleItemVm
                {
                    PaymentDate = payment.PaymentDate,
                    PaymentAmount = payment.PaymentAmount
                });
            }

            return paymentsVm;
        }

        public LoanDetailsVm MapConvertLoanDetailsToLoanDetailsVm(LoanDetails loanDetails)
        {
            var loanDetailsVm = new LoanDetailsVm();
            loanDetailsVm.Summary = MapLoanSummaryToLoanSummaryVm(loanDetails.Summary);
            loanDetailsVm.Payments = MapPaymentScheduleItemListToPaymentScheduleItemVmList(loanDetails.Payments);

            return loanDetailsVm;
        }
    }
}
