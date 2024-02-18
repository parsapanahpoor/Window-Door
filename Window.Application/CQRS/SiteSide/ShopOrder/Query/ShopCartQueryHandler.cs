using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public record ShopCartQueryHandler : IRequestHandler<ShopCartQuery, ShopCartOrderDTO>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;

    public ShopCartQueryHandler(IShopProductQueryRepository shopProductQueryRepository ,
                                IShopColorsQueryRepository shopColorsQueryRepository ,
                                IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<ShopCartOrderDTO?> Handle(ShopCartQuery request, CancellationToken cancellationToken)
    {
        ShopCartOrderDTO model = new ShopCartOrderDTO();

        //Get Order Waiting
        var order = await _orderQueryRepository.IsExistAnyWaitingOrder(request.UserId , cancellationToken);
        if (order == null) return null;

        //Get Order's OrderDetail Ids
        var orderDetailIds = await _orderQueryRepository.GetOrderDetailIds_OrderDetails_ByOrderId(order.Id , cancellationToken);
        if (orderDetailIds == null) return null;

        foreach (var orderDetailId in orderDetailIds)
        {
            model.ProductItems.Add(await _orderQueryRepository.FillShopCartOrderDetailItems(orderDetailId , cancellationToken));
        }

        model.TotalPrice = (int)model.ProductItems.Sum(p => p.Products.ProductPrice);

        return model;
    }
}
