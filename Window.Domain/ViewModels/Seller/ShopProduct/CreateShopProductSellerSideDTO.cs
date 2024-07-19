using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Window.Domain.ViewModels.Seller.ShopProduct;

public class CreateShopProductSellerSideDTO
{
    #region Properties

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

    [Display(Name = "تگ  ")]
    [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string? ProductTag { get; set; }

    public ulong ShopColorId { get; set; }

    public ulong SaleScaleId { get; set; }

    public string Price { get; set; }

    public decimal SaleRatio { get; set; }

    public int ProductInventory { get; set; }

    public bool ShowProductInventory { get; set; }

    #endregion
}

public enum CreateShopProductFromSellerPanelResult
{
    [Display(Name = "موفق")]
    Success,
    [Display(Name = "ناموفق")]
    Faild,
    [Display(Name = "دسته بندی وارد نشده است ")]
    MainCategoryNotFound,
    SellerIsNotFound
}
