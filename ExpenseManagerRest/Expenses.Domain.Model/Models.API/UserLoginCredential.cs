using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models.API
{
    public class UserLoginCredential
    {
        [Range(1000000000, 9999999999, ErrorMessage ="Mobile Number should have 10 digits!")]
        public long Mobile { get; set; }
        
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; }
    }
}
