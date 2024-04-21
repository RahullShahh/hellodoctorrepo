using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public  class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="E-mail cannot be empty")]
        public string email {  get; set; }  
    }
}
