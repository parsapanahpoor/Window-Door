using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Enums.ShopCategory;

public enum ShopCategory
{
	[Display(Name = "پروفیل های UPVC")]
	ProfilUPVC,

	[Display(Name = "یراق آلومینیوم")]
	ProfilAL,

	[Display(Name = "یراق آلات پنجره UPVC")]
	YaraghPanjerehUPVC,

	[Display(Name = "یراق آلات پنجره آلومینیومی")]
	YaraghPanjerehAL,

	[Display(Name = "یراق های خاص")]
	YaraghKhas,

	[Display(Name = "گالوانیزه")]
	Galvanize,

	[Display(Name = "درب های چوبی و ضد سرقت")]
	DarbHayeZedeSerghat,

	[Display(Name = "چهارچوب فلزی")]
	ChaharChobFelezi
}
