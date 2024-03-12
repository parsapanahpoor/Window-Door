
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.AdminPanel.OrderCheques.Command;

public record ChangeOrderChequeStateAdminCommandHandler : IRequestHandler<ChangeOrderChequeStateAdminCommand, bool>
{
    #region Ctor

    private readonly IOrderChequeCommandRepository _commandRepository;
    private readonly IOrderChequeQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderChequeStateAdminCommandHandler(IOrderChequeCommandRepository commandRepository ,
                                                     IOrderChequeQueryRepository queryRepository ,
                                                     IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(ChangeOrderChequeStateAdminCommand request, CancellationToken cancellationToken)
    {
        #region Get Order Cheque By Id 

        var cheque = await _queryRepository.Get_OrderCheque_ByIdAsync(request.OrderChequeId , cancellationToken);
        if (cheque == null) return false;

        #endregion

        #region Update Cheque

        cheque.OrderChequeAdminState = request.OrderChequeAdminState;
        cheque.AdminRejectDescription = request.AdminRejectDescription;

        if (request.OrderChequeAdminState == Domain.Enums.Order.OrderChequeAdminState.Accept)
                                                                cheque.AdminRejectDescription = null;

        //Update Method
        _commandRepository.Update_OrderCheque(cheque);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
