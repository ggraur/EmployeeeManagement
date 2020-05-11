using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeeManagement.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles  = new List<string>();
        }
        public String Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Valid Email id")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public IList<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
