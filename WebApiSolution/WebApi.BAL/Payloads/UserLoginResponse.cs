using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BAL.Payloads
{
    public class UserLoginResponse
    {
        public string UserToken { get; set; }
        public string UserKey { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string About { get; set; }

        public List<string> Roles { get; set; }

        public UserLoginResponse(string token, string key, string name, string email, string about, List<string> roles) { 
            UserToken = token;
            UserKey = key;
            Name = name;
            Email = email;
            About = about;
            Roles = roles;
            
        }
    }
  
}
