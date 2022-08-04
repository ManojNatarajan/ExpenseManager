using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IUserStatusDomainRepository : IDomainRepositoryBase<UserStatusDTO, Userstatus>
    {
    }

    public class UserStatusDomainRepository : DomainRepositoryBase<UserStatusDTO, Userstatus>, IUserStatusDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public UserStatusDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
