using Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndustryTower.Models
{
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int messageID { get; set; }
        
        public int? senderCompanyID { get; set; }
        public int? senderStoreID { get; set; }

        public int senderUserID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(1000, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "message", ResourceType = typeof(ModelDisplayName))]
        public string body { get; set; }


        public DateTime messageDate { get; set; }

        [InverseProperty("SentMSGs")]
        [ForeignKey("senderUserID")]
        public virtual ActiveUser SenderUser { get; set; }
        [InverseProperty("ReceivedMSGs")]
        public virtual ICollection<ActiveUser> ReceiverUsers { get; set; }

        [ForeignKey("senderCompanyID")]
        [InverseProperty("SentMSGs")]
        public virtual Company SenderCompany { get; set; }
        [InverseProperty("ReceivedMSGs")]
        public virtual ICollection<Company> ReceiverCompanies { get; set; }

        [ForeignKey("senderStoreID")]
        [InverseProperty("SentMSGs")]
        public virtual Store SenderStore { get; set; }
        [InverseProperty("ReceivedMSGs")]
        public virtual ICollection<Store> ReceiverStores { get; set; }

        //public virtual ICollection<Abuse> Abuses { get; set; }

    }
}