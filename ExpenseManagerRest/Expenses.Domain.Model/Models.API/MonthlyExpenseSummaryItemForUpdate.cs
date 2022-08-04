using System.ComponentModel.DataAnnotations;

namespace Expenses.Domain.Model.Models.API
{
    public class MonthlyExpenseSummaryItemForUpdate
    {
        [Range(1, Int64.MaxValue, ErrorMessage = "UserID is invalid.")]
        public long UserID { get; set; }

        [Range(1, Int64.MaxValue, ErrorMessage = "MonthlyExpenseID is invalid.")]
        public long MonthlyExpenseID { get; set; }

        [StringLength(300, ErrorMessage = "AdditionalRemarks should not exceed 300 characters.")]
        public string? AdditionalRemarks { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "PaymentStatusID is invalid.")]
        public int PaymentStatusID { get; set; }
    }
}
