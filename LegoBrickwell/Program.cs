using Identity.Models;
using LegoBrickwell.IdentityPolicy;
using LegoBrickwell.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(opts =>
{
    // Configure password requirements
    opts.Password.RequiredLength = 8; // Minimum password length
    opts.Password.RequireUppercase = true; // Require at least one uppercase letter
    opts.Password.RequireLowercase = true; // Require at least one lowercase letter
    opts.Password.RequireDigit = true; // Require at least one digit
    opts.Password.RequireNonAlphanumeric = true; // Require at least one special character
    opts.SignIn.RequireConfirmedAccount = true;
    opts.User.RequireUniqueEmail = true;
});
builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();

builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");


// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();