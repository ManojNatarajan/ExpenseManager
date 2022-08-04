using System.ComponentModel.DataAnnotations;

namespace Expenses.Domain.Model.Models.API
{
    public class ExpenseEntryContract
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "UserID is invalid.")]
        public long UserID { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "BillMonth is invalid.")]
        public int BillMonth { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "BillYear is invalid.")]
        public int BillYear { get; set; }
        
        [Required(ErrorMessage ="Expense Entry is required.")]
        public ExpenseEntryDTO ExpenseEntry { get; set; }
    }
}
