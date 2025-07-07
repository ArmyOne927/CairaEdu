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
		}
	}
}
