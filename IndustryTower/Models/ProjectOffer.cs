using Resource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class ProjectOffer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int offerID { get; set; }

        public int projectID { get; set; }

        public int offererID { get; set; }

        [DefaultValue(false)]
        [Display(Name = "acceptedProjectOffer", ResourceType = typeof(ModelDisplayName))]
        public bool accepted { get; set; }
        
        [ForeignKey("projectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("offererID")]
        public virtual ActiveUser Offerer { get; set; }

        //public virtual ICollection<Abuse> Abuses { get; set; }
    }
}