using Window.Domain.Entities;

namespace Window.Domain.Interfaces.ShopCategory;

public interface IShopCategoryCommandRepository
{
    Task AddAsync(Domain.Entities.ShopCategory shopCategory, CancellationToken cancellationToken);
}
