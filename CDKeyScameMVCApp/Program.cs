/*using Core.BusinessLogic;
using Core.RepositoryInterface;
using Microsoft.AspNetCore.Authentication.Cookies;
using RepositoryCRM.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IBusinessLayer, MainBusinessLayer>();//Dependency
builder.Services.AddScoped<IRepositoryVideoGame, RepositoryVideoGame>();// RepositoryCorsiMock>();//Dependency: Serve per prendere controllo per prendere tutti metodi nel RepositoryEF passare attraverso Interface


/*
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Person/Login");
        option.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Person/Forbidden");
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Adm", policy => policy.RequireRole("Ristoratore"));
    options.AddPolicy("User", policy => policy.RequireRole("Cliente"));
});
*/
/*var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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

*/