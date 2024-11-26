using Infinicare_Ojash_Devkota.Data;
using Infinicare_Ojash_Devkota.Models;
using Infinicare_Ojash_Devkota.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);
var cookiePolicyOptions = new CookiePolicyOptions()
{
	Secure = CookieSecurePolicy.SameAsRequest 
};
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<ApiServices>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<TransactionServices>();
builder.Services.AddHttpContextAccessor();
// Add services to the container.

builder.Services.AddDbContext<ApplicationDBContext>(optionsBuilder => optionsBuilder.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromDays(30);
	options.SlidingExpiration = true;
	options.AccessDeniedPath = "/Error/403";
	options.Cookie.Name = "Infinicare";
	options.LoginPath = "/Account/Login";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(cookiePolicyOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();