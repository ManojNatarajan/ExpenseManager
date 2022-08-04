using Expenses.DAL.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public interface IUserStatusRepository<Userstatus> : IRepositoryBase<Userstatus> where Userstatus : class
    {        
    }

    public class UserStatusRepository : RepositoryBase<Userstatus>, IUserStatusRepository<Userstatus>
    {
        private ExpenseManagerContext _dbContext;
        public UserStatusRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
