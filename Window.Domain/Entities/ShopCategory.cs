using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities;

public sealed class ShopCategory : BaseEntity
{
	#region properties

	[Display(Name = "عنوان")]
	[Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
	[MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
	public string Title { get; set; }

    public ShopCategory ShopCategoryType { get; set; }

    public ulong? ParentId { get; set; }

	#endregion
}
