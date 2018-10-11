using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{

    public enum SearchType
    {
        ALL = 0,
        Company = 1,
        Store = 2,
        People = 3,
        Product = 4,
        Service = 5,
        Project = 6,
        Job = 7,
        Question = 8,
        Event = 9,
        Group = 10,
        Webinar = 11,
        Dictionary = 12,
        Word = 13,
        Book = 14
    }

    public class UserSearchViewModel
    {
        public ActiveUser userResult { get; set; }
        public int relevance { get; set; }
    }
    public class CompanySearchViewModel
    {
        public Company companyResult { get; set; }
        public int relevance { get; set; }
    }
    public class StoreSearchViewModel
    {
        public Store storeResult { get; set; }
        public int relevance { get; set; }
    }

    #region ProductsViewModels
    public class ProductsSearchViewModel
    {
        public Product productResult { get; set; }
        public int relevance { get; set; }
    }
    //public class ProductsSearchViewModel
    //{
    //    public Product productResult { get; set; }
    //    public int relevance { get; set; }
    //}
    //public class ProductSearchViewModel
    //{
    //    public Company company { get; set; }
    //    public IEnumerable<ProductsSearchViewModel> product { get; set; }

    //    public int maxRelevance { get; set; }
    //}
    #endregion

    #region ServicesViewModels
    public class ServicesSearchViewModel
    {
        public Service serviceResult { get; set; }
        public int relevance { get; set; }
    }
    public class ServiceSearchViewModel
    {
        public Company company { get; set; }
        public IEnumerable<ServicesSearchViewModel> service { get; set; }

        public int maxRelevance { get; set; }
    }
    #endregion
    public class JobSearchViewModel
    {
        public Job jobResult { get; set; }
        public int relevance { get; set; }
    }
    public class ProjectSearchViewModel
    {
        public Project projectResult { get; set; }
        public int relevance { get; set; }
    }

    public class QuestionSearchViewModel
    {
        public Question questionResult { get; set; }
        public int relevance { get; set; }
        public int Answers { get; set; }
    }

    public class BookSearchViewModel
    {
        public Book bookResult { get; set; }
        public int relevance { get; set; }
    }

    public class DictionarySearchViewModel
    {
        public Dict dicResult { get; set; }
        public int relevance { get; set; }
        public int Words { get; set; }
    }

    public class WordSearchViewModel
    {
        public Word wordResult { get; set; }
        public int relevance { get; set; }
    }

    public class EventSearchViewModel
    {
        public Event eventResult { get; set; }
        public int relevance { get; set; }
        public int Attendors { get; set; }
    }

    public class GroupSearchViewModel
    {
        public Group groupResult { get; set; }
        public int relevance { get; set; }
        public int Sessions { get; set; }
        public int Members { get; set; }
    }
    public class WebinarSearchViewModel
    {
        public Seminar seminarResult { get; set; }
        public int relevance { get; set; }
        public string Discriminator { get; set; }

    }


    public class ProfessionSearchViewModel
    {
        public Profession professionResult { get; set; }
        public int relevance { get; set; }
    }

    public class CategorySearchViewModel
    {
        public Category categoryResult { get; set; }
        public int relevance { get; set; }
    }

    public class SearchViewModel
    {
        public ActiveUser user { get; set; }
        public Company company { get; set; }
        public Store store { get; set; }
        public Project project { get; set; }
        public Job job { get; set; }
        public QuestionSearchViewModel question { get; set; }
        public Event events { get; set; }
        public Group groups { get; set; }
        public Seminar webinars { get; set; }

        public Product products { get; set; }
        public Service services { get; set; }

        //public ProductSearchViewModel products { get; set; }
        //public ServiceSearchViewModel services { get; set; }

        //public int relevance { get; set; }
 
    }

    public class SearchPanelStates
    {
        public CountState state { get; set; }
        public int count { get; set; }
        public bool checkedBox { get; set; }
    }

    public class SearchPanelCategories
    {
        public Category category { get; set; }
        public int count { get; set; }
        public bool checkedBox { get; set; }
    }

    public class SearchPanelProfessions
    {
        public Profession profession { get; set; }
        public int count { get; set; }
        public bool checkedBox { get; set; }
    }


    public class SearchPanelCompanies
    {
        public Company company { get; set; }
        public int count { get; set; }
        public bool checkedBox { get; set; }
    }

    public class SearchPanelCompanySize
    {
        public CompanySize companySize { get; set; }
        public int count { get; set; }
        public bool checkedBox { get; set; }
    }

    public class SearchPanelViewModel
    {
        public IEnumerable<SearchPanelStates> StatePanel { get; set; }
        public IEnumerable<SearchPanelCategories> CategoryPanel { get; set; }
        public IEnumerable<SearchPanelProfessions> ProfessionPanel { get; set; }
        public IEnumerable<SearchPanelCompanies> CompanyPanel { get; set; }
        public IEnumerable<SearchPanelCompanySize> CompanySizePanel { get; set; }

    }

    public class SearchAllMini
    {
        public IList<ActiveUser> users { get; set; }
        public IList<Company> companies { get; set; }
        public IList<Store> stores { get; set; }
        public IList<Product> products { get; set; }
        public IList<Service> services { get; set; }
        public IList<Profession> professions { get; set; }
        public IList<Category> categories { get; set; }
        public IList<Question> questions { get; set; }
        public IList<Group> groups { get; set; }
        public IList<Dict> dicts { get; set; }
        public IList<Book> books { get; set; }

    }
}