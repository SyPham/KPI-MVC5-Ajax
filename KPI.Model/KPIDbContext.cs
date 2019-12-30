using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.EF;
namespace KPI.Model
{
    public class KPIDbContext : DbContext
    {
        const String DefaultConnectionName = "KPIDbContext";
        public KPIDbContext() : this(DefaultConnectionName)
        {
            
        }
        
        public KPIDbContext(String sqlConnectionName) : base(String.Format("Name={0}", sqlConnectionName))
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<EF.KPI> KPIs { get; set; }
        public DbSet<EF.KPILevel> KPILevels { get; set; }

        public DbSet<Data> Datas { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Level> Levels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<SeenComment> SeenComments { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet <Resource> Resources { get; set; }
        public DbSet <Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Revise> Revises { get; set; }
        public DbSet<ActionPlan> ActionPlans { get; set; }
        public DbSet <Notification> Notifications { get; set; }
        public DbSet<ActionPlanDetail> ActionPlanDetails { get; set; }
        public DbSet<NotificationDetail> NotificationDetails { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<ErrorMessage> ErrorMessages { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Uploader> Uploaders { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<CategoryKPILevel> CategoryKPILevels { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<OCCategory> OCCategories { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<StateSendMail> StateSendMails { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<MenuLang> MenuLangs { get; set; }
        public DbSet<LateOnUpLoad> LateOnUpLoads { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            //builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId });
            //builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId);
        }
    }
}
