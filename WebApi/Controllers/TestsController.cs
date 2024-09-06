
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
        public string AccountNumber { get; set; }
        public string Reference { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string Currency { get; set; }
        public string PaymentLinkId { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string CounterAccountBankId { get; set; }
        public string CounterAccountBankName { get; set; }
        public string CounterAccountName { get; set; }
        public string CounterAccountNumber { get; set; }
        public string VirtualAccountName { get; set; }
        public string VirtualAccountNumber { get; set; }
    }
    public class TestsController : BaseController
    {
        [HttpGet]
        public IActionResult Test([FromBody] ResponseData data)
        {
            return Ok("Oke: \n" + data);
        }
    }

}