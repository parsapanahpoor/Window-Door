using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;
using Window.Domain.Enums.Types;

namespace Window.Domain.Entities.Product
{
    public class ProductTypeCategory : BaseEntity
    {
        #region properties

        public ProductType ProductType { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        #endregion

        #region relations


        #endregion
    }
}
