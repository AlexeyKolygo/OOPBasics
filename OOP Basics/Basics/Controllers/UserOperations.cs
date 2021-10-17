using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    public class UserOperationsController : ControllerBase
    {
        private readonly IDbRepository<UserModel> _userRepository;
        private readonly IDbRepository<UserAccount> _userAccountRepository;
        private readonly IMapper _mapper;

        public UserOperationsController(IDbRepository<UserModel> usersRepository, 
            IDbRepository<UserAccount> userAccountRepository,
            IDbRepository<OperationsHistory> operationsHistory,
            IMapper mapper)
        {
            _userRepository = usersRepository;
            _userAccountRepository = userAccountRepository;
            this._mapper = mapper;
        }
       
        [HttpPost("UserOperations/registerUser")]
        public async Task<IActionResult> Register([FromQuery] string UserName)
        {
            var user = new User();
            user.Name = UserName;
            var result = _mapper.Map<UserModel>(user);
           await _userRepository.AddAsync(result);
           var inverted = user.InvertUserName(UserName);

            return Ok($"Your user ID is:{result.Id}. Please refer to it for further Operations if necessary. As a joke this is inverted UserName:{inverted}");
        }
        
        [HttpPost("UserOperations/generateAccount")]
        public async Task<IActionResult> GenerateAccountForUser([FromQuery] int UserId, int AccType)
        {

            var account = new Account();
            var user =  _userRepository.GetAll()?.FirstOrDefault(x => x.Id == UserId);
            if (user == null)
            {
                return BadRequest("User is not exist in database.Please add new user to generate an account");
            }
            else
            {
                account.UserId = UserId;
                account.AccountType = (AccountType) AccType;
                var newAcc = _mapper.Map<UserAccount>(account);
                await _userAccountRepository.AddAsync(newAcc);
                return Ok(newAcc);
            }
        }

       
        [HttpGet("UserOperations/GetAllUserAccounts")]
        public async Task<IActionResult> GetAllUserAccounts([FromQuery] int UserId)
        {
            var result = _userAccountRepository.GetAll().Where(x => x.UserId == UserId);
            return Ok(result);
        }
        [HttpPost("UpdateUserEmail")]
        public async Task<IActionResult> UpdateUserEmail()
        {
            var model = new User();
            var users = model.UploadUsersFromFile().ToArray();
            if (users == null)
            {
                return BadRequest("File not found");

            }
            var logstring = new List<string>();
            var log = "Start logging:";
            logstring.Add(log);
            foreach (var entry in users)
            {
                var user = _userRepository.GetAll()?.FirstOrDefault(x => x.Email == entry.Email);
                if (user == null)
                {
                    var newuser = _mapper.Map<UserModel>(entry);
                    await _userRepository.AddAsync(newuser);
                    log = $"User was added:{newuser.Name} & {newuser.Email}";
                    logstring.Add(log);
                }
                else
                {
                    var updateuser = _mapper.Map<UserModel>(user);
                    updateuser.Email = entry.Email;
                    updateuser.Name = entry.Name;
                    await _userRepository.UpdateAsync(updateuser);
                    log = $"User was updated:{updateuser.Name} & {updateuser.Email}";
                    logstring.Add(log);
                }

            }
            return Ok(logstring);
        }
    }
}
