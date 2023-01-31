using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class AddCommentSiteSideViewModel 
    {
        #region properties

        [Required]
        public ulong SellerId { get; set; }

        [Required]
        public string Description { get; set; }

        #endregion
    }
}
