using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using Expenses.Domain.Model.Models.API;
using System.Linq.Expressions;
using System.Reflection;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    
    public interface IExpenseEntryDomainRepository : IDomainRepositoryBase<ExpenseEntryDTO, Expenseentry>
    {
        DomainResponse<List<ExpenseEntryDTO>> GetExpenseEntriesForMonth(long monthlyExpenseId);
        DomainResponse<long> AddExpenseEntry(ExpenseEntryContract expenseEntryContract);
        DomainResponse<long> UpdateExpenseEntry(ExpenseEntryContract expenseItem);
        DomainResponse<bool> DeleteExpenseEntry(long expenseEntryId);
        DomainResponse<List<ExpenseEntryDTO>> GetExpenseEntriesForMonthYear(long userId, int month, int year);
    }

    public class ExpenseEntryDomainRepository : DomainRepositoryBase<ExpenseEntryDTO, Expenseentry>, IExpenseEntryDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly GE.ILogger _logger;

        public ExpenseEntryDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) :
            base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public DomainResponse<List<ExpenseEntryDTO>> GetExpenseEntriesForMonth(long monthlyExpenseId)
        {
            DomainResponse<List<ExpenseEntryDTO>> response = new DomainResponse<List<ExpenseEntryDTO>>();
            try
            {
                Monthlyexpense e = _unitOfWork.MonthlyExpenseRepo.Get(monthlyExpenseId);
                if (e == null)
                {
                    response.AddErrorDescription(-1, "Failed to Get Expense Entries For Month. ", $"Monthly expense summary item [{monthlyExpenseId}] NOT found.");
                    return response;
                }
                Expression<Func<Expenseentry, bool>> isMonthExpenseEntriesExists = u => u.Monthlyexpenseid == monthlyExpenseId;
                response.Value = base.Find(isMonthExpenseEntriesExists)?.Value?.ToList();
                if (response.Value == null || !response.Value.Any())
                    response.AddErrorDescription(-1, $"Expense Entries NOT found For Month: [{monthlyExpenseId}]");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                response.AddErrorDescription(-1, "Exception in updating Monthly Expense Summary.", $"Exception information: {e}");
            }
            return response;
        }


        public DomainResponse<List<ExpenseEntryDTO>> GetExpenseEntriesForMonthYear(long userId, int month, int year)
        {
            DomainResponse<List<ExpenseEntryDTO>> response = new DomainResponse<List<ExpenseEntryDTO>>();
            try
            {
                if (!IsUserExist(userId))
                {
                    response.AddErrorDescription(-1, "Failed to get Monthly Expense Summary list for user. ", $"User [{userId}] NOT found.");
                    return response;
                }
                Expression<Func<Monthlyexpense, bool>> isUserHasMonthlyExpenses = u => u.Userid == userId && u.Billmonth == month && u.Billyear == year;
                Monthlyexpense mSummary = _unitOfWork.MonthlyExpenseRepo.Find(isUserHasMonthlyExpenses).FirstOrDefault();
                if (mSummary == null)
                {
                    response.AddErrorDescription(-1, "Failed to Get Expense Entries For Month and Year. ", $"Monthly expense summary was NOT found.");
                    return response;
                }

                Expression<Func<Expenseentry, bool>> isMonthExpenseEntriesExists = u => u.Monthlyexpenseid == mSummary.Id;
                response.Value = base.Find(isMonthExpenseEntriesExists)?.Value?.ToList();
                if (response.Value == null || !response.Value.Any())
                    response.AddErrorDescription(-1, "Failed to get Monthly Expense Summary list for user. ", $"No entries found for User [{userId}]!");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                response.AddErrorDescription(-1, "Exception in getting Monthly Expense Summary.", $"Exception information: {e}");
            }
            return response;
        }

        public DomainResponse<long> AddExpenseEntry(ExpenseEntryContract expenseItem)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                if(!Validate(expenseItem).IsSuccess)
                    return returnValue;

                //Step 1: Get Monthly Summary (If not exist for bill month, then create one)
                Monthlyexpense monthSummary = GetMonthlyExpenseSummary(expenseItem);
                if (monthSummary == null)
                {
                    monthSummary = new Monthlyexpense();
                    monthSummary.Userid = expenseItem.UserID;
                    monthSummary.Billmonth = expenseItem.BillMonth;
                    monthSummary.Billyear = expenseItem.BillYear;
                    monthSummary.Monthlypaymentstatusid = 2; //default is unpaid
                }
                              

                //Step 2: Update MonthlyExpense Amount Fields as per the Expense Entry Received. 
                monthSummary.Paidamount += expenseItem.ExpenseEntry.Paymentamount;
                if (expenseItem.ExpenseEntry.Issplittedpayment)
                {
                    decimal prevSplitDueAmount = 0;
                    decimal? totalPaidAmount = 0;
                    if (monthSummary.Expenseentries != null && monthSummary.Expenseentries.Any())
                    {
                        var e = monthSummary.Expenseentries.Where(x =>
                            x.Expensetypeid == expenseItem.ExpenseEntry.ExpenseTypeId
                        )?.MaxBy(x => x.Id);

                        if (e != null)
                            prevSplitDueAmount = e.Dueamount;

                        totalPaidAmount = monthSummary.Expenseentries.Where(x =>
                            x.Expensetypeid == expenseItem.ExpenseEntry.ExpenseTypeId
                        )?.Sum(x => x.Paymentamount);
                    }

                    monthSummary.Totalamount -= prevSplitDueAmount;
                    monthSummary.Totalamount += expenseItem.ExpenseEntry.Dueamount;

                    //monthSummary.Dueamount -= prevSplitDueAmount;
                    //var paidAmount = expenseItem.ExpenseEntry.Paymentamount + totalPaidAmount;
                    //var amountToBeMinus = paidAmount >= expenseItem.ExpenseEntry.Dueamount ? expenseItem.ExpenseEntry.Dueamount : paidAmount.Value;
                    //monthSummary.Dueamount = expenseItem.ExpenseEntry.Dueamount - amountToBeMinus;
                }
                else
                {
                    monthSummary.Totalamount += expenseItem.ExpenseEntry.Dueamount;
                }
                monthSummary.Dueamount = monthSummary.Totalamount - monthSummary.Paidamount;

                monthSummary.Monthlypaymentstatusid = GetMonthlyPaymentStatus(monthSummary);
                if (monthSummary.Monthlypaymentstatusid == -1)
                    throw new Exception("Error in Monthly Summary calculation! Due amount cannot be greater than total amount.");
                monthSummary.Modifieddate = DateTime.Now;

                //Step 3: Add ExpenseEntry finally. This wil add/update MonthlyExpense table accordingly
                Expenseentry expenseEntry = _mapper.Map<Expenseentry>(expenseItem.ExpenseEntry);
                expenseEntry.Monthlyexpense = monthSummary;
                _unitOfWork.ExpenseEntryRepo.Add(expenseEntry);

                //Step 4: Commit changes
                var count = _unitOfWork.CommitChanges();
                returnValue.Value = expenseEntry.Id;

                return returnValue;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in adding Expense entry. {e}");
                returnValue.AddErrorDescription(-1, "Exception in adding Expense entry.", $"Exception information: {e}");
            }
            return returnValue;
        }

        public DomainResponse<long> UpdateExpenseEntry(ExpenseEntryContract expenseItem)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                if (!Validate(expenseItem, true).IsSuccess)
                    return returnValue;

                //Step 1: Get Expense Entry and existing Amount fields
                Expenseentry e = _unitOfWork.ExpenseEntryRepo.Get(expenseItem.ExpenseEntry.Id);
                decimal oldDueAmount = e.Dueamount;
                decimal oldPaymentAmount = e.Paymentamount;

                //Step 2: Update Expense Entry Fields with Updated values.
                ExpenseEntryDTO eDto = expenseItem.ExpenseEntry;
                e.Expensetypeid = eDto.ExpenseTypeId;
                e.Duedate = eDto.Duedate;
                e.Dueamount = eDto.Dueamount;
                e.Paymentamount = eDto.Paymentamount;
                e.Paymentdate = eDto.Paymentdate;
                e.Expensepaymentstatusid = eDto.Expensepaymentstatusid;
                e.Additionalremarks = eDto.Additionalremarks;
                e.Modifieddate = DateTime.Now;

                //Step 2: Update Monthly Expense Summary with updated amounts & payment status
                e.Monthlyexpense.Totalamount -= oldDueAmount;
                e.Monthlyexpense.Paidamount -= oldPaymentAmount; 
                
                e.Monthlyexpense.Totalamount += expenseItem.ExpenseEntry.Dueamount;
                e.Monthlyexpense.Paidamount += expenseItem.ExpenseEntry.Paymentamount;
                e.Monthlyexpense.Dueamount = e.Monthlyexpense.Totalamount - e.Monthlyexpense.Paidamount;
                e.Monthlyexpense.Monthlypaymentstatusid = GetMonthlyPaymentStatus(e.Monthlyexpense);
                if (e.Monthlyexpense.Monthlypaymentstatusid == -1)
                    throw new Exception("Error in Monthly Summary calculation! Due amount cannot be greater than total amount.");
                e.Monthlyexpense.Modifieddate = DateTime.Now;

                //Step 3: Update Expense Entry, Monthly Expense and Commit changes. 
                _unitOfWork.ExpenseEntryRepo.Update(e);
                var count = _unitOfWork.CommitChanges();

                returnValue.Value = e.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in updating Expense Entry. {e}");
                returnValue.AddErrorDescription(-1, "Exception in updating Expense Entry.", $"Exception information: {e}");
            }
            return returnValue;
        }

        public DomainResponse<bool> DeleteExpenseEntry(long expenseEntryId)
        {
            DomainResponse<bool> returnValue = new DomainResponse<bool>();
            try
            {
                //Validation
                Expenseentry e = _unitOfWork.ExpenseEntryRepo.Get(expenseEntryId);
                if (e == null)
                {
                    returnValue.AddErrorDescription(-1, "Failed to Delete expense entry. ", "ExpenseEntry NOT exists!.");
                    return returnValue;
                }
                
                //Step 1: Load Existing Amount values and delete Expense Entry
                decimal oldDueAmount = e.Dueamount;
                decimal oldPaymentAmount = e.Paymentamount;
                long monthlySummaryId = e.Monthlyexpenseid;                
                long expenseTypeId = e.Expensetypeid;
                _unitOfWork.ExpenseEntryRepo.Remove(e);

                //Step 2: Check if Month Summary has other Expense Entries. If yes, then update Amounts accordingly else delete Month Summary also
                Monthlyexpense mSummary = _unitOfWork.MonthlyExpenseRepo.Get(monthlySummaryId); 
                Expression<Func<Expenseentry, bool>> isMonthExpenseEntriesExists = u => u.Monthlyexpenseid == monthlySummaryId;
                var expEntriesList = base.Find(isMonthExpenseEntriesExists)?.Value?.ToList();
                if(expEntriesList == null || expEntriesList.Count - 1 == 0) //-1 since above ExpenseEntry Removal is NOT yet committed to DB at this point. 
                {
                    _unitOfWork.MonthlyExpenseRepo.Remove(mSummary);
                }
                else
                {
                    mSummary.Paidamount -= oldPaymentAmount;
                    if (e.Issplittedpayment)
                    {
                        decimal prevSplitDueAmount = 0;
                        decimal? totalPaidAmount = 0;
                        if (mSummary.Expenseentries != null && mSummary.Expenseentries.Any())
                        {
                            var expenseEntry = mSummary.Expenseentries.Where(x =>
                                x.Expensetypeid == expenseTypeId
                            )?.MaxBy(x => x.Id);

                            if (e != null)
                                prevSplitDueAmount = e.Dueamount;

                            totalPaidAmount = mSummary.Expenseentries.Where(x =>
                                x.Expensetypeid == expenseTypeId
                            )?.Sum(x => x.Paymentamount);
                        }
                        decimal balanceAmount = totalPaidAmount.Value - oldPaymentAmount;
                        if (balanceAmount == 0)
                            mSummary.Totalamount -= oldDueAmount;
                        mSummary.Dueamount += balanceAmount;
                    }
                    else
                    {
                        mSummary.Totalamount -= oldDueAmount;
                        mSummary.Dueamount += mSummary.Paidamount;
                    }
                    mSummary.Monthlypaymentstatusid = GetMonthlyPaymentStatus(mSummary);
                    if (mSummary.Monthlypaymentstatusid == -1)
                        throw new Exception("Error in Monthly Summary calculation! Due amount cannot be greater than total amount.");
                    mSummary.Modifieddate = DateTime.Now;
                    _unitOfWork.MonthlyExpenseRepo.Update(mSummary);
                }

                //Finally Commit DB changes
                _unitOfWork.CommitChanges();
                returnValue.Value = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in Delete Expense Entry. {e}");
                returnValue.AddErrorDescription(-1, "Exception in deleting Expense Entry.", $"Exception information: {e}");
            }
            return returnValue;
        }

        #region Private methods
        private DomainResponse<long> Validate(ExpenseEntryContract expenseEntryContract, bool isUpdateRequest = false)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            if (expenseEntryContract == null || expenseEntryContract.ExpenseEntry == null)
            {
                returnValue.AddErrorDescription(-1, "Failed to add expense entry. ", "Expense entry is null.");
                return returnValue;
            }
            if(isUpdateRequest)
            {
                Expenseentry e = _unitOfWork.ExpenseEntryRepo.Get(expenseEntryContract.ExpenseEntry.Id);
                if (e == null)
                {
                    returnValue.AddErrorDescription(-1, "Failed to add expense entry. ", "ExpenseEntry NOT exists!.");
                    return returnValue;
                }
            }
            

            //User Validation
            User user = _unitOfWork.UserRepo.Get(expenseEntryContract.UserID);
            if (user == null || user.Id <= 0)
            {
                returnValue.AddErrorDescription(-1, "Failed to add expense entry. ", "User NOT exists!.");
                return returnValue;
            }
            //Rest of validation goes here...
            return returnValue;
        }

        private Monthlyexpense GetMonthlyExpenseSummary(ExpenseEntryContract expenseItem)
        {
            Expression<Func<Monthlyexpense, bool>> isBillSummaryExists =
                m => m.Userid == expenseItem.UserID
                && m.Billmonth == expenseItem.BillMonth
                && m.Billyear == expenseItem.BillYear;

            Monthlyexpense m = _unitOfWork.MonthlyExpenseRepo.Find(isBillSummaryExists).FirstOrDefault();
            return m;
        }
                
        private int GetMonthlyPaymentStatus(Monthlyexpense monthSummary)
        {
            if (monthSummary.Dueamount == 0)
                return 1; //means no pending amount & all paid
            else if (monthSummary.Dueamount == monthSummary.Totalamount)
                return 2; //means Unpaid as full amount is due
            else if (monthSummary.Dueamount < monthSummary.Totalamount)
                return 3; //means Partial payment made
            else
                return -1;
        }

        private object GetPropertyValue(Expenseentry? entity, string propertyName)
        {
            PropertyInfo prop = entity?.GetType()?.GetProperty(propertyName);
            return prop?.GetValue(entity);
        }

        private bool IsUserExist(long userID)
        {
            User u = _unitOfWork.UserRepo.Get(userID);
            return u == null ? false : true;
        }

        #endregion

    }

}
