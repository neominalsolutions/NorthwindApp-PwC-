using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace NorthwindApp.Entities.Configurations
{
  // Nesnenin veri tabanı ile alakalı olan ayarlarını başka bir nesne üzerinde yaptım.
  // IEntityTypeConfiguration denilen generic bir interface sayesinde

  // config dosyalarındaki yapılan değişiklikler ile veri tabanında güncelleme yaparız.
  public class CategoryConfiguration : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {

    
        //entity.ToTable("KategoriTablous");
        builder.HasIndex(e => e.CategoryName, "CategoryName");
        builder.ToTable("Kategori Tablosu");
        builder.Property(e => e.CategoryId).HasColumnName("CategoryID");
        builder.Property(e => e.CategoryName).HasMaxLength(15);
        builder.Property(e => e.Description).HasColumnType("ntext");
        builder.Property(e => e.Picture).HasColumnType("image");
        builder.Property(x => x.CategoryName).HasColumnName("KategoriAdi");




      //builder.HasKey(x => x.CategoryId);

      // 1 e çok ilişkili bir tablo relation kurmuş olduk.

      //builder.HasMany<Product>()
      //  .WithOne(x => x.Category)
      //  .HasForeignKey(x => x.CategoryId);
     

     
    }
  }
}
