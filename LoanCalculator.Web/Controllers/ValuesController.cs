using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoanCalculator.Model;
using LoanCalculator.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoanCalculator.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoanNoFeeCalculatorController : Controller
    {
        // GET: api/values
        private readonly ILoanNoFeeService _service;

        public LoanNoFeeCalculatorController(ILoanNoFeeService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<LoanDetails> Post([FromBody]LoanSummary loanSummary)
        {
            //var summary = JsonConvert.DeserializeObject<LoanSummary>(loanSummary.ToString());
            var loanDetails = await _service.CalculateLoan(loanSummary);

            //return JsonConvert.SerializeObject(loanDetails);
            return loanDetails;
        }
    }
}
