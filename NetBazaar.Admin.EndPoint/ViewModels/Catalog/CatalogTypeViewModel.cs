using System.ComponentModel.DataAnnotations;

namespace NetBazaar.Admin.EndPoint.ViewModels.Catalog
{
    public class CatalogTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نوع محصول الزامی است.")]
        [StringLength(100, ErrorMessage = "نوع محصول نمی‌تواند بیش از 100 کاراکتر باشد.")]
        public string Type { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }

}
