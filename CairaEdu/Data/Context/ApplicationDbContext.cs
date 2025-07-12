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
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Ciudad> Ciudades {  get; set; }
        public DbSet<Institucion> Instituciones { get; set; }
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

            builder.Entity<Provincia>(entity =>
            {
                entity.ToTable("Provincia");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).HasColumnName("prov_nombre").HasMaxLength(50);
                entity.Property(p => p.Estado).HasColumnName("prov_estado").HasMaxLength(1);
            });

            builder.Entity<Ciudad>(entity =>
            {
                entity.ToTable("Ciudad");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nombre).HasColumnName("ciud_nombre").HasMaxLength(50);
                entity.Property(c => c.Estado).HasColumnName("ciud_estado").HasMaxLength(1);
                entity.HasOne(c => c.Provincia)
                      .WithMany(p => p.Ciudades)
                      .HasForeignKey(c => c.ProvinciaId)
                      .HasConstraintName("FK_Ciudad_Provincia");
            });

            builder.Entity<Institucion>(entity =>
            {
                entity.ToTable("Institucion");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Nombre).HasColumnName("inst_nombre").HasMaxLength(100);
                entity.Property(i => i.Direccion).HasColumnName("inst_direccion").HasMaxLength(100);
                entity.Property(i => i.Dominio).HasColumnName("inst_dominio").HasMaxLength(50);
                entity.Property(i => i.Ruc).HasColumnName("inst_ruc").HasMaxLength(50);
                entity.Property(i => i.Telefono).HasColumnName("inst_telefono").HasMaxLength(10);
                entity.Property(i => i.Estado).HasColumnName("inst_estado").HasMaxLength(1);
                entity.Property(i => i.Logo).HasColumnName("inst_logo");
                entity.HasOne(i => i.Ciudad)
                      .WithMany(c => c.Instituciones)
                      .HasForeignKey(i => i.CiudadId)
                      .HasConstraintName("FK_Institucion_Ciudad");
            });

        }
    }
}
