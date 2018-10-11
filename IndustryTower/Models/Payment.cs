using Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public enum PayBank
    { 
        PayPal
    }
   
    public class Payment
    {

        [Key]
        public Int32 reqID { get; set; }

        public int? coID { get; set; }

        public int? storeID { get; set; }

        public int? userID { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "payBank", ResourceType = typeof(ModelDisplayName))]
        public PayBank payBank { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, long.MaxValue, ErrorMessageResourceName = "payCode", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "payCode", ResourceType = typeof(ModelDisplayName))]
        public long payCode { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "payDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime payDate { get; set; }

        [Display(Name = "payAcceptDate", ResourceType = typeof(ModelDisplayName))]
        public DateTime? payAcceptDate { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 10000000, ErrorMessageResourceName = "payAmount", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "payAmount", ResourceType = typeof(ModelDisplayName))]
        public long payAmount { get; set; }

        [ForeignKey("coID")]
        public virtual Company Company { get; set; }

        [ForeignKey("storeID")]
        public virtual Store Store { get; set; }

        [ForeignKey("userID")]
        public virtual ActiveUser User { get; set; }

        public virtual PlanRequest planRequest { get; set; }
    }
}