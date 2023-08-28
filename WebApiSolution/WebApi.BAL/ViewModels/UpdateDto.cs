namespace WebApi.BAL.ViewModels
{
    public class UpdateDto
    {



        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string About { get; set; }


        public UpdateDto() { }

        public UpdateDto(string name = null, string email = null, string password = null, string about = null)
        {
            Name = name;
            Email = email;
            Password = password;
            About = about;

        }


    }
}
