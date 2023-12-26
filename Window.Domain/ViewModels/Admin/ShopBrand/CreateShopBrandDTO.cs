using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Window.Domain.ViewModels.Admin.ShopBrand;

public record CreateShopBrandDTO
{
	#region properties

	[Display(Name = "عنوان")]
	[Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
	[MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
	public string Title { get; set; }

    public decimal Priority { get; set; }

	#endregion
}

public enum CreateShopBrandResult
{
	Success,
	Fail,
}
