using System.ComponentModel.DataAnnotations;

namespace CairaEdu.Core.ViewModels
{
	public class RegisterViewModel
	{	
		[Required]
		[EmailAddress]
		public  string Email { get; set; }
		
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public string Role = "Administrador";

	}
}
