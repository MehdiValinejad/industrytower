using IndustryTower.App_Start;
using IndustryTower.Helpers;
using Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Certificate //: IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int certID { get; set; }
        
        public int? userID { get; set; }
        
        public int? coID { get; set; }

        
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "certificator", ResourceType = typeof(ModelDisplayName))]
        public string Certificator { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "certificatorEN", ResourceType = typeof(ModelDisplayName))]
        public string CertificatorEN { get; set; }

        [Display(Name = "cultureCertificator", ResourceType = typeof(ModelDisplayName))]
        public string CultureCertificator
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return Certificator;
                else return CertificatorEN;
            }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "certificate", ResourceType = typeof(ModelDisplayName))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "certificateEN", ResourceType = typeof(ModelDisplayName))]
        public string NameEN { get; set; }

        [Display(Name = "cultureCertificateName", ResourceType = typeof(ModelDisplayName))]
        public string CultureCertificateName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return Name;
                else return NameEN;
            }
        }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [Display(Name = "certificateLicence", ResourceType = typeof(ModelDisplayName))]
        [Range(0, long.MaxValue, ErrorMessageResourceName = "mustInt", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        public long licenceNo { get; set; }

        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(150, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "certificateURL", ResourceType = typeof(ModelDisplayName))]
        [CustomValidation(typeof(ValidationHelpers.URL), "URLCheck")]
        [RegularExpression(@"^(https?|ftp)://(-\.)?([^\s/?\.#-]+\.?)+(/[^\s]*)?$", ErrorMessageResourceName = "urlField", ErrorMessageResourceType = typeof(ModelValidation))]
        public string certificatorURL { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime certificationDate { get; set; }

        [ForeignKey("userID")]
        public virtual ActiveUser User { get; set; }
        [ForeignKey("coID")]
        public virtual Company Company { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

        //    DateTime expectedDate;
        //    if (!DateTime.TryParseExact(certificationDate.ToString(), formats, CultureInfo.InvariantCulture,
        //                                DateTimeStyles.None, out expectedDate))
        //    {
        //        yield return new ValidationResult("dorost.", new[] { "certificationDate" });
        //    }
        //}
    }
    
}