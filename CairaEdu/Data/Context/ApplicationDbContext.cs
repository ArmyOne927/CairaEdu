using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Data.Context
{
	public class ApplicationDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<ApplicationUser>(b =>
			{
				b.ToTable("Users");
			});
			builder.Entity<ApplicationRole>(b =>
			{
				b.ToTable("Roles");
			});

            // Configuración de TipoDocumento
            builder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Descripcion)
                      .HasColumnName("tpdoc_descripcion")
                      .HasMaxLength(50);

                entity.Property(e => e.Estado)
                      .HasColumnName("tpdoc_estado")
                      .HasMaxLength(1);

                // Relación con ApplicationUser
                entity.HasMany(e => e.Usuarios)
                      .WithOne(u => u.TipoDocumento)
                      .HasForeignKey(u => u.TipoDocumentoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
