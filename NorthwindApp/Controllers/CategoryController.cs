using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NorthwindApp.Entities;
using System.Security.Cryptography.Xml;

namespace NorthwindApp.Controllers
{
  public class CategoryController : Controller
  {

    //private readonly NORTHWNDContext db =  new NORTHWNDContext();
    private readonly NORTHWNDContext db;

    // Dependecy Injection diyourz. CategoryController'a ait dışardan hizmet verecek olan servisleri contructor vasıtası ile paramtere olarak geçiyoruz, IserviceColletion tarafından zaten bu servis instanceları yönetildiği için ekstra bir kod maliyeti çıkmıyor.  
    // resolve ediyoruz.
    public CategoryController(NORTHWNDContext db) // contructor dependency injection yapıyoruz.
    {
      
      this.db = db;
    }


    public IActionResult Index()
    {

      // linq lambda extension method yöntemi
      var clist = db.Categories.ToList(); // lamda expression yöntemi ile veri çekmemizi sağlar // select * from Categories
      var clist2 = (from c in db.Categories select c).ToList(); // linq sorgusu (yukarıdakide aşağıdaki de sql giderken aynı çıktıya sahip)

      // birden fazla join varsa linq yapısını kullanmak daha mantıklı
      var cp2 = (from c in db.Categories
                 join p in db.Products on c.CategoryId equals p.CategoryId
                 select new
                 {
                   CategoryName = c.CategoryName,
                   ProductName = p.ProductName

                 }).ToList();



      var cp3 = db.Categories.Join(
              db.Products,
              cat => cat.CategoryId,
              pro => pro.CategoryId,
              (cat, pro) => new { CategoryName = cat.CategoryName, ProductName = pro.ProductName });



      // ürün fiyatı 10 tl üzerinde olan ürünler
      var p1 = db.Products.Where(x => x.UnitPrice > 10).ToList();
      // UrunId 1 olan ürünü bul
      var p2 = db.Products.Find(1); // 1 nolu ürün
      // ilk 5 ürünü atla, sonraki 10 ürünü seç // a-z orderby bir sırlama yaptık
      var p3 = db.Products.OrderBy(x => x.UnitPrice).Skip(5).Take(10).ToList();
      // ürünleri isimlerine filtrele
      var p4 = db.Products.Where(x => x.ProductName.Contains("abc")).ToList();
      var p5 = db.Products.Where(x => EF.Functions.Like(x.ProductName, $"%abc%")).ToList();
      // tersten ürünleri sırala
      var p6 = db.Products.OrderByDescending(x => x.UnitsInStock).ThenBy(x => x.ProductName); // thenBy ikinci 
      // ürün stok değer 10 ile 15 arasında olanları bul
      var p7 = db.Products.Where(x => x.UnitsInStock >= 10 && x.UnitsInStock <= 15).ToList(); // between yerine
      // ürünlerin toplam maliyetini bul
      var p8 = db.Products.Sum(x => x.UnitPrice * x.UnitsInStock);
      // toplam ürün adetini bul
      var p9 = db.Products.Count();
      // böyle bir ürün varmı
      var p10 = db.Products.Any(x => x.ProductName == "Chai");
      // LINQ => SQL diline çevirir.

      // EF Kısmında Store Procedure Çağıurma, Raw Sql Query Çağırabiliriz. Performans için
      // Insert, Update, Delete, Crud.


      // nesne disposing işlemlerini artık mvc core bizim için yöentiyor.
      //using (NORTHWNDContext db = new NORTHWNDContext())
      //{

      //}

      return View();
    }
  }
}
