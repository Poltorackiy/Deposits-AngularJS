using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Deposits.Response.Models
{
    public class DepositsCollection
    {
        ICollection<Deposit> _deposits;
        object sync = new object();

        public ICollection<Deposit> Deposits
        {
            get
            {
                if (_deposits == null)
                {
                    lock(sync)
                    {
                        if (_deposits == null)
                            _deposits = new List<Deposit>();
                    }
                }
                return _deposits;
            }
        }
    }

    public class DepositsResponse : Response
    {
        DepositsCollection _depositsCollection;
        public DepositsCollection Body
        {
            get => _depositsCollection; set => _depositsCollection = value;
        }
    }
}
