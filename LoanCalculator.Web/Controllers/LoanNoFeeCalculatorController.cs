using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoanCalculator.Model;
using LoanCalculator.Services;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoanCalculator.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoanNoFeeCalculatorController : Controller
    {
        // GET: api/values
        private readonly ILoanNoFeeService _service;
        private readonly IOptions<Settings> _settings;

        public LoanNoFeeCalculatorController(ILoanNoFeeService service, IOptions<Settings> settings)
        {
            _service = service;
            _settings = settings;
        }


        [HttpPost]
        public async Task<ActionResult<LoanDetails>> Post([FromBody]LoanSummary loanSummary)
        {
            loanSummary.ArrangmentFee = Convert.ToDecimal(_settings.Value.ArrangementFee);
            loanSummary.CompletionFee = Convert.ToDecimal(_settings.Value.CompletionFee);

            var loanDetails = await _service.CalculateLoan(loanSummary);

            return Ok(loanDetails);
        }
    }
}
