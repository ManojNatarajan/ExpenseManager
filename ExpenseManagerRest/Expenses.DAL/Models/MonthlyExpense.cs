using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class Monthlyexpense
    {
        public Monthlyexpense()
        {
            Expenseentries = new HashSet<Expenseentry>();
        }

        public long Id { get; set; }
        public long Userid { get; set; }
        public int Billmonth { get; set; }
        public int Billyear { get; set; }
        public decimal Totalamount { get; set; }
        public decimal Paidamount { get; set; }
        public decimal Dueamount { get; set; }
        public int Monthlypaymentstatusid { get; set; }
        public string? Additionalremarks { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Expenseentry> Expenseentries { get; set; }
    }
}
