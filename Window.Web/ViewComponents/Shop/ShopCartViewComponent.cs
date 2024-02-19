using MediatR;
using Microsoft.AspNetCore.Mvc;
using Window.Application.CQRS.SiteSide.ShopOrder.Query;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.Order;
namespace Window.Web.ViewComponents.Shop;

public class ShopCartViewComponent : ViewComponent
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;

    public ShopCartViewComponent(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    #endregion

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        if (User.Identity.IsAuthenticated)
        {
            var model = await Mediator.Send(new ShopCartQuery()
            {
                UserId = User.GetUserId(),
            },cancellationToken);
                   
            return View("ShopCart", model);
        }

        return View("ShopCart" , new ShopCartOrderDTO());
    }

}
