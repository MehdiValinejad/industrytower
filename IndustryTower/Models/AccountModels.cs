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
    //public class UsersContext : DbContext
    //{
    //    public UsersContext()
    //        : base("DefaultConnection")
    //    {
    //    }

    //    public DbSet<UserProfile> UserProfiles { get; set; }
    //}

    public enum gender
    {
        Male, Female
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "firstName", ResourceType = typeof(ModelDisplayName))]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "lastName", ResourceType = typeof(ModelDisplayName))]
        public string lastName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "firstNameEN", ResourceType = typeof(ModelDisplayName))]
        public string firstNameEN { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "lastNameEN", ResourceType = typeof(ModelDisplayName))]
        public string lastNameEN { get; set; }

        [Display(Name = "cultureFullName", ResourceType = typeof(ModelDisplayName))]
        public string CultureFullName
        {
            get
            {
                if (ITTConfig.CurrentCultureIsNotEN) return firstName + " " + lastName;
                else return firstNameEN + " " + lastNameEN;
            }
        }

        public string image { get; set; }

        [StringLength(400, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutUser", ResourceType = typeof(ModelDisplayName))]
        public string about { get; set; }

        [StringLength(400, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "aboutUserEN", ResourceType = typeof(ModelDisplayName))]
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

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DefaultValue(gender.Male)]
        public gender gender { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"([a-z0-9][-a-z0-9_\+\.]*[a-z0-9])@([a-z0-9][-a-z0-9\.]*[a-z0-9]\.(arpa|root|aero|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)|([0-9]{1,3}\.{3}[0-9]{1,3}))", ErrorMessageResourceName = "emailField", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.EMAIL), "EmailCheck")]
        [Display(Name = "email", ResourceType = typeof(ModelDisplayName))]
        public string Email { get; set; }

        [Range(0, 99999999999, ErrorMessageResourceName = "mobileNo", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "mobile", ResourceType = typeof(ModelDisplayName))]
        public long? mobile { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "birthday", ResourceType = typeof(ModelDisplayName))]
        public DateTime birthDay { get; set; }

        [Display(Name = "age", ResourceType = typeof(ModelDisplayName))]
        public int age
        {
            get { return DateTime.Now.Year - birthDay.Year; }
        }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int stateID { get; set; }
        //public int? settingID { get; set; }

        [CustomValidation(typeof(ValidationHelpers.MelliCode), "MelliCheck")]
        [RegularExpression(@"^((?!0000000000|1111111111|2222222222|3333333333|4444444444|5555555555|6666666666|7777777777|8888888888|9999999999))(\d{10})$", ErrorMessageResourceName = "melliCode", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "melliCode", ResourceType = typeof(ModelDisplayName))]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(ModelValidation), ErrorMessageResourceName = "melliCode")]
        public string melliCode { get; set; }


        public int Bdg_Golds { get; set; }
        public int Bdg_Silvers { get; set; }
        public int Bdg_Bronzes { get; set; }
        public int ScoresSum { get; set; }



        [ForeignKey("stateID")]
        public virtual CountState State { get; set; }
        //[ForeignKey("settingID")]
        public virtual Setting Setting { get; set; }
        public UserProfile()
        {
            this.webpages_UsersInRoles = new HashSet<webpages_UsersInRoles>();
        }

        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public virtual webpages_Membership webpages_Membership { get; set; }
    }

    public class AdminUser : UserProfile
    {

    }

    public class ActiveUser : UserProfile
    {
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Patent> Patents { get; set; }
        public virtual ICollection<Experience> Experiences { get; set; }
        public virtual ICollection<Education> Education { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<ProjectOffer> ProjectOffers { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }

        [InverseProperty("Attendors")]
        public virtual ICollection<Event> EventsAttended { get; set; }
        [InverseProperty("Creator")]
        public virtual ICollection<Event> EventsCreated { get; set; }

        public virtual ICollection<Company> adminCompanies { get; set; }

        public virtual ICollection<Store> adminStores { get; set; }

        [InverseProperty("Admins")]
        public virtual ICollection<Group> GroupsAdmined { get; set; }
        [InverseProperty("Members")]
        public virtual ICollection<Group> GroupsMembered { get; set; }
        public virtual ICollection<GroupSessionOffer> GroupsSessionOffers { get; set; }


        [InverseProperty("SenderUser")]
        public virtual ICollection<Message> SentMSGs { get; set; }
        [InverseProperty("ReceiverUsers")]
        public virtual ICollection<Message> ReceivedMSGs { get; set; }

        [InverseProperty("PosterUser")]
        public virtual ICollection<Post> SentPosts { get; set; }
        [InverseProperty("PostEditorUser")]
        public virtual ICollection<Post> EditedPosts { get; set; }
        [InverseProperty("PostedUser")]
        public virtual ICollection<Post> ReceivedPosts { get; set; }

        public virtual ICollection<Share> Shares { get; set; }

        public virtual ICollection<CommentPost> SentPostComments { get; set; }
        public virtual ICollection<CommentQuestion> SentQuestionComments { get; set; }
        public virtual ICollection<CommentAnswer> SentAnswerComments { get; set; }
        public virtual ICollection<CommentGSO> SentGSOComments { get; set; }
        public virtual ICollection<CommentProduct> SentProductComments { get; set; }
        public virtual ICollection<CommentService> SentServiceComments { get; set; }

        public virtual ICollection<LikePost> SentPostLikes { get; set; }
        public virtual ICollection<LikeQuestion> SentQuestionLikes { get; set; }
        public virtual ICollection<LikeAnswer> SentAnswerLikes { get; set; }
        public virtual ICollection<LikeGSO> SentGSOLikes { get; set; }
        public virtual ICollection<LikeProduct> SentProductLikes { get; set; }
        public virtual ICollection<LikeService> SentServiceLikes { get; set; }
        public virtual ICollection<LikeComment> SentCommentLikes { get; set; }


        public virtual ICollection<Feed> Feeds { get; set; }

        [InverseProperty("RequesterUser")]
        public virtual ICollection<FriendRequest> SentFriendRequests { get; set; }
        [InverseProperty("RequestReceiverUser")]
        public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Friendship> Friends { get; set; }
        [InverseProperty("Friend")]
        public virtual ICollection<Friendship> FriendsImIn { get; set; }

        public virtual ICollection<Following> Follows { get; set; }


        //[InverseProperty("reporterUser")]
        public virtual ICollection<Abuse> ReportedAbuses { get; set; }
        //[InverseProperty("abuserUser")]
        //public virtual ICollection<Abuse> AbusesByMe { get; set; }


        public virtual ICollection<PlanRequest> PlanRequests { get; set; }

        public virtual ICollection<RevivalRequest> PlanRequestsForUserPlan { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        [InverseProperty("Moderator")]
        public virtual ICollection<Seminar> SeminarsModerate { get; set; }
        [InverseProperty("Broadcasters")]
        public virtual ICollection<Seminar> SeminarsBroadcast { get; set; }
        [InverseProperty("Audiences")]
        public virtual ICollection<Seminar> SeminarsAudience { get; set; }
        public virtual ICollection<SeminarRequest> SeminarsRequests { get; set; }


        public virtual ICollection<Logins> Logins { get; set; }
        public virtual ICollection<BadgeUser> Badges { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Score> Scores { get; set; }
        [InverseProperty("Granter")]
        public virtual ICollection<Score> ScoresSent { get; set; }

        public virtual ICollection<Dict> DictsAdmined { get; set; }


        public virtual ICollection<Word> WordsCreated { get; set; }
        public virtual ICollection<WordEdit> WordsEdited { get; set; }
        public virtual ICollection<WordDesc> WordDescCreated { get; set; }
        public virtual ICollection<WordDescEdit> WordDescEdited { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<ReviewBook> BookReviews { get; set; }
    }

    public class FreeUser : ActiveUser
    {

    }

    public class Professional : ActiveUser
    {

    }
    public class Premium : ActiveUser
    {

    }
    public class Basic : ActiveUser
    {

    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.Password)]
        [Display(Name = "currentPass", ResourceType = typeof(ModelDisplayName))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "minCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(ModelDisplayName))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPassword", ResourceType = typeof(ModelDisplayName))]
        [Compare("NewPassword", ErrorMessageResourceName = "passMissMatch", ErrorMessageResourceType = typeof(ModelValidation))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "userName", ResourceType = typeof(ModelDisplayName))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "minCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(ModelDisplayName))]
        public string Password { get; set; }

        [Display(Name = "rememberMe", ResourceType = typeof(ModelDisplayName))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "firstName", ResourceType = typeof(ModelDisplayName))]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "lastName", ResourceType = typeof(ModelDisplayName))]
        public string lastName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "firstNameEN", ResourceType = typeof(ModelDisplayName))]
        public string firstNameEN { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "lastNameEN", ResourceType = typeof(ModelDisplayName))]
        public string lastNameEN { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [DefaultValue(gender.Male)]
        public gender gender { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "userName", ResourceType = typeof(ModelDisplayName))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "minCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(ModelDisplayName))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPassword", ResourceType = typeof(ModelDisplayName))]
        [Compare("Password", ErrorMessageResourceName = "passMissMatch", ErrorMessageResourceType = typeof(ModelValidation))]
        public string ConfirmPassword { get; set; }

        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression(@"([a-z0-9][-a-z0-9_\+\.]*[a-z0-9])@([a-z0-9][-a-z0-9\.]*[a-z0-9]\.(arpa|root|aero|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)|([0-9]{1,3}\.{3}[0-9]{1,3}))", ErrorMessageResourceName = "emailField", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.EMAIL), "EmailCheck")]
        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "email", ResourceType = typeof(ModelDisplayName))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "datetime", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "birthday", ResourceType = typeof(ModelDisplayName))]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "state", ResourceType = typeof(ModelDisplayName))]
        [Range(0, int.MaxValue, ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int stateID { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
