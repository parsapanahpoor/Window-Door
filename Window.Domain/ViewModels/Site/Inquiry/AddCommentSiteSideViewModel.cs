using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Enums.RequestForContract;

namespace Window.Domain.ViewModels.Site.Inquiry
{
    public class AddCommentSiteSideViewModel 
    {
        #region properties

        [Required]
        public ulong SellerId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public RequestForContractStatus ContractStep { get; set; }

        #endregion
    }
}
