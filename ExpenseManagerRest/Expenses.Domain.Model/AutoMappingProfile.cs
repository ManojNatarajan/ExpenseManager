using AutoMapper;
using Expenses.DAL.Models;
using Expenses.Domain.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model
{
    
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // Name should be same for all properties while auto mapping, 
            // if not same it will not map automatically and we need to do manually 

            CreateMap<UserStatusDTO, Userstatus>().ReverseMap();
            CreateMap<RecurringintervaltypeDTO, Recurringintervaltype>().ReverseMap();
            CreateMap<MonthlypaymentstatusDTO, Monthlypaymentstatus>().ReverseMap();
            CreateMap<ExpensepaymentstatusDTO, Expensepaymentstatus>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            
            CreateMap<Expensetype, ExpenseTypeDTO>()
                .ForMember(dest => dest.User, source => source.Ignore()) //Ignoring EF Navigation Property to avoid circular dependency issue
                .ForMember(dest => dest.ExpenseEntries, source => source.Ignore()) //Ignoring EF Navigation Property to avoid circular dependency issue
                .ReverseMap();

            CreateMap<Monthlyexpense, MonthlyExpenseDTO>()
                .ForMember(dest => dest.User, source => source.Ignore()) //Ignoring EF Navigation Property to avoid circular dependency issue
                .ReverseMap();

            CreateMap<Expenseentry, ExpenseEntryDTO>()
                .ForMember(dest => dest.MonthlyExpense, source => source.Ignore()) //Ignoring EF Navigation Property to avoid circular dependency issue
                .ReverseMap();
        }
    }
}
