
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
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
        [HttpPost("/api/webhook-event-handler")]
        public IActionResult Test([FromBody] ResponseData response)
        {
            return Ok($"{response.Code}, {response.Desc}, {response.Success}, {response.Data.OrderCode}, {response.Signature}");
        }
    }

}