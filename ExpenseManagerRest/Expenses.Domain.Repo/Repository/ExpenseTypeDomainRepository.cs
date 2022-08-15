using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using System.Linq.Expressions;
using System.Reflection;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{

    public interface IExpenseTypeDomainRepository : IDomainRepositoryBase<ExpenseTypeDTO, Expensetype>
    {
        DomainResponse<List<ExpenseTypeDTO>> GetExpenseTypesForUser(long userID);
        DomainResponse<ExpenseTypeDTO> CheckExpenseTypeExists(long? userId, string description);
    }

    public class ExpenseTypeDomainRepository : DomainRepositoryBase<ExpenseTypeDTO, Expensetype>, IExpenseTypeDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly GE.ILogger _logger;

        public ExpenseTypeDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : 
            base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public DomainResponse<List<ExpenseTypeDTO>> GetExpenseTypesForUser(long userID)
        {
            DomainResponse<List<ExpenseTypeDTO>> response = new DomainResponse<List<ExpenseTypeDTO>>();
            try
            {
                User u = _unitOfWork.UserRepo.Get(userID);
                if (u == null)
                {
                    response.AddErrorDescription(-1, "Failed to get Expense Type list for user. ", "User NOT found.");
                    return response;
                }

                Expression<Func<Expensetype, bool>> isUserHasExpenseTypes = u => u.Userid == userID;
                response.Value = base.Find(isUserHasExpenseTypes)?.Value?.ToList();
                if (response.Value == null || !response.Value.Any())
                    response.AddErrorDescription(-1, "Failed to get Expense Type list for user. ", $"No entries found for User [{userID}]!");
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception in GetExpenseTypesForUser. {e}");
                response.AddErrorDescription(-1, "Exception in GetExpenseTypesForUser.", $"Exception information: {e}");
            }
            return response;
        }
        public DomainResponse<ExpenseTypeDTO> CheckExpenseTypeExists(long? userId, string description)
        {
            DomainResponse<ExpenseTypeDTO> response = new DomainResponse<ExpenseTypeDTO>();
            try
            {
                Expression<Func<Expensetype, bool>> isUserExist = u => u.Userid == userId.Value && u.Description == description;
                response.Value = base.Find(isUserExist)?.Value?.FirstOrDefault();
                if (response.Value != null)
                    response.AddErrorDescription(1, "Expense type already exist!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in validate expense type. {e}");
                response.AddErrorDescription(-1, "Exception in CheckExpenseTypeExists.", $"Exception information: {e}");
            }
            return response;
        }

        public override DomainResponse<long> AddEntity(ExpenseTypeDTO entityDTO)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                returnValue = Validate(entityDTO);
                if (!returnValue.IsSuccess)
                    return returnValue;

                var entity = _mapper.Map<Expensetype>(entityDTO);
                _unitOfWork.ExpenseTypeRepo.Add(entity);
                var count = _unitOfWork.CommitChanges();

                object idValue = GetPropertyValue(entity, "Id");
                returnValue.Value = idValue != null ? (long)idValue : 0;
                return returnValue;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in adding Expense Type. {e}");
                returnValue.AddErrorDescription(-1, "Exception in adding Expense Type.", $"Exception information: {e}");
            }
            return returnValue;
        }

        public override DomainResponse<long> UpdateEntity(ExpenseTypeDTO entityDTO)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                returnValue = Validate(entityDTO);
                if (!returnValue.IsSuccess)
                    return returnValue;

                var entity = _mapper.Map<Expensetype>(entityDTO);
                _unitOfWork.ExpenseTypeRepo.Update(entity);
                var count = _unitOfWork.CommitChanges();

                object idValue = GetPropertyValue(entity, "Id");
                returnValue.Value = idValue != null ? (long)idValue : 0;
                return returnValue;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in adding Expense Type. {e}");
                returnValue.AddErrorDescription(-1, "Exception in updating Expense Type.", $"Exception information: {e}");
            }
            return returnValue;
        }

        private DomainResponse<long> Validate(ExpenseTypeDTO entityDTO)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            if (entityDTO == null)
            {
                returnValue.AddErrorDescription(-1, "Failed to Create Expense Type. ", "Expense Type not passed as input.");
                return returnValue;
            }
            //User Validation
            User user = _unitOfWork.UserRepo.Get(entityDTO.UserId);
            if (user == null || user.Id <= 0)
            {
                returnValue.AddErrorDescription(-1, "Failed to Create Expense Type. ", "User NOT exists!.");
                return returnValue;
            }

            return returnValue;
        }

        private object GetPropertyValue(Expensetype? entity, string propertyName)
        {
            PropertyInfo prop = entity?.GetType()?.GetProperty(propertyName);
            return prop?.GetValue(entity);
        }
    }
    
}
