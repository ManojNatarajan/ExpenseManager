using System;
using System.Collections.Generic;

namespace Expenses.DAL.Models
{
    public partial class User
    {
        public User()
        {
            Expensetypes = new HashSet<Expensetype>();
            Monthlyexpenses = new HashSet<Monthlyexpense>();
        }

        public long Id { get; set; }
        public long? Mobile { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Userstatusid { get; set; }
        public string? Socialuserid { get; set; }
        public string? Socialprovider { get; set; }
        public bool Isverified { get; set; }
        public bool Accepttandc { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime? Updateddate { get; set; }

        public virtual ICollection<Expensetype> Expensetypes { get; set; }
        public virtual ICollection<Monthlyexpense> Monthlyexpenses { get; set; }
    }
}
