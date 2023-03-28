using Microsoft.EntityFrameworkCore;
using NorthwindApp.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// mvc uygulamasý olmasý sebebi ile AddRazorPages yerine AddControllersWithViews yani MVC uygulamasýný ayaða kaldýr.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient();


//builder.Services.AddControllers(); // Web API olarak uygulamayý ayaða kaldýrma

// ConfigurationManager sýnýfý web config gibi config dosyalarýndan ayarlar okumamýza olanak saðlar.
// appSetting.json dosyasýndaki json olan nesneleri okumak için ConfigurationManager sýnýfýný kullanýyoruz.
// UseSqlServer MSSql Server kullandýk
// NORTHWNDContext ile veri tabaný nesnesini uygulamanýn servis sürecine kattýk. artýk veri tabaný instancelarýný uygulama yönetecek.

// registration kýsmý
builder.Services.AddDbContext<NORTHWNDContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// yani ben UseSqlServer yerine useNpl() postgres üzerinden tüm uygulamayý tek yerden deðiþtirmiþ olurum.

// Mvc de uygulama içerisine tanýtýlacak olan servisler IServiceCollection ile servisleri uygulama ayakta olduðu sürece yönetimini saðlar.

// net core service yönetimi, class instance yönetimini merkezi olarak bu dosyadan yapar. böylelikle tek bir dosyadan uygulamadaki baðýmlýlýklarý deðiþtirebiliriz.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();

}

// Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=NORTHWND;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default sayfa ilk açýldýðýnda uygulama Home/Index route yönlendirir.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
