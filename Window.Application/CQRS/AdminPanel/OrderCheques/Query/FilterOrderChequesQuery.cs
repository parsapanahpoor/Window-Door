using Window.Domain.ViewModels.Admin.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Query;

public  class FilterOrderChequesQuery : IRequest<FilterOrderChequesDTO>
{
    #region properties

    public FilterOrderChequesDTO FilterOrderChequesDTO { get; set; }

    #endregion
}
