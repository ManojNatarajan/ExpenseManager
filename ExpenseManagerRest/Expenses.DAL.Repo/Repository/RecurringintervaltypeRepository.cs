using Expenses.DAL.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public interface IRecurringintervaltypeRepository<Recurringintervaltype> : IRepositoryBase<Recurringintervaltype> where Recurringintervaltype : class
    {
    }

    public class RecurringintervaltypeRepository : RepositoryBase<Recurringintervaltype>, IRecurringintervaltypeRepository<Recurringintervaltype>
    {
        private ExpenseManagerContext _dbContext;
        public RecurringintervaltypeRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
