using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DAL.Model
{
    [Table("users")]
    public class User
    {
      

        [Required]
        public string UserKey { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? About { get; set; } = "";

        public bool IsArchived { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime DateUpdated { get; set; }

        public DateTime DateCreated { get; set; }

        public int UserId { get; }
        public User() { }

        public User(string key, string name, string email, string password, string about)
        {
            this.UserKey = key;
            this.UserName = name;
            this.Email = email;
            this.Password = password;
            this.About = about;

        }

    }
}
