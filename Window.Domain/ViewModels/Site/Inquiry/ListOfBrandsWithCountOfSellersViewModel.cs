#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;

namespace Window.Domain.ViewModels.Site.Inquiry;

#endregion

public class ListOfBrandsWithCountOfSellersViewModel
{
	#region properties

	public MainBrand Brand { get; set; }

	public int CountOfSellers { get; set; }

	#endregion
}
