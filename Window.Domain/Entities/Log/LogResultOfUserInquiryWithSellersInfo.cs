#region Usings

using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Log;

#endregion

public sealed class LogResultOfUserInquiryWithSellersInfo : BaseEntity
{
	#region properties

	public ulong UserId { get; set; }

	public ulong LogInquiryForUserId { get; set; }

	public ulong SellerUserId { get; set; }

	public ulong BrandId { get; set; }

	public double Price { get; set; }

	public int SellerScore { get; set; }

	public string? SellerShopName { get; set; }

	#endregion
}
