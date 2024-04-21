using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.ShopCategories;
using Window.Domain.Entities.ShopProduct;

namespace Window.Domain.Interfaces.ShopProduct;

public interface IShopProductCommandRepository
{
    #region Admin Side

    void Update_CustomerSuggestion(CustomersSuggestions customersSuggestions);

    Task Add_CustomerSuggestions(CustomersSuggestions customersSuggestions);

    Task Add_IncredileProducts(IncredibleProducts incredible);

    Task AddAsync(Entities.ShopProduct.ShopProduct  product, CancellationToken cancellationToken);

    Task AddShopTagAsync(ProductTag tag, CancellationToken cancellationToken);

    void Update(Entities.ShopProduct.ShopProduct product);

    void DeleteRange(IEnumerable<ProductTag> productTags);

    void DeleteRangeProductSelectedCategories(IEnumerable<ShopProductSelectedCategories> productSelectedCategories);

    void Update_IncredibleProducts(IncredibleProducts incredibleProducts);

    #endregion
}
