using Microsoft.AspNetCore.Identity;

namespace Fresh_Farm_Market.ViewModels
{
	public class ApplicationUser : IdentityUser
	{
		public string? AuthToken { get; set; }
		public string FullName { get; set; }
		public string CreditCard { get; set; }
		public string Address { get; set; }
		public string AboutMe { get; set; }
		public string Gender { get; set; }
		public string Photo {  get; set; }

	}
}
