using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DB.Models
{
    public class TransferHistory : BaseEntity
    {
        public long FromAcc { get; set; }
        public long ToAcc { get; set; }
        public decimal Amount { get; set; }
        public DateTime InsertDate { get; set; }

        public TransferHistory CreateEntry(long FromAcc, long ToAcc, decimal Amount)
        {
            var result = new TransferHistory();
            result.FromAcc = FromAcc;
            result.ToAcc = ToAcc;
            result.Amount = Amount;
            result.InsertDate = DateTime.Now;

            return result;
        }


    }
}
