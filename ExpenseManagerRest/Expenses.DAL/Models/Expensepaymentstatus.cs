using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class Expensepaymentstatus
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
    }
}
