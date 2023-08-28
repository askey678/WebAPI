using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DAL.Model
{
    [Table("roles")]
    public class Role
    {
        public string RoleKey {  get; set; }
        public string RoleName { get; set; }

        public Boolean IsArchived { get; set; } = false;

        public Boolean IsActive { get; set; } = true;

        public DateTime DateUpdated { get; set; }

        public DateTime DateCreated { get; set; }

        public int RoleId { get;}
        public Role()
        {

        }
        public Role(string key, string roleName)
        {
            this.RoleKey=key;
            RoleName=roleName;
        }   
    }
}
