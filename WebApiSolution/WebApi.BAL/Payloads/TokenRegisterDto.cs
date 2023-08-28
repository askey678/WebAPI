using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BAL.Payloads
{
    public class TokenRegisterDto
    {
        public string UserKey { get; set; }


        public string TokenType { get; set; }


        public string TokenHash { get; set; }

        public TokenRegisterDto() { }

        public TokenRegisterDto(string userKey, string tokenType, string tokenHash)
        {
            UserKey = userKey;
            TokenType = tokenType;
            TokenHash = tokenHash;
        }
    }
}
