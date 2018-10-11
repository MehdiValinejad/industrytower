using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using IndustryTower.Helpers;
using Resource;
using System.Web.Configuration;

namespace IndustryTower.ViewModels
{
    
    public class PlanRequestViewModel
    {
        // for read only
        public SelectList payTypeToSelectList { get; set; }
        public SelectList bankTypeToSelectList { get; set; }
        public SelectList country { get; set; }


        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        public int stateID { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 9999999, ErrorMessageResourceName = "tooLongFloat", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "regCode", ResourceType = typeof(ModelDisplayName))]
        public long regCode { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [Range(0, 999999999999, ErrorMessageResourceName = "digitCount", ErrorMessageResourceType = typeof(ModelValidation))]
        [RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "phoneNo", ResourceType = typeof(ModelDisplayName))]
        public long phoneNo { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "address", ResourceType = typeof(ModelDisplayName))]
        public string address { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "planRequestCoStoreName", ResourceType = typeof(ModelDisplayName))]
        public string name { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.WhiteSpace), "WhitSpaceCheck")]
        [StringLength(1000, MinimumLength = 250, ErrorMessageResourceName = "CharactersBetween", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "planRequestCoStoreAbout", ResourceType = typeof(ModelDisplayName))]
        public string about { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        [CustomValidation(typeof(ValidationHelpers.EMAIL), "EmailCheck")]
        [RegularExpression(@"([a-z0-9][-a-z0-9_\+\.]*[a-z0-9])@([a-z0-9][-a-z0-9\.]*[a-z0-9]\.(arpa|root|aero|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)|([0-9]{1,3}\.{3}[0-9]{1,3}))", ErrorMessageResourceName = "emailField", ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(70, MinimumLength = 1, ErrorMessageResourceName = "maxCharacters", ErrorMessageResourceType = typeof(ModelValidation))]
        [Display(Name = "email", ResourceType = typeof(ModelDisplayName))]
        public string email { get; set; }


        public string plan { get; set; }

        public DateTime reqDate { get; set; }

    }

    public class PlanReivalRequestViewModel
    {
        //for read only
        public Company companyToRevive { get; set; }
        public Store storeToRevive { get; set; }
        public ActiveUser userToRevive { get; set; }
        public string Name { get; set; }

        public string coId { get; set; }
        public string storeId { get; set; }
        public string userId { get; set; }

        public PlanType planType { get; set; }

        public string plan { get; set; }
    }

    
    public class AllPlanRequests
    {
        public IEnumerable<PlanRequest> CompanyRequests { get; set; }
        public IEnumerable<PlanRequest> StoreRequests { get; set; }
        public IEnumerable<PlanRequest> UserRequests { get; set; }
    }


    public class SentRequests
    {
        public IEnumerable<RevivalRequest> RevialRequests { get; set; }
        public IEnumerable<RequestForNew> RequestsFoNew { get; set; }

        public string CoId { get; set; }
        public string StId { get; set; }
        public string UId { get; set; }

    }


    public class PlanToApprove
    {
        public ActiveUser requesterUser { get; set; }
        public DateTime requestDate { get; set; }
        public string req { get; set; }
        public string requester { get; set; }

        public PlanType plantype { get; set; }
        public string plan { get; set; }

        public bool approve { get; set; }
    }

    public static class PlansPrices
    {
        static public Dictionary<string, int> CompanyPlans { get; set; }
        static public Dictionary<string, int> StorePlans { get; set; }
        static public Dictionary<string, int> UserPlans { get; set; }
        static public Dictionary<string, int> AllPlans { get; set; }


        static PlansPrices()
        {
            CompanyPlans = typeof(CompanyNotExpired).Assembly.GetTypes()
                    .Where(t => t.BaseType == typeof(CompanyNotExpired))
                    .ToDictionary(k => k.Name.ToString(), v => int.Parse(WebConfigurationManager.AppSettings["Plan_Co_" + v.Name.ToLower()]));
            StorePlans = typeof(StoreNotExpired).Assembly.GetTypes()
                    .Where(t => t.BaseType == typeof(StoreNotExpired))
                    .ToDictionary(k => k.Name.ToString(), v => int.Parse(WebConfigurationManager.AppSettings["Plan_St_" + v.Name.ToLower()]));
            UserPlans = typeof(ActiveUser).Assembly.GetTypes()
                    .Where(t => t.BaseType == typeof(ActiveUser))
                    .ToDictionary(k => k.Name.ToString(), v => int.Parse(WebConfigurationManager.AppSettings["Plan_Ur_" + v.Name.ToLower()]));
            AllPlans = CompanyPlans.Concat(StorePlans).Concat(UserPlans).ToDictionary(k => k.Key, v => v.Value);
        }
        
    }



}