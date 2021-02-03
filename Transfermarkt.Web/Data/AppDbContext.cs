using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transfermarkt.Web.Models;

namespace Transfermarkt.Web.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachClub> CoachesClubs { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Corner> Corners { get; set; }
        public DbSet<Foul> Fouls { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<RefereeMatch> RefereeMatches { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Favorite>().HasIndex(b => new { b.ClubId, b.UserId }).IsUnique();
            builder.Entity<PlayerPosition>().HasIndex(b => new { b.PlayerId, b.PositionId}).IsUnique();
            //builder.Entity<ManyToManyTable>().HasKey(x => new { x.Key1, x.Key2 });
        }
    }
}
