
namespace IndustryTower.Models
{
    public class webpages_UsersInRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual webpages_Roles webpages_Roles { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}