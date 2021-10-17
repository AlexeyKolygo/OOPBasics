using System;
using System.Text;

namespace Basics.Models
{
    public enum AccountType
    {
        Debit=1,
        Credit=2,
    }
    public class Account
    {
        public int id { get; set; }
        public long UserId { get; set; }
        public long AccountNumber { get; set; }
        public AccountType AccountType{ get; set; }
        private decimal RemainSum { get; set; }

        public Account()
        {
            this.AccountNumber = GenerateAccount();
        }
        private long GenerateAccount()
        {
            Random randomizer = new Random();
            StringBuilder accountString = new StringBuilder();

            for (int i = 0; i < 12; i++)
            {
                var newchar = randomizer.Next(1, 9).ToString();
                accountString.Append(newchar);
            }

            long AccountNumber = long.Parse(accountString.ToString());
            return AccountNumber;
        }

        public bool CheckIfSumIsValid(decimal FromAccount, decimal amount)
        {
            if (FromAccount >= amount) return true;
            return false;
        }

    }

}
