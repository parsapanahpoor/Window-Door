namespace Window.Domain.Interfaces.ShopColors;

public interface IShopColorsCommandRepository
{
	#region Admin Side 

	Task AddAsync(Domain.Entities.ShopColors.ShopColor shopColor, CancellationToken cancellationToken);

	void Update(Domain.Entities.ShopColors.ShopColor shopColor);

	#endregion
}
