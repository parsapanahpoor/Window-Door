using System.ComponentModel.DataAnnotations;

namespace Window.Domain.Enums.ShopCategory;

public enum ShopCategory
{
    [Display(Name = "پروفیل upvc")]
    ProfilUPVC,

    [Display(Name = "پروفیل آلومینیومی")]
    ProfilAL,

    [Display(Name = "– پروفیل گالوانیزه ( تقویت )")]
    ProfilGalvanize,

    [Display(Name = "– یراق آلات درب و پنجره upvc")]
    YaraghPanjerehUPVC,

    [Display(Name = "– یراق آلات درب و پنجره آلومینیومی")]
    YaraghPanjerehAL,

    [Display(Name = "– یراق آلات درب و پنجره های خاص")]
    YaraghKhas,

    [Display(Name = "– نوار مویی و لاستیکها")]
    NavarMoie,

    [Display(Name = "درب ضد سرقت و داخلی")]
    DarbHayeZedeSerghat,

    [Display(Name = "دیگر محصولات")]
    OtherProducts,

    [Display(Name = "دستگاه های مونتاژ پنجره دوجداره")]
    DoubleGlazedWindowAssemblyMachines,

    [Display(Name = "خدمات لمینیت پروفیل upvc")]
    UpvcProfileLaminationServices,

    [Display(Name = "شیشه دوجداره")]
    DoubleGlazedWindow,
}
