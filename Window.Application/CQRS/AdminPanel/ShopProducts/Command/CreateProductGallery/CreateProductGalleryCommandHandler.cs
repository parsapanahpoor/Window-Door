
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopProductGallery;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductGallery;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateProductGallery;

public record CreateProductGalleryCommandHandler : IRequestHandler<CreateProductGalleryCommand, bool>
{
    #region Ctor

    private readonly IShopProductGalleryCommandRepository _commandRepository;
    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMarketQueryRepository _maketsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public CreateProductGalleryCommandHandler(IShopProductGalleryCommandRepository commandRepository,
                                            IUnitOfWork unitOfWork,
                                            IMarketQueryRepository maketsQueryRepository,
                                            IShopProductQueryRepository shopProductQueryRepository,
                                            IShopProductCommandRepository shopProductCommandRepository)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _maketsQueryRepository = maketsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _shopProductCommandRepository = shopProductCommandRepository;
    }

    #endregion

    public async Task<bool> Handle(CreateProductGalleryCommand request, CancellationToken cancellationToken)
    {
        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.productId);
        if (originProduct == null) return false;

        #endregion

        #region Add Gallery

        ShopProductGallery productGallery = new ShopProductGallery()
        {
            ProductId = request.productId,
        };

        if (request.image != null && request.image.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(request.image.FileName);
            request.image.AddImageToServer(imageName, FilePaths.ProductsPathServer, 400, 300, FilePaths.ProductsPathThumbServer);
            productGallery.ImageName = imageName;

            //Add Product Origin Image
            if (originProduct.ProductImage == "default.png")
            {
                originProduct.ProductImage = productGallery.ImageName;

                _shopProductCommandRepository.Update(originProduct);
            }

            await _commandRepository.AddAsync(productGallery, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        return true;
    }
}
