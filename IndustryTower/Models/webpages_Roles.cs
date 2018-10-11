using System.Collections.Generic;

namespace IndustryTower.Models
{
    public class webpages_Roles
    {
        public webpages_Roles()
        {
            this.webpages_UsersInRoles = new HashSet<webpages_UsersInRoles>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
}