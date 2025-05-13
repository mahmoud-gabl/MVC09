using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ResetPasswordViewModel
    {
        //[Required]
        //public string Email { get; set; }

        //[Required]
        //public string Token { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [Compare ("Password",ErrorMessage ="Password Doesnot Match")]
         public string ConfirmPassword { get; set; }


    }
}
