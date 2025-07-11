using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Pages.Admin
{
    public class CrearUsuarioAdmModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string? Nombres { get; set; }

            [Required]
            public string? Apellidos { get; set; }

            [Required]
            public string Documento { get; set; }

            [Required]
            public int TipoDocumentoId { get; set; }

            public string? Telefono { get; set; }

            public DateTime? FechaNacimiento { get; set; }

            public IFormFile? Foto { get; set; }  // Para subir imagen si usas formulario <input type="file" />

            public string Email { get; set; }

            public string Password { get; set; }

            public string? Estado { get; set; }  // A, I, etc.
        }
    }
}
