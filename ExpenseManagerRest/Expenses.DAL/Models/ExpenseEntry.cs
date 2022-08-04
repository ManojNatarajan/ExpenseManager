using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class Expenseentry
    {
        public long Id { get; set; }
        public long Monthlyexpenseid { get; set; }
        public long Expensetypeid { get; set; }
        public DateOnly Duedate { get; set; }
        public decimal Dueamount { get; set; }
        public decimal Paymentamount { get; set; }
        public DateTime? Paymentdate { get; set; }
        public int Expensepaymentstatusid { get; set; }
        public string? Additionalremarks { get; set; }
        public bool? Isdeleted { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }

        public virtual Expensetype Expensetype { get; set; } = null!;
        public virtual Monthlyexpense Monthlyexpense { get; set; } = null!;
    }
}
