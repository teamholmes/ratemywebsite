using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MyApp.Business.DomainObjects.Mapping;
using MyApp.Business.DomainObjects.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Core.Metadata.Edm;

namespace MyApp.DAL.Repository
{
    //internal class DBContext : DbContext
    //{
    //    static DBContext()
    //    {
    //        Database.SetInitializer<DBContext>(null);
    //    }

    //    public DBContext()
    //        : base("Name=DefaultConnection")
    //    {

    //    }

    //    public DbSet<UserProfile> UserProfiles { get; set; }
    //    public DbSet<AppConfiguration> AppConfigurations { get; set; }

    //    public DbSet<PassphraseHistory> PassphraseHistories { get; set; }
    //    public DbSet<PassphraseWord> PassphraseWords { get; set; }

    //    public DbSet<MembershipTable> MembershipTables { get; set; }

    //    public DbSet<Role> Roles { get; set; }
    //    public DbSet<UserInRole> UserInRoles { get; set; }
    //    public DbSet<AuditLog> AuditLogs { get; set; }
    //    public DbSet<SectionQuestion> SectionQuestions { get; set; }
    //    public DbSet<SectionResponse> SectionResponses { get; set; }


    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {

    //        modelBuilder.Configurations.Add(new UserProfileMap());
    //        modelBuilder.Configurations.Add(new MembershipMap());
    //        modelBuilder.Configurations.Add(new AppConfigurationMap());


    //        modelBuilder.Configurations.Add(new PassphraseHistoryMap());
    //        modelBuilder.Configurations.Add(new PassphraseWordMap());

    //        modelBuilder.Configurations.Add(new RoleMap());
    //        modelBuilder.Configurations.Add(new UserInRoleMap());
    //        modelBuilder.Configurations.Add(new EmployerMap());
    //        modelBuilder.Configurations.Add(new AuditLogMap());
    //        modelBuilder.Configurations.Add(new SectionQuestionMap());
    //        modelBuilder.Configurations.Add(new SectionResponseMap());

    //        modelBuilder.Entity<UserInRole>().HasRequired(t => t.UserProfile).WithMany(u => u.UserInRole).HasForeignKey(t => t.UserId);
    //    }


    public class ApplicationDbContext : DbContext // IdentityDbContext // DbContext //  IdentityDbContext<ApplicationUser> 
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        //public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        //public DbSet<PassphraseHistory> PassphraseHistorys { get; set; }
        //public DbSet<PassphraseWord> PassphraseWords { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<ApplicationDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ApplicationUserMap());
            //modelBuilder.Configurations.Add(new ApplicationUserLoginMap());
            //modelBuilder.Configurations.Add(new ApplicationUserRoleMap());
            //modelBuilder.Configurations.Add(new PassphraseHistoryMap());
            //modelBuilder.Configurations.Add(new PassphraseWordMap());
            modelBuilder.Configurations.Add(new ApplicationUserClaimMap());
            modelBuilder.Configurations.Add(new WebsiteReviewMap());
            



            modelBuilder.Entity<ApplicationUserClaim>().HasRequired(t => t.ApplicationUser).WithMany(u => u.Claims).HasForeignKey(t => t.UserId);



            //modelBuilder.Entity<ApplicationUser>().HasRequired(t => t.Claims).WithMany(u => u).HasForeignKey(t => t.UserId);


            //modelBuilder.Entity<ApplicationUser>()
            //   .HasRequired(a => a.Id)
            //   .WithMany()
            //   .HasForeignKey(u => u.Claimz);


            // modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");

            // modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            // modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId).ToTable("AspNetUserLogins");

            // modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("AspNetUserRoles");

            // modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");



        }
    }
}

