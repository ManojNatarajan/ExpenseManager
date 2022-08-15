using Expenses.Domain.Model.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models
{
    public class ExpenseEntryDTO
    {
        public long Id { get; set; }
        public long MonthlyExpenseId { get; set; }
        public long ExpenseTypeId { get; set; }
        
        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Duedate { get; set; }
        
        public decimal Dueamount { get; set; }
        public decimal Paymentamount { get; set; }
        public bool Issplittedpayment { get; set; }
        public DateTime? Paymentdate { get; set; }
        public int Expensepaymentstatusid { get; set; }
        public string? Additionalremarks { get; set; }
        public bool? Isdeleted { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }
        public ExpenseTypeDTO? ExpenseType { get; set; } = null!;
        public MonthlyExpenseDTO? MonthlyExpense { get; set; } = null!;

    }
}
