
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
        public string Code { get; set; }
        public string Desc { get; set; }
        public bool Success { get; set; }
        public Data Data { get; set; }
        public string Signature { get; set; }
    }

    public class Data
    {
        public int OrderCode { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string CounterAccountName { get; set; }
        public string CounterAccountNumber { get; set; }
        public string VirtualAccountName { get; set; }
        public string VirtualAccountNumber { get; set; }
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
            _logger.LogInformation($"{response.Code}, {response.Desc}, {response.Success}, {response.Data.OrderCode}, {response.Signature}");
            return Ok($"{response.Code}, {response.Desc}, {response.Success}, {response.Data.OrderCode}, {response.Signature}");
        }
    }

}