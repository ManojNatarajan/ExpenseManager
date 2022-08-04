using Expenses.DAL.Models;
using Expenses.DAL.Repo.Repository;
using GE = GoldenEagles.Logger;

namespace Expenses.DAL.Repo
{
    public interface IUnitOfWork
    {
        IUserStatusRepository<Userstatus> UserStatusRepo { get; }
        IRecurringintervaltypeRepository<Recurringintervaltype> RecurringIntervalTypeRepo { get; }
        IMonthlypaymentstatusRepository<Monthlypaymentstatus> MonthlyPaymentStatusRepo { get; }
        IExpensepaymentstatusRepository<Expensepaymentstatus> ExpensePaymentStatusRepo { get; }
        IUserRepository<User> UserRepo { get; }
        IExpenseTypeRepository<Expensetype> ExpenseTypeRepo { get; }
        IMonthlyExpenseRepository<Monthlyexpense> MonthlyExpenseRepo { get; }
        IExpenseEntryRepository<Expenseentry> ExpenseEntryRepo { get; }                
        Task<int> CommitChangesAsync();
        int CommitChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        readonly ExpenseManagerContext _context;
        IUserStatusRepository<Userstatus> _userStatusRepo;
        IRecurringintervaltypeRepository<Recurringintervaltype> _recurringIntervalTypeRepo;
        IMonthlypaymentstatusRepository<Monthlypaymentstatus> _monthlyPaymentStatusRepo;
        IExpensepaymentstatusRepository<Expensepaymentstatus> _expensePaymentStatusRepo;
        IUserRepository<User> _usersRepo;
        IExpenseTypeRepository<Expensetype> _expenseTypeRepo;
        IMonthlyExpenseRepository<Monthlyexpense> _monthlyExpenseRepo;
        IExpenseEntryRepository<Expenseentry> _expenseEntryRepo;
        private GE.ILogger _logger;

        public UnitOfWork(ExpenseManagerContext context, GE.ILogger logger)
        {
            _logger = logger;
            _context = context;
        }


        #region enum repos

        public IUserStatusRepository<Userstatus> UserStatusRepo 
        { 
            get
            {
                if (_userStatusRepo == null)
                    _userStatusRepo = new UserStatusRepository(_context, _logger);
                return _userStatusRepo;
            }
        }

        public IRecurringintervaltypeRepository<Recurringintervaltype> RecurringIntervalTypeRepo
        {
            get
            {
                if (_recurringIntervalTypeRepo == null)
                    _recurringIntervalTypeRepo = new RecurringintervaltypeRepository(_context, _logger);
                return _recurringIntervalTypeRepo;
            }
        }

        public IMonthlypaymentstatusRepository<Monthlypaymentstatus> MonthlyPaymentStatusRepo
        {
            get
            {
                if (_monthlyPaymentStatusRepo == null)
                    _monthlyPaymentStatusRepo = new MonthlypaymentstatusRepository(_context, _logger);
                return _monthlyPaymentStatusRepo;
            }
        }
        public IExpensepaymentstatusRepository<Expensepaymentstatus> ExpensePaymentStatusRepo
        {
            get
            {
                if (_expensePaymentStatusRepo == null)
                    _expensePaymentStatusRepo = new ExpensepaymentstatusRepository(_context, _logger);
                return _expensePaymentStatusRepo;
            }
        }

        #endregion

        public IUserRepository<User> UserRepo
        {
            get
            {
                if (_usersRepo == null)
                    _usersRepo = new UserRepository(_context, _logger);
                return _usersRepo;
            }
        }
        public IExpenseTypeRepository<Expensetype> ExpenseTypeRepo
        {
            get
            {
                if (_expenseTypeRepo == null)
                    _expenseTypeRepo = new ExpenseTypeRepository(_context, _logger);
                return _expenseTypeRepo;
            }
        }
        public IMonthlyExpenseRepository<Monthlyexpense> MonthlyExpenseRepo
        {
            get
            {
                if (_monthlyExpenseRepo == null)
                    _monthlyExpenseRepo = new MonthlyExpenseRepository(_context, _logger);
                return _monthlyExpenseRepo;
            }
        }
        public IExpenseEntryRepository<Expenseentry> ExpenseEntryRepo
        {
            get
            {
                if (_expenseEntryRepo == null)
                    _expenseEntryRepo = new ExpenseEntryRepository(_context, _logger);
                return _expenseEntryRepo;
            }
        }
                
        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public int CommitChanges()
        {
            return _context.SaveChanges();
        }
    }
}