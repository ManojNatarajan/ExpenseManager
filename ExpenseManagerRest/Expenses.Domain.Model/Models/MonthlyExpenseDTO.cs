using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models
{
    public class MonthlyExpenseDTO
    {
        public MonthlyExpenseDTO()
        {
            ExpenseEntries = new HashSet<ExpenseEntryDTO>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public int Billmonth { get; set; }
        public int Billyear { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int Monthlypaymentstatusid { get; set; }
        public string? Additionalremarks { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }

        public UserDTO User { get; set; } = null!;
        public ICollection<ExpenseEntryDTO> ExpenseEntries { get; set; }
    }
}
