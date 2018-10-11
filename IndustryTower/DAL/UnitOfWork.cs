using IndustryTower.Models;
using System;

namespace IndustryTower.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ITTContext context = new ITTContext();
        private GenericRepository<UserProfile> userRepository;
        private GenericRepository<ActiveUser> activeUserRepository;
        private GenericRepository<AdminUser> adminUserRepository;
        private GenericRepository<webpages_Membership> webpages_MembershipRepository;
        private GenericRepository<webpages_Roles> webpages_RolesRepository;
        private GenericRepository<webpages_UsersInRoles> webpages_UsersInRolesRepository;
        private GenericRepository<webpages_OAuthMembership> webpages_OAuthMembershipRepository;
        private GenericRepository<Abuse> abuseRepository;
        private GenericRepository<Answer> answerRepository;
        private GenericRepository<Badge> badgeRepository;
        private GenericRepository<BadgeUser> badgeUserRepository;
        private GenericRepository<Book> bookRepository;
        private GenericRepository<CommentPost> commentPostRepository;
        private GenericRepository<CommentQuestion> commentQuestionRepository;
        private GenericRepository<CommentAnswer> commentAnswerRepository;
        private GenericRepository<CommentGSO> commentGSORepository;
        private GenericRepository<CommentProduct> commentProductRepository;
        private GenericRepository<CommentService> commentServiceRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Certificate> certificateRepository;
        private GenericRepository<Company> companyRepository;
            private GenericRepository<CompanyNotExpired> notExpiredCompanyRepository;
            private GenericRepository<CompanyExpired> expiredCompanyRepository;
        private GenericRepository<CountState> countstateRepository;
        private GenericRepository<Dict> dictionaryRepository;
        private GenericRepository<Education> educationRepository;
        private GenericRepository<Event> eventRepository;
        private GenericRepository<Experience> experienceRepository;
        private GenericRepository<Feed> feedRepository;
        private GenericRepository<Following> followingRepository;
        private GenericRepository<FriendRequest> friendshipRequestRepository;
        private GenericRepository<Friendship> friendshipRepository;
        private GenericRepository<Group> groupRepository;
        private GenericRepository<GroupSession> groupSessionRepository;
        private GenericRepository<GroupSessionOffer> groupSessionOfferRepository;
        private GenericRepository<GroupSesssionResult> groupSessionResultRepository;
        private GenericRepository<Job> jobRepository;
        private GenericRepository<JobOffer> jobOfferRepository;
        private GenericRepository<LikePost> likePostRepository;
        private GenericRepository<LikeQuestion> likeQuestionRepository;
        private GenericRepository<LikeAnswer> likeAnswerRepository;
        private GenericRepository<LikeGSO> likeGSORepository;
        private GenericRepository<LikeProduct> likeProductRepository;
        private GenericRepository<LikeService> likeServiceRepository;
        private GenericRepository<LikeComment> likeCommentRepository;
        private GenericRepository<Message> messageRepository;
        private GenericRepository<Notification> notificationRepository;
        private GenericRepository<Patent> patentRepository;
        private GenericRepository<Payment> paymentRepository;
        private GenericRepository<PlanRequest> planRequestRepository;
            private GenericRepository<RevivalRequest> revivalPlanRequestRepository;
            private GenericRepository<RequestForNew> newPlanRequestRepository;
        private GenericRepository<Post> postRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Profession> professionRepository;
        private GenericRepository<Project> projectRepository;
        private GenericRepository<ProjectOffer> projectofferRepository;
        private GenericRepository<Question> questionRepository;
        private GenericRepository<ReviewBook> reviewBookRepository;
        private GenericRepository<Seminar> seminarRepository;
            private GenericRepository<Webinar> webinarRepository;
            private GenericRepository<Workshop> workshopRepository;
            private GenericRepository<VideoConference> videoConferenceRepository;
        private GenericRepository<SeminarRequest> seminarReauestRepository;
        private GenericRepository<Service> serviceRepository;
        private GenericRepository<Store> storeRepository;
            private GenericRepository<StoreNotExpired> storeNotExpiredRepository;
            private GenericRepository<StoreExpired> storeExpiredRepository;
        private GenericRepository<Share> shareRepository;
        private GenericRepository<Setting> settingRepository;
        private GenericRepository<Word> wordRepository;
        private GenericRepository<WordEdit> wordEditRepository;
        private GenericRepository<WordDesc> wordDescRepository;
        private GenericRepository<WordDescEdit> wordDescEditRepository;

        private GenericReader readerRepository;


        public GenericReader ReaderRepository
        {
            get 
            {
                if (this.readerRepository == null)
                {
                    this.readerRepository = new GenericReader();
                }
                return readerRepository;
            }
        }

        public GenericRepository<UserProfile> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<UserProfile>(context);
                }
                return userRepository;
            }
        }

        public GenericRepository<ActiveUser> ActiveUserRepository
        {
            get
            {
                if (this.activeUserRepository == null)
                {
                    this.activeUserRepository = new GenericRepository<ActiveUser>(context);
                }
                return activeUserRepository;
            }
        }

        public GenericRepository<AdminUser> AdminUserRepository
        {
            get
            {
                if (this.adminUserRepository == null)
                {
                    this.adminUserRepository = new GenericRepository<AdminUser>(context);
                }
                return adminUserRepository;
            }
        }

        public GenericRepository<Book> BookRepository
        {
            get
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(context);
                }
                return bookRepository;
            }
        }

        public GenericRepository<webpages_Membership> Webpages_MembershipRepository
        {
            get
            {
                if (this.webpages_MembershipRepository == null)
                {
                    this.webpages_MembershipRepository = new GenericRepository<webpages_Membership>(context);
                }
                return webpages_MembershipRepository;
            }
        }

        public GenericRepository<webpages_Roles> Webpages_RolesRepository
        {
            get
            {
                if (this.webpages_RolesRepository == null)
                {
                    this.webpages_RolesRepository = new GenericRepository<webpages_Roles>(context);
                }
                return webpages_RolesRepository;
            }
        }

        public GenericRepository<webpages_UsersInRoles> Webpages_UsersInRolesRepository
        {
            get
            {
                if (this.webpages_UsersInRolesRepository == null)
                {
                    this.webpages_UsersInRolesRepository = new GenericRepository<webpages_UsersInRoles>(context);
                }
                return webpages_UsersInRolesRepository;
            }
        }

        public GenericRepository<webpages_OAuthMembership> Webpages_OAuthMembershipRepository
        {
            get
            {
                if (this.webpages_OAuthMembershipRepository == null)
                {
                    this.webpages_OAuthMembershipRepository = new GenericRepository<webpages_OAuthMembership>(context);
                }
                return webpages_OAuthMembershipRepository;
            }
        }

        public GenericRepository<Abuse> AbuseRepository
        {
            get
            {
                if (this.abuseRepository == null)
                {
                    this.abuseRepository = new GenericRepository<Abuse>(context);
                }
                return abuseRepository;
            }
        }

        public GenericRepository<Answer> AnswerRepository
        {
            get
            {
                if (this.answerRepository == null)
                {
                    this.answerRepository = new GenericRepository<Answer>(context);
                }
                return answerRepository;
            }
        }

        public GenericRepository<Badge> BadgeRepository
        {
            get
            {
                if (this.badgeRepository == null)
                {
                    this.badgeRepository = new GenericRepository<Badge>(context);
                }
                return badgeRepository;
            }
        }

        public GenericRepository<BadgeUser> BadgeUserRepository
        {
            get
            {
                if (this.badgeUserRepository == null)
                {
                    this.badgeUserRepository = new GenericRepository<BadgeUser>(context);
                }
                return badgeUserRepository;
            }
        }
        public GenericRepository<CommentPost> CommentPostRepository
        {
            get
            {
                if (this.commentPostRepository == null)
                {
                    this.commentPostRepository = new GenericRepository<CommentPost>(context);
                }
                return commentPostRepository;
            }
        }
        public GenericRepository<CommentQuestion> CommentQuestionRepository
        {
            get
            {
                if (this.commentQuestionRepository == null)
                {
                    this.commentQuestionRepository = new GenericRepository<CommentQuestion>(context);
                }
                return commentQuestionRepository;
            }
        }

        public GenericRepository<CommentAnswer> CommentAnswerRepository
        {
            get
            {
                if (this.commentAnswerRepository == null)
                {
                    this.commentAnswerRepository = new GenericRepository<CommentAnswer>(context);
                }
                return commentAnswerRepository;
            }
        }
        public GenericRepository<CommentGSO> CommentGSORepository
        {
            get
            {
                if (this.commentGSORepository == null)
                {
                    this.commentGSORepository = new GenericRepository<CommentGSO>(context);
                }
                return commentGSORepository;
            }
        }
        public GenericRepository<CommentProduct> CommentProductRepository
        {
            get
            {
                if (this.commentProductRepository == null)
                {
                    this.commentProductRepository = new GenericRepository<CommentProduct>(context);
                }
                return commentProductRepository;
            }
        }
        public GenericRepository<CommentService> CommentServiceRepository
        {
            get
            {
                if (this.commentServiceRepository == null)
                {
                    this.commentServiceRepository = new GenericRepository<CommentService>(context);
                }
                return commentServiceRepository;
            }
        }
        
        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new GenericRepository<Category>(context);
                }
                return categoryRepository;
            }
        }

        public GenericRepository<Certificate> CertificateRepository
        {
            get
            {
                if (this.certificateRepository == null)
                {
                    this.certificateRepository = new GenericRepository<Certificate>(context);
                }
                return certificateRepository;
            }
        }

        public GenericRepository<Company> CompanyRepository
        {
            get
            {
                if (this.companyRepository == null)
                {
                    this.companyRepository = new GenericRepository<Company>(context);
                }
                return companyRepository;
            }
        }

        public GenericRepository<CompanyNotExpired> NotExpiredCompanyRepository
        {
            get
            {
                if (this.notExpiredCompanyRepository == null)
                {
                    this.notExpiredCompanyRepository = new GenericRepository<CompanyNotExpired>(context);
                }
                return notExpiredCompanyRepository;
            }
        }

        public GenericRepository<CompanyExpired> ExpiredCompanyRepository
        {
            get
            {
                if (this.expiredCompanyRepository == null)
                {
                    this.expiredCompanyRepository = new GenericRepository<CompanyExpired>(context);
                }
                return expiredCompanyRepository;
            }
        }

        public GenericRepository<CountState> CountstateRepository
        {
            get
            {
                if (this.countstateRepository == null)
                {
                    this.countstateRepository = new GenericRepository<CountState>(context);
                }
                return countstateRepository;
            }
        }

        public GenericRepository<Dict> DictionaryRepository
        {
            get
            {
                if (this.dictionaryRepository == null)
                {
                    this.dictionaryRepository = new GenericRepository<Dict>(context);
                }
                return dictionaryRepository;
            }
        }

        public GenericRepository<Education> EducationRepository
        {
            get
            {
                if (this.educationRepository == null)
                {
                    this.educationRepository = new GenericRepository<Education>(context);
                }
                return educationRepository;
            }
        }
        
        public GenericRepository<Event> EventRepository
        {
            get
            {
                if (this.eventRepository == null)
                {
                    this.eventRepository = new GenericRepository<Event>(context);
                }
                return eventRepository;
            }
        }

        public GenericRepository<Experience> ExperienceRepository
        {
            get
            {
                if (this.experienceRepository == null)
                {
                    this.experienceRepository = new GenericRepository<Experience>(context);
                }
                return experienceRepository;
            }
        }

        public GenericRepository<Feed> FeedRepository
        {
            get
            {
                if (this.feedRepository == null)
                {
                    this.feedRepository = new GenericRepository<Feed>(context);
                }
                return feedRepository;
            }
        }

        public GenericRepository<Following> FollowingRepository
        {
            get
            {
                if (this.followingRepository == null)
                {
                    this.followingRepository = new GenericRepository<Following>(context);
                }
                return followingRepository;
            }
        }
        
        public GenericRepository<FriendRequest> FriendshipRequestRepository
        {
            get
            {
                if (this.friendshipRequestRepository == null)
                {
                    this.friendshipRequestRepository = new GenericRepository<FriendRequest>(context);
                }
                return friendshipRequestRepository;
            }
        }

        public GenericRepository<Friendship> FriendshipRepository
        {
            get
            {
                if (this.friendshipRepository == null)
                {
                    this.friendshipRepository = new GenericRepository<Friendship>(context);
                }
                return friendshipRepository;
            }
        }

        public GenericRepository<Group> GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository = new GenericRepository<Group>(context);
                }
                return groupRepository;
            }
        }

        public GenericRepository<GroupSession> GroupSessionRepository
        {
            get
            {
                if (this.groupSessionRepository == null)
                {
                    this.groupSessionRepository = new GenericRepository<GroupSession>(context);
                }
                return groupSessionRepository;
            }
        }

        public GenericRepository<GroupSessionOffer> GroupSessionOfferRepository
        {
            get
            {
                if (this.groupSessionOfferRepository == null)
                {
                    this.groupSessionOfferRepository = new GenericRepository<GroupSessionOffer>(context);
                }
                return groupSessionOfferRepository;
            }
        }

        public GenericRepository<GroupSesssionResult> GroupSessionResultRepository
        {
            get
            {
                if (this.groupSessionResultRepository == null)
                {
                    this.groupSessionResultRepository = new GenericRepository<GroupSesssionResult>(context);
                }
                return groupSessionResultRepository;
            }
        }

        public GenericRepository<Job> JobRepository
        {
            get
            {
                if (this.jobRepository == null)
                {
                    this.jobRepository = new GenericRepository<Job>(context);
                }
                return jobRepository;
            }
        }

        public GenericRepository<JobOffer> JobOfferRepository
        {
            get
            {
                if (this.jobOfferRepository == null)
                {
                    this.jobOfferRepository = new GenericRepository<JobOffer>(context);
                }
                return jobOfferRepository;
            }
        }

        public GenericRepository<LikePost> LikePostRepository
        {
            get
            {
                if (this.likePostRepository == null)
                {
                    this.likePostRepository = new GenericRepository<LikePost>(context);
                }
                return likePostRepository;
            }
        }
        public GenericRepository<LikeQuestion> LikeQuestionRepository
        {
            get
            {
               if (this.likeQuestionRepository == null)
                {
                    this.likeQuestionRepository = new GenericRepository<LikeQuestion>(context);
                }
               return likeQuestionRepository;
            }
        }
        public GenericRepository<LikeAnswer> LikeAnswerRepository
        {
            get
            {
                if (this.likeAnswerRepository == null)
                {
                    this.likeAnswerRepository = new GenericRepository<LikeAnswer>(context);
                }
                return likeAnswerRepository;
            }
        }
        public GenericRepository<LikeGSO> LikeGSORepository
        {
            get
            {
                if (this.likeGSORepository == null)
                {
                    this.likeGSORepository = new GenericRepository<LikeGSO>(context);
                }
                return likeGSORepository;
            }
        }
        public GenericRepository<LikeProduct> LikeProductRepository
        {
            get
            {
                if (this.likeProductRepository == null)
                {
                    this.likeProductRepository = new GenericRepository<LikeProduct>(context);
                }
                return likeProductRepository;
            }
        }
        public GenericRepository<LikeService> LikeServiceRepository
        {
            get
            {
                if (this.likeServiceRepository == null)
                {
                    this.likeServiceRepository = new GenericRepository<LikeService>(context);
                }
                return likeServiceRepository;
            }
        }
        public GenericRepository<LikeComment> LikeCommentRepository
        {
            get
            {
                if (this.likeCommentRepository == null)
                {
                    this.likeCommentRepository = new GenericRepository<LikeComment>(context);
                }
                return likeCommentRepository;
            }
        }

        public GenericRepository<Message> MessageRepository
        {
            get
            {
                if (this.messageRepository == null)
                {
                    this.messageRepository = new GenericRepository<Message>(context);
                }
                return messageRepository;
            }
        }

        public GenericRepository<Notification> NotificationRepository
        {
            get
            {
                if (this.notificationRepository == null)
                {
                    this.notificationRepository = new GenericRepository<Notification>(context);
                }
                return notificationRepository;
            }
        }


        public GenericRepository<Patent> PatentRepository
        {
            get
            {
                if (this.patentRepository == null)
                {
                    this.patentRepository = new GenericRepository<Patent>(context);
                }
                return patentRepository;
            }
        }
        

        public GenericRepository<Payment> PaymentRepository
        {
            get
            {
                if (this.paymentRepository == null)
                {
                    this.paymentRepository = new GenericRepository<Payment>(context);
                }
                return paymentRepository;
            }
        }

        public GenericRepository<PlanRequest> PlanRequetRepository
        {
            get
            {
                if (this.planRequestRepository == null)
                {
                    this.planRequestRepository = new GenericRepository<PlanRequest>(context);
                }
                return planRequestRepository;
            }
        }


        public GenericRepository<RevivalRequest> RevivalPlanRequestRepository
        {
            get
            {
                if (this.revivalPlanRequestRepository == null)
                {
                    this.revivalPlanRequestRepository = new GenericRepository<RevivalRequest>(context);
                }
                return revivalPlanRequestRepository;
            }
        }

        public GenericRepository<RequestForNew> NewPlanRequestRepository
        {
            get
            {
                if (this.newPlanRequestRepository == null)
                {
                    this.newPlanRequestRepository = new GenericRepository<RequestForNew>(context);
                }
                return newPlanRequestRepository;
            }
        }

        public GenericRepository<Post> PostRepository
        {
            get
            {
                if (this.postRepository == null)
                {
                    this.postRepository = new GenericRepository<Post>(context);
                }
                return postRepository;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(context);
                }
                return productRepository;
            }
        }

        public GenericRepository<Profession> ProfessionRepository
        {
            get
            {
                if (this.professionRepository == null)
                {
                    this.professionRepository = new GenericRepository<Profession>(context);
                }
                return professionRepository;
            }
        }

        public GenericRepository<Project> ProjectRepository
        {
            get
            {
                if (this.projectRepository == null)
                {
                    this.projectRepository = new GenericRepository<Project>(context);
                }
                return projectRepository;
            }
        }

        public GenericRepository<ProjectOffer> ProjectofferRepository
        {
            get
            {
                if (this.projectofferRepository == null)
                {
                    this.projectofferRepository = new GenericRepository<ProjectOffer>(context);
                }
                return projectofferRepository;
            }
        }

        public GenericRepository<Question> QuestionRepository
        {
            get
            {
                if (this.questionRepository == null)
                {
                    this.questionRepository = new GenericRepository<Question>(context);
                }
                return questionRepository;
            }
        }

        public GenericRepository<ReviewBook> BookReviewRepository
        {
            get
            {
                if (this.reviewBookRepository == null)
                {
                    this.reviewBookRepository = new GenericRepository<ReviewBook>(context);
                }
                return reviewBookRepository;
            }
        }

        public GenericRepository<Seminar> SeminarRepository
        {
            get
            {
                if (this.seminarRepository == null)
                {
                    this.seminarRepository = new GenericRepository<Seminar>(context);
                }
                return seminarRepository;
            }
        }


        public GenericRepository<Webinar> WebinarRepository
        {
            get
            {
                if (this.webinarRepository == null)
                {
                    this.webinarRepository = new GenericRepository<Webinar>(context);
                }
                return webinarRepository;
            }
        }

        public GenericRepository<Workshop> WorkshopRepository
        {
            get
            {
                if (this.workshopRepository == null)
                {
                    this.workshopRepository = new GenericRepository<Workshop>(context);
                }
                return workshopRepository;
            }
        }

        public GenericRepository<VideoConference> VideoConferenceRepository
        {
            get
            {
                if (this.videoConferenceRepository == null)
                {
                    this.videoConferenceRepository = new GenericRepository<VideoConference>(context);
                }
                return videoConferenceRepository;
            }
        }

        public GenericRepository<SeminarRequest> SeminarRequestRepository
        {
            get
            {
                if (this.seminarReauestRepository == null)
                {
                    this.seminarReauestRepository = new GenericRepository<SeminarRequest>(context);
                }
                return seminarReauestRepository;
            }
        }


        public GenericRepository<Service> ServiceRepository
        {
            get
            {
                if (this.serviceRepository == null)
                {
                    this.serviceRepository = new GenericRepository<Service>(context);
                }
                return serviceRepository;
            }
        }

        public GenericRepository<Store> StoreRepository
        {
            get
            {
                if (this.storeRepository == null)
                {
                    this.storeRepository = new GenericRepository<Store>(context);
                }
                return storeRepository;
            }
        }

        public GenericRepository<StoreNotExpired> StoreNotExpiredRepository
        {
            get
            {
                if (this.storeNotExpiredRepository == null)
                {
                    this.storeNotExpiredRepository = new GenericRepository<StoreNotExpired>(context);
                }
                return storeNotExpiredRepository;
            }
        }

        public GenericRepository<StoreExpired> StoreExpiredRepository
        {
            get
            {
                if (this.storeExpiredRepository == null)
                {
                    this.storeExpiredRepository = new GenericRepository<StoreExpired>(context);
                }
                return storeExpiredRepository;
            }
        }

        public GenericRepository<Share> ShareRepository
        {
            get
            {
                if (this.shareRepository == null)
                {
                    this.shareRepository = new GenericRepository<Share>(context);
                }
                return shareRepository;
            }
        }

        public GenericRepository<Setting> SettingRepository
        {
            get
            {
                if (this.settingRepository == null)
                {
                    this.settingRepository = new GenericRepository<Setting>(context);
                }
                return settingRepository;
            }
        }


        public GenericRepository<Word> WordRepository
        {
            get
            {
                if (this.wordRepository == null)
                {
                    this.wordRepository = new GenericRepository<Word>(context);
                }
                return wordRepository;
            }
        }

        public GenericRepository<WordEdit> WordEditRepository
        {
            get
            {
                if (this.wordEditRepository == null)
                {
                    this.wordEditRepository = new GenericRepository<WordEdit>(context);
                }
                return wordEditRepository;
            }
        }

        public GenericRepository<WordDescEdit> WordDescEditRepository
        {
            get
            {
                if (this.wordDescEditRepository == null)
                {
                    this.wordDescEditRepository = new GenericRepository<WordDescEdit>(context);
                }
                return wordDescEditRepository;
            }
        }

        public GenericRepository<WordDesc> WordDescRepository
        {
            get
            {
                if (this.wordDescRepository == null)
                {
                    this.wordDescRepository = new GenericRepository<WordDesc>(context);
                }
                return wordDescRepository;
            }
        }


        //public void CompanyToExpire(int coid)
        //{
        //    context.Database.ExecuteSqlCommand("exec CompanyToExpire @CoId", new SqlParameter("@CoId", coid));
        //}
        //public void CompanyNotToExpire(int coid)
        //{
        //    context.Database.ExecuteSqlCommand("exec CompanyNotToExpire @CoId", new SqlParameter("@CoId", coid));
        //}

        public void CompanyPlanChange(int coid, string Discriminator)
        {
            context.Database.ExecuteSqlCommand("UPDATE Company SET Discriminator = {1} WHERE coID = {0}",coid, Discriminator );
            //context.Database.ExecuteSqlCommand("exec CompanyPlanChange @CoId, @Discriminator", new[] { new SqlParameter("@CoId", coid), new SqlParameter("@Discriminator", Discriminator) });
        }
        public void StorePlanChange(int Stid, string Discriminator)
        {
            context.Database.ExecuteSqlCommand("UPDATE Store SET Discriminator = {1} WHERE storeID = {0}", Stid, Discriminator);
            //context.Database.ExecuteSqlCommand("exec StorePlanChange @StId, @Discriminator", new[] { new SqlParameter("@StId", Stid), new SqlParameter("@Discriminator", Discriminator) });
        }
        public void UserPlanChange(int UId, string Discriminator)
        {
            context.Database.ExecuteSqlCommand("UPDATE UserProfile SET Discriminator = {1} WHERE UserID = {0}", UId, Discriminator);
            //context.Database.ExecuteSqlCommand("exec UserPlanChange @UId, @Discriminator", new[] { new SqlParameter("@UId", UId), new SqlParameter("@Discriminator", Discriminator) });
        }

        public void seminarTypeChange(int SnId, string Discriminator)
        {
            context.Database.ExecuteSqlCommand("UPDATE Seminar SET Discriminator = {1} WHERE seminarId = {0}", SnId, Discriminator);
        }


        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}