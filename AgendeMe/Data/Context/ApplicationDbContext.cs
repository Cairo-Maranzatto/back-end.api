using AgendeMe.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgendeMe.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<ClienteTelefoneEntity> ClienteTelefones { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ClienteEntity>()
                .HasIndex(c => c.Email)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
