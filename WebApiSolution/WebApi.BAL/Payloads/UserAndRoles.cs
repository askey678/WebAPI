using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BAL.Payloads
{
    public class UserAndRoles
    {
        public string UserKey { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string About { get; set; }

        public List<string> Roles { get; set; }

        public  UserAndRoles() { }

        public UserAndRoles(string key, string name, string email, string about)
        {
            UserKey = key;
            Name = name;
            Email = email;
            About = about;

        }
        public override string ToString()
        {
            return $" Key: {this.UserKey}, Name: {this.Name}, Email: {this.Email}, About: {this.About} with Role: {Roles.ToString()}";
        }   
    }
}
