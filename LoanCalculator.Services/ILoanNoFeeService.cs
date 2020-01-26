using System.Threading.Tasks;
using LoanCalculator.Model;

namespace LoanCalculator.Services
{
    public interface ILoanNoFeeService
    {
        Task<LoanDetails> CalculateLoan(LoanSummary requestSummary);

    }
}