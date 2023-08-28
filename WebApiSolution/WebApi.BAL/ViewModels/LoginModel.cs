using System.ComponentModel.DataAnnotations;


namespace WebApi.BAL.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


    }
}
