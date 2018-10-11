using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IndustryTower.Models
{
    public enum EmailType
    {
        individual, Weekly, Nomail
    }
    public enum Visibility
    { 
        Public, Friends, Onlyme
    }
    public class Setting
    {
        [Key]
        public int settingID { get; set; }

        [DefaultValue(EmailType.individual)]
        public EmailType Epost { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType Ecomment { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType Emessage { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EfriendReq { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EfriendReqAcc { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EfriendSuggest { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EcompanySuggest { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EJobSuggest { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EProjectSuggest { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EJobOffer { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType EprojectOffer { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType Efollow { get; set; }
        [DefaultValue(EmailType.individual)]
        public EmailType Emensions { get; set; }

        [DefaultValue(EmailType.Weekly)]
        public EmailType EPopularFeeds { get; set; }

        [DefaultValue(Visibility.Public)]
        public Visibility Vfeed { get; set; }
        [DefaultValue(Visibility.Public)]
        public Visibility Vproject { get; set; }
        [DefaultValue(Visibility.Public)]
        public Visibility VJob { get; set; }
        [DefaultValue(Visibility.Public)]
        public Visibility Vquestion { get; set; }
        [DefaultValue(Visibility.Public)]
        public Visibility Vanswer { get; set; }
        [DefaultValue(Visibility.Public)]
        public Visibility Vconnections { get; set; }

        //public int coID { get; set; }
        //public int storeID { get; set; }
        //public int userID { get; set; }


        public virtual UserProfile User { get; set; }
        
        public virtual Company Company { get; set; }

        public virtual Store Store { get; set; }

    }
}