using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IRecurringintervaltypeDomainRepository : IDomainRepositoryBase<RecurringintervaltypeDTO, Recurringintervaltype>
    {
    }

    public class RecurringintervaltypeDomainRepository : DomainRepositoryBase<RecurringintervaltypeDTO, Recurringintervaltype>, IRecurringintervaltypeDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public RecurringintervaltypeDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
