using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Deposits.Response.Models
{
    public class Response : IResponse
    {
        public string StatusCode { get; set; } = "200";
        public string Message { get; set; } = "OK";
    }
}
