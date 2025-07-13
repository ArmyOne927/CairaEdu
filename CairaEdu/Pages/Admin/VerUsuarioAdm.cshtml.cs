using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            var rolesDeseados = new[] { "Administrador", "Docente", "Representante", "Estudiante" };

            foreach (var rol in rolesDeseados)
            {
                UsuariosPorRol[rol] = new List<ApplicationUser>();
            }

            var usuarios = _userManager.Users.ToList();

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
