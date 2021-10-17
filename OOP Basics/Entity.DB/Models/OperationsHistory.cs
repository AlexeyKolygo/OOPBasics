using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DB.Models
{
    public class OperationsHistory : BaseEntity
    {
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public decimal BaseSum { get; set; }
        public decimal OpsSum { get; set; }
        public DateTime InsertDate { get; set; }


        public OperationsHistory CreateEntry(long UserId, long AccountId, decimal BaseSum, decimal OpsSum)
        {
            var result = new OperationsHistory();
            result.UserId = UserId;
            result.AccountId = AccountId;
            result.BaseSum = BaseSum;
            result.OpsSum = OpsSum;
            result.InsertDate = DateTime.Now;

            return result;
        }
    }
}
