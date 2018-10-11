using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int catID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "catName", ResourceType = typeof(ModelDisplayName))]
        public string catName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name="catNameEN",ResourceType=typeof(ModelDisplayName))]
        public string catNameEN { get; set; }


        public string CultureCatName { 
            get 
            {
                 if (ITTConfig.CurrentCultureIsNotEN)
            {
                return catName;
            }
            else { return catNameEN ;}

            }
        } 
        
        public int? parent1ID { get; set; }
        public int? parent2ID { get; set; }
        public int? parent3ID { get; set; }
        public int? parent4ID { get; set; }


        [ForeignKey("parent1ID")]
        public virtual Category parent1 { get; set; }
        [ForeignKey("parent2ID")]
        public virtual Category parent2 { get; set; }
        [ForeignKey("parent3ID")]
        public virtual Category parent3 { get; set; }
        [ForeignKey("parent4ID")]
        public virtual Category parent4 { get; set; }
        
        public virtual ICollection<Company> Coomopanies { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Seminar> Seminars { get; set; }
    }
}