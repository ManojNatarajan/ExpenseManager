using Expenses.Domain.Model.Models;
using Expenses.Domain.Repo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using GE = GoldenEagles.Logger;

namespace ExpenseManagerRest.Controllers
{
    [EnableCors("AngularOrigin")]
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExpenseTypeController : ControllerBase
    {
        private GE.ILogger _logger;
        private IExpenseTypeDomainRepository _expenseTypeRepo;

        public ExpenseTypeController(GE.ILogger logger, IExpenseTypeDomainRepository expenseTypeRepo)
        {
            _logger = logger;
            _expenseTypeRepo = expenseTypeRepo;
        }

        //We don't want to expose this to outside world -- use Admin claim
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            DomainResponse<List<ExpenseTypeDTO>> response = _expenseTypeRepo.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Expense Types not found or Error occurred. Please contact admin!");
        }

        [HttpGet]
        [Route("user/{userId:long}")]
        public IActionResult GetEntityTypesForUser(long userId)
        {
            if(userId <= 0)
                return BadRequest("User NOT found!");

            DomainResponse<List<ExpenseTypeDTO>> result = _expenseTypeRepo.GetExpenseTypesForUser(userId);
            if (result.IsSuccess && result.Value != null && result.Value.Any())
                return Ok(result.Value.ToList<ExpenseTypeDTO>());
            else
                return BadRequest("expense type not found!");
        }

        [HttpGet]
        [Route("GetByID/{typeid:long}")]
        public IActionResult GetByExpenseTypeID(long typeid)
        {
            DomainResponse<ExpenseTypeDTO> expenseType = _expenseTypeRepo.GetEntityById(typeid);
            if (expenseType != null && expenseType.Value != null)
                return Ok(expenseType.Value);
            else
                return BadRequest("Expense type not found!");
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddExpenseType(ExpenseTypeDTO expenseType)
        {
            //To Do: Duplicate Expense Type validation to be added 
            DomainResponse<ExpenseTypeDTO> response = _expenseTypeRepo.CheckExpenseTypeExists(expenseType?.UserId, expenseType?.Description);
            if (response.ErrorDetails != null && response.ErrorDetails.Count > 0)
            {
                bool? userExist = response?.ErrorDetails?.Any(x => x.Number == 1);
                if (userExist.Value)
                    return Conflict("Expense type already exist.");
                else
                    return BadRequest(response.ErrorSummary);
            }

            DomainResponse<long> responseExpenseType = _expenseTypeRepo.AddEntity(expenseType);
            if (responseExpenseType.IsSuccess)
                return Ok(responseExpenseType.Value);
            else
                return BadRequest(responseExpenseType.ErrorSummary);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult Update(ExpenseTypeDTO expenseType)
        {

            DomainResponse<long> response = _expenseTypeRepo.UpdateEntity(expenseType);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return BadRequest(response.ErrorSummary);
        }

        [HttpDelete]
        [Route("delete/{typeid:long}")]
        public IActionResult Delete(long typeId)
        {
            DomainResponse<bool> response = _expenseTypeRepo.DeleteEntity(typeId);
            if (response.IsSuccess)
                return Ok(response.Value);
            else
                return BadRequest("Error occurred! cannot delete expense type! Please contact Admin.");
        }

    }
}
