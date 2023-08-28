using System.ComponentModel.DataAnnotations;

namespace WebApi.BAL.ViewModels
{
    public class UserRegisterDto
    {


        [Required(ErrorMessage = "Name is required while Registering")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required while Registering")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required while Registering")]
        public string Password { get; set; }

        public string About { get; set; }

        public string Role { get; set; } = "User";


        public UserRegisterDto() { }

        public UserRegisterDto(string name, string email, string password, string about)
        {
            Name = name;
            Email = email;
            Password = password;
            About = about;

        }
        public override string ToString()
        {
            return $"Name: {Name}, Email: {Email}, Password: {Password}, About: {About} ";
        }
    }
}
