using AgendeMe.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            base.OnModelCreating(builder);

            builder.Entity<ClienteEntity>()
                .HasIndex(c => c.Email)
                .IsUnique();

            builder.Entity<ClienteTelefoneEntity>(ConfigureClienteTelefoneEntity);
        }

        private void ConfigureClienteTelefoneEntity(EntityTypeBuilder<ClienteTelefoneEntity> builder)
        {
            builder.HasKey(ct => ct.Id);
            builder.Property(ct => ct.ClienteId).IsRequired();
            builder.Property(ct => ct.Descricao).IsRequired();
            builder.Property(ct => ct.Numero).IsRequired().HasMaxLength(20);

            builder.HasOne(ct => ct.Cliente)
                .WithMany(c => c.ClienteTelefones)
                .HasForeignKey(ct => ct.ClienteId);
        }
    }
}
