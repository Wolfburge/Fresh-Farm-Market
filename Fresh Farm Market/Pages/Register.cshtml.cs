using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace Fresh_Farm_Market.Pages
{
	[ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		[BindProperty]
		public Register RModel { get; set; }
		public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var fileExtension = Path.GetExtension(RModel.Photo.FileName).ToLower();

				if (fileExtension != ".jpg" && fileExtension != ".jpeg")
				{
					ModelState.AddModelError("RModel.Photo", "Only .jpg files are allowed");
					return Page();
				}
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");
				var user = new ApplicationUser()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
					CreditCard = protector.Protect(RModel.CreditCard),
					Address = RModel.DeliveryAddress,
					PhoneNumber = RModel.PhoneNumber,
					Gender = RModel.Gender,
					Photo = RModel.Photo.FileName,
					AboutMe = HttpUtility.HtmlEncode (RModel.AboutMe),
					FullName = RModel.FullName
				};

				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return Page();
		}

	}
}
