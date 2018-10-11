using IndustryTower.App_Start;
using Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class CountState
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int stateID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "stateName", ResourceType = typeof(ModelDisplayName))]
        public string stateName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "stateNameEN", ResourceType = typeof(ModelDisplayName))]
        public string stateNameEN { get; set; }

        [Display(Name = "country", ResourceType = typeof(ModelDisplayName))]
        public int? countryID { get; set; }

        [Display(Name = "cultureStateName", ResourceType = typeof(ModelDisplayName))]

        public string CultureStateName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return stateName;
                else { return stateNameEN; }
            }
        }

        [Display(Name = "cultureStateName", ResourceType = typeof(ModelDisplayName))]
        public string CultureFullState
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN)
                {
                    if(country == null)
                    {
                        return stateName;
                    }
                    return stateName + ", " + country.stateName;
                }
                else 
                { 
                    if(country == null)
                    {
                        return stateNameEN;
                    }
                    return stateNameEN + ", " + country.stateNameEN; 
                }
            }
        }

        [ForeignKey("countryID")]
        public virtual CountState country { get; set; }

        public virtual ICollection<UserProfile> Users { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Patent> Patents { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        
    }
}