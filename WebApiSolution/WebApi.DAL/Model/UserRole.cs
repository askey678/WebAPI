using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DAL.Model
{
    [Table("userRole")]
    public class UserRole
    {
       public string UserRoleKey { get; set; }
        public string UserKey { get; set; }

        public string RoleKey { get; set; }

        public bool? IsArchived { get; set; } = false;

        public bool? IsActive { get; set; } = true;

        public DateTime DateUpdated { get; set; }

        public DateTime DateCreated { get; set; }

        public int UserRoleId { get; }

    }
}
