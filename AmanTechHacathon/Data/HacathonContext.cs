using AmanTechHackathon.Model;
using Microsoft.EntityFrameworkCore;

namespace AmanTechHackathon.Data
{
    public class HacathonContext : DbContext
    {


        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public HacathonContext(DbContextOptions<HacathonContext> options, IHttpContextAccessor httpContextAccessor)
             : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }

        public HacathonContext(DbContextOptions<HacathonContext> options)
            : base(options)
        {
            _httpContextAccessor = null!;
            UserName = "Seed Data";
        }


        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Prevent cascade delete from Region to Member
            // so we are prevented from deleting a Region
            // when Members are assigned to it
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Regions)
                .WithMany(r => r.Members)
                .HasForeignKey(m => m.RegionID)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent cascade delete from Challenge to Member
            // so we are prevented from deleting a Challenge
            // when Members are participating in it
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Challenges)
                .WithMany(c => c.Members)
                .HasForeignKey(m => m.ChallengeID)
                .OnDelete(DeleteBehavior.Restrict);

            // Add a unique index to ensure each MemberCode
            // is unique across all Members
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.MemberCode)
                .IsUnique();

            // Add a unique index to ensure each Region
            // has a unique 2-letter Region Code
            modelBuilder.Entity<Region>()
                .HasIndex(r => r.Code)
                .IsUnique();

            // Add a unique index to ensure each Challenge
            // has a unique 3-letter Challenge Code
            modelBuilder.Entity<Challenge>()
                .HasIndex(c => c.Code)
                .IsUnique();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}
