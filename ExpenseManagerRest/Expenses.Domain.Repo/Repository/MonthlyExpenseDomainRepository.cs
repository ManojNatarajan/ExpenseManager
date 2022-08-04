using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using Expenses.Domain.Model.Models.API;
using System.Linq.Expressions;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IMonthlyExpenseDomainRepository : IDomainRepositoryBase<MonthlyExpenseDTO, Monthlyexpense>
    {
        DomainResponse<List<MonthlyExpenseDTO>> GetMonthlyExpenseSummaryForUser(long userID);
        DomainResponse<long> UpdateMonthlyExpenseSummary(MonthlyExpenseSummaryItemForUpdate expenseSummaryItem);
    }

    public class MonthlyExpenseDomainRepository : DomainRepositoryBase<MonthlyExpenseDTO, Monthlyexpense>, IMonthlyExpenseDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly GE.ILogger _logger;

        public MonthlyExpenseDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : 
            base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public DomainResponse<List<MonthlyExpenseDTO>> GetMonthlyExpenseSummaryForUser(long userID)
        {
            DomainResponse<List<MonthlyExpenseDTO>> response = new DomainResponse<List<MonthlyExpenseDTO>>();
            try
            {
                if (!IsUserExist(userID))
                {
                    response.AddErrorDescription(-1, "Failed to get Monthly Expense Summary list for user. ", $"User [{userID}] NOT found.");
                    return response;
                }
                Expression<Func<Monthlyexpense, bool>> isUserHasMonthlyExpenses = u => u.Userid == userID;
                response.Value = base.Find(isUserHasMonthlyExpenses)?.Value?.ToList();
                if(response.Value == null || !response.Value.Any())
                    response.AddErrorDescription(-1, "Failed to get Monthly Expense Summary list for user. ", $"No entries found for User [{userID}]!");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                response.AddErrorDescription(-1, "Exception in getting Monthly Expense Summary.", $"Exception information: {e}");
            }
            return response;
        }

        public DomainResponse<long> UpdateMonthlyExpenseSummary(MonthlyExpenseSummaryItemForUpdate expenseSummaryItem)
        {
            DomainResponse<long> response = new DomainResponse<long>();
            try
            {
                if (!IsUserExist(expenseSummaryItem.UserID))
                {
                    response.AddErrorDescription(-1, "Failed to update monthly expense summary. ", $"User [{expenseSummaryItem.UserID}] NOT found.");
                    return response;
                }
                Monthlyexpense e = _unitOfWork.MonthlyExpenseRepo.Get(expenseSummaryItem.MonthlyExpenseID);
                if (e == null)
                {
                    response.AddErrorDescription(-1, "Failed to update monthly expense summary. ", $"Monthly expense summary item [{expenseSummaryItem.MonthlyExpenseID}] NOT found.");
                    return response;
                }

                e.Additionalremarks = expenseSummaryItem.AdditionalRemarks;
                e.Monthlypaymentstatusid = expenseSummaryItem.PaymentStatusID;
                e.Modifieddate = DateTime.Now;
                _unitOfWork.MonthlyExpenseRepo.Update(e);
                _unitOfWork.CommitChanges();
                response.Value = e.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                response.AddErrorDescription(-1, "Exception in updating Monthly Expense Summary.", $"Exception information: {e}");
            }
            
            return response;
        }

        private bool IsUserExist(long userID)
        {
            User u = _unitOfWork.UserRepo.Get(userID);
            return u == null ? false : true;
        }
    }
 
}
