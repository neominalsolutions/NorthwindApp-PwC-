namespace NorthwindApp.Models
{
  // Sayfa için gerekli olan modelleri üzerinden barındıran yapılara Page view Model yapıları diyoruz
  public class ProductListPageViewModel
  {
    public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    public ErrorViewModel Error { get; set; }

  }
}
