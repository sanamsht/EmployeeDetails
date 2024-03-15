using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkingWithMultipleTable_Prod.Data;

using WorkingWithMultipleTable_Prod.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddDbContext<DBContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = "/Account/Login";
    option.LoginPath = "/Home/Index";
    option.LogoutPath = "/Account/Logout";
    option.Cookie.Name = "Auth";
    option.ExpireTimeSpan = TimeSpan.FromHours(30);
    option.SlidingExpiration = true;

});



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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
