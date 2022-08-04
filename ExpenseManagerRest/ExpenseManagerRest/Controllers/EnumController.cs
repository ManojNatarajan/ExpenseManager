using Expenses.Domain.Model.Models;
using Expenses.Domain.Repo.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GE = GoldenEagles.Logger;

namespace ExpenseManagerRest.Controllers
{
    [EnableCors("AngularOrigin")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GE.ILogger _logger;
        private readonly IUserStatusDomainRepository _userStatusDomainRepo;
        private readonly IRecurringintervaltypeDomainRepository _recurringIntervalTypeDomainRepo;
        private readonly IMonthlypaymentstatusDomainRepository _monthlyPaymentStatusDomainRepository;
        private readonly IExpensepaymentstatusDomainRepository _expensePaymentStatusDomainRepository;

        public EnumController(GE.ILogger logger, IConfiguration config, 
            IUserStatusDomainRepository userStatusDomainRepo,
            IRecurringintervaltypeDomainRepository recurringIntervalTypeDomainRepo,
            IMonthlypaymentstatusDomainRepository monthlyPaymentStatusDomainRepository,
            IExpensepaymentstatusDomainRepository expensePaymentStatusDomainRepository)
        {
            _logger = logger;
            _configuration = config;
            _userStatusDomainRepo = userStatusDomainRepo;
            _recurringIntervalTypeDomainRepo = recurringIntervalTypeDomainRepo;
            _monthlyPaymentStatusDomainRepository = monthlyPaymentStatusDomainRepository;
            _expensePaymentStatusDomainRepository = expensePaymentStatusDomainRepository;
        }

        [HttpGet]
        [Route("userstatus")]
        public IActionResult GetUserStatus()
        {
            DomainResponse<List<UserStatusDTO>> response = _userStatusDomainRepo.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Error Occurred. Please contact admin!");
        }

        [HttpGet]
        [Route("ExpenseTypeRecurringIntervalCategory")]
        public IActionResult GetExpenseTypeRecurringIntervalType()
        {
            DomainResponse<List<RecurringintervaltypeDTO>> response = _recurringIntervalTypeDomainRepo.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Error Occurred. Please contact admin!");
        }

        [HttpGet]
        [Route("MonthlyPaymentStatus")]
        public IActionResult GetMonthlyPaymentStatus()
        {
            DomainResponse<List<MonthlypaymentstatusDTO>> response = _monthlyPaymentStatusDomainRepository.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Error Occurred. Please contact admin!");
        }

        [HttpGet]
        [Route("ExpensePaymentStatus")]
        public IActionResult GetExpensePaymentStatus()
        {
            DomainResponse<List<ExpensepaymentstatusDTO>> response = _expensePaymentStatusDomainRepository.GetAllEntityList();
            if (response != null && response.Value != null && response.Value.Any())
                return Ok(response.Value);
            else
                return BadRequest("Sorry, Error Occurred. Please contact admin!");
        }
    }
}
