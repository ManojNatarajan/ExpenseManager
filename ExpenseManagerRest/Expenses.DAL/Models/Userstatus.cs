using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class Userstatus
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
    }
}
