
using Application;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class WebHook : BaseEntity
    {
        public RequestData? RequestData { get; set; }
    }
    public class RequestData
    {
        public string Code { get; set; } = default!;
        public string Desc { get; set; } = default!;
        public bool Success { get; set; }
        public Data? Data { get; set; }
        public string? Signature { get; set; }
    }

    public class Data
    {
        public double OrderCode { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
        public string? AccountNumber { get; set; }
        public string? Reference { get; set; }
        public string? TransactionDateTime { get; set; }
        public string? Currency { get; set; }
        public string? PaymentLinkId { get; set; }
        public string? Code { get; set; }
        public string? Desc { get; set; }
        public string? CounterAccountBankId { get; set; }
        public string? CounterAccountBankName { get; set; }
        public string? CounterAccountName { get; set; }
        public string? CounterAccountNumber { get; set; }
        public string? VirtualAccountName { get; set; }
        public string? VirtualAccountNumber { get; set; }

    }

    public class WebhookResponse
    {
        public bool Success { get; set; }
    }
    public class TestsController : BaseController
    {
        private readonly IMongoDatabase _db;

        private readonly IMongoRepository _repository;

        public TestsController(IMongoRepository repository, AppSettings appSettings)
        {
            _repository = repository;
            var client = new MongoClient(appSettings.ConnectionStrings.MongoDbConnection);
            _db = client.GetDatabase(appSettings.DatabaseName);
        }
        [HttpPost("/api/webhook-event-handler")]
        public async Task<IActionResult> Test([FromBody] RequestData request)
        {
            var collection = _db.GetCollection<RequestData>("webHook");
            await collection.InsertOneAsync(request);

            // Return the response
            return Ok(new { success = true });
        }
    }

}