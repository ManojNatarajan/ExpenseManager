using Expenses.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{

    public interface IExpenseEntryRepository<Expenseentry> : IRepositoryBase<Expenseentry> where Expenseentry : class
    { }

    public class ExpenseEntryRepository : RepositoryBase<Expenseentry>, IExpenseEntryRepository<Expenseentry>
    {
        private ExpenseManagerContext _dbContext;
        public ExpenseEntryRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }

}
