using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IndustryTower.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Resource;


namespace IndustryTower.ViewModels
{
    public class ExpiredViewModel
    {
        public IEnumerable<CompanyExpired> expiredCompanies { get; set; }
        public IEnumerable<StoreExpired> expiredStores { get; set; }
        public IEnumerable<UserProfile> expiredUsers { get; set; }
    }

    public class PayViewModel
    {
        public Company companyToPay { get; set; }
        public Store StoreToPay { get; set; }
        public ActiveUser UserToPay { get; set; }
        public SelectList payTypeToSelectList { get; set; }
        public SelectList bankTypeToSelectList { get; set; }
        
        public string CoId { get; set; }
        public string StId { get; set; }
        public string UId { get; set; }
        public PayBank payBank { get; set; }
        public long payCode { get; set; }
        public string Name { get; set; }
        public string __RequestVerificationToken { get; set; }
    }

    public class PayForPlanViewModel
    {
        [Editable(false)]
        public RequestForNew PlanRequest { get; set; }

        public RevivalRequest PlanRequestRevival { get; set; }

        public string reqId { get; set; }

        public string plan { get; set; }

        public string requesterUserId { get; set; }


        

        public PayBank payBank { get; set; }

        //[Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        //[Range(0, long.MaxValue, ErrorMessageResourceName = "payCode", ErrorMessageResourceType = typeof(ModelValidation))]
        //[RegularExpression("[0-9]{1,}", ErrorMessageResourceName = "mustNumber", ErrorMessageResourceType = typeof(ModelValidation))]
        //[Display(Name = "payCode", ResourceType = typeof(ModelDisplayName))]
        //public long? payCode { get; set; }

        //[Required(ErrorMessageResourceName = "YouMustSpecify", ErrorMessageResourceType = typeof(ModelValidation))]
        //[Range(0, 10000000, ErrorMessageResourceName = "payAmount", ErrorMessageResourceType = typeof(ModelValidation))]
        //[Display(Name = "payAmount", ResourceType = typeof(ModelDisplayName))]
        //public long? payAmount { get; set; }

        //For Read Only
        public SelectList bankTypeToSelectList { get; set; }

    }
}
