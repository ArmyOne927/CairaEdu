using System.Reflection.Emit;
using CairaEdu.Data.Entities;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CairaEdu.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Institucion> Instituciones { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<MateriaProfesor> MateriaProfesores { get; set; }
        public DbSet<CicloLectivo> CiclosLectivos { get; set; }
        public DbSet<Periodo> Periodos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
            });
            builder.Entity<ApplicationUser>()   
            .HasOne(u => u.Institucion)
            .WithMany() // o WithMany(u => u.Usuarios) si quieres que una institución tenga varios usuarios
            .HasForeignKey(u => u.InstitucionId)
            .HasConstraintName("FK_Usuario_Institucion");
        
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

            builder.Entity<Materia>(entity =>
            {
                entity.ToTable("Materia");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).HasColumnName("mat_nombre").HasMaxLength(50);
                entity.Property(e => e.Competencias).HasColumnName("mat_competencias").HasMaxLength(1000);
                entity.Property(e => e.Objetivos).HasColumnName("mat_objetivos").HasMaxLength(1000);
                entity.Property(e => e.Imagen).HasColumnName("mat_imagen").HasMaxLength(64);
                entity.Property(e => e.Estado).HasColumnName("mat_estado").HasMaxLength(1);
                entity.Property(e => e.InstitucionId).HasColumnName("mat_inst_id");

                entity.HasOne(e => e.Institucion)
                      .WithMany()
                      .HasForeignKey(e => e.InstitucionId)
                      .HasConstraintName("FK_Materia_Institucion");

            });

            builder.Entity<MateriaProfesor>(entity =>
            {
                entity.ToTable("MateriaProfesor");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId).HasColumnName("mp_user_id");
                entity.Property(e => e.MateriaId).HasColumnName("mp_mat_id");

                entity.HasOne(e => e.Profesor)
                      .WithMany(p => p.MateriaProfesores)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Materia)
                      .WithMany(m => m.MateriaProfesores)
                      .HasForeignKey(e => e.MateriaId);

                entity.HasIndex(mp => new { mp.MateriaId, mp.UserId })
                      .IsUnique()
                      .HasDatabaseName("UX_MateriaProfesor_Materia_User");
            });

            //ciclos y periodos
            base.OnModelCreating(builder);

            builder.Entity<CicloLectivo>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            builder.Entity<CicloLectivo>()
                .HasMany(c => c.Periodos)
                .WithOne(p => p.CicloLectivo)
                .HasForeignKey(p => p.CicloLectivoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
