using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class Expensetype
    {
        public Expensetype()
        {
            Expenseentries = new HashSet<Expenseentry>();
        }

        public long Id { get; set; }
        public long Userid { get; set; }
        public string Description { get; set; } = null!;
        public decimal Defaultdueamount { get; set; }
        public int Defaultduedateinmonth { get; set; }
        public bool? Isrecurring { get; set; }
        public int? Recurringintervaltypeid { get; set; }
        public bool? Isactive { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Modifieddate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Expenseentry> Expenseentries { get; set; }
    }
}
