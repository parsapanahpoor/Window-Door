using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Window.Domain.Enums.ShopCategory;

namespace Window.Domain.ViewModels.Admin.ShopCategory;

public record EditShopCartDTO
{
    #region properties

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
    [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
    public string Title { get; set; }

    public Domain.Enums.ShopCategory.ShopCategory ShopCategory{ get; set; }

    public ulong ShopCategoryId { get; set; }

    public ulong? ParentId { get; set; }

    #endregion
}

public enum EditShopCartResult
{
    Success,
    Fail,
}