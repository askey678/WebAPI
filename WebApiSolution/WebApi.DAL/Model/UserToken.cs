using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Model
{
    [Table("UserTokens")]
    public class UserToken
    {
        [Required]
        public string TokenKey { get; set; }

        [Required]
        public string UserKey { get; set; }

        [Required]
        public string TokenType { get; set; }

        [Required]
        public string TokenHash { get; set; }

        public bool? IsArchived { get; set; } = false;

        public bool? IsActive { get; set; } = true;

        public DateTime DateUpdated { get; set; }

        public DateTime DateCreated { get; set; }

        public int TokenId { get; }
    }
}
