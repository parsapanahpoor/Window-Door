#region Usings

using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.BulkSMS;

#endregion


public sealed class BulkSMS : BaseEntity
{
	#region properties

	public string Mobile { get; set; }

	public string Username { get; set; }

	public bool IsSent { get; set; }

	#endregion
}
