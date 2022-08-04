using AutoMapper;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IDomainRepositoryBase<TEntity,TEntityDB> 
        where TEntity : class
        where TEntityDB : class
    {
        DomainResponse<IEnumerable<TEntity>> Find(Expression<Func<TEntityDB, bool>> predicate);
        DomainResponse<List<TEntity>> GetAllEntityList();
        DomainResponse<TEntity> GetEntityById(long id);
        DomainResponse<long> AddEntity(TEntity entityDTO);
        DomainResponse<long> UpdateEntity(TEntity entityDTO);
        DomainResponse<bool> DeleteEntity(long id);
    }

    public class DomainRepositoryBase<TEntity,TEntityDB> : IDomainRepositoryBase<TEntity,TEntityDB> 
        where TEntity : class
        where TEntityDB : class
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        private dynamic _repo;
        private GE.ILogger _logger;

        public DomainRepositoryBase(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = GetRepo();
        }

        public virtual DomainResponse<IEnumerable<TEntity>> Find(Expression<Func<TEntityDB, bool>> predicate)
        {
            DomainResponse<IEnumerable<TEntity>> response = new DomainResponse<IEnumerable<TEntity>>();
            try
            {
                _logger.LogInfo(_repo.GetType() + "(" + typeof(TEntity) + ")" + ":Find");
                List<TEntityDB> data = _repo.Find(predicate);
                response.Value = _mapper.Map<List<TEntity>>(data);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in Find. {e}");
                response.AddErrorDescription(-1, "Exception in Find.", $"Exception information: {e}");
            }
            return response;
        }

        public virtual DomainResponse<List<TEntity>> GetAllEntityList()
        {
            DomainResponse<List<TEntity>> response = new DomainResponse<List<TEntity>>();
            try
            {
                _logger.LogInfo(_repo.GetType() + "(" + typeof(TEntity) + ")" + ":GetAllEntityList");
                var data = _repo.GetAll();
                response.Value = _mapper.Map<List<TEntity>>(data);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in GetAllEntityList. {e}");
                response.AddErrorDescription(-1, "Exception in GetAllEntityList.", $"Exception information: {e}");
            }
            return response;
        }

        public virtual DomainResponse<TEntity> GetEntityById(long id)
        {
            DomainResponse<TEntity> response = new DomainResponse<TEntity>();
            try
            {
                var data = _repo.Get(id);
                response.Value = _mapper.Map<TEntity>(data);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in GetEntityById. {e}");
                response.AddErrorDescription(-1, "Exception in GetEntityById.", $"Exception information: {e}");
            }
            return response;
        }

        public virtual DomainResponse<long> AddEntity(TEntity entityDTO)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                #region validation
                if (entityDTO == null)
                {
                    returnValue.AddErrorDescription(-1, "Failed to Create User. ", "User not passed as input.");
                    return returnValue;
                }
                #endregion

                var entity = _mapper.Map<TEntityDB>(entityDTO);
                _repo.Add(entity);
                var count = _unitOfWork.CommitChanges();

                PropertyInfo prop = entity?.GetType()?.GetProperty("Id");
                object idValue = prop?.GetValue(entity);

                returnValue.Value = idValue != null ? (long)idValue : 0;
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception in AddEntity. {e}");
                returnValue.AddErrorDescription(-1, "Exception in AddEntity.", $"Exception information: {e}");
            }
            return returnValue;
        }

        public virtual DomainResponse<long> UpdateEntity(TEntity entityDTO)
        {
            DomainResponse<long> returnValue = new DomainResponse<long>();
            try
            {
                #region validation
                if (entityDTO == null)
                {
                    returnValue.AddErrorDescription(-1, "Failed to Update User. ", "User not passed as input.");
                    return returnValue;
                }
                #endregion

                var entity = _mapper.Map<TEntityDB>(entityDTO);
                _repo.Update(entity);
                var count = _unitOfWork.CommitChanges();

                PropertyInfo prop = entity?.GetType()?.GetProperty("Id");
                object idValue = prop?.GetValue(entity);

                returnValue.Value = idValue != null ? (long)idValue : 0;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in UpdateEntity. {e}");
                returnValue.AddErrorDescription(-1, "Exception in UpdateEntity.", $"Exception information: {e}");
            }
            return returnValue;
        }

        public virtual DomainResponse<bool> DeleteEntity(long id)
        {
            DomainResponse<bool> response = new DomainResponse<bool>();
            try
            {
                var entity = _repo.Get(id);
                _repo.Remove(entity);
                var count = _unitOfWork.CommitChanges();
                response.Value = true;
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception in DeleteEntity. {e}");
                response.AddErrorDescription(-1, "Exception in DeleteEntity.", $"Exception information: {e}");
            }
            return response;
        }

        private dynamic GetRepo()
        {
            if (typeof(TEntity) == typeof(UserStatusDTO))
                return _unitOfWork.UserStatusRepo;
            else if (typeof(TEntity) == typeof(RecurringintervaltypeDTO))
                return _unitOfWork.RecurringIntervalTypeRepo;
            else if (typeof(TEntity) == typeof(MonthlypaymentstatusDTO))
                return _unitOfWork.MonthlyPaymentStatusRepo;
            else if (typeof(TEntity) == typeof(ExpensepaymentstatusDTO))
                return _unitOfWork.ExpensePaymentStatusRepo;
            else if (typeof(TEntity) == typeof(UserDTO))
                return _unitOfWork.UserRepo;
            else if(typeof(TEntity) == typeof(ExpenseTypeDTO))
                return _unitOfWork.ExpenseTypeRepo;
            else if(typeof(TEntity) == typeof(MonthlyExpenseDTO))
                return _unitOfWork.MonthlyExpenseRepo;
            else if (typeof(TEntity) == typeof(ExpenseEntryDTO))
                return _unitOfWork.ExpenseEntryRepo;
            else
                return null;
        }

    }
}
