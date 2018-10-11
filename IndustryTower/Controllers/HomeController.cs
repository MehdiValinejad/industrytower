using IndustryTower.App_Start;
using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.Helpers;
using IndustryTower.Models;
using IndustryTower.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class HomeController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult TotalSearch(SearchType searchType, string currentFilter, string searchString, int? page, string sortOrder,
                                        string [] searchStates, string [] searchCats, string [] searchProfessions,
                                        string [] searchCoSize, string [] searchExpCo)
        {
            //bool queryHasDone = false;
            //IEnumerable<string> words = Enumerable.Empty<string>();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                //searchString = searchString.ToUpper();
                //words = searchString.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                //                    .Where(w => w.Length > 2);
                page = null;
            }
            else
            {
                searchString = currentFilter;
                //words = currentFilter != null
                //        ? currentFilter.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                //                       .Where(w => w.Length > 2) 
                //        : Enumerable.Empty<string>();
            }
            ViewData["currentFilter"] = searchString;
            ViewData["searchType"] = searchType;
            ViewData["sortOrder"] = sortOrder;
            if (page == null)
            {
                ViewData["initialProff"] = searchProfessions != null ? searchProfessions.FirstOrDefault() : null;
                ViewData["initialCats"] = searchCats != null ? searchCats.FirstOrDefault() : null;
                return View();
            }

            HashSet<int> selectedStatesHS = new HashSet<int>();
            HashSet<int> selectedCategoriesHS = new HashSet<int>();
            HashSet<int> selectedProfessionsHS = new HashSet<int>();
            HashSet<CompanySize> selectedCoSizeHS = new HashSet<CompanySize>();
            HashSet<int> selectedExpCosHS = new HashSet<int>();

            SearchPanelViewModel searchPanel = new SearchPanelViewModel();

            //IEnumerable<ProductSearchViewModel> productsSearch = Enumerable.Empty<ProductSearchViewModel>();
            //IEnumerable<UserSearchViewModel> users = Enumerable.Empty<UserSearchViewModel>();
            //IEnumerable<ProductsSearchViewModel> products = Enumerable.Empty<ProductsSearchViewModel>();
            //IEnumerable<ServicesSearchViewModel> services = Enumerable.Empty<ServicesSearchViewModel>();
            //IEnumerable<CompanySearchViewModel> companies = Enumerable.Empty<CompanySearchViewModel>();
            //IEnumerable<StoreSearchViewModel> stores = Enumerable.Empty<StoreSearchViewModel>();
            //IEnumerable<JobSearchViewModel> jobs = Enumerable.Empty<JobSearchViewModel>();
            //IEnumerable<ProjectSearchViewModel> projects = Enumerable.Empty<ProjectSearchViewModel>();
            //IEnumerable<QuestionSearchViewModel> questions = Enumerable.Empty<QuestionSearchViewModel>();
            //IEnumerable<EventSearchViewModel> events = Enumerable.Empty<EventSearchViewModel>();
            //IEnumerable<GroupSearchViewModel> groups = Enumerable.Empty<GroupSearchViewModel>();
            //IEnumerable<WebinarSearchViewModel> webinars = Enumerable.Empty<WebinarSearchViewModel>();

            List<SqlParameter> prams = new List<SqlParameter>();
            var totalRows = new SqlParameter("TotalRows", SqlDbType.Int, 50);
            totalRows.Direction = ParameterDirection.Output;
            prams.Add(totalRows);

            int pageSize = 12;
            prams.Add(new SqlParameter("pageSize", pageSize));
            prams.Add(new SqlParameter("pagNum", page));

            

            var cts = new List<SearchPanelCategories>();
            var css = new List<SearchPanelCompanySize>();
            var fss = new List<SearchPanelProfessions>();
            var exs = new List<SearchPanelCompanies>();
            var sss = new List<SearchPanelStates>();


            SqlCommand outputCommand;
            int TotalRows = 0;

            IEnumerable<SearchViewModel> searchViewModel = Enumerable.Empty<SearchViewModel>();

            object finalModel = null;
            switch (searchType)
            {
                #region ALL
                case SearchType.ALL:

                    if (!String.IsNullOrWhiteSpace(searchString))
                    {
                        prams.Add(new SqlParameter("text", searchString));
                        var allReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchAll", prams, out outputCommand);
                        List<SearchViewModel> als = new List<SearchViewModel>();
                        finalModel = als;
                        while (allReader.Read())
                        {
                            var type = allReader.GetInt32(6);
                            var us = new SearchViewModel();

                            switch (type)
                            {
                                case 0:
                                    var u = new ActiveUser();
                                    u.UserId = allReader.GetInt32(1);
                                    u.firstName = allReader[2] as string;
                                    u.firstNameEN = allReader[3] as string;
                                    u.image = allReader[4] as string;
                                    us.user = u;
                                    break;
                                case 1:
                                    var c = new Company();
                                    c.coID = allReader.GetInt32(1);
                                    c.coName = allReader[2] as string;
                                    c.coNameEN = allReader[3] as string;
                                    c.logo = allReader[4] as string;
                                    us.company = c;
                                    break;
                                case 2:
                                    var s = new Store();
                                    s.storeID = allReader.GetInt32(1);
                                    s.storeName = allReader[2] as string;
                                    s.storeNameEN = allReader[3] as string;
                                    s.logo = allReader[4] as string;
                                    us.store = s;
                                    break;
                                case 3:
                                    var p = new Product();
                                    p.productID = allReader.GetInt32(1);
                                    p.productName = allReader[2] as string;
                                    p.productNameEN = allReader[3] as string;
                                    p.image = allReader[4] as string;
                                    us.products = p;
                                    break;
                                case 4:
                                    var sr = new Service();
                                    sr.serviceID = allReader.GetInt32(1);
                                    sr.serviceName = allReader[2] as string;
                                    sr.serviceNameEN = allReader[3] as string;
                                    sr.image = allReader[4] as string;
                                    us.services = sr;
                                    break;
                                case 5:
                                    var g = new Group();
                                    g.groupId = allReader.GetInt32(1);
                                    g.groupName = allReader[2] as string;
                                    us.groups = g;
                                    break;
                                case 6:
                                    var q = new Question();
                                    q.questionID = allReader.GetInt32(1);
                                    q.questionSubject = allReader[2] as string;
                                    var qq = new QuestionSearchViewModel();
                                    qq.questionResult = q;
                                    us.question = qq;
                                    break;
                                case 7:
                                    var sm = new Seminar();
                                    sm.seminarId = allReader.GetInt32(1);
                                    sm.title = allReader[2] as string;
                                    us.webinars = sm;
                                    break;
                            }

                            als.Add(us);
                        }
                        finalModel = als;
                        allReader.Close();

                        TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    }
                    else
                    {
                        finalModel = new List<SearchViewModel>();
                        TotalRows = 0;
                    }

                    

                    //if (words.Count() > 0)
                    //{
                    //    companies = unitOfWork.NotExpiredCompanyRepository.Get(s => words.Any(q => s.coName.ToUpper().Contains(q)
                    //                                                                            || s.coNameEN.ToUpper().Contains(q)))
                    //                        .Select(c => new CompanySearchViewModel { companyResult = c, relevance = 0 });
                    //    stores = unitOfWork.StoreNotExpiredRepository.Get(s => words.Any(q => s.storeName.ToUpper().Contains(q)
                    //                                                                       || s.storeNameEN.ToUpper().Contains(q)))
                    //                        .Select(c => new StoreSearchViewModel { storeResult = c, relevance = 0 });
                    //    products = unitOfWork.ProductRepository.Get(s => s.company is CompanyNotExpired &&  words.Any(q => s.productName.ToUpper().Contains(q)
                    //                                                                       || s.productNameEN.ToUpper().Contains(q)))
                    //                        .Select(c => new ProductsSearchViewModel { productResult = c, relevance = 0 });
                    //    services = unitOfWork.ServiceRepository.Get(s => s.company is CompanyNotExpired && words.Any(q => s.serviceName.ToUpper().Contains(q)
                    //                                                                       || s.serviceNameEN.ToUpper().Contains(q)))
                    //                        .Select(c => new ServicesSearchViewModel { serviceResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        companies = companies.Where(c => selectedStatesHS.Contains(c.companyResult.stateID)
                    //                                  || selectedStatesHS.Contains((int)c.companyResult.State.countryID));
                    //        stores = stores.Where(c => selectedStatesHS.Contains(c.storeResult.stateID)
                    //                                      || selectedStatesHS.Contains((int)c.storeResult.State.countryID));
                    //        products = products.Where(c => selectedStatesHS.Contains(c.productResult.productionCountryID));
                    //        services = services.Where(c => selectedStatesHS.Contains(c.serviceResult.serviceCountryID));
                    //    }
                    //    else
                    //    {
                    //        companies = unitOfWork.NotExpiredCompanyRepository.Get(c => selectedStatesHS.Contains(c.stateID)
                    //                                  || selectedStatesHS.Contains((int)c.State.countryID))
                    //                              .Select(g => new CompanySearchViewModel { companyResult = g, relevance = 0 });
                    //        stores = unitOfWork.StoreNotExpiredRepository.Get(c => selectedStatesHS.Contains(c.stateID)
                    //                                      || selectedStatesHS.Contains((int)c.State.countryID))
                    //                           .Select(g => new StoreSearchViewModel { storeResult = g, relevance = 0 });
                    //        products = unitOfWork.ProductRepository.Get(c => selectedStatesHS.Contains(c.productionCountryID))
                    //                             .Select(g => new ProductsSearchViewModel { productResult = g, relevance = 0 });
                    //        services = unitOfWork.ServiceRepository.Get(c => selectedStatesHS.Contains(c.serviceCountryID))
                    //                           .Select(g => new ServicesSearchViewModel { serviceResult = g, relevance = 0 });
                    //        queryHasDone = true;
                    //    }
                        
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        companies = companies.Where(s => s.companyResult.Categories.Select(j => j.catID)
                    //                         .Intersect(selectedCategoriesHS).Count() > 0)
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += c.companyResult.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count();
                    //                             return c;
                    //                         });
                    //        stores = stores.Where(s => s.storeResult.Categories.Select(j => j.catID)
                    //                       .Intersect(selectedCategoriesHS).Count() > 0)
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.storeResult.Categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //        products = products.Where(s => s.productResult.categories.Select(j => j.catID)
                    //                           .Intersect(selectedCategoriesHS).Count() > 0)
                    //                           .Select(c =>
                    //                           {
                    //                               c.relevance += c.productResult.categories.Select(j => j.catID)
                    //                                                     .Intersect(selectedCategoriesHS).Count();
                    //                               return c;
                    //                           });
                    //        services = services.Where(s => s.serviceResult.categories.Select(j => j.catID)
                    //                           .Intersect(selectedCategoriesHS).Count() > 0)
                    //                           .Select(c =>
                    //                           {
                    //                               c.relevance += c.serviceResult.categories.Select(j => j.catID)
                    //                                                     .Intersect(selectedCategoriesHS).Count();
                    //                               return c;
                    //                           });
                    //    }
                    //    else
                    //    {
                    //        companies = unitOfWork.NotExpiredCompanyRepository.Get(s => s.Categories.Select(j => j.catID)
                    //                         .Intersect(selectedCategoriesHS).Count() > 0)
                    //                         .Select(c => new CompanySearchViewModel
                    //                         {
                    //                             companyResult = c,
                    //                             relevance = c.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count()
                    //                         });
                    //        stores = unitOfWork.StoreNotExpiredRepository.Get(s => s.Categories.Select(j => j.catID)
                    //                       .Intersect(selectedCategoriesHS).Count() > 0)
                    //                       .Select(c => new StoreSearchViewModel
                    //                       {
                    //                           storeResult = c,
                    //                           relevance = c.Categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        products = unitOfWork.ProductRepository.Get(s => s.categories.Select(j => j.catID)
                    //                           .Intersect(selectedCategoriesHS).Count() > 0)
                    //                           .Select(c => new ProductsSearchViewModel
                    //                           {
                    //                               productResult = c,
                    //                               relevance = c.categories.Select(j => j.catID)
                    //                                                     .Intersect(selectedCategoriesHS).Count()
                    //                           });
                    //        services = unitOfWork.ServiceRepository.Get(s => s.categories.Select(j => j.catID)
                    //                           .Intersect(selectedCategoriesHS).Count() > 0)
                    //                           .Select(c => new ServicesSearchViewModel
                    //                           {
                    //                               serviceResult = c,
                    //                               relevance = c.categories.Select(j => j.catID)
                    //                                                     .Intersect(selectedCategoriesHS).Count()
                    //                           });
                    //        queryHasDone = true;
                    //    }
                        
                    //}

                    //if (!queryHasDone)
                    //{
                    //    companies = unitOfWork.NotExpiredCompanyRepository.Get()
                    //                          .Select(c => new CompanySearchViewModel { companyResult = c, relevance = 0 });
                    //    stores = unitOfWork.StoreNotExpiredRepository.Get()
                    //                       .Select(c => new StoreSearchViewModel { storeResult = c, relevance = 0 });
                    //    products = unitOfWork.ProductRepository.Get()
                    //                         .Select(c => new ProductsSearchViewModel { productResult = c, relevance = 0 });
                    //    services = unitOfWork.ServiceRepository.Get()
                    //                         .Select(c => new ServicesSearchViewModel { serviceResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //    var companyStates = companies.Select(s => s.companyResult.State);
                    //    var companyCountries = companies.Select(s => s.companyResult.State.country);
                    //    var storeStates = stores.Select(s => s.storeResult.State);
                    //    var storeCountries = stores.Select(s => s.storeResult.State.country);
                    //    var productStates = products.Select(s => s.productResult.productionCountry);
                    //    var serviceStates = services.Select(s => s.serviceResult.serviceCountry);
                    //    searchPanel.StatePanel = companyCountries.Concat(storeCountries)
                    //                                          .Concat(productStates)
                    //                                          .Concat(companyStates)
                    //                                          .Concat(serviceStates)
                    //                                          .Concat(storeStates)
                    //                                          .GroupBy(p => p, p => p, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                                          .OrderByDescending(f => f.count)
                    //                                          .Take(10);

                    //    var companyCats = companies.SelectMany(f => f.companyResult.Categories);
                    //    var storeCats = stores.SelectMany(f => f.storeResult.Categories);
                    //    var productCats = products.SelectMany(f => f.productResult.categories);
                    //    var serviceCats = services.SelectMany(f => f.serviceResult.categories);
                    //    searchPanel.CategoryPanel = companyCats.Concat(storeCats)
                    //                                           .Concat(productCats)
                    //                                           .Concat(serviceCats)
                    //                                           .GroupBy(p => p)
                    //                                           .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                           .OrderByDescending(f => f.count)
                    //                                           .Take(8);
                                                          


                    //    var companiesResult = companies.Select(f => new SearchViewModel
                    //    {
                    //        company = f.companyResult,
                    //        //relevance = f.relevance
                    //    });
                    //    var storeResult = stores.Select(f => new SearchViewModel
                    //    {
                    //        store = f.storeResult,
                    //        //relevance = f.relevance
                    //    });

                        //var productResult = products.GroupBy(p => p.productResult.company,
                        //                             p => p,
                        //                             (key, g) => new ProductSearchViewModel
                        //                             {
                        //                                 company = key,
                        //                                 product = g.ToList(),
                        //                                 maxRelevance = g.Max(d => d.relevance)
                        //                             })
                        //                    .Select(f => new SearchViewModel { products = f, relevance = f.maxRelevance });
                        //var serviceResult = services.GroupBy(p => p.serviceResult.company,
                        //                              p => p,
                        //                              (key, g) => new ServiceSearchViewModel
                        //                              {
                        //                                  company = key,
                        //                                  service = g.ToList(),
                        //                                  maxRelevance = g.Max(d => d.relevance)
                        //                              })
                        //                    .Select(f => new SearchViewModel { services = f, relevance = f.maxRelevance });
                        //searchViewModel = companiesResult
                        //                  .Concat(storeResult);
                                          //.Concat(productResult)
                                          //.Concat(serviceResult);
                                          
                    break;
                #endregion
                #region People
                case SearchType.People:
                    //bool isState = searchStates.Length > 0,
                    //     isProf = searchProfessions.Length > 0,
                    //     isText = String.IsNullOrWhiteSpace(searchString),
                    //     isExpCo = searchExpCo.Length > 0;

                    //string statement = String.Empty,
                    //       joinClause = String.Empty,
                    //       whereClause = String.Empty;
                    //List<string> wherePartial = new List<string>();
                    //List<SqlParameter> parms = new List<SqlParameter>();


                    //if (isText)
                    //{

                    //    if (isState)
                    //    {
                    //        joinClause = String.Concat(joinClause,"INNER JOIN CountState ON CountState.stateID = FT_TBL.stateID ");
                    //        wherePartial.Add(String.Concat(" (FT_TBL.stateID IN (@stts) OR countryID IN (@stts)) "));
                    //        parms.Add(new SqlParameter("stts", String.Join(",", searchStates)));
                    //    }
                    //    if (isProf)
                    //    {
                    //        joinClause = String.Concat(joinClause,"INNER JOIN ProfessionActiveUser ON ActiveUser_UserId = UserId ");
                    //        wherePartial.Add(String.Concat(" Profession_profID IN (@proffs) "));
                    //        parms.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                    //    }
                    //    if (isExpCo)
                    //    {
                    //        joinClause = String.Concat(joinClause,"INNER JOIN Experience ON Experience.userID = FT_TBL.UserId ");
                    //        wherePartial.Add(String.Concat(" (Experience.CoId IN (@expco)) "));
                    //        parms.Add(new SqlParameter("expco", String.Join(",", searchExpCo)));
                    //    }

                    //    if (isState || isProf || isExpCo)
                    //    {
                    //        whereClause = String.Concat("WHERE Discriminator NOT IN ('AdminUser', 'UserProfile') AND ", String.Join("AND", wherePartial), ") AS FF) AS CC ");
                    //    }

                    //    statement = String.Concat("SELECT UserId,firstName,firstNameEN,lastName,lastNameEN,about,aboutEN,[image],Discriminator, count(*) * 10 + RNK  AS REL ",
                    //        "FROM (SELECT UserId,firstName,firstNameEN,lastName,lastNameEN,about,aboutEN,[image],Discriminator, RNK ",
                    //        "FROM (SELECT FT_TBL.UserId,FT_TBL.lastName,FT_TBL.lastNameEN,FT_TBL.firstName,FT_TBL.firstNameEN,FT_TBL.about,FT_TBL.aboutEN,FT_TBL.[image],FT_TBL.Discriminator,KEY_TBL.RANK as RNK ",
                    //        "FROM UserProfile AS FT_TBL INNER JOIN FREETEXTTABLE (UserProfile, (about, aboutEN, firstName,lastName,firstNameEN,lastNameEN),@searchS,LANGUAGE N'English',30) AS KEY_TBL ON FT_TBL.UserId = KEY_TBL.[KEY] ",
                    //        joinClause,
                    //        whereClause,
                    //        "GROUP BY UserId,firstName,firstNameEN,lastName,lastNameEN,about,aboutEN,[image],Discriminator,RNK ORDER BY REL DESC"
                    //        );
                    //    parms.Add(new SqlParameter("searchS", searchString));
                    //}
                    //else
                    //{

                    //}

                    

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchStates != null && searchStates.Length > 0) 
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }
                    if (searchProfessions != null && searchProfessions.Length > 0) 
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }
                    if (searchExpCo != null && searchExpCo.Length > 0)
                    {
                        prams.Add(new SqlParameter("expCo", String.Join(",", searchExpCo)));
                        selectedExpCosHS = new HashSet<int>(searchExpCo.Select(u => int.Parse(u)));
                    }




                    var userReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchUser", prams, out outputCommand);
                    var uss = new List<SearchViewModel>();
                    finalModel = uss;
                    while (userReader.Read())
                    {
                        var us = new SearchViewModel();
                        var u = new ActiveUser();
                        u.UserId = userReader.GetInt32(1);
                        u.firstName = userReader.GetString(2);
                        u.firstNameEN = userReader.GetString(3);
                        u.lastName = userReader.GetString(4);
                        u.lastNameEN = userReader.GetString(5);
                        u.about = userReader[6] as string;
                        u.aboutEN = userReader[7] as string;
                        u.image = userReader[8] as string;

                        var sttt = new CountState();
                        sttt.stateID = userReader.GetInt32(9);
                        sttt.stateName = userReader.GetString(10);
                        sttt.stateNameEN = userReader.GetString(11);
                        u.State = sttt;
                        us.user = u;
                        uss.Add(us);
                    }
                    finalModel = uss;
                    userReader.NextResult();


                    while (userReader.Read())
                    {
                        
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = userReader.GetInt32(0);
                        prf.professionName = userReader.GetString(1);
                        prf.professionNameEN = userReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = userReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    userReader.NextResult();


                    while (userReader.Read())
                    {
                        
                        var cos = new SearchPanelCompanies();
                        var co = new Company();
                        co.coID = userReader.GetInt32(0);
                        co.coName = userReader.GetString(3);
                        co.coNameEN = userReader.GetString(4);
                        cos.company = co;
                        cos.checkedBox = selectedExpCosHS.Contains(co.coID) ? true : false;
                        cos.count = userReader.GetInt32(5);

                        exs.Add(cos);
                    }
                    searchPanel.CompanyPanel = exs;
                    userReader.NextResult();


                    while (userReader.Read())
                    {
                       
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = userReader.GetInt32(0);
                        stst.stateName = userReader.GetString(1);
                        stst.stateNameEN = userReader.GetString(2);
                        stt.count = userReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;
                    userReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;


                    //if (words.Count() > 0)
                    //{
                    //    users = unitOfWork.ActiveUserRepository
                    //                        .Get(s => s.webpages_Membership.IsConfirmed == true &&  words.Any(q => s.firstName.ToUpper().Contains(q)
                    //                                              || s.firstNameEN.ToUpper().Contains(q)
                    //                                              || s.lastName.ToUpper().Contains(q)
                    //                                              || s.lastNameEN.ToUpper().Contains(q)))
                    //                        .Select(f => new UserSearchViewModel
                    //                        {
                    //                            userResult = f,
                    //                            relevance = String.Concat(f.firstName, ' ',
                    //                                                      f.firstNameEN, ' ',
                    //                                                      f.lastName, ' ',
                    //                                                      f.lastNameEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;

                    //}
                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        users = users.Where(c => selectedStatesHS.Contains(c.userResult.stateID)
                    //                            || selectedStatesHS.Contains((int)c.userResult.State.countryID));
                    //    }
                    //    else
                    //    {
                    //        users = unitOfWork.ActiveUserRepository.Get(c => c.webpages_Membership.IsConfirmed == true && selectedStatesHS.Contains(c.stateID)
                    //                          || selectedStatesHS.Contains((int)c.State.countryID))
                    //                          .Select(g => new UserSearchViewModel { userResult = g, relevance = 0 });
                    //        queryHasDone = true;
                    //    }
                    //}

                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));

                    //    //users = users.Where(s => s.userResult.Professions
                    //    //                          .SelectMany(j => j.categories.Select(h => h.catID))
                    //    //                          .Intersect(selectedCategoriesHS).Count() > 0)
                    //    //             .Select(c =>
                    //    //             {
                    //    //                 c.relevance += c.userResult.Professions.SelectMany(k => k.categories.Select(o => o.catID)).Intersect(selectedCategoriesHS).Count();
                    //    //                 return c;
                    //    //             });
                    //}
                    //if (searchProfessions != null && searchProfessions.Length > 0)
                    //{
                    //    selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    //    if (queryHasDone)
                    //    {
                    //        users = users.Where(s => s.userResult.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.userResult.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        users = unitOfWork.ActiveUserRepository.Get(s => s.webpages_Membership.IsConfirmed == true && s.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c => new UserSearchViewModel
                    //                       {
                    //                           userResult = c,
                    //                           relevance = c.Professions.Select(j => j.profID)
                    //                                        .Intersect(selectedCategoriesHS).Count()

                    //                       });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //if (searchExpCo != null && searchExpCo.Length > 0)
                    //{
                    //    selectedExpCosHS = new HashSet<int>(searchExpCo.Select(u => int.Parse(u)));
                    //    if (queryHasDone)
                    //    {
                    //        users = users.Where(s => selectedExpCosHS.Intersect(s.userResult.Experiences
                    //                                                         .Where(d => d.untilDate == null && d.CoId != null)
                    //                                                         .Select(g => (int)g.CoId)).Count() > 0)
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += 1;
                    //                             return c;
                    //                         });
                    //    }
                    //    else
                    //    {
                    //        users = unitOfWork.ActiveUserRepository.Get(s => s.webpages_Membership.IsConfirmed == true && selectedExpCosHS.Intersect(s.Experiences
                    //                                                         .Where(d => d.untilDate == null && d.CoId != null)
                    //                                                         .Select(g => (int)g.CoId)).Count() > 0)
                    //                         .Select(c => new UserSearchViewModel
                    //                         {
                    //                             userResult = c,
                    //                             relevance = 0
                    //                         });
                    //        queryHasDone = true;
                    //    }
                        
                    //}

                    //if (!queryHasDone)
                    //{
                    //    users = unitOfWork.ActiveUserRepository.Get(s => s.webpages_Membership.IsConfirmed == true)
                    //                          .Select(c => new UserSearchViewModel { userResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //var userStates = users.Select(s => s.userResult.State);
                    //var userCountries = users.Select(s => s.userResult.State.country);
                    //searchPanel.StatePanel = userCountries.Concat(userStates).GroupBy(p => p, p => p, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                              .OrderByDescending(f => f.count)
                    //                              .Take(5);
                    //searchPanel.ProfessionPanel = users.SelectMany(f => f.userResult.Professions)
                    //                                   .GroupBy(p => p)
                    //                                   .Select(p => new SearchPanelProfessions { profession = p.Key, count = p.Count(), checkedBox = selectedProfessionsHS.Contains(p.Key.profID) ? true : false })
                    //                                   .OrderByDescending(f => f.count)
                    //                                   .Take(5);
                    //searchPanel.CompanyPanel = users.SelectMany(f => f.userResult.Experiences.Where(s => s.CoId != null).Select(h => h.Company))
                    //                                .GroupBy(p => p).Select(p => new SearchPanelCompanies { company = p.Key, count = p.Count(), checkedBox = selectedExpCosHS.Contains(p.Key.coID) ? true : false })
                    //                                .OrderByDescending(f => f.count)
                    //                                .Take(5);

                    //searchViewModel = users.Select(f => new SearchViewModel
                    //{
                    //    user = f.userResult,
                    //    relevance = f.relevance
                    //});

                    break;
                #endregion
                #region Companies
                case SearchType.Company:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchStates != null && searchStates.Length > 0) 
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0) 
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }
                    if (searchCoSize != null && searchCoSize.Length > 0)
                    {
                        var companySizes = searchCoSize.Select(u => (CompanySize)Enum.Parse(typeof(CompanySize), u, true));
                        prams.Add(new SqlParameter("cosize", String.Join(",", companySizes.Select(f => (int)f))));
                        selectedCoSizeHS = new HashSet<CompanySize>(companySizes);
                    }




                    var companyReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchCompany", prams, out outputCommand);
                    var crs = new List<SearchViewModel>();
                    finalModel = crs;
                    while (companyReader.Read())
                    {
                        var svm = new SearchViewModel();
                        var c = new Company();
                        c.coID = companyReader.GetInt32(1);
                        c.coName = companyReader.GetString(2);
                        c.coNameEN = companyReader.GetString(3);
                        c.about = companyReader[4] as string;
                        c.aboutEN = companyReader[5] as string;
                        c.logo = companyReader[6] as string;
                        c.companySize = (CompanySize)(companyReader.GetInt32(7));

                        var stt = new CountState();
                        stt.stateID = companyReader.GetInt32(8);
                        stt.stateName = companyReader.GetString(9);
                        stt.stateNameEN = companyReader.GetString(10);
                        c.State = stt;

                        svm.company = c;
                        crs.Add(svm);
                    }
                    finalModel = crs;
                    companyReader.NextResult();


                    while (companyReader.Read())
                    {
                        var cp = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = companyReader.GetInt32(0);
                        cat.catName = companyReader.GetString(1);
                        cat.catNameEN = companyReader.GetString(2);
                        cp.category = cat;
                        cp.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        cp.count = companyReader.GetInt32(3);
                        cts.Add(cp);
                    }
                    searchPanel.CategoryPanel = cts;
                    companyReader.NextResult();


                    while (companyReader.Read())
                    {
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = companyReader.GetInt32(0);
                        stst.stateName = companyReader.GetString(1);
                        stst.stateNameEN = companyReader.GetString(2);
                        stt.count = companyReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;
                    companyReader.NextResult();


                    while (companyReader.Read())
                    {
                        var cosize = new SearchPanelCompanySize();
                        var enumVal = (CompanySize)companyReader.GetInt32(0);
                        cosize.count = companyReader.GetInt32(1);
                        cosize.companySize = enumVal;
                        cosize.checkedBox = selectedCoSizeHS.Contains(enumVal) ? true : false;
                        css.Add(cosize);
                    }
                    searchPanel.CompanySizePanel = css;
                    companyReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;




                    //if (words.Count() > 0)
                    //{
                    //    companies = unitOfWork.NotExpiredCompanyRepository
                    //                        .Get(s => words.Any(q => s.coName.ToUpper().Contains(q)
                    //                                              || s.coNameEN.ToUpper().Contains(q)))
                    //                        .Select(f => new CompanySearchViewModel
                    //                        {
                    //                            companyResult = f,
                    //                            relevance = String.Concat(f.coName, ' ', f.coNameEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        companies = companies.Where(c => selectedStatesHS.Contains(c.companyResult.stateID)
                    //                                      || selectedStatesHS.Contains((int)c.companyResult.State.countryID));
                    //    }
                    //    else
                    //    {
                    //        companies = unitOfWork.NotExpiredCompanyRepository.Get(c => selectedStatesHS.Contains(c.stateID)
                    //                                      || selectedStatesHS.Contains((int)c.State.countryID))
                    //                              .Select(g => new CompanySearchViewModel { companyResult = g, relevance = 0 });
                    //        queryHasDone = true;
                    //    }
                        
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        companies = companies.Where(s => s.companyResult.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += c.companyResult.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count();
                    //                             return c;
                    //                         });
                    //    }
                    //    else
                    //    {
                    //        companies = unitOfWork.NotExpiredCompanyRepository.Get(s => s.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c => new CompanySearchViewModel
                    //                         {
                    //                             companyResult = c,
                    //                             relevance = c.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count()
                    //                         });
                    //        queryHasDone = true;
                    //    }
                        
                        
                    //}
                    //if (searchCoSize != null && searchCoSize.Length > 0)
                    //{
                    //    selectedCoSizeHS = new HashSet<CompanySize>(searchCoSize.Select(u => (CompanySize)Enum.Parse(typeof(CompanySize), u, true)));
                    //    if (queryHasDone)
                    //    {
                    //        companies = companies.Where(s => selectedCoSizeHS.Contains(s.companyResult.companySize))
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += 1;
                    //                             return c;
                    //                         });
                    //    }
                    //    else
                    //    {
                    //        companies = unitOfWork.NotExpiredCompanyRepository.Get(s => selectedCoSizeHS.Contains(s.companySize))
                    //                         .Select(c => new CompanySearchViewModel
                    //                         {
                    //                             companyResult = c,
                    //                             relevance = 0
                    //                         });
                    //        queryHasDone = true;
                    //    }
                    //}

                    //if (!queryHasDone)
                    //{
                    //    companies = unitOfWork.NotExpiredCompanyRepository.Get()
                    //                          .Select(c => new CompanySearchViewModel { companyResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //var CompanyStates = companies.Select(s => s.companyResult.State);
                    //var CompanyCountries = companies.Select(s => s.companyResult.State.country);
                    //searchPanel.StatePanel = CompanyCountries.Concat(CompanyStates).GroupBy(p => p, p => p, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                         .OrderByDescending(f => f.count)
                    //                         .Take(5);
                    //searchPanel.CategoryPanel = companies.SelectMany(f => f.companyResult.Categories)
                    //                                      .GroupBy(p => p)
                    //                                      .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                      .OrderByDescending(f => f.count)
                    //                                      .Take(5);
                    //searchPanel.CompanySizePanel = companies.Select(f => f.companyResult.companySize)
                    //                                .GroupBy(p => p).Select(p => new SearchPanelCompanySize { companySize = p.Key, count = p.Count(), checkedBox = selectedCoSizeHS.Contains(p.Key) ? true : false })
                    //                                .OrderByDescending(f => f.count)
                    //                                .Take(5);
                    //searchViewModel = companies.Select(f => new SearchViewModel
                    //{
                    //    company = f.companyResult,
                    //    relevance = f.relevance
                    //});
                    break;
                #endregion
                #region Stores
                case SearchType.Store:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchStates != null && searchStates.Length > 0) 
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0) 
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }


                    var storeReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchStore", prams, out outputCommand);
                    var srst = new List<SearchViewModel>();
                    finalModel = srst;
                    while (storeReader.Read())
                    {
                        var svm = new SearchViewModel();
                        var s = new Store();
                        s.storeID = storeReader.GetInt32(1);
                        s.storeName = storeReader.GetString(2);
                        s.storeNameEN = storeReader.GetString(3);
                        s.about = storeReader[4] as string;
                        s.aboutEN = storeReader[5] as string;
                        s.logo = storeReader[6] as string;

                        var stt = new CountState();
                        stt.stateID = storeReader.GetInt32(7);
                        stt.stateName = storeReader.GetString(8);
                        stt.stateName = storeReader.GetString(9);
                        s.State = stt;

                        svm.store = s;
                        srst.Add(svm);
                    }
                    finalModel = srst;
                    storeReader.NextResult();


                    while (storeReader.Read())
                    {
                        var cp = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = storeReader.GetInt32(0);
                        cat.catName = storeReader.GetString(1);
                        cat.catNameEN = storeReader.GetString(2);
                        cp.category = cat;
                        cp.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        cp.count = storeReader.GetInt32(3);
                        cts.Add(cp);
                    }
                    searchPanel.CategoryPanel = cts;
                    storeReader.NextResult();


                    while (storeReader.Read())
                    {
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = storeReader.GetInt32(0);
                        stst.stateName = storeReader.GetString(1);
                        stst.stateNameEN = storeReader.GetString(2);
                        stt.count = storeReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;
                    storeReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;

                    //if (words.Count() > 0)
                    //{
                    //    stores = unitOfWork.StoreNotExpiredRepository
                    //                        .Get(s => words.Any(q => s.storeName.ToUpper().Contains(q)
                    //                                              || s.storeNameEN.ToUpper().Contains(q)))
                    //                        .Select(f => new StoreSearchViewModel
                    //                        {
                    //                            storeResult = f,
                    //                            relevance = String.Concat(f.storeName, ' ',
                    //                                                      f.storeNameEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        stores = stores.Where(c => selectedStatesHS.Contains(c.storeResult.stateID)
                    //                                  || selectedStatesHS.Contains((int)c.storeResult.State.countryID));
                    //    }
                    //    else
                    //    {
                    //        stores = unitOfWork.StoreNotExpiredRepository.Get(c => selectedStatesHS.Contains(c.stateID)
                    //                                  || selectedStatesHS.Contains((int)c.State.countryID))
                    //                                  .Select(g => new StoreSearchViewModel { storeResult = g, relevance = 0 });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        stores = stores.Where(s => s.storeResult.Categories.Select(j => j.catID)
                    //                   .Intersect(selectedCategoriesHS).Count() > 0)
                    //                   .Select(c =>
                    //                   {
                    //                       c.relevance += c.storeResult.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count();
                    //                       return c;
                    //                   });
                    //    }
                    //    else
                    //    {
                    //        stores = unitOfWork.StoreNotExpiredRepository.Get(s => s.Categories.Select(j => j.catID)
                    //                   .Intersect(selectedCategoriesHS).Count() > 0)
                    //                   .Select(c => new StoreSearchViewModel 
                    //                   {
                    //                       storeResult = c,
                    //                       relevance = c.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count()
                    //                   });
                    //        queryHasDone = true;
                    //    }
                        
                    //}
                    //if (!queryHasDone)
                    //{
                    //    stores = unitOfWork.StoreNotExpiredRepository.Get()
                    //                       .Select(c => new StoreSearchViewModel { storeResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}
                    //var StoreStates = stores.Select(s => s.storeResult.State);
                    //var StoreCountries = stores.Select(s => s.storeResult.State.country);
                    //searchPanel.StatePanel = StoreCountries.Concat(StoreStates).GroupBy(p => p, p => p, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                           .OrderByDescending(f => f.count)
                    //                           .Take(5);
                    //searchPanel.CategoryPanel = stores.SelectMany(f => f.storeResult.Categories)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);
                    //searchViewModel = stores.Select(f => new SearchViewModel
                    //{
                    //    store = f.storeResult,
                    //    relevance = f.relevance
                    //});
                    break;
                #endregion
                #region Products
                case SearchType.Product:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchStates != null && searchStates.Length > 0) 
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0) 
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }


                    var productReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchProduct", prams, out outputCommand);
                    var prs = new List<SearchViewModel>();
                    finalModel = prs;
                    while (productReader.Read())
                    {
                        var svm = new SearchViewModel();
                        var p = new Product();
                        p.productID = productReader.GetInt32(1);
                        p.productName = productReader.GetString(2);
                        p.productNameEN = productReader.GetString(3);
                        p.about = productReader.GetString(4);
                        p.aboutEN = productReader[5] as string;
                        p.image = productReader[6] as string;

                        var stt = new CountState();
                        stt.stateID = productReader.GetInt32(7);
                        stt.stateName = productReader.GetString(8);
                        stt.stateName = productReader.GetString(9);
                        p.productionCountry = stt;

                        var co = new Company();
                        co.coID = productReader.GetInt32(10);
                        co.coName = productReader.GetString(11);
                        co.coNameEN = productReader.GetString(12);
                        co.logo = productReader[13] as string;
                        p.company = co;

                        svm.products = p;
                        prs.Add(svm);
                    }
                    finalModel = prs;
                    productReader.NextResult();


                    while (productReader.Read())
                    {
                        var pf = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = productReader.GetInt32(0);
                        cat.catName = productReader.GetString(1);
                        cat.catNameEN = productReader.GetString(2);
                        pf.category = cat;
                        pf.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        pf.count = productReader.GetInt32(3);
                        cts.Add(pf);
                    }
                    searchPanel.CategoryPanel = cts;
                    productReader.NextResult();


                    while (productReader.Read())
                    {
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = productReader.GetInt32(0);
                        stst.stateName = productReader.GetString(1);
                        stst.stateNameEN = productReader.GetString(2);
                        stt.count = productReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;
                    productReader.Close();


                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;

                    //if (words.Count() > 0)
                    //{
                    //    products = unitOfWork.ProductRepository
                    //                        .Get(s => words.Any(q => s.productName.ToUpper().Contains(q)
                    //                                              || s.productNameEN.ToUpper().Contains(q)))
                    //                        .Select(f => new ProductsSearchViewModel
                    //                        {
                    //                            productResult = f,
                    //                            relevance = String.Concat(f.productName, ' ',
                    //                                                      f.productNameEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        products = products.Where(c => selectedStatesHS.Contains(c.productResult.productionCountryID)
                    //                                || selectedStatesHS.Contains(c.productResult.company.stateID));
                    //    }
                    //    else
                    //    {
                    //        products = unitOfWork.ProductRepository.Get(c => c.company is CompanyNotExpired && selectedStatesHS.Contains(c.productionCountryID)
                    //                                || selectedStatesHS.Contains(c.company.stateID))
                    //                                .Select(g => new ProductsSearchViewModel { productResult = g, relevance = 0 });
                    //        queryHasDone = true;
                    //    }
                        
                        
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        products = products.Where(s => s.productResult.categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.productResult.categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {

                    //        products = unitOfWork.ProductRepository.Get(s => s.company is CompanyNotExpired && s.categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c =>  new ProductsSearchViewModel
                    //                       {
                    //                           productResult = c,
                    //                           relevance = c.categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                        
                    //}
                    //if (!queryHasDone)
                    //{
                    //    products = unitOfWork.ProductRepository.Get()
                    //                         .Select(c => new ProductsSearchViewModel { productResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //searchPanel.StatePanel = products.GroupBy(p => p.productResult.productionCountry, p => p.productResult, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                           .OrderByDescending(f => f.count)
                    //                           .Take(5);
                    //searchPanel.CategoryPanel = products.SelectMany(f => f.productResult.categories)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);
                    //searchViewModel = products.GroupBy(p => p.productResult.company,
                    //                             p => p,
                    //                             (key, g) => new ProductSearchViewModel
                    //                             {
                    //                                 company = key,
                    //                                 product = g.ToList(),
                    //                                 maxRelevance = g.Max(d => d.relevance)
                    //                             })
                    //                    .Select(f => new SearchViewModel { products = f, relevance = f.maxRelevance });
                    break;
                #endregion
                #region Services
                case SearchType.Service:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchStates != null && searchStates.Length > 0) 
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0) 
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }


                    var serviceReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchService", prams, out outputCommand);
                    var srs = new List<SearchViewModel>();
                    finalModel = srs;
                    while (serviceReader.Read())
                    {
                        var svm = new SearchViewModel();
                        var s = new Service();
                        s.serviceID = serviceReader.GetInt32(1);
                        s.serviceName = serviceReader.GetString(2);
                        s.serviceNameEN = serviceReader.GetString(3);
                        s.about = serviceReader.GetString(4);
                        s.aboutEN = serviceReader[5] as string;
                        s.image = serviceReader[6] as string;

                        var stt = new CountState();
                        stt.stateID = serviceReader.GetInt32(7);
                        stt.stateName = serviceReader.GetString(8);
                        stt.stateName = serviceReader.GetString(9);
                        s.serviceCountry = stt;

                        var co = new Company();
                        co.coID = serviceReader.GetInt32(10);
                        co.coName = serviceReader.GetString(11);
                        co.coNameEN = serviceReader.GetString(12);
                        co.image = serviceReader[13] as string;
                        s.company = co;

                        svm.services = s;
                        srs.Add(svm);
                    }
                    finalModel = srs;
                    serviceReader.NextResult();


                    while (serviceReader.Read())
                    {
                        var pf = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = serviceReader.GetInt32(0);
                        cat.catName = serviceReader.GetString(1);
                        cat.catNameEN = serviceReader.GetString(2);
                        pf.category = cat;
                        pf.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        pf.count = serviceReader.GetInt32(3);
                        cts.Add(pf);
                    }
                    searchPanel.CategoryPanel = cts;
                    serviceReader.NextResult();


                    while (serviceReader.Read())
                    {
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = serviceReader.GetInt32(0);
                        stst.stateName = serviceReader.GetString(1);
                        stst.stateNameEN = serviceReader.GetString(2);
                        stt.count = serviceReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;

                    serviceReader.Close();


                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;

                    //if (words.Count() > 0)
                    //{
                    //    services = unitOfWork.ServiceRepository
                    //                        .Get(s => words.Any(q => s.serviceName.ToUpper().Contains(q)
                    //                                              || s.serviceNameEN.ToUpper().Contains(q)))
                    //                        .Select(f => new ServicesSearchViewModel
                    //                        {
                    //                            serviceResult = f,
                    //                            relevance = String.Concat(f.serviceName, ' ',
                    //                                                      f.serviceNameEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        services = services.Where(s => s.serviceResult.categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.serviceResult.categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        services = unitOfWork.ServiceRepository.Get(s => s.company is CompanyNotExpired && s.categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c => new ServicesSearchViewModel
                    //                       {
                    //                           serviceResult = c,
                    //                           relevance = c.categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                        
                        
                    //}
                    //if (!queryHasDone)
                    //{
                    //    services = unitOfWork.ServiceRepository.Get()
                    //                         .Select(c => new ServicesSearchViewModel { serviceResult = c, relevance = 0});
                    //    queryHasDone = true;
                    //}

                    //searchPanel.StatePanel = services.GroupBy(p => p.serviceResult.serviceCountry, p => p.serviceResult, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                           .OrderByDescending(f => f.count)
                    //                           .Take(5);
                    //searchPanel.CategoryPanel = services.SelectMany(f => f.serviceResult.categories)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);
                    //searchViewModel = services.GroupBy(p => p.serviceResult.company,
                    //                              p => p,
                    //                              (key, g) => new ServiceSearchViewModel
                    //                              {
                    //                                  company = key,
                    //                                  service = g.ToList(),
                    //                                  maxRelevance = g.Max(d => d.relevance)
                    //                              })
                    //                    .Select(f => new SearchViewModel { services = f, relevance = f.maxRelevance });
                    break;
                #endregion
                #region Question
                case SearchType.Question:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchProfessions != null && searchProfessions.Length > 0)
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }

                    switch (sortOrder)
                    {
                        case "Date":
                            prams.Add(new SqlParameter("sort", "questionID"));
                            break;
                        case "date_desc":
                            prams.Add(new SqlParameter("sort", "questionID DESC"));
                            break;
                        case "recentAnswer":
                            prams.Add(new SqlParameter("ans", ">"));
                            break;
                        case "unAnswer":
                            prams.Add(new SqlParameter("ans", "="));
                            break;
                        case "lang_en":
                            prams.Add(new SqlParameter("lang", "0"));
                            break;
                        case "lang":
                            prams.Add(new SqlParameter("lang", "1"));
                            break;
                    }

                    
                    var questionReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchQuestion", prams, out outputCommand);
                    var Qss = new List<QuestionSearchViewModel>();
                    finalModel = Qss;
                    while (questionReader.Read())
                    {
                        var svm = new QuestionSearchViewModel();
                        var q = new Question();
                        q.questionID = questionReader.GetInt32(1);
                        q.questionSubject = questionReader.GetString(2);
                        q.questionBody = questionReader.GetString(3);
                        q.language = (lang)questionReader.GetInt32(4);
                        q.image = questionReader[5] as string;


                        svm.questionResult = q;
                        svm.Answers = questionReader.GetInt32(6);
                        Qss.Add(svm);
                    }
                    finalModel = Qss;
                    questionReader.NextResult();


                    while (questionReader.Read())
                    {
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = questionReader.GetInt32(0);
                        prf.professionName = questionReader.GetString(1);
                        prf.professionNameEN = questionReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = questionReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    questionReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;

                    ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                    ViewData["recentAnswerSortParm"] = "recentAnswer";//sortOrder == "recentAnswer" ? "" : "recentAnswer";
                    ViewData["unAnswerSortParm"] = "unAnswer";
                    ViewData["langSortParm"] = sortOrder == "lang" ? "lang_en" : "lang";



                    //if (words.Count() > 0)
                    //{
                    //    questions = unitOfWork.QuestionRepository
                    //                        .Get(s => words.Any(q => s.questionSubject.ToUpper().Contains(q)))
                    //                        .Select(f => new QuestionSearchViewModel
                    //                        {
                    //                            questionResult = f,
                    //                            relevance = String.Concat(f.questionSubject.ToUpper())
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    //questions = questions.Where(s => s.questionResult.Professions
                    //    //                          .SelectMany(j => j.categories.Select(h => h.catID))
                    //    //                          .Intersect(selectedCategoriesHS).Count() > 0)
                    //    //                     .Select(c =>
                    //    //                     {
                    //    //                         c.relevance += c.questionResult.Professions.SelectMany(k => k.categories.Select(o => o.catID)).Intersect(selectedCategoriesHS).Count();
                    //    //                         return c;
                    //    //                     });


                    //}
                    //if (searchProfessions != null && searchProfessions.Length > 0)
                    //{
                    //    selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    //    if (queryHasDone)
                    //    {
                    //        questions = questions.Where(s => s.questionResult.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.questionResult.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        questions = unitOfWork.QuestionRepository.Get(s => s.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c => new QuestionSearchViewModel
                    //                       {
                    //                           questionResult =c,
                    //                           relevance = c.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //switch (sortOrder)
                    //{
                    //    case "Date":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.OrderBy(s => s.questionResult.questionDate);
                                                                           
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get().OrderBy(s => s.questionDate)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                    //        break;
                    //    case "date_desc":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.OrderByDescending(s => s.questionResult.questionDate);
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get().OrderByDescending(s => s.questionDate)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    case "recentAnswer":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.OrderByDescending(s => s.questionResult.Answers.Select(d=>d.answerDate));
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.AnswerRepository.Get(includeProperties: "Question").OrderByDescending(t => t.answerDate)
                    //                                                     .GroupBy(q => q.Question)
                    //                                                     .Select(q => new QuestionSearchViewModel { questionResult = q.Key, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                    //        break;
                    //    case "unAnswer":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.Where(s => s.questionResult.Answers.Count() == 0 );
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get(includeProperties: "Answers", filter: s => s.Answers.Count == 0)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    case "lang_en":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.Where(s => s.questionResult.language == lang.en);
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get(includeProperties: "Answers", filter: s => s.language == lang.en)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    case "lang":
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.Where(s => s.questionResult.language == lang.fa);
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get(includeProperties: "Answers", filter: s => s.language == lang.fa)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    default: // Name ascending
                    //        if (queryHasDone)
                    //        {
                    //            questions = questions.OrderByDescending(s => s.questionResult.questionDate);
                    //        }
                    //        else
                    //        {
                    //            questions = unitOfWork.QuestionRepository.Get().OrderByDescending(s => s.questionDate)
                    //                                                       .Select(g => new QuestionSearchViewModel { questionResult = g, relevance = 0});
                    //            queryHasDone = true;
                    //        }
                    //        break;
                    //}
                    //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                    //ViewData["recentAnswerSortParm"] = "recentAnswer";//sortOrder == "recentAnswer" ? "" : "recentAnswer";
                    //ViewData["unAnswerSortParm"] = "unAnswer";
                    //ViewData["langSortParm"] = sortOrder == "lang" ? "lang_en" : "lang";
                    
                    
                    //searchPanel.ProfessionPanel = questions.SelectMany(f => f.questionResult.Professions)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelProfessions { profession = p.Key, count = p.Count(), checkedBox = selectedProfessionsHS.Contains(p.Key.profID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);
                    //searchViewModel = questions.Select(f => new SearchViewModel
                    //{
                    //    question = f.questionResult,
                    //    relevance = f.relevance
                    //});
                    break;
                #endregion
                #region Events
                case SearchType.Event:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchCats != null && searchCats.Length > 0)
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }
                    if (searchStates != null && searchStates.Length > 0)
                    {
                        prams.Add(new SqlParameter("states", String.Join(",", searchStates)));
                        selectedStatesHS = new HashSet<int>(searchStates.Select(u => int.Parse(u)));
                    }

                    switch (sortOrder)
                    {
                        case "Date":
                            prams.Add(new SqlParameter("sort", "eventDate"));
                            break;
                        case "date_desc":
                            prams.Add(new SqlParameter("sort", "eventDate DESC"));
                            break;
                        case "attendors":
                            prams.Add(new SqlParameter("sort", "Attendors"));
                            break;
                        case "expired":
                            prams.Add(new SqlParameter("atndr", "Expired"));
                            break;
                    }

                    var eventReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchEvent", prams, out outputCommand);
                    var Evs = new List<EventSearchViewModel>();
                    finalModel = Evs;
                    while (eventReader.Read())
                    {
                        var svm = new EventSearchViewModel();
                        var e = new Event();
                        e.eventID = eventReader.GetInt32(1);
                        e.eventSubj = eventReader.GetString(2);
                        e.eventSubjEN = eventReader.GetString(3);
                        e.description = eventReader.GetString(4);
                        e.descriptionEN = eventReader.GetString(5);
                        e.eventDate = eventReader.GetDateTime(6);
                        e.untilDate = eventReader.GetDateTime(7);
                        
                        var stt =  new CountState();
                        stt.stateID = eventReader.GetInt32(8);
                        stt.stateName = eventReader.GetString(9);
                        stt.stateNameEN = eventReader.GetString(10);
                        e.State = stt;

                        svm.eventResult = e;
                        svm.Attendors = eventReader.GetInt32(11);

                        Evs.Add(svm);
                    }
                    finalModel = Evs;
                    eventReader.NextResult();


                    while (eventReader.Read())
                    {
                        var pf = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = eventReader.GetInt32(0);
                        cat.catName = eventReader.GetString(1);
                        cat.catNameEN = eventReader.GetString(2);
                        pf.category = cat;
                        pf.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        pf.count = eventReader.GetInt32(3);
                        cts.Add(pf);
                    }
                    searchPanel.CategoryPanel = cts;
                    eventReader.NextResult();


                    while (eventReader.Read())
                    {
                        var stt = new SearchPanelStates();
                        var stst = new CountState();
                        stst.stateID = eventReader.GetInt32(0);
                        stst.stateName = eventReader.GetString(1);
                        stst.stateNameEN = eventReader.GetString(2);
                        stt.count = eventReader.GetInt32(3);
                        stt.state = stst;
                        stt.checkedBox = selectedStatesHS.Contains(stst.stateID) ? true : false;
                        sss.Add(stt);
                    }
                    searchPanel.StatePanel = sss;
                    eventReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;

                    ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                    ViewData["recentAttendorsSortParm"] = "attendors";//sortOrder == "recentAnswer" ? "" : "recentAnswer";
                    ViewData["expiredSortParm"] = "expired";


                    //if (words.Count() > 0)
                    //{
                    //    events = unitOfWork.EventRepository
                    //                        .Get(s => words.Any(q => s.eventSubj.ToUpper().Contains(q)
                    //                                              || s.eventSubjEN.ToUpper().Contains(q)))
                    //                        .Select(f => new EventSearchViewModel
                    //                        {
                    //                            eventResult = f,
                    //                            relevance = String.Concat(f.eventSubj, ' ', f.eventSubjEN).ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchStates != null && searchStates.Length > 0)
                    //{
                    //    selectedStatesHS = new HashSet<int>(searchStates.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {

                    //    }
                    //    else 
                    //    {
                    //        events = unitOfWork.EventRepository.Get(c => selectedStatesHS.Contains(c.stateID)
                    //                            || selectedStatesHS.Contains((int)c.State.countryID))
                    //                            .Select(g => new EventSearchViewModel { eventResult = g, relevance = 0 });
                    //    }
                    //    queryHasDone = true;
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        events = events.Where(s => s.eventResult.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.eventResult.Categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        events = unitOfWork.EventRepository.Get(s => s.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                       .Select(c => new EventSearchViewModel
                    //                       {
                    //                           eventResult =c,
                    //                           relevance = c.Categories.Select(j => j.catID)
                    //                                                 .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //switch (sortOrder)
                    //{
                    //    case "Date":
                    //        if (queryHasDone)
                    //        {
                    //            events = events.Where(e => e.eventResult.eventDate >= DateTime.UtcNow)
                    //                                           .OrderBy(s => s.eventResult.eventDate);
                    //        }
                    //        else
                    //        {
                    //            events = unitOfWork.EventRepository.Get(e => e.eventDate >= DateTime.UtcNow)
                    //                                           .OrderBy(s => s.eventDate)
                    //                                           .Select(g => new EventSearchViewModel { eventResult = g, relevance =0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    case "date_desc":
                    //        if (queryHasDone)
                    //        {
                    //            events = events.Where(e => e.eventResult.eventDate >= DateTime.UtcNow)
                    //                                              .OrderByDescending(s => s.eventResult.eventDate);
                    //        }
                    //        else
                    //        {
                    //            events = unitOfWork.EventRepository.Get(e => e.eventDate >= DateTime.UtcNow)
                    //                                              .OrderByDescending(s => s.eventDate)
                    //                                              .Select(g => new EventSearchViewModel { eventResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                    //        break;
                    //    case "attendors":
                    //        if (queryHasDone)
                    //        {
                    //            events = events.Where(e => e.eventResult.eventDate >= DateTime.UtcNow)
                    //                                                     .OrderByDescending(f => f.eventResult.Attendors.Count);
                    //        }
                    //        else
                    //        {
                    //            events = unitOfWork.EventRepository.Get(e => e.eventDate >= DateTime.UtcNow, includeProperties: "Attendors")
                    //                                                     .OrderByDescending(f => f.Attendors.Count)
                    //                                                     .Select(q => new EventSearchViewModel { eventResult = q, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    case "expired":
                    //        if (queryHasDone)
                    //        {
                    //            events = events.Where(e => e.eventResult.eventDate < DateTime.UtcNow)
                    //                                                     .OrderByDescending(f => f.eventResult.Attendors.Count);
                    //        }
                    //        else
                    //        {
                    //            events = unitOfWork.EventRepository.Get(e => e.eventDate < DateTime.UtcNow, includeProperties: "Attendors")
                    //                                                     .OrderByDescending(f => f.Attendors.Count)
                    //                                                     .Select(q => new EventSearchViewModel { eventResult = q, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //    default: // Name ascending
                    //        if (queryHasDone)
                    //        {
                    //            events = events.Where(e => e.eventResult.eventDate >= DateTime.UtcNow)
                    //                                           .OrderBy(s => s.eventResult.eventDate);
                    //        }
                    //        else
                    //        {
                    //            events = unitOfWork.EventRepository.Get(e => e.eventDate >= DateTime.UtcNow)
                    //                                           .OrderBy(s => s.eventDate)
                    //                                           .Select(g => new EventSearchViewModel { eventResult = g, relevance = 0 });
                    //            queryHasDone = true;
                    //        }
                            
                    //        break;
                    //}
                    //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                    //ViewData["recentAttendorsSortParm"] = "attendors";//sortOrder == "recentAnswer" ? "" : "recentAnswer";
                    //ViewData["expiredSortParm"] = "expired";
                
                    //var eventStates = events.Select(s => s.eventResult.State);
                    //var eventCountries = events.Select(s => s.eventResult.State.country);
                    //searchPanel.StatePanel = eventCountries.Concat(eventStates).GroupBy(p => p, p => p, (key, g) => new SearchPanelStates { state = key, count = g.Count(), checkedBox = selectedStatesHS.Contains(key.stateID) ? true : false })
                    //                           .OrderByDescending(f => f.count)
                    //                           .Take(5);
                    // searchPanel.CategoryPanel = events.SelectMany(f => f.eventResult.Categories)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);
                    // searchViewModel = events.Select(f => new SearchViewModel
                    // {
                    //     events = f.eventResult,
                    //     relevance = f.relevance
                    // });
                    break;
                #endregion
                #region Group
                case SearchType.Group:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchProfessions != null && searchProfessions.Length > 0)
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0)
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }

                    //switch (sortOrder)
                    //{
                    //    case "Date":
                    //        prams.Add(new SqlParameter("sort", "eventDate"));
                    //        break;
                    //    case "date_desc":
                    //        prams.Add(new SqlParameter("sort", "eventDate DESC"));
                    //        break;
                    //    case "attendors":
                    //        prams.Add(new SqlParameter("sort", "Attendors"));
                    //        break;
                    //    case "expired":
                    //        prams.Add(new SqlParameter("atndr", "Expired"));
                    //        break;
                    //}

                    var groupReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchGroup", prams, out outputCommand);
                    var Gps = new List<GroupSearchViewModel>();
                    finalModel = Gps;
                    while (groupReader.Read())
                    {
                        var svm = new GroupSearchViewModel();
                        Group g = new Group();
                        g.groupId = groupReader.GetInt32(1);
                        g.groupName = groupReader.GetString(2);
                        g.groupDesc = groupReader.GetString(3);
                        g.isPublic = groupReader.GetBoolean(4);

                        svm.groupResult = g;
                        svm.Sessions = groupReader.GetInt32(5);
                        svm.Members = groupReader.GetInt32(6);
                        Gps.Add(svm);
                    }
                    finalModel = Gps;
                    groupReader.NextResult();


                    while (groupReader.Read())
                    {
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = groupReader.GetInt32(0);
                        prf.professionName = groupReader.GetString(1);
                        prf.professionNameEN = groupReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = groupReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    groupReader.NextResult();

                    while (groupReader.Read())
                    {
                        var pf = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = groupReader.GetInt32(0);
                        cat.catName = groupReader.GetString(1);
                        cat.catNameEN = groupReader.GetString(2);
                        pf.category = cat;
                        pf.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        pf.count = groupReader.GetInt32(3);
                        cts.Add(pf);
                    }
                    searchPanel.CategoryPanel = cts;
                    groupReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    //if (words.Count() > 0)
                    //{
                    //    groups = unitOfWork.GroupRepository
                    //                        .Get(s => words.Any(q => s.groupName.ToUpper().Contains(q)))
                    //                        .Select(f => new GroupSearchViewModel
                    //                        {
                    //                            groupResult = f,
                    //                            relevance = f.groupName.ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        groups = groups.Where(s => s.groupResult.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += c.groupResult.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count();
                    //                             return c;
                    //                         });
                    //    }
                    //    else
                    //    {
                    //        groups = unitOfWork.GroupRepository.Get(s => s.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c => new GroupSearchViewModel
                    //                         {
                    //                             groupResult = c,
                    //                             relevance = c.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count()
                    //                         });
                    //        queryHasDone = true;
                    //    }


                    //}
                    //if (searchProfessions != null && searchProfessions.Length > 0)
                    //{
                    //    selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    //    if (queryHasDone)
                    //    {
                    //        groups = groups.Where(s => s.groupResult.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.groupResult.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        groups = unitOfWork.GroupRepository.Get(s => s.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c => new GroupSearchViewModel
                    //                       {
                    //                           groupResult = c,
                    //                           relevance = c.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //if (!queryHasDone)
                    //{
                    //    groups = unitOfWork.GroupRepository.Get()
                    //                          .Select(c => new GroupSearchViewModel { groupResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //searchPanel.CategoryPanel = groups.SelectMany(f => f.groupResult.Categories)
                    //                                      .GroupBy(p => p)
                    //                                      .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                      .OrderByDescending(f => f.count)
                    //                                      .Take(5);
                    //searchPanel.ProfessionPanel = groups.SelectMany(f => f.groupResult.Professions)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelProfessions { profession = p.Key, count = p.Count(), checkedBox = selectedProfessionsHS.Contains(p.Key.profID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);

                    //searchViewModel = groups.Select(f => new SearchViewModel
                    //{
                    //    groups = f.groupResult,
                    //    relevance = f.relevance
                    //});
                    break;
                #endregion
                #region Webinar
                case SearchType.Webinar:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchProfessions != null && searchProfessions.Length > 0)
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }
                    if (searchCats != null && searchCats.Length > 0)
                    {
                        prams.Add(new SqlParameter("cats", String.Join(",", searchCats)));
                        selectedCategoriesHS = new HashSet<int>(searchCats.Select(u => int.Parse(u)));
                    }

                    //switch (sortOrder)
                    //{
                    //    case "Date":
                    //        prams.Add(new SqlParameter("sort", "eventDate"));
                    //        break;
                    //    case "date_desc":
                    //        prams.Add(new SqlParameter("sort", "eventDate DESC"));
                    //        break;
                    //    case "attendors":
                    //        prams.Add(new SqlParameter("sort", "Attendors"));
                    //        break;
                    //    case "expired":
                    //        prams.Add(new SqlParameter("atndr", "Expired"));
                    //        break;
                    //}

                    var webinarReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchWebinar", prams, out outputCommand);
                    List<WebinarSearchViewModel> Wns = new List<WebinarSearchViewModel>();
                    finalModel = Wns;
                    while (webinarReader.Read())
                    {
                        var svm = new WebinarSearchViewModel();
                        Seminar w = new Seminar();
                        w.seminarId = webinarReader.GetInt32(1);
                        w.title = webinarReader.GetString(2);
                        w.desc = webinarReader.GetString(3);
                        w.date = webinarReader.GetDateTime(4);
                        w.duration = webinarReader.GetInt16(5);
                        w.maxAudiences = webinarReader.GetInt16(6);
                        w.isPublic = webinarReader.GetBoolean(7);
                        w.price = webinarReader.GetInt32(8);

                        svm.seminarResult = w;

                        Wns.Add(svm);
                    }
                    finalModel = Wns;
                    webinarReader.NextResult();


                    while (webinarReader.Read())
                    {
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = webinarReader.GetInt32(0);
                        prf.professionName = webinarReader.GetString(1);
                        prf.professionNameEN = webinarReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = webinarReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    webinarReader.NextResult();

                    while (webinarReader.Read())
                    {
                        var pf = new SearchPanelCategories();
                        Category cat = new Category();
                        cat.catID = webinarReader.GetInt32(0);
                        cat.catName = webinarReader.GetString(1);
                        cat.catNameEN = webinarReader.GetString(2);
                        pf.category = cat;
                        pf.checkedBox = selectedCategoriesHS.Contains(cat.catID) ? true : false;
                        pf.count = webinarReader.GetInt32(3);
                        cts.Add(pf);
                    }
                    searchPanel.CategoryPanel = cts;
                    webinarReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;
                    //if (words.Count() > 0)
                    //{
                    //    webinars = unitOfWork.SeminarRepository
                    //                        .Get(s => words.Any(q => s.title.ToUpper().Contains(q)))
                    //                        .Select(f => new WebinarSearchViewModel
                    //                        {
                    //                            seminarResult = f,
                    //                            relevance = f.title.ToUpper()
                    //                                .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    //                                .Distinct()
                    //                                .Intersect(words)
                    //                                .Count()
                    //                        });
                    //    queryHasDone = true;
                    //}
                    //if (searchCats != null && searchCats.Length > 0)
                    //{
                    //    selectedCategoriesHS = new HashSet<int>(searchCats.Select(e => int.Parse(e)));
                    //    if (queryHasDone)
                    //    {
                    //        webinars = webinars.Where(s => s.seminarResult.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c =>
                    //                         {
                    //                             c.relevance += c.seminarResult.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count();
                    //                             return c;
                    //                         });
                    //    }
                    //    else
                    //    {
                    //        webinars = unitOfWork.SeminarRepository.Get(s => s.Categories.Any(j => selectedCategoriesHS.Contains(j.catID)))
                    //                         .Select(c => new WebinarSearchViewModel
                    //                         {
                    //                             seminarResult = c,
                    //                             relevance = c.Categories.Select(j => j.catID)
                    //                                             .Intersect(selectedCategoriesHS).Count()
                    //                         });
                    //        queryHasDone = true;
                    //    }


                    //}
                    //if (searchProfessions != null && searchProfessions.Length > 0)
                    //{
                    //    selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    //    if (queryHasDone)
                    //    {
                    //        webinars = webinars.Where(s => s.seminarResult.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c =>
                    //                       {
                    //                           c.relevance += c.seminarResult.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count();
                    //                           return c;
                    //                       });
                    //    }
                    //    else
                    //    {
                    //        webinars = unitOfWork.SeminarRepository.Get(s => s.Professions.Any(j => selectedProfessionsHS.Contains(j.profID)))
                    //                       .Select(c => new WebinarSearchViewModel
                    //                       {
                    //                           seminarResult = c,
                    //                           relevance = c.Professions.Select(j => j.profID)
                    //                                           .Intersect(selectedCategoriesHS).Count()
                    //                       });
                    //        queryHasDone = true;
                    //    }
                    //}
                    //if (!queryHasDone)
                    //{
                    //    webinars = unitOfWork.SeminarRepository.Get()
                    //                          .Select(c => new WebinarSearchViewModel { seminarResult = c, relevance = 0 });
                    //    queryHasDone = true;
                    //}

                    //searchPanel.CategoryPanel = webinars.SelectMany(f => f.seminarResult.Categories)
                    //                                      .GroupBy(p => p)
                    //                                      .Select(p => new SearchPanelCategories { category = p.Key, count = p.Count(), checkedBox = selectedCategoriesHS.Contains(p.Key.catID) ? true : false })
                    //                                      .OrderByDescending(f => f.count)
                    //                                      .Take(5);
                    //searchPanel.ProfessionPanel = webinars.SelectMany(f => f.seminarResult.Professions)
                    //                                  .GroupBy(p => p)
                    //                                  .Select(p => new SearchPanelProfessions { profession = p.Key, count = p.Count(), checkedBox = selectedProfessionsHS.Contains(p.Key.profID) ? true : false })
                    //                                  .OrderByDescending(f => f.count)
                    //                                  .Take(5);

                    //searchViewModel = webinars.Select(f => new SearchViewModel
                    //{
                    //    webinars = f.seminarResult,
                    //    relevance = f.relevance
                    //});
                    break;
                #endregion
                #region Dictionary
                case SearchType.Dictionary:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchProfessions != null && searchProfessions.Length > 0)
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }


                    var dicReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchDictionary", prams, out outputCommand);
                    var Dss = new List<DictionarySearchViewModel>();
                    finalModel = Dss;
                    while (dicReader.Read())
                    {
                        var svm = new DictionarySearchViewModel();
                        var d = new Dict();
                        d.dicId = dicReader.GetInt32(1);
                        d.name = dicReader.GetString(2);
                        d.desc = dicReader.GetString(3);


                        svm.dicResult = d;
                        svm.Words = dicReader.GetInt32(4);
                        Dss.Add(svm);
                    }
                    finalModel = Dss;
                    dicReader.NextResult();


                    while (dicReader.Read())
                    {
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = dicReader.GetInt32(0);
                        prf.professionName = dicReader.GetString(1);
                        prf.professionNameEN = dicReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = dicReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    dicReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;;

                    break;
                #endregion
                #region Word
                case SearchType.Word:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));

                    var wordReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchWord", prams, out outputCommand);
                    var Wss = new List<WordSearchViewModel>();
                    finalModel = Wss;
                    while (wordReader.Read())
                    {
                        var svm = new WordSearchViewModel();
                        var d = new Word();
                        d.wordId = wordReader.GetInt32(1);
                        d.lang = (lang)wordReader.GetInt32(2);
                        d.word = wordReader[3] as string;


                        svm.wordResult = d;
                        Wss.Add(svm);
                    }
                    finalModel = Wss;

                    wordReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value; ;

                    break;
                #endregion
                #region Book
                case SearchType.Book:

                    if (!String.IsNullOrWhiteSpace(searchString)) prams.Add(new SqlParameter("text", searchString));
                    if (searchProfessions != null && searchProfessions.Length > 0)
                    {
                        prams.Add(new SqlParameter("proffs", String.Join(",", searchProfessions)));
                        selectedProfessionsHS = new HashSet<int>(searchProfessions.Select(u => int.Parse(u)));
                    }

                    var bookReader = unitOfWork.ReaderRepository.GetSPDataReader("TotalSearchBook", prams, out outputCommand);
                    var Bss = new List<BookSearchViewModel>();
                    finalModel = Bss;
                    while (bookReader.Read())
                    {
                        var svm = new BookSearchViewModel();
                        var b = new Book();
                        b.BookId = bookReader.GetInt32(1);
                        b.title = bookReader[2] as string;
                        b.abtrct = bookReader[3] as string;
                        b.writer = bookReader[4] as string;
                        b.image = bookReader[5] as string;

                        svm.bookResult = b;
                        Bss.Add(svm);
                    }
                    finalModel = Bss;
                    bookReader.NextResult();


                    while (bookReader.Read())
                    {
                        var pf = new SearchPanelProfessions();
                        Profession prf = new Profession();
                        prf.profID = bookReader.GetInt32(0);
                        prf.professionName = bookReader.GetString(1);
                        prf.professionNameEN = bookReader.GetString(2);
                        pf.profession = prf;
                        pf.checkedBox = selectedProfessionsHS.Contains(prf.profID) ? true : false;
                        pf.count = bookReader.GetInt32(3);
                        fss.Add(pf);
                    }
                    searchPanel.ProfessionPanel = fss;
                    bookReader.Close();

                    TotalRows = (int)outputCommand.Parameters["TotalRows"].Value;


                    break;
                #endregion
            }

            ViewData["finalPage"] = TotalRows < pageSize * page;
            ViewData["pageNum"] = page;

            return Json(new
            {
                Result = RenderPartialViewHelper.RenderPartialView(this,
                                                                   "Search" + searchType,
                                                                   finalModel),//.OrderByDescending(f => f.relevance).ToPagedList((int)page, pageSize)),
                ResultCount = TotalRows.ToString("n0"),
                SearchPanel = RenderPartialViewHelper.RenderPartialView(this,
                                                                        "TotalSearchPanel",
                                                                        searchPanel)
            }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (AuthorizationHelper.isAdmin())
                {
                    return RedirectToAction("MainMenu", "Admin");
                }
                return View("~/Views/UserProfile/Home.cshtml");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult AboutUs()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Rules()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult _AllSearchPartial(string searchString)
        {
            SearchAllMini searchViewModel = new SearchAllMini();
            //var searchWords = searchString.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                        .Where(w => w.Length > 2);
            var reader = unitOfWork.ReaderRepository.GetSPDataReader(
                         "SearchAll",
                         new SqlParameter("text",searchString),
                         new SqlParameter("isNotEN",ITTConfig.CurrentCultureIsNotEN));
            searchViewModel.users = new List<ActiveUser>();
            searchViewModel.questions = new List<Question>();
            searchViewModel.books = new List<Book>();
            searchViewModel.groups = new List<Group>();
            searchViewModel.dicts = new List<Dict>();
            searchViewModel.categories = new List<Category>();
            searchViewModel.professions = new List<Profession>();
            searchViewModel.companies = new List<Company>();
            searchViewModel.stores = new List<Store>();
            searchViewModel.products = new List<Product>();
            searchViewModel.services = new List<Service>();


            while (reader.Read())
            {
                searchViewModel.users.Add(
                    new ActiveUser
                    {
                        UserId = reader.GetInt32(0),
                        firstName = reader[1] as string,
                        image = reader[2] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.questions.Add(
                    new Question
                    {
                        questionID = reader.GetInt32(0),
                        questionSubject = reader[1] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.groups.Add(
                    new Group
                    {
                        groupId = reader.GetInt32(0),
                        groupName = reader[1] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.dicts.Add(
                    new Dict
                    {
                        dicId = reader.GetInt32(0),
                        name = reader[1] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.categories.Add(
                    new Category
                    {
                        catID = reader.GetInt32(0),
                        catName = reader[1] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.professions.Add(
                    new Profession
                    {
                        profID= reader.GetInt32(0),
                        professionName = reader[1] as string
                    });
            }
            reader.NextResult();

            while (reader.Read())
            {
                searchViewModel.books.Add(
                    new Book
                    {
                        BookId = reader.GetInt32(0),
                        title = reader[1] as string
                    });
            }
            reader.Close();

            //#region Users
            //searchViewModel.users = unitOfWork.ActiveUserRepository
            //                                .Get(s => searchWords.Any(q => s.firstName.ToUpper().Contains(q)
            //                                                      || s.firstNameEN.ToUpper().Contains(q)
            //                                                      || s.lastName.ToUpper().Contains(q)
            //                                                      || s.lastNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new UserSearchViewModel
            //                                {
            //                                    userResult = f,
            //                                    relevance = String.Concat(f.firstName, ' ',
            //                                                              f.firstNameEN, ' ',
            //                                                              f.lastName, ' ',
            //                                                              f.lastNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o=>o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Companies
            //searchViewModel.companies = unitOfWork.NotExpiredCompanyRepository
            //                                .Get(s => searchWords.Any(q => s.coName.ToUpper().Contains(q)
            //                                                      || s.coNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new CompanySearchViewModel
            //                                {
            //                                    companyResult = f,
            //                                    relevance = String.Concat(f.coName, ' ', f.coNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Stores
            //searchViewModel.stores = unitOfWork.StoreNotExpiredRepository
            //                                .Get(s => searchWords.Any(q => s.storeName.ToUpper().Contains(q)
            //                                                      || s.storeNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new StoreSearchViewModel
            //                                {
            //                                    storeResult = f,
            //                                    relevance = String.Concat(f.storeName, ' ',
            //                                                              f.storeNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Products
            //searchViewModel.products = unitOfWork.ProductRepository
            //                                .Get(s => searchWords.Any(q => s.productName.ToUpper().Contains(q)
            //                                                      || s.productNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new ProductsSearchViewModel
            //                                {
            //                                    productResult = f,
            //                                    relevance = String.Concat(f.productName, ' ',
            //                                                              f.productNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Services
            //searchViewModel.services = unitOfWork.ServiceRepository
            //                                .Get(s => searchWords.Any(q => s.serviceName.ToUpper().Contains(q)
            //                                                      || s.serviceNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new ServicesSearchViewModel
            //                                {
            //                                    serviceResult = f,
            //                                    relevance = String.Concat(f.serviceName, ' ',
            //                                                              f.serviceNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Professions
            //searchViewModel.professions = unitOfWork.ProfessionRepository
            //                                .Get(s => searchWords.Any(q => s.professionName.ToUpper().Contains(q)
            //                                                      || s.professionNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new ProfessionSearchViewModel
            //                                {
            //                                    professionResult = f,
            //                                    relevance = String.Concat(f.professionName, ' ',
            //                                                              f.professionNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Categories
            //searchViewModel.categories = unitOfWork.CategoryRepository
            //                                .Get(s => searchWords.Any(q => s.catName.ToUpper().Contains(q)
            //                                                      || s.catNameEN.ToUpper().Contains(q)))
            //                                .Select(f => new CategorySearchViewModel
            //                                {
            //                                    categoryResult = f,
            //                                    relevance = String.Concat(f.catName, ' ',
            //                                                              f.catNameEN).ToUpper()
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
            //#region Questions
            //searchViewModel.questions = unitOfWork.QuestionRepository
            //                                .Get(s => searchWords.Any(q => s.questionSubject.ToUpper().Contains(q)))
            //                                .Select(f => new QuestionSearchViewModel
            //                                {
            //                                    questionResult = f,
            //                                    relevance = String.Concat(f.questionSubject.ToUpper())
            //                                        .Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                                        .Distinct()
            //                                        .Intersect(searchWords)
            //                                        .Count()
            //                                })
            //                                .OrderByDescending(o => o.relevance)
            //                                .Take(2);
            //#endregion
           
            return PartialView(searchViewModel);
        }

        [AllowAnonymous]
        public ActionResult FileSlideshow(string folder, string fileNamesString)
        {
            HashSet<string> files = new HashSet<string>(fileNamesString.Split(new char[] { ',' }));
            var final = from s in files
                        select new FilesBarViewModel
                        {
                            filepath = String.Concat("~/Uploads/", s.Substring(s.LastIndexOf('.') + 1), "/", folder, "/", s),
                        };

            return PartialView(final);

        }

        [HttpPost]
        public string postFileUpload(HttpPostedFileBase file)
        {
            string filepath = null;
            foreach (string item in Request.Files)
            {
                file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    filepath = UploadHelper.renameUploadFile(file, ITTConfig.ImageHeightPost);
                }
            }
            return filepath;
        }

        [HttpPost]
        public string ProfilePicUpload(HttpPostedFileBase file)
        {
            string filepath = null;
            foreach (string item in Request.Files)
            {
                file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    filepath = UploadHelper.renameUploadFile(file, ITTConfig.ImageHeightProfile, true);
                }
            }
            return filepath;
        }

        [HttpPost]
        public string ProServFileUpload(HttpPostedFileBase file)
        {
            string filepath = null;
            foreach (string item in Request.Files)
            {
                file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    filepath = UploadHelper.renameUploadFile(file, ITTConfig.ImageHeightProServ);
                }
            }
            return filepath;
        }

        [HttpPost]
        public string BookFileUpload(HttpPostedFileBase file)
        {
            string filepath = null;
            foreach (string item in Request.Files)
            {
                file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    filepath = UploadHelper.renameUploadFile(file, ITTConfig.ImageHeightBook,fileSizeLimit: ITTConfig.FileSizeLimitBook);
                }
            }
            return filepath;
        }

        [AllowAnonymous]
        public ActionResult FileList(string folder, string fileNamesString)
        {
            HashSet<string> files = new HashSet<string>(fileNamesString.Split(new char[] { ',' }));
            var final = from s in files
                        let ext = s.Substring(s.LastIndexOf('.') + 1)
                        select new FilesBarViewModel
                      {
                          filepath = String.Concat("~/Uploads/", ext, "/", folder, "/", s),
                          fileIcon = String.Concat("icon-", ext)
                      };

            return PartialView(final);
           
        }

        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            //// Validate input
            //culture = CultureHelper.GetImplementedCulture(culture);
            //// Save culture in a cookie
            //HttpCookie cookie = Request.Cookies["IT_culture"];
            //if (cookie != null)
            //    cookie.Value = culture;   // update cookie value
            //else
            //{
            //    cookie = new HttpCookie("IT_culture");
            //    cookie.Value = culture;
            //    cookie.Expires = DateTime.Now.AddYears(1);
            //}
            //Response.Cookies.Add(cookie);
            //return Redirect(Request.UrlReferrer.ToString());

            var returnUrl = Request.UrlReferrer.PathAndQuery.ToString();
            if (returnUrl.Length >= 3)
            {
                returnUrl = returnUrl.Substring(3);
            }
            //HttpContext.Session["_CIFa"] = culture.StartsWith("fa");
            //HttpCookie culCookie = new HttpCookie("_ITCul");
            //culCookie.Value = culture.StartsWith("fa").ToString();
            //HttpContext.Response.Cookies.Add(culCookie);

            return Redirect("/" + culture.ToString() + returnUrl);
        }

        [AllowAnonymous]
        public ActionResult populateDopDownStates(int? countryId = null, object selectedItem = null)
        {
            var countries = unitOfWork.CountstateRepository.Get(c => c.countryID == countryId)
                                                           .OrderBy(f=>f.CultureStateName);
            if (ITTConfig.CurrentCultureIsNotEN)
            {
                return Json(new SelectList(countries, "stateID", "stateName", selectedItem));
            }
            else return Json(new SelectList(countries, "stateID", "stateNameEN", selectedItem));
        }

        [AllowAnonymous]
        public ActionResult _StatesSearchPartial(string searchString)
        {
            var states = unitOfWork.CountstateRepository.Get(c => c.stateName.Contains(searchString)
                                                                       || c.stateNameEN.Contains(searchString)).Take(10);
            return PartialView(states);
        }

        [AllowAnonymous]
        public ActionResult generateCaptcha()
        {
            System.Drawing.FontFamily family = new System.Drawing.FontFamily("Arial");
            CaptchaImage img = new CaptchaImage(150, 50, family);
            string text = img.CreateRandomText(4) + " " + img.CreateRandomText(3);
            img.SetText(text);
            img.GenerateImage();
            img.Image.Save(Server.MapPath("~") +
            this.Session.SessionID.ToString() + ".png",
            System.Drawing.Imaging.ImageFormat.Png);
            Session["captchaText"] = text;
            return Json(this.Session.SessionID.ToString() + ".png?t=" +
            DateTime.Now.Ticks, JsonRequestBehavior.AllowGet);
        } 
    }
}
