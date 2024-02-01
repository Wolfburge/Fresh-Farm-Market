using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Core_Identity.Model;

namespace Fresh_Farm_Market.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> UserManager;
		private readonly AuthDbContext dbContext;

		public LogoutModel(AuthDbContext dbContext, SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
		{
			this.signInManager = signInManager;
			this.UserManager = userManager;
			this.dbContext = dbContext;	

		}

		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			var user = await UserManager.GetUserAsync(User);
			user.AuthToken = null;
			await UserManager.UpdateAsync(user);
			await signInManager.SignOutAsync();
			var audit = new Audit
			{

				UserID = user.Id,
				Action = "Logged Out",
				DateTime = DateTime.Now,
			};
			dbContext.AuditLogs.Add(audit);
			await dbContext.SaveChangesAsync();
			HttpContext.Session.Clear();
			return RedirectToPage("Login");
		}

		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
