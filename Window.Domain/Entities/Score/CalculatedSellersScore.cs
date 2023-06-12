#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Score;

#endregion

public class CalculatedSellersScore  : BaseEntity
{
	#region properties

	public ulong UserSellerId { get; set; }

	public int CalculatedScore { get; set; }

	#endregion
}
