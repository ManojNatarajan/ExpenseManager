using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models
{
    public class UserDTO
    {
        public UserDTO()
        {
            ExpenseTypes = new HashSet<ExpenseTypeDTO>();
            MonthlyExpenses = new HashSet<MonthlyExpenseDTO>();
        }

        public long? Id { get; set; }
        public long? Mobile { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Userstatusid { get; set; }
        public string? Socialuserid { get; set; }
        public string? Socialprovider { get; set; }
        public bool? Isverified { get; set; }
        public bool? Accepttandc { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }

        public ICollection<ExpenseTypeDTO> ExpenseTypes { get; set; }
        public ICollection<MonthlyExpenseDTO> MonthlyExpenses { get; set; }
    }
}
