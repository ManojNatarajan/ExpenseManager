using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IMonthlypaymentstatusDomainRepository : IDomainRepositoryBase<MonthlypaymentstatusDTO, Monthlypaymentstatus>
    {
    }

    public class MonthlypaymentstatusDomainRepository : DomainRepositoryBase<MonthlypaymentstatusDTO, Monthlypaymentstatus>, IMonthlypaymentstatusDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public MonthlypaymentstatusDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
