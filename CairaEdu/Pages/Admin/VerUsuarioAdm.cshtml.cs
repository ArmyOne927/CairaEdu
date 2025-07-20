using CairaEdu.Data.Context;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    [Authorize(Roles = "Administrador")]
    public class VerUsuarioAdmModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerUsuarioAdmModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Dictionary<string, List<ApplicationUser>> UsuariosPorRol { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Obtener el usuario actual con su InstitucionId
            var usuarioActual = await _userManager.FindByIdAsync(userId);
            if (usuarioActual == null || usuarioActual.InstitucionId == null)
            {
                TempData["ErrorMessage"] = "Usuario no encontrado o sin institución asignada.";
                return RedirectToPage("/Admin/VerInstitucionAdm");
            }

            var institucionId = usuarioActual.InstitucionId;

            // Filtrar usuarios que pertenezcan a la misma institución
            var usuarios = _userManager.Users
                .Where(u => u.InstitucionId == institucionId && u.Estado == 'A')
                .ToList();

            // Inicializar los roles deseados
            var rolesDeseados = new[] { "Administrador", "Docente", "Representante", "Estudiante" };
            foreach (var rol in rolesDeseados)
            {
                UsuariosPorRol[rol] = new List<ApplicationUser>();
            }

            // Clasificar usuarios por rol (de los roles deseados únicamente)
            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var rol in roles)
                {
                    if (UsuariosPorRol.ContainsKey(rol))
                    {
                        UsuariosPorRol[rol].Add(user);
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario != null)
            {
                usuario.Estado = 'I'; // Cambiar a Inactivo
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el usuario que deseas eliminar.";
            }

            return RedirectToPage("/Admin/VerUsuarioAdm");
        }
    }
}
