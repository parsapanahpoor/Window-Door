#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.Entities.BulkSMS;

#endregion

public sealed class AllBulkSMS
{
	#region Properties

	public int CountOfSentSMS { get; set; }

	public bool IsUserRegistered { get; set; }

	public string Username { get; set; }

	public string Mobile { get; set; }

	public DateTime? LastestSMSSent { get; set; }

	#endregion
}
