using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{

    public enum CompanySize
    { 
        [Description("1-10")]
        VerySmall = 1,
        [Description("11-50")]
        Small = 2,
        [Description("50-200")]
        Medium = 3,
        [Description("201-500")]
        Large = 4,
        [Description("501-1000")]
        VeryLarge = 5,
        [Description("1001-5000")]
        SuperLarge = 6,
        [Description("5001-10000")]
        Grand = 6,
        [Description("10000+")]
        Giant = 7,
    }
    public class Company
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int coID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "coName", ResourceType = typeof(ModelDisplayName))]
        public string coName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "coNameEN", ResourceType = typeof(ModelDisplayName))]
        public string coNameEN { get; set; }

        [Display(Name = "cultureCoName", ResourceType = typeof(ModelDisplayName))]
        public string CultureCoName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return coName;
                else return coNameEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1000, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutCo", ResourceType = typeof(ModelDisplayName))]
        public string about { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1000, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutCoEN", ResourceType = typeof(ModelDisplayName))]
        public string aboutEN { get; set; }

        [Display(Name = "cultureAbout", ResourceType = typeof(ModelDisplayName))]
        public string CultureAbout
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return about;
                else return aboutEN;
            }
        }

        public string logo { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        public int stateID { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "companySize", ResourceType = typeof(ModelDisplayName))]
        public CompanySize companySize { get; set; }

        //[Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        //public int? settingID { get; set; }
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "regCode", ResourceType = typeof(ModelDisplayName))]
        public long regCode { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "iranCode", ResourceType = typeof(ModelDisplayName))]
        public long? iranCode { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "ecoCode", ResourceType = typeof(ModelDisplayName))]
        public long? ecoCode { get; set; }

        [DefaultValue(false)]
        [Display(Name = "niocAVL", ResourceType = typeof(ModelDisplayName))]
        public bool niocAVL { get; set; }

        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.URL), "URLCheck")]
        [RegularExpression(@"^(https?|ftp)://(-\.)?([^\s/?\.#-]+\.?)+(/[^\s]*)?$", ErrorMessageResourceName = "urlField", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [Display(Name = "website", ResourceType = typeof(ModelDisplayName))]
        public string website { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"([a-z0-9][-a-z0-9_\+\.]*[a-z0-9])@([a-z0-9][-a-z0-9\.]*[a-z0-9]\.(arpa|root|aero|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)|([0-9]{1,3}\.{3}[0-9]{1,3}))", ErrorMessageResourceName = "emailField", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.EMAIL), "EmailCheck")]
        [Display(Name = "email", ResourceType = typeof(ModelDisplayName))]
        public string email { get; set; }

        [Display(Name = "countryCode", ResourceType = typeof(ModelDisplayName))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 500, ErrorMessageResourceName = "countryCode", ErrorMessageResourceType = typeof(ModelValidation))]
        public Int16 countryCode { get; set; }

        [Display(Name = "stateCode", ResourceType = typeof(ModelDisplayName))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 500, ErrorMessageResourceName = "stateCode", ErrorMessageResourceType = typeof(ModelValidation))]
        public Int16 stateCode { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 999999999999, ErrorMessageResourceName = "digitCount", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "phoneNo", ResourceType = typeof(ModelDisplayName))]
        public long phoneNo { get; set; }

        [Range(0, 999999999999, ErrorMessageResourceName = "digitCount", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "faxNo", ResourceType = typeof(ModelDisplayName))]
        public long? faxNo { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "address", ResourceType = typeof(ModelDisplayName))]
        public string address { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "addressEN", ResourceType = typeof(ModelDisplayName))]
        public string addressEN { get; set; }

        [Display(Name = "cultureAddress", ResourceType = typeof(ModelDisplayName))]
        public string CultureAddress
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return address;
                else return addressEN;
            }
        }

        public string image { get; set; }
        public string Video { get; set; }
        public string document { get; set; }
        
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        public DateTime registerDate { get; set; }

        public DateTime InsertionDate { get; set; }
        
               
        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<ActiveUser> Admins { get; set; }

        public virtual ICollection<Following> Followers { get; set; }


        [InverseProperty("PosterCompany")]
        public virtual ICollection<Post> Sentposts { get; set; }
        [InverseProperty("PostedCompany")]
        public virtual ICollection<Post> Receivedposts { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Experience> Experiences { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Service> Services { get; set; }



        [InverseProperty("SenderCompany")]
        public virtual ICollection<Message> SentMSGs { get; set; }
        [InverseProperty("ReceiverCompanies")]
        public virtual ICollection<Message> ReceivedMSGs { get; set; }

        //public virtual ICollection<Abuse> Abuses { get; set; }

        //[ForeignKey("settingID")]
        public virtual Setting Setting { get; set; }

        public virtual ICollection<RevivalRequest> PlanRequests { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class CompanyNotExpired : Company
    {

    }

    public class CompanyExpired : Company
    {

    }

    public class Imperial : CompanyNotExpired
    {

    }
    public class Luxury : CompanyNotExpired
    {

    }
    public class HighClass : CompanyNotExpired
    {

    }
    
}