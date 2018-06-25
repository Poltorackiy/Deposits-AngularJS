using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Deposits.Response.Models
{
    /// <summary>
    /// Расчет выплат по вкладу
    /// </summary>
    public class DepositPaymentsCollection
    {
        ICollection<DepositPayment> _depositPayments;
        object sync = new object();

        /// <summary>
        /// Первоначальная сумма вклада
        /// </summary>
        public decimal InitialAmount { get; set; }

        /// <summary>
        /// Выплаты процентов итого
        /// </summary>
        public decimal TotalPercents { get; set; }

        /// <summary>
        /// Конечная сумма выплаты по закрытии вклада
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Список выплат по месяцам
        /// </summary>
        public ICollection<DepositPayment> DepositPayments
        {
            get
            {
                if (_depositPayments == null)
                {
                    lock(sync)
                    {
                        if (_depositPayments == null)
                            _depositPayments = new List<DepositPayment>();
                    }
                }
                return _depositPayments;
            }
        }
    }

    public class DepositPaymentsResponse : Response
    {
        DepositPaymentsCollection _payments;
        public DepositPaymentsCollection Body
        {
            get => _payments; set => _payments = value;
        }
    }
}
