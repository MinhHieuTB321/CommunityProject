
using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class WebHook : BaseEntity
    {
        public ResponseData? ResponseData { get; set; }
    }
    public class ResponseData
    {
        // public string Code { get; set; }
        // public string Desc { get; set; }
        // public bool Success { get; set; }
        // public Data Data { get; set; }
        // public string Signature { get; set; }
        public string Error { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        // public int OrderCode { get; set; }
        // public int Amount { get; set; }
        // public string Description { get; set; }
        // public string AccountNumber { get; set; }
        // public string Reference { get; set; }
        // public string TransactionDateTime { get; set; }
        // public string Currency { get; set; }
        // public string PaymentLinkId { get; set; }
        // public string Code { get; set; }
        // public string Desc { get; set; }

        // public string CounterAccountBankId { get; set; }
        // public string CounterAccountBankName { get; set; }

        // public string CounterAccountName { get; set; }
        // public string CounterAccountNumber { get; set; }
        // public string VirtualAccountName { get; set; }
        // public string VirtualAccountNumber { get; set; }
        public int Id { get; set; }
        public string Tid { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal CusumBalance { get; set; }
        public DateTime When { get; set; }
        public string BankSubAccId { get; set; }
        public string SubAccId { get; set; }
        public string BankName { get; set; }
        public string BankAbbreviation { get; set; }
        public string VirtualAccount { get; set; }
        public string VirtualAccountName { get; set; }
        public string CorresponsiveName { get; set; }
        public string CorresponsiveAccount { get; set; }
        public string CorresponsiveBankId { get; set; }
        public string CorresponsiveBankName { get; set; }

    }
    public class TestsController : BaseController
    {
        private readonly ILogger<TestsController> _logger;

        private readonly IMongoRepository _repository;

        public TestsController(ILogger<TestsController> logger, IMongoRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        [HttpPost("/api/webhook-event-handler")]
        public async Task<IActionResult> Test([FromBody] ResponseData response)
        {
            var data = new WebHook { ResponseData = response };
            await _repository.InsertAsync<WebHook>("webHook", data);
            return Ok(new { Success = true });
        }
    }

}