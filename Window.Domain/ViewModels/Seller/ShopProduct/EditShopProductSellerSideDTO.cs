using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Window.Domain.ViewModels.Seller.ShopProduct;

public record EditShopProductSellerSideDTO
{
    #region Properties

    public ulong ShopProductId { get; set; }

    [Display(Name = "عنوان ")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(400)]
    public string Title { get; set; }

    [Display(Name = "توضیح مختصر")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(500)]
    public string ShortDescription { get; set; }

    [Display(Name = "متن ")]
    public string? Description { get; set; }

    public string Price { get; set; }

    [Display(Name = "تگ  ")]
    [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? ProductTag { get; set; }

    public ulong ShopColorId { get; set; }

    public ulong SaleScaleId { get; set; }

    public int SaleRatio { get; set; }

    #endregion
}

public enum EditShopProductFromSellerPanelResult
{
    [Display(Name = "موفق")]
    Success,
    [Display(Name = "ناموفق")]
    Faild,
    [Display(Name = "دسته بندی وارد نشده است ")]
    MainCategoryNotFound,
    SellerIsNotFound
}
