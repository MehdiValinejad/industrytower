using IndustryTower.DAL;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace IndustryTower.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            //if (!Roles.RoleExists("ITAdmin"))
            //{
            //    Roles.CreateRole("ITAdmin");
            //}
            //if (!Roles.RoleExists("CoAdmin"))
            //{
            //    Roles.CreateRole("CoAdmin");
            //}
            //if (!Roles.RoleExists("StAdmin"))
            //{
            //    Roles.CreateRole("StAdmin");
            //}
            //if (!WebSecurity.UserExists("ITHeadAdmin"))
            //{
            //    WebSecurity.CreateUserAndAccount("ITHeadAdmin", "*******", new
            //    {
            //        Birthday = "04/25/2014",
            //        Email = "vjdmhd@gmail.com",
            //        stateID = 1,
            //        gender = 0,
            //        firstName = "ITA",
            //        firstNameEN = "ITA",
            //        lastName = "ITA",
            //        lastNameEN = "ITA",
            //    });
            //    var adminId = WebSecurity.GetUserId("ITHeadAdmin");
            //    UnitOfWork unitOfWork = new UnitOfWork();
            //    unitOfWork.UserPlanChange(adminId, "AdminUser");
            //    if (!Roles.IsUserInRole("ITHeadAdmin", "ITAdmin"))
            //    {
            //        Roles.AddUserToRole("ITHeadAdmin", "ITAdmin");
            //    }
            //}
            
        }
        
        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<ITTContext>(null);

                //try
                //{
                    //using (var context = new ITTContext())
                    //{
                    //    if (!context.Database.Exists())
                    //    {
                    //        // Create the SimpleMembership database without Entity Framework migration schema
                    //        ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    //    }
                    //}

                    WebSecurity.InitializeDatabaseConnection("ITTContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);

                    
                //}
                //catch (Exception ex)
                //{
                //    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                //}
            }
        }
    }
}
