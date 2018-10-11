using IndustryTower.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

  //*************************CopyToSeed In Migration ***************************\\
  //context.Database.ExecuteSqlCommand("ALTER TABLE webpages_Roles ADD CONSTRAINT uc_RoleName UNIQUE(RoleName)");
  //context.Database.ExecuteSqlCommand("ALTER TABLE UserProfile ADD DEFAULT 'FreeUser' FOR Discriminator");
  //context.Database.ExecuteSqlCommand("ALTER TABLE UserProfile ADD CONSTRAINT up_Email UNIQUE(Email)");

namespace IndustryTower.DAL
{
    public class ITTContext : DbContext
    {
        public ITTContext()
            : base("ITTContext")
        { }

        public DbSet<Abuse> Abuses { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<BadgeUser> BadgeUser { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CommentPost> CommentsPost { get; set; }
        public DbSet<CommentQuestion> CommentsQuestion { get; set; }
        public DbSet<CommentAnswer> CommentsAnswer { get; set; }
        public DbSet<CommentGSO> CommentsGSO { get; set; }
        public DbSet<CommentProduct> CommentsProduct { get; set; }
        public DbSet<CommentService> CommentsService { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CountState> CountStates { get; set; }
        public DbSet<Dict> Doctionaries { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Following> Following { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupSession> GroupSessions { get; set; }
        public DbSet<GroupSessionOffer> GroupSessionOffers { get; set; }
        public DbSet<GroupSesssionResult> GroupSessionResult { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<LikeBook> LikesBook { get; set; }
        public DbSet<LikePost> LikesPost { get; set; }
        public DbSet<LikeQuestion> LikesQuestion { get; set; }
        public DbSet<LikeAnswer> LikesAnswer { get; set; }
        public DbSet<LikeGSO> LikesGSO { get; set; }
        public DbSet<LikeProduct> LikesProduct { get; set; }
        public DbSet<LikeReviewBook> LikesReviewBook { get; set; }
        public DbSet<LikeService> LikesService { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Patent> Patents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PlanRequest> PlanRequets { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectOffer> ProjectOffers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Seminar> Seminars { get; set; }
        public DbSet<SeminarRequest> SeminarRequests { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordEdit> WordEdits { get; set; }
        public DbSet<WordDesc> WordDescs { get; set; }
        public DbSet<WordDescEdit> WordDescEdits { get; set; }


        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }


        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new MembershipMapping());
            modelBuilder.Configurations.Add(new RolesMapping());
            modelBuilder.Configurations.Add(new UsersInRolesMapping());
            modelBuilder.Configurations.Add(new OAuthMembershipMapping());


            modelBuilder.Entity<Setting>().HasOptional(t => t.Company).WithOptionalPrincipal(g => g.Setting).Map(x => x.MapKey("settingID"));
            modelBuilder.Entity<Setting>().HasOptional(t => t.Store).WithOptionalPrincipal(g => g.Setting).Map(x => x.MapKey("settingID"));
            modelBuilder.Entity<Setting>().HasOptional(t => t.User).WithOptionalPrincipal(g => g.Setting).Map(x => x.MapKey("settingID"));


            modelBuilder.Entity<GroupSesssionResult>()
                                .HasKey(t => t.sessionId);
            modelBuilder.Entity<GroupSession>().HasRequired(t => t.Result).WithRequiredPrincipal(t => t.groupSession);
            //modelBuilder.Entity<GroupSesssionResult>().HasOptional(t => t.groupSession).WithOptionalPrincipal(g => g.Result).Map(x => x.MapKey("sessionId"));

            modelBuilder.Entity<UserProfile>().HasRequired(t => t.webpages_Membership).WithRequiredPrincipal(x => x.UserProfile);

            //One To One => PlanRequest - Payment
            modelBuilder.Entity<Payment>().HasKey(t => t.reqID);
            modelBuilder.Entity<PlanRequest>().HasRequired(p => p.payment).WithRequiredPrincipal(t => t.planRequest);


            modelBuilder.Entity<Group>().HasMany(u => u.Professions).WithMany(d => d.Groups)
                                        .Map(d => 
                                        { 
                                            d.MapLeftKey("Group_groupId"); 
                                            d.MapRightKey("Profession_profID"); 
                                            d.ToTable("ProfessionGroup"); 
                                        });
           
        }


        internal partial class MembershipMapping : EntityTypeConfiguration<webpages_Membership>
        {
            public MembershipMapping()
            {
                this.HasKey(t => t.UserId);
                this.ToTable("webpages_Membership");
                this.Property(t => t.UserId).HasColumnName("UserId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
                this.Property(t => t.CreateDate).HasColumnName("CreateDate");
                this.Property(t => t.ConfirmationToken).HasColumnName("ConfirmationToken").HasMaxLength(128);
                this.Property(t => t.IsConfirmed).HasColumnName("IsConfirmed");
                this.Property(t => t.LastPasswordFailureDate).HasColumnName("LastPasswordFailureDate");
                this.Property(t => t.PasswordFailuresSinceLastSuccess).HasColumnName("PasswordFailuresSinceLastSuccess");
                this.Property(t => t.Password).HasColumnName("Password").IsRequired().HasMaxLength(128);
                this.Property(t => t.PasswordChangedDate).HasColumnName("PasswordChangedDate");
                this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt").IsRequired().HasMaxLength(128);
                this.Property(t => t.PasswordVerificationToken).HasColumnName("PasswordVerificationToken").HasMaxLength(128);
                this.Property(t => t.PasswordVerificationTokenExpirationDate).HasColumnName("PasswordVerificationTokenExpirationDate");
            }
        }
        internal partial class RolesMapping : EntityTypeConfiguration<webpages_Roles>
        {
            public RolesMapping()
            {
                this.HasKey(t => t.RoleId);
                this.ToTable("webpages_Roles");
                this.Property(t => t.RoleId).HasColumnName("RoleId");
                this.Property(t => t.RoleName).HasColumnName("RoleName").IsRequired().HasMaxLength(256);
            }
        }

        internal partial class UsersInRolesMapping : EntityTypeConfiguration<webpages_UsersInRoles>
        {
            public UsersInRolesMapping()
            {
                this.HasKey(t => new { t.UserId, t.RoleId });
                this.ToTable("webpages_UsersInRoles");
                this.Property(t => t.UserId).HasColumnName("UserId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
                this.Property(t => t.RoleId).HasColumnName("RoleId").HasDatabaseGeneratedOption(new Nullable<DatabaseGeneratedOption>(DatabaseGeneratedOption.None));
                this.HasRequired(t => t.webpages_Roles).WithMany(t => t.webpages_UsersInRoles).HasForeignKey(d => d.RoleId);
                this.HasRequired(t => t.UserProfile).WithMany(t => t.webpages_UsersInRoles).HasForeignKey(d => d.UserId);

            }
        }

        internal partial class OAuthMembershipMapping : EntityTypeConfiguration<webpages_OAuthMembership>
        {
            public OAuthMembershipMapping()
            {
                this.HasKey(t => new { t.UserId, t.ProviderUserId });
                this.ToTable("webpages_OAuthMembership");

            }
        }
    }
}