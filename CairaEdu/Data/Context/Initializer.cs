using CairaEdu.Core.Enums;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace CairaEdu.Data.Context
{
	public static class Initializer
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			foreach (var rol in Enum.GetValues<Roles>()) { 
				var nombreRol = rol.ToString();

				if (!await roleManager.RoleExistsAsync(nombreRol))
				{
					await roleManager.CreateAsync(new ApplicationRole { Name = nombreRol });
				}
			}


		}
	}
}
