using CairaEdu.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace CairaEdu.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Nombres y apellidos
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }


        // Foto de perfil (almacenada como binario)
        public byte[]? Foto { get; set; }
        public string? Documento { get; set; }

        // Fecha de nacimiento
        public DateTime? FechaNacimiento { get; set; }

        public int? InstitucionId { get; set; }
        public Institucion? Institucion { get; set;}

        // Estado (por ejemplo, 'A' para activo, 'I' para inactivo, etc.)
        public char Estado { get; set; }

        // FK y navegación
        public int TipoDocumentoId { get; set; }
        public virtual Entities.TipoDocumento? TipoDocumento { get; set; }

    }
}
