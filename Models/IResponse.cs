using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Deposits.Response.Models
{
    public interface IResponse
    {
        /// <summary>
        /// Код ошибки (200 - запрос выполнен успешно)
        /// </summary>
        string StatusCode { get; set; }

        /// <summary>
        /// Сообщение о состоянии запроса (OK - запрос выполнен успешно)
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Тело запроса
        /// </summary>
        //object Body { get; set; }
    }
}
