using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DB.Models
{
   public class UserAccount:BaseEntity
    {
        public long UserId { get; set; }
        public long AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal RemainSum { get; set; }


    }
}
