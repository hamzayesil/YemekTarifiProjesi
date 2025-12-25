using Microsoft.EntityFrameworkCore;
using YemekTarifiProjesi.Models; // Eðer hata verirse burayý .Data olarak dene

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. VERÝTABANI BAÐLANTISI VE PORT DÜZELTME KODU (BURASI ÇOK ÖNEMLÝ)
// ---------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Link temizliði (Boþluk ve týrnaklarý at)
if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = connectionString.Trim().Trim('"');
}

// Render URL Dönüþtürücü
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres"))
{
    try
    {
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':');

        // PORT DÜZELTME: Eðer port -1 gelirse (yazmýyorsa) 5432 yap.
        int port = databaseUri.Port == -1 ? 5432 : databaseUri.Port;

        connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.LocalPath.TrimStart('/')};User Id={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine("URL Dönüþtürme Hatasý: " + ex.Message);
        // Hata olsa bile devam etsin, belki connectionString doðrudur.
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ---------------------------------------------------------

// Diðer servisler
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Otomatik Veritabaný Güncelleme (Migration)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Veritabaný oluþturma hatasý: " + ex.Message);
    }
}

// HTTP Ýsteði hattý (Pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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