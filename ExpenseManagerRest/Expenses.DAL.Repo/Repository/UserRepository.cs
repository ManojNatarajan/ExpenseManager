using Expenses.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public interface IUserRepository<User> : IRepositoryBase<User> where User : class
    {
    }

    public class UserRepository : RepositoryBase<User>, IUserRepository<User>
    {
        private ExpenseManagerContext _dbContext;
        public UserRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }


}
