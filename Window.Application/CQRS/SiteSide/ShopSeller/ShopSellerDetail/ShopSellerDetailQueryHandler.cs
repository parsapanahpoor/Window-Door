using Window.Application.Interfaces;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.SellerDetail;

namespace Window.Application.CQRS.SiteSide.ShopSeller.ShopSellerDetail;

public record ShopSellerDetailQueryHandler : IRequestHandler<ShopSellerDetailQuery, SellerDetailDTO>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBrandService _brandService;

    public ShopSellerDetailQueryHandler(IShopProductQueryRepository shopProductQueryRepository,
                                        IBrandService brandService,
                                        IShopColorsQueryRepository shopColorsQueryRepository,
                                        IUserRepository userRepository)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _userRepository = userRepository;
        _brandService = brandService;
    }

    #endregion

    public async Task<SellerDetailDTO?> Handle(ShopSellerDetailQuery request, CancellationToken cancellationToken)
    {
        #region Get User By Id

        var user = await _userRepository.GetUserById(request.sellerId);
        if (user == null) return null;

        #endregion

        #region Fill Model 

        SellerDetailDTO model = new SellerDetailDTO()
        {
            ShopCards = await _shopProductQueryRepository.FillShopCard(user.Id, cancellationToken),
            UserDetail = new UserDetail()
            {
                RegisterDate = user.CreateDate,
                SellerMobile = user.Mobile,
                SellerUserId = user.Id,
                SellerUsername = user.Username,
                ShopCenterName = user.ShopName,
                SellerImageName = user.Avatar
            }
        };

        #endregion

        return model;
    }
}
