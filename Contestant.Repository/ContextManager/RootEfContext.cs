using Contestant.Models;
using Microsoft.EntityFrameworkCore;

namespace Contestant.Repository.ContextManager
{
    public class RootEfContext : DbContext
    {
        public RootEfContext(DbContextOptions<RootEfContext> options) : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        //public RootEfContext(DbContextOptions<RootEfContext> options, string connectionString) //, IHttpContextAccessor accessor)
        //    : base(options)
        //{
        //    _connectionString = connectionString;
        //}

        //public RootEfContext(DbContextOptionsBuilder options)
        //    : base()
        //{
        //    this.OnConfiguring(options);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //builder.HasDefaultSchema("HR");
            //base.OnModelCreating(builder);
            //// Customize the ASP.NET Identity model and override the defaults if needed.
            //// For example, you can rename the ASP.NET Identity table names and more.
            //// Add your customizations after calling base.OnModelCreating(builder);

            //foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //builder.Entity<SyncApplicationUser>().HasIndex(a => a.Username).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        public DbSet<District> District { get; set; }
        public DbSet<Contestants> Contestants { get; set; }
        public DbSet<ContestantRating> ContestantRating { get; set; }

        public DbSet<CodeSequence> CodeSequences { get; set; }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public DbSet<ApplicationMenu> ApplicationMenu { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ActivityLogHist> ActivityLogHist { get; set; }
    }
}
