using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Site.Shop.Order;

namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public record ShowInvoiceQueryHandler : IRequestHandler<ShowInvoiceQuery, InvoiceDTO>
{
    #region Ctor

    private readonly IOrderQueryRepository _orderQueryRepository;

    public ShowInvoiceQueryHandler(IOrderQueryRepository orderQueryRepository)
    {
        _orderQueryRepository = orderQueryRepository;
    }

    #endregion

    public async Task<InvoiceDTO?> Handle(ShowInvoiceQuery request, CancellationToken cancellationToken)
    {
        //Get Lastest Waiting For Payment Order
        var order = await _orderQueryRepository.GetLastest_WaitingForPaymentOrder_ByUserId(request.UserId , cancellationToken);
        if (order == null) return null;

        return await _orderQueryRepository.FillInvoiceDTO(request.UserId , order.Id , cancellationToken);
    }
}
