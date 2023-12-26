using System.ComponentModel.DataAnnotations;
namespace Window.Domain.ViewModels.Admin.ShopBrand;

public record EditShopBrandDTO 
{
    #region properties

    public ulong Id { get; set; }

    [Display(Name = "عنوان")]
	[Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
	[MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
	public string Title { get; set; }

	public decimal Priority { get; set; }

    #endregion
}

public enum EditShopBrandResult
{
	Success,
	Fail,
}