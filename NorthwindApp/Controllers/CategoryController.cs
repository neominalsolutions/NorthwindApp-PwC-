using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NorthwindApp.Entities;
using NorthwindApp.Models;
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


   
    public IActionResult List()
    {

      var vm = new CategoryViewModel();
      vm.Categories = db.Categories.OrderBy(x=> x.CategoryName).ToList();


      return View(vm);
    }

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public JsonResult Create(Category model)
    {

      // application katmanı, service olan api katmanına burası taşınıcak.

      //db.Categories.Attach(model); // Attached
      db.Categories.Add(model);  // attach oluyor
      db.SaveChanges();
     


      // db ekleme işlemi 
      // mvc JsonResult sayfa yenilenmeden back ile front arası haberleşme sağlar
      // dbye kaydı atıp güncel modelimizi arayüze göndereceğiz.
      return Json(model); // bir request bulunuyoruz request sonucu yeniden bir response dönüyor, JSON formatında bir response
    }

    // {ControllerName}/{ActionName}/{id}
    public IActionResult Delete(int id)
    {

      var dbEntity = db.Categories.Find(id); // ATTACHED

      if(dbEntity == null)
      {
        return NotFound();
      }


      // delete edilecek veriyi göstermek için kullanıcaz
      return View(dbEntity);
    }

    [HttpPost]
    public IActionResult Delete(Category model)
    {
      // db deb ilgili yadı bulucaz
      // redirect Result

      // connected yöntem
      /*
      var dbEntity = db.Categories.Find(model.CategoryId); // find sorgusu kendi içerisinde changetracker mekanizmasını kullanmak için attach yapıyor.
      db.Categories.Remove(dbEntity); // deleted
      db.SaveChanges(); // deatach
      */

      // şuan çalıştığımız model nesnesin EntityState.Added ise 
      if (db.Entry<Category>(model).State == EntityState.Added)
      {

      }

      // disconneted yöntem
      //db.Categories.Attach(model);
      db.Categories.Remove(model); // remove da attach yapıyor gizliden, id olduğu için yaptı
      db.SaveChanges();

      return Redirect("/Category/List");
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
      var model = db.Categories.Find(id);

      if (id == null && model == null)
      {
        return NotFound();
      }

      // update edilecek kaydı arayüzde göstermek için Edit sayfası
      return View(model);
    }

    [HttpPost]
    public IActionResult Update(Category model)
    {

      // connected mimari
      // nesne db den bulunuyor, bulunan nesnenin state model ile değiştiriliyor
      // bulunan nesne attached olduğundan
      // save cahnages sonrasında db ye yansıyor
      //var dbEntity = db.Categories.Find(model.CategoryId);

      //if(dbEntity == null)
      //{
      //  return NotFound();
      //}

      //dbEntity.CategoryName = model.CategoryName;
      //dbEntity.Description = model.Description;

      // ADO.NET disconnected Mimari Tekniği
      // model nesnesi program nesnesi olduğu için db nesnesi gibi EF tarafından algılanlaıdır.
      // update methodu disconnect çalışır attach etmek zorundayız.
      db.Categories.Attach(model);
      // attach ile db tarafındaki nesne ile aynı yap.
      // EF attached olmuş nesneyi takibe alabiliyor.
      db.Categories.Update(model);
      // update methoduda ilgili nesneyi category tablosunda bulup güncelliyor
      // update ile savechanges olmadan önce entity modified olarak işaretleniyor.

      db.SaveChanges(); // excute sql yapmıyor.
      // update işlemi olucak
      // RedirectToAction ile list sayfasına yönlendirme yapacağız
      return RedirectToAction("List");
    }






    public IActionResult Index()
    {

      // linq lambda extension method yöntemi
      // var clist = db.Categories.AsNoTracking().ToList(); 
      // AsNoTracking ile veri çekilirken program tarafındaki bağ ile db tarafındaki bağ koparılır bu sayede update,delete, add gibi CUD işlemler yapana kadar kaynak tüketimi az olur. 
      // AsNoTracking ile çekilen nesneler artık DEATTACH modedadır.
      var clist = db.Categories.AsNoTracking().ToList(); // lamda expression yöntemi ile veri çekmemizi sağlar // select * from Categories
      //db.Categories.Attach(clist[0]);
      //db.Categories.Add(clist[0]);
      

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
