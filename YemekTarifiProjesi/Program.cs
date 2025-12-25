using System;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using YemekTarifiProjesi.Models; 

var builder = WebApplication.CreateBuilder(args);
// YERÝNE YAPIÞTIRACAÐIN KISIM
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Eðer adres "postgres://" ile baþlýyorsa (Render formatý), parçalayýp düzelt
if (connectionString != null && connectionString.StartsWith("postgres://"))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');

    connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.TrimStart('/')};User Id={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
// Veritabaný tablolarýný otomatik oluþturma kodu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Tablolar yoksa oluþturur
    }
    catch (Exception ex)
    {
        // Hata oluþursa loglara yazar
        Console.WriteLine("Veritabaný oluþturma hatasý: " + ex.Message);
    }
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipes}/{action=Index}/{id?}");

app.Run();
