using Expenses.DAL.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{    
    public interface IExpensepaymentstatusRepository<Expensepaymentstatus> : IRepositoryBase<Expensepaymentstatus> where Expensepaymentstatus : class
    {
    }

    public class ExpensepaymentstatusRepository : RepositoryBase<Expensepaymentstatus>, IExpensepaymentstatusRepository<Expensepaymentstatus>
    {
        private ExpenseManagerContext _dbContext;
        public ExpensepaymentstatusRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
