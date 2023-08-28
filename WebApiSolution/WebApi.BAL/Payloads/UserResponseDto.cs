namespace WebApi.BAL.Payloads
{
    public class UserResponseDto
    {

        public string UserKey { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string About { get; set; }

        public List<string> Roles { get; set; }

        public UserResponseDto() { }

        public UserResponseDto(string key, string name, string email, string about)
        {
            UserKey = key;
            Name = name;
            Email = email;
            About = about;

        }
       
    }
}

