using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Configuration;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Services;
using PropagatingKindness.Infra.Db;
using PropagatingKindness.Infra.DbAccess;
using PropagatingKindness.Infra.Repository;
using PropagatingKindness.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => 
{
    options.Filters.Add<EnsureUserImageActionFilter>();
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAdvertService, AdvertService>();
builder.Services.AddTransient<IAdvertRepository, AdvertRepository>();
builder.Services.AddTransient<IPhotosManagerService, PhotosManagerService>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IBlogRepository, BlogRepository>();
builder.Services.AddTransient<IBlogService, BlogService>();


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
builder.Services.Configure<AzureConfiguration>(builder.Configuration.GetSection(AzureConfiguration.SectionKey));

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
