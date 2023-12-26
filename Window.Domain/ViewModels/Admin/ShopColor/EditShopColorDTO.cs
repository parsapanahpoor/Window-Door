using System.ComponentModel.DataAnnotations;
namespace Window.Domain.ViewModels.Admin.ShopColor;

public record EditShopColorDTO 
{
    #region properties

    public ulong Id { get; set; }

    [Display(Name = "عنوان")]
	[Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
	[MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
	public string Title { get; set; }

	public decimal Priority { get; set; }

    public string ColorCode { get; set; }

    #endregion
}

public enum EditShopColorResult
{
	Success,
	Fail,
}