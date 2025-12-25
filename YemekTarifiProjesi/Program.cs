using Microsoft.EntityFrameworkCore;
using YemekTarifiProjesi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- VERİTABANI BAĞLANTISI ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// URL Düzeltme (Render İçin)
if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = connectionString.Trim().Trim('"');

    if (connectionString.StartsWith("postgres"))
    {
        try
        {
            var databaseUri = new Uri(connectionString);
            var userInfo = databaseUri.UserInfo.Split(':');
            int port = databaseUri.Port == -1 ? 5432 : databaseUri.Port;
            connectionString = $"Host={databaseUri.Host};Port={port};Database={databaseUri.LocalPath.TrimStart('/')};User Id={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata: " + ex.Message);
        }
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// -----------------------------

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- KRİTİK BÖLÜM: VERİTABANINI SIFIRLAYIP YENİDEN KURUYORUZ ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // ⚠️ BU SATIR ESKİ HATALI TABLOLARI SİLER
        context.Database.EnsureDeleted();

        // ⚠️ BU SATIR YENİ MODELİNE GÖRE TERTEMİZ KURAR
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Veritabanı yenileme hatası: " + ex.Message);
    }
}
// ----------------------------------------------------------------

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();