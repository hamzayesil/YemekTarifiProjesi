using System.ComponentModel.DataAnnotations;

namespace YemekTarifiProjesi.Models
{
    public class Recipe
    {
      
        public int Id { get; set; }

        [Required(ErrorMessage = "Yemek adı boş bırakılamaz.")]
        [Display(Name = "Yemek Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçiniz.")]
        [Display(Name = "Kategori")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Malzemeler gereklidir.")]
        [Display(Name = "Malzemeler")]
        public string Ingredients { get; set; } = string.Empty;

        [Display(Name = "Hazırlanışı")]
        public string Instructions { get; set; } = string.Empty;

        [Display(Name = "Eklenme Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}