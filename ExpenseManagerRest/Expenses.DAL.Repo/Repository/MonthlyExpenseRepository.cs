using Expenses.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo.Repository
{   
    public interface IMonthlyExpenseRepository<Monthlyexpense> : IRepositoryBase<Monthlyexpense> where Monthlyexpense : class
    { }

    public class MonthlyExpenseRepository : RepositoryBase<Monthlyexpense>, IMonthlyExpenseRepository<Monthlyexpense>
    {
        private ExpenseManagerContext _dbContext;
        public MonthlyExpenseRepository(ExpenseManagerContext dbContext, GE.ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
    }
}
