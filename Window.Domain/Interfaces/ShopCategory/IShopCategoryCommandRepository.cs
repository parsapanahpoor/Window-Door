using Window.Domain.Entities;

namespace Window.Domain.Interfaces.ShopCategory;

public interface IShopCategoryCommandRepository
{
    Task AddAsync(Domain.Entities.ShopCategory shopCategory, CancellationToken cancellationToken);

    Task AddShopProductSelectedCategoriesAsync(Domain.Entities.ShopCategories.ShopProductSelectedCategories selectedCategories, CancellationToken cancellationToken);

    void Update(Domain.Entities.ShopCategory shopCategory);
}
