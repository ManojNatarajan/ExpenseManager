using AutoMapper;
using Expenses.DAL.Models;
using Expenses.DAL.Repo;
using Expenses.Domain.Model.Models;
using GE = GoldenEagles.Logger;

namespace Expenses.Domain.Repo.Repository
{
    public interface IExpensepaymentstatusDomainRepository : IDomainRepositoryBase<ExpensepaymentstatusDTO, Expensepaymentstatus>
    {
    }

    public class ExpensepaymentstatusDomainRepository : DomainRepositoryBase<ExpensepaymentstatusDTO, Expensepaymentstatus>, IExpensepaymentstatusDomainRepository
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public ExpensepaymentstatusDomainRepository(IUnitOfWork unitOfWork, IMapper mapper, GE.ILogger logger) : base(unitOfWork, mapper, logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
