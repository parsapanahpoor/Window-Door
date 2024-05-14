using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Counseling;
using Window.Domain.Entities.SiteSetting;
using Window.Domain.Entities.TechnicalIssues;

namespace Window.Domain.ViewModels.Site.Home
{
    public class IndexViewModel
    {
        #region properties

        public TechnicalIssues TazminDarKharid { get; set; }

        public List<Counseling> FreeConsultant { get; set; }

        public List<Entities.Article.Article> Articles { get; set; }

        public List<MainBrand> MainBrands { get; set; }

        public SiteSetting1 SiteSetting1 { get; set; }

        public List<ColorFullSiteSetting> ColorFullSiteSettings { get; set; }

        public MohasebeyeOnlineGheymat MohasebeyeOnlineGheymat { get; set; }

        public List<FreeConsultant> FreeConsultants { get; set; }

        public TazminDarKharid TazminKharid { get; set; }

        public List<LastestComponent> LastestComponents { get; set; }

        #endregion
    }
}
