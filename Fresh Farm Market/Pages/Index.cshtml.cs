using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fresh_Farm_Market.Pages
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public async Task OnGet()
		{
            var user = await userManager.GetUserAsync(HttpContext.User);

			if (user != null)
			{
				if(user.AuthToken == HttpContext .Session.GetString("AuthToken"))
				{
					HttpContext.Session.SetString("Email", user.Email);
					HttpContext.Session.SetString("FullName", user.FullName);
					HttpContext.Session.SetString("Gender", user.Gender);
					HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber);
					HttpContext.Session.SetString("DeliveryAddress", user.Address);
					HttpContext.Session.SetString("AboutMe", user.AboutMe);
					var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
					var protector = dataProtectionProvider.CreateProtector("MySecretKey");

					HttpContext.Session.SetString("CreditCard", protector.Unprotect(user.CreditCard));
				}
				else
				{
					await signInManager.SignOutAsync();
					HttpContext.Session.Clear();
					Response.Redirect("Login");
				}
			}
		}
	}
}