using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.Counseling;
using Window.Domain.Entities.TechnicalIssues;

namespace Window.Domain.ViewModels.Site.Home
{
    public class IndexViewModel
    {
        #region properties

        public TechnicalIssues TazminDarKharid { get; set; }

        public List<Counseling> FreeConsultant { get; set; }

        public List<Entities.Article.Article> Articles { get; set; }

        #endregion
    }
}
