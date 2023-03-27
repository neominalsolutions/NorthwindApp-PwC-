using System.ComponentModel.DataAnnotations;

namespace NorthwindApp.Models
{
  public class CreateProductInputModel
  {
    [Required(ErrorMessage ="Name alanı boş geçilemez")]
    [MaxLength(15, ErrorMessage = "Makimum 15 karakter uzunluğunda olabilir")]
    public string Name { get; set; }

  }
}
