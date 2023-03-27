using Microsoft.AspNetCore.Mvc;
using NorthwindApp.Models;

namespace NorthwindApp.Controllers
{
  public class ProductController : Controller
  {
    // 1. adım modeli açtık
    // 2. controllers klasörü add controller products
    // 3. list action açtık, modeli actionda çağırıp view'e göndericez, dinamik view oluşsun.
    public IActionResult List()
    {

      var plist = new List<ProductViewModel>();
      plist.Add(new ProductViewModel { Id = 1, Name = "Kazak", Price = 20, Stock = 30 });
      plist.Add(new ProductViewModel { Id = 1, Name = "Gömlek", Price = 30, Stock = 30 });
      // veri tabanınan bağlanıp ilgili dataları view'hazır edicez.

      // view' içerisine model gönderilmeyince boş statik sayfa oluştururuz.

      return View(plist);


      // tupple kullanamdan OOP ile başka model içine alarak çözdük

      //var pageVm = new ProductListPageViewModel();
      //pageVm.Products = plist;
      //pageVm.Error = new ErrorViewModel();

      //return View(pageVm);



      // Birden fazla model var
      //var tpModel =
      //      new Tuple<List<ProductViewModel>, ErrorViewModel>(plist,new ErrorViewModel());

      // return View(tpModel);

      // view'e sadece 1 model gönderme hakkımız var ya plist yada pageVM, yada tupple

    


    }


    // sayfa açılıdğında bize create view yönlendirecek
    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }


    [HttpPost]
    public IActionResult Create(CreateProductInputModel model)
    {
      // mvc default validasyon kavramı modelState
      if(ModelState.IsValid)
      {
        // veri tabanı işlemleri yap

        ModelState.AddModelError("Name", "Name alanını beğenmedim");
        ModelState.Clear();
      }

      return View();
    }



  }
}
