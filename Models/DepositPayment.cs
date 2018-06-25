using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Deposits.Response.Models
{
    public class DepositPayment
    {
        /// <summary>
        /// Начальная сумма
        /// </summary>
        public decimal InitialAmount { get; set; }

        /// <summary>
        /// Начисленные проценты за месяц
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Результирующая сумма на конец расчетного периода
        /// </summary>
        public decimal TotalAmount
        {
            get
            {
                return InitialAmount + Fee;
            }
        }
    }
}
