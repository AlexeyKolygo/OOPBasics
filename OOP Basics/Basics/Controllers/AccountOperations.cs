using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basics.Models;
using Entity.DB;
using Entity.DB.Models;

namespace Basics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountOperationsController : ControllerBase
    {
        private readonly IDbRepository<UserModel> _userRepository;
        private readonly IDbRepository<UserAccount> _userAccountRepository;
        private readonly IDbRepository<OperationsHistory> _operationsHistory;
        private readonly IDbRepository<TransferHistory> _transferHistory;
        private readonly IMapper _mapper;

        public AccountOperationsController(IDbRepository<UserModel> usersRepository, 
            IDbRepository<UserAccount> userAccountRepository,
            IDbRepository<OperationsHistory> operationsHistory,
            IDbRepository<TransferHistory> transferHistory,
            IMapper mapper)
        {
            _userRepository = usersRepository;
            _userAccountRepository = userAccountRepository;
            _operationsHistory = operationsHistory;
            _transferHistory = transferHistory;
            this._mapper = mapper;
        }

        [HttpPut("ReplenishAccount")]
        public async Task<IActionResult> ReplenishAccount([FromQuery] long AccountNumber, decimal Ammount)
        {
            var account = _userAccountRepository.GetAll().FirstOrDefault(x => x.AccountNumber == AccountNumber);
            if (account == null)
            {
                return BadRequest($"{AccountNumber} doesn't exist");
            }
            else if (Ammount < 0)
            {
                return BadRequest($"{Ammount} cannot be negative"); ;
            }

            var initialsum = account.RemainSum;
            account.RemainSum = account.RemainSum + Ammount;
            await _userAccountRepository.UpdateAsync(account);

            var historyentry = new OperationsHistory();
            var history = historyentry.CreateEntry(account.UserId, account.AccountNumber, initialsum, Ammount);
            await _operationsHistory.AddAsync(history);
            return Ok(account);

        }

        [HttpPut("Withdraw")]
        public async Task<IActionResult> Withdraw([FromQuery] long AccountNumber, decimal Ammount)
        {
            
            var account = _userAccountRepository.GetAll()?.FirstOrDefault(x => x.AccountNumber == AccountNumber);
            if (account == null)
            {
                return BadRequest($"{AccountNumber} doesn't exist");
            }
            else if (account.RemainSum < Ammount)
            {
                return BadRequest(
                    $"You're trying to withdraw {Ammount}, yet your current savings are: {account.RemainSum}");
            }
            else if(Ammount<0)

            {
                return BadRequest($"Ты дурак? как можно снять {Ammount}");
            }
            var initialsum = account.RemainSum;
            account.RemainSum = account.RemainSum - Ammount;
            await _userAccountRepository.UpdateAsync(account);

            var historyentry = new OperationsHistory();
            var history = historyentry.CreateEntry(account.UserId, account.AccountNumber, initialsum, Ammount);
            await _operationsHistory.AddAsync(history);
            return Ok(account);

        }

        [HttpPut("ChangeAccountType")]
        public async Task<IActionResult> ChangeAccountTypebyUserId([FromQuery] long AccountNumber, int AccType)
        {
            var account = _userAccountRepository.GetAll().FirstOrDefault(x => x.AccountNumber == AccountNumber);
            if (account == null)
            {
                return BadRequest($"{AccountNumber} doesn't exist");
            }
            account.AccountType = ((AccountType)AccType).ToString();
            await _userAccountRepository.UpdateAsync(account);
            return Ok(account);

        }
        [HttpGet("AllOperations")]
        public async Task<IActionResult> GetAllOperations()
        {
            var result = _operationsHistory.GetAll();
            return Ok(result);
        }
        [HttpPut("TransferBetweenAccounts")]
        public async Task<IActionResult> TransferBetweenAccounts([FromQuery] long FromAccount, long ToAccount, decimal Amount)
        {

            var fromAccount = _userAccountRepository.GetAll()?.FirstOrDefault(x => x.AccountNumber == FromAccount);
            var toAccount = _userAccountRepository.GetAll()?.FirstOrDefault(x => x.AccountNumber == ToAccount);
            if (fromAccount == null&&ToAccount==null)
            {
                return BadRequest($"Account doesn't exist");
            }
            
            var account = new Account();
            var traIsValid = account.CheckIfSumIsValid(fromAccount.RemainSum, Amount);
            if (traIsValid)
            {
                fromAccount.RemainSum = fromAccount.RemainSum - Amount;
                await _userAccountRepository.UpdateAsync(fromAccount);
                toAccount.RemainSum = toAccount.RemainSum + Amount;
                await _userAccountRepository.UpdateAsync(toAccount);
                var history = new TransferHistory().CreateEntry(FromAccount, ToAccount, Amount);
                await _transferHistory.AddAsync(history);

                return Ok($"Transfer details: from {FromAccount} to {toAccount} the follwoing amount was transferred:{Amount} ");
            }

            return BadRequest($"Something went wrong. Account validation results:{traIsValid}");

        }

    }
}
