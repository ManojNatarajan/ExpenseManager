using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using System.Linq.Expressions;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IUserDomainRepository : IDomainRepositoryBase<UserDTO,User> 
    {
        DomainResponse<UserDTO> AuthenticateUserByMobileAndPassword(long mobile, string password);
        DomainResponse<UserDTO> AuthenticateUserBySocialUserId(string socialUserId);
        DomainResponse<UserDTO> CheckUserExits(string email, long mobile);
    }

    public class UserDomainRepository : DomainRepositoryBase<UserDTO, User>, IUserDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        private GE.ILogger _logger;

        public UserDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public DomainResponse<UserDTO> CheckUserExits(string email, long mobile)
        {
            DomainResponse<UserDTO> response = new DomainResponse<UserDTO>();
            try
            {
                Expression<Func<User, bool>> isUserExist = u => u.Email == email && u.Mobile == mobile && string.IsNullOrEmpty(u.Socialuserid);
                response.Value = base.Find(isUserExist)?.Value?.FirstOrDefault();
                if (response.Value != null)
                    response.AddErrorDescription(1, "User already exist!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in validate user. {e}");
                response.AddErrorDescription(-1, "Exception in CheckUserExits.", $"Exception information: {e}");
            }
            return response;
        }

        public DomainResponse<UserDTO> AuthenticateUserByMobileAndPassword(long mobile, string password)
        {
            DomainResponse<UserDTO> response = new DomainResponse<UserDTO>();
            try
            {
                Expression<Func<User, bool>> isUserAuthorized = u => u.Mobile == mobile && u.Password == password;
                response.Value = base.Find(isUserAuthorized)?.Value?.FirstOrDefault();
                if (response.Value == null)
                    response.AddErrorDescription(-1, "User not authorized!");
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception in AuthenticateUserByMobileAndPassword. {e}");
                response.AddErrorDescription(-1, "Exception in AuthenticateUserByMobileAndPassword.", $"Exception information: {e}");
            }
            return response;
        }

        public DomainResponse<UserDTO> AuthenticateUserBySocialUserId(string socialUserId)
        {
            DomainResponse<UserDTO> response = new DomainResponse<UserDTO>();
            try
            {
                Expression<Func<User, bool>> isUserAuthorized = u => u.Socialuserid == socialUserId;
                response.Value = base.Find(isUserAuthorized)?.Value?.FirstOrDefault();
                if (response.Value == null)
                    response.AddErrorDescription(-1, "User not authorized!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception in AuthenticateUserBySocialUserId. {e}");
                response.AddErrorDescription(-1, "Exception in AuthenticateUserBySocialUserId.", $"Exception information: {e}");
            }
            return response;
        }
    }
}
