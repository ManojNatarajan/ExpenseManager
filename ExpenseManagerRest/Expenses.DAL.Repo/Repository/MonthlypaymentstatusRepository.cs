using Expenses.DAL.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public interface IMonthlypaymentstatusRepository<Monthlypaymentstatus> : IRepositoryBase<Monthlypaymentstatus> where Monthlypaymentstatus : class
    {
    }

    public class MonthlypaymentstatusRepository : RepositoryBase<Monthlypaymentstatus>, IMonthlypaymentstatusRepository<Monthlypaymentstatus>
    {
        private ExpenseManagerContext _dbContext;
        public MonthlypaymentstatusRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
