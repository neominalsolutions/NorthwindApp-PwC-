using Microsoft.EntityFrameworkCore;
using NorthwindApp.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// mvc uygulamas� olmas� sebebi ile AddRazorPages yerine AddControllersWithViews yani MVC uygulamas�n� aya�a kald�r.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient();


//builder.Services.AddControllers(); // Web API olarak uygulamay� aya�a kald�rma

// ConfigurationManager s�n�f� web config gibi config dosyalar�ndan ayarlar okumam�za olanak sa�lar.
// appSetting.json dosyas�ndaki json olan nesneleri okumak i�in ConfigurationManager s�n�f�n� kullan�yoruz.
// UseSqlServer MSSql Server kulland�k
// NORTHWNDContext ile veri taban� nesnesini uygulaman�n servis s�recine katt�k. art�k veri taban� instancelar�n� uygulama y�netecek.

// registration k�sm�
builder.Services.AddDbContext<NORTHWNDContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// yani ben UseSqlServer yerine useNpl() postgres �zerinden t�m uygulamay� tek yerden de�i�tirmi� olurum.

// Mvc de uygulama i�erisine tan�t�lacak olan servisler IServiceCollection ile servisleri uygulama ayakta oldu�u s�rece y�netimini sa�lar.

// net core service y�netimi, class instance y�netimini merkezi olarak bu dosyadan yapar. b�ylelikle tek bir dosyadan uygulamadaki ba��ml�l�klar� de�i�tirebiliriz.


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

// Default sayfa ilk a��ld���nda uygulama Home/Index route y�nlendirir.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
