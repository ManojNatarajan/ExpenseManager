using Expenses.Domain.Model.Models;
using Expenses.Domain.Repo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GE = GoldenEagles.Logger;

namespace ExpenseManagerRest.Controllers
{

    [EnableCors("AngularOrigin")]
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private GE.ILogger _logger;
        private IUserDomainRepository _userDomainRepo;

        public UserController(GE.ILogger logger, IUserDomainRepository userDomainRepo)
        {
            _logger = logger;
            _userDomainRepo = userDomainRepo;
        }

        //Note: AddUser/SignupUser goes into AuthController as User is not logged in still at that point. 

        //We don't want to expose this to outside world -- use Admin claim
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            _logger.LogInfo("UserController: Getting All Users!");            
            DomainResponse<List<UserDTO>> response = _userDomainRepo.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Users not found or Error occurred. Please contact admin!");
        }

        [HttpGet]
        [Route("{userId:long}")]
        public IActionResult GetByUserID(long userId)
        {
            DomainResponse<UserDTO> user = _userDomainRepo.GetEntityById(userId);
            if (user != null && user.Value != null)
                return Ok(user.Value);
            else
                return BadRequest("User not found!");
        }
       
        [HttpPut]
        [Route("update")]
        public IActionResult Update(UserDTO user)
        {
            DomainResponse<long> response = _userDomainRepo.UpdateEntity(user);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return BadRequest(response.ErrorSummary);
        }

        [HttpDelete]
        [Route("delete/{userId:long}")]
        public IActionResult Delete(long userId)
        {
            DomainResponse<bool> response = _userDomainRepo.DeleteEntity(userId);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return BadRequest("Error occurred! cannot delete user! Please contact Admin.");
        }

    }
}
