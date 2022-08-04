using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models
{
    public class ExpenseTypeDTO
    {
        public ExpenseTypeDTO()
        {
            ExpenseEntries = new HashSet<ExpenseEntryDTO>();
        }
        public long? Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Defaultdueamount { get; set; }
        public int Defaultduedateinmonth { get; set; }
        public bool? Isrecurring { get; set; }
        public int? Recurringintervaltypeid { get; set; }
        public bool? IsActive { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }

        public UserDTO? User { get; set; }
        public ICollection<ExpenseEntryDTO>? ExpenseEntries { get; set; }
    }
}
