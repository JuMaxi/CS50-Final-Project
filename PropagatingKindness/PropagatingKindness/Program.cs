using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Configuration;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Services;
using PropagatingKindness.Infra.Db;
using PropagatingKindness.Infra.DbAccess;
using PropagatingKindness.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts => 
    {
        opts.LoginPath = "/Account/Login";
        opts.LogoutPath = "/Account/Logout";
        opts.AccessDeniedPath = "/Account/Error";
        opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        opts.SlidingExpiration = true;
        opts.Cookie.IsEssential = true;
        opts.Cookie.SameSite = SameSiteMode.Strict;
    });
builder.Services.AddDbContext<PlantsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlantsDB")));
builder.Services.AddHttpClient<IReCaptchaService, ReCaptchaService>();

builder.Services.Configure<ReCaptchaConfiguration>(builder.Configuration.GetSection(ReCaptchaConfiguration.ConfigSection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
