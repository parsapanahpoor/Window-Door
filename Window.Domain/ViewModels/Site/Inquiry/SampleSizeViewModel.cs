using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class SampleSizeViewModel
    {
        #region properties

        public ulong SampleId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ulong UserId { get; set; }

        #endregion
    }
}
