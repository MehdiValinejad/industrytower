using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class JobOffer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int offerID { get; set; }

        public int jobID { get; set; }

        public int offererID { get; set; }

        [DefaultValue(false)]
        public bool accepted { get; set; }

        [ForeignKey("jobID")]
        public virtual Job Job { get; set; }

        [ForeignKey("offererID")]
        public virtual ActiveUser Offerer { get; set; }

        //public virtual ICollection<Abuse> Abuses { get; set; }
    }
}