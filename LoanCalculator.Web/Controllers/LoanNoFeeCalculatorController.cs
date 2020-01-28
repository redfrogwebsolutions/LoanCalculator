using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoanCalculator.Model;
using LoanCalculator.Services;
using Microsoft.Extensions.Options;
using LoanCalculator.Web.MapperService;
using LoanCalculator.Web.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoanCalculator.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoanNoFeeCalculatorController : Controller
    {
        // GET: api/values
        private readonly ILoanNoFeeService _service;
        private readonly IOptions<Settings> _settings;
        private readonly IMapper _mapper;

        public LoanNoFeeCalculatorController(ILoanNoFeeService service, IOptions<Settings> settings, IMapper mapper)
        {
            _service = service;
            _settings = settings;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<LoanDetailsVm>> Post([FromBody]LoanSummaryVm loanSummary)
        {
            try
            {
                if(!ValidateLoanSummaryVm(loanSummary) || !ValidateSettings() || !ModelState.IsValid)
                {
                    throw new Exception("invalid model");
                }

                loanSummary.ArrangmentFee = Convert.ToDecimal(_settings.Value.ArrangementFee);
                loanSummary.CompletionFee = Convert.ToDecimal(_settings.Value.CompletionFee);

                var loanDetails = await _service.CalculateLoan(_mapper.MapLoanSummaryVmToLoanSummary(loanSummary));

                return Ok(_mapper.MapConvertLoanDetailsToLoanDetailsVm(loanDetails));
            }
            catch (Exception ex)
            {
                //ToDo: log exception

                return BadRequest(ex);
            }
        }

        private bool ValidateSettings()
        {
            return (!string.IsNullOrEmpty(_settings.Value.ArrangementFee) && !string.IsNullOrEmpty(_settings.Value.CompletionFee));
        }

        private bool ValidateLoanSummaryVm(LoanSummaryVm vm)
        {
            var deliveryDate = new DateTime(vm.DeliveryDate.Year, vm.DeliveryDate.Month, vm.DeliveryDate.Day);

            if (vm == null || deliveryDate < DateTime.Today || vm.Deposit > vm.FullPrice || vm.Deposit < (vm.FullPrice * Convert.ToDecimal(0.15)))
                return false;
            return true;
        }
    }
}
