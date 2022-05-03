using CA.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CA.DAL.Context
{
    public class EntityDbContext : DbContext
    {
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
        }
        public DbSet<TemporaryUser> TemporaryUsers { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<TEntity> WriterSet<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

               modelbuilder
                .Entity<Criteria>()
                .HasOne(c => c.Crypto)
                .WithMany(cr => cr.Criterias)
                .HasForeignKey(c => c.CryptoId)
                .OnDelete(DeleteBehavior.Cascade);

               modelbuilder
                .Entity<Symptom>()
                .HasOne(cr => cr.User)
                .WithMany(u => u.Symptoms)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                modelbuilder
                .Entity<History>()
                .HasOne(h => h.Cryptocurrency)
                .WithMany(c => c.Histories)
                .HasForeignKey(h => h.CryptocurrencyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}