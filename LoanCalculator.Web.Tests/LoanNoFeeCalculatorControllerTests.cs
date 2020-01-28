using LoanCalculator.Model;
using LoanCalculator.Services;
using LoanCalculator.Web.Controllers;
using LoanCalculator.Web.MapperService;
using LoanCalculator.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace LoanCalculator.Web.Tests
{
    [TestClass]
    public class LoanNoFeeCalculatorControllerTests
    {
        private Mock<ILoanNoFeeService> _service;
        private Mock<IOptions<Settings>> _settings;
        private Mock<IMapper> _mapper;
        private LoanNoFeeCalculatorController _controller;

        [TestInitialize]
        public void SetUpTest()
        {
            _service = new Mock<ILoanNoFeeService>();
            _settings = new Mock<IOptions<Settings>>();
            _mapper = new Mock<IMapper>();
            _controller = new LoanNoFeeCalculatorController(_service.Object, _settings.Object, _mapper.Object);

        }

        [TestMethod]
        public void PostReturnsStatusCode200Test()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfLoanSummaryIsNullTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(null).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfDeliveryDateIsInThePastTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();
            sampleDetailsVm.Summary.DeliveryDate = DateTime.Now.AddDays(-1);

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfDepositIsHighrerThanVehiclePriceTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();
            sampleDetailsVm.Summary.Deposit = 1000;
            sampleDetailsVm.Summary.FullPrice = 900;

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfDepositIsLestThan15PercentOfVehiclePriceTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();
            sampleDetailsVm.Summary.Deposit = 149;
            sampleDetailsVm.Summary.FullPrice = 1000;

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfArrangmentFeeIsEmptyTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "",
                CompletionFee = "88"
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsBadRequestIfCompletionFeeIsEmptyTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = ""
            };

            var sampleDetailsVm = GenerateLoanDetailsVmObject();

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(GenerateLoanDetailsObject());
            _settings.Setup(x => x.Value).Returns(sampleSettings);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void PostReturnsExpectedObjectTest()
        {
            var sampleSettings = new Settings
            {
                ArrangementFee = "20",
                CompletionFee = "88"
            };

            var sampleDetails = GenerateLoanDetailsObject();
            var sampleDetailsVm = GenerateLoanDetailsVmObject();

            _service.Setup(x => x.CalculateLoan(It.IsAny<LoanSummary>())).ReturnsAsync(sampleDetails);
            _settings.Setup(x => x.Value).Returns(sampleSettings);
            _mapper.Setup(x => x.MapConvertLoanDetailsToLoanDetailsVm(sampleDetails)).Returns(sampleDetailsVm);

            var result = _controller.Post(sampleDetailsVm.Summary).Result.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(sampleDetailsVm, result.Value);
        }

        private LoanDetails GenerateLoanDetailsObject()
        {
            return new LoanDetails
            {
                Summary = new LoanCalculator.Model.LoanSummary
                {
                    ArrangmentFee = 20,
                    CompletionFee = 88,
                    DeliveryDate = DateTime.Now,
                    FullPrice = 10000,
                    Deposit = 5000,
                    LoanLenght = 2

                },
                Payments = new List<PaymentScheduleItem>
                {
                    new PaymentScheduleItem
                    {
                        PaymentAmount = 2520,
                        PaymentDate = DateTime.Now.AddMonths(1)
                    }
                }
            };
        
        }

        private LoanDetailsVm GenerateLoanDetailsVmObject()
        {
            return new LoanDetailsVm
            {
                Summary = new Model.LoanSummaryVm
                {
                    ArrangmentFee = 20,
                    CompletionFee = 88,
                    DeliveryDate = DateTime.Now,
                    FullPrice = 10000,
                    Deposit = 5000,
                    LoanLenght = 2

                },
                Payments = new List<PaymentScheduleItemVm>
                {
                    new PaymentScheduleItemVm
                    {
                        PaymentAmount = 2520,
                        PaymentDate = DateTime.Now.AddMonths(1)
                    }
                }
            };

        }
    }
}
