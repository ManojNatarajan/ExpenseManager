using ExpenseManagerRest.ActionFilters;
using Expenses.Domain.Model.Models;
using Expenses.Domain.Model.Models.API;
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
    public class ExpenseManagerController : ControllerBase
    {
        private GE.ILogger _logger;
        private IMonthlyExpenseDomainRepository _monthlyExpenseRepo;
        private IExpenseEntryDomainRepository _expenseEntryRepo;

        public ExpenseManagerController(GE.ILogger logger,
            IMonthlyExpenseDomainRepository monthlyExpenseRepo,
            IExpenseEntryDomainRepository expenseEntryRepo)
        {
            _logger = logger;
            _monthlyExpenseRepo = monthlyExpenseRepo;
            _expenseEntryRepo = expenseEntryRepo;
        }

        [EnableCors("AngularOrigin")]
        [HttpGet]
        [Route("ExpenseSummaryList/{userId:long}")]
        public IActionResult GetMonthlyExpenseSummaryForUser(long userId)
        {
            if (userId <= 0)
                return BadRequest("ExpenseSummaryList Failed: User NOT found!");

            DomainResponse<List<MonthlyExpenseDTO>> result = _monthlyExpenseRepo.GetMonthlyExpenseSummaryForUser(userId);
            if (result.IsSuccess && result.Value != null && result.Value.Any())
                return Ok(result.Value.ToList<MonthlyExpenseDTO>());
            else
                return BadRequest($"ExpenseSummaryList Failed: {result.ErrorSummary}");
        }

        [HttpGet]
        [Route("GetExpenseEntriesForMonth/{monthlyExpenseId:long}")]
        public IActionResult GetExpenseEntriesForMonth(long monthlyExpenseId)
        {
            if (monthlyExpenseId <= 0)
                return BadRequest("GetExpenseEntriesForMonth Failed: Invalid Arguments! monthlyExpenseId should be greater than 0.");

            DomainResponse<List<ExpenseEntryDTO>> result = _expenseEntryRepo.GetExpenseEntriesForMonth(monthlyExpenseId);
            if (result.IsSuccess && result.Value != null && result.Value.Any())
                return Ok(result.Value.ToList<ExpenseEntryDTO>());
            else
                return BadRequest($"GetExpenseEntriesForMonth Failed: {result.ErrorSummary}");
        }

        [HttpPost]
        [Route("AddExpenseEntry")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult AddExpenseEntry(ExpenseEntryContract expenseEntry)
        {
            DomainResponse<long> result = _expenseEntryRepo.AddExpenseEntry(expenseEntry);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest($"AddExpenseEntry Failed: {result.ErrorSummary}");
        }

        /// <summary>
        /// Currently only "AdditionalRemarks & PaymentStatus update is directly allowed."
        /// </summary>
        /// <param name="expenseSummaryItem"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateExpenseSummaryItem")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult UpdateMonthlyExpenseSummary(MonthlyExpenseSummaryItemForUpdate expenseSummaryItem)
        {            
            if (expenseSummaryItem == null)
                return BadRequest("UpdateMonthlyExpenseSummary Failed: Invalid Arguments!");

            DomainResponse<long> result = _monthlyExpenseRepo.UpdateMonthlyExpenseSummary(expenseSummaryItem);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest($"UpdateMonthlyExpenseSummary Failed: {result.ErrorSummary}");
        }
        
        [HttpPut]
        [Route("UpdateExpenseEntry")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult UpdateExpenseEntry(ExpenseEntryContract expenseEntry)
        {
            DomainResponse<long> result = _expenseEntryRepo.UpdateExpenseEntry(expenseEntry);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest($"UpdateExpenseEntry Failed: {result.ErrorSummary}");
        }

        [HttpDelete]
        [Route("DeleteExpenseEntry/{expenseEntryId:long}")]
        public IActionResult DeleteExpenseEntry(long expenseEntryId)
        {
            DomainResponse<bool> result = _expenseEntryRepo.DeleteExpenseEntry(expenseEntryId);
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return BadRequest($"DeleteExpenseEntry Failed: {result.ErrorSummary}");
        }
        

    }
}
