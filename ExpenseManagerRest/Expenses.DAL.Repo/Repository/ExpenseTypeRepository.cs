using Expenses.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{
    public interface IExpenseTypeRepository<Expensetype> : IRepositoryBase<Expensetype> where Expensetype : class
    {
    }

    public class ExpenseTypeRepository : RepositoryBase<Expensetype>, IExpenseTypeRepository<Expensetype>
    {
        private ExpenseManagerContext _dbContext;
        public ExpenseTypeRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
