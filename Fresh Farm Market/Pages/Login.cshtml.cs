using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using WebApp_Core_Identity.Model;

namespace Fresh_Farm_Market.Pages
{
	[ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> UserManager;
		private readonly AuthDbContext dbContext;

		public LoginModel(AuthDbContext dbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			this.signInManager = signInManager;
			this.UserManager = userManager;
			this.dbContext = dbContext;
		}
		public void OnGet()
        {
        }
		public bool ValidateCaptcha()
		{
			string Response = Request.Form["g-recaptcha-response"];
			bool valid = false;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LdyXWIpAAAAANGChe7PU6mlmpeZ3l-4nyCxL_36&response=" + Response);
			try
			{
				using (WebResponse wResponse = request.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();
						var data = JsonSerializer.Deserialize<CaptchaRes>(jsonResponse);
						valid = Convert.ToBoolean(data.success);
					}
				}
				return valid;
			}
			catch (WebException ex)
			{
				throw ex;
			}

		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (!ValidateCaptcha())
				{
					ModelState.AddModelError("", "Captcha is invalid");
					return Page();
				}
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, false);
				if (identityResult.Succeeded)
				{
					string guid = Guid.NewGuid().ToString();
					var user = await UserManager.FindByEmailAsync(LModel.Email);
					if (user != null)
					{
						HttpContext.Session.SetString("AuthToken", guid);
						user.AuthToken = guid;
						await UserManager.UpdateAsync(user);
					}
					var audit = new Audit
					{
						UserID = user.Id,
						Action = "Login",
						DateTime = DateTime.Now,
					};
					dbContext.AuditLogs.Add(audit);
					await dbContext.SaveChangesAsync();
					return RedirectToPage("Index");
				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}
