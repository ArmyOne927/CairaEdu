using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CairaEdu.Pages.Admin
{
    public class VerUsuarioAdmModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VerUsuarioAdmModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Dictionary<string, List<ApplicationUser>> UsuariosPorRol { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Obtener el usuario actual con su InstitucionId
            var usuarioActual = await _userManager.FindByIdAsync(userId);
            if (usuarioActual == null || usuarioActual.InstitucionId == null)
            {
                // Manejar error o redirigir si el usuario no tiene institución
                return;
            }

            var institucionId = usuarioActual.InstitucionId;

            // Filtrar usuarios que pertenezcan a la misma institución
            var usuarios = _userManager.Users
                .Where(u => u.InstitucionId == institucionId)
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
        }
    }
}
