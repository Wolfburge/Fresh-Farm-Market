using Fresh_Farm_Market.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using WebApp_Core_Identity.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 3;
})
    .AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddDataProtection();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromSeconds(10); 
});

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.Cookie.HttpOnly = true;
    Config.LoginPath = "/Login";
    Config.ExpireTimeSpan = TimeSpan.FromSeconds(10); 

    Config.SlidingExpiration = true; 
    Config.Cookie.SameSite = SameSiteMode.Strict; 

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
