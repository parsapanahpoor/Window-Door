
using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.AdminPanel.OrderCheques.Command;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.OrderCheque.Command;

internal class ChangeOrderChequeStateSellerCommandHandler : IRequestHandler<ChangeOrderChequeStateSellerCommand, bool>
{
    #region Ctor

    private readonly IOrderChequeCommandRepository _commandRepository;
    private readonly IOrderChequeQueryRepository _queryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderChequeStateSellerCommandHandler(IOrderChequeCommandRepository commandRepository,
                                                     IOrderChequeQueryRepository queryRepository,
                                                     IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(ChangeOrderChequeStateSellerCommand request, CancellationToken cancellationToken)
    {
        #region Get Order Cheque By Id 

        var cheque = await _queryRepository.Get_OrderCheque_ByIdAsync(request.OrderChequeId, cancellationToken);
        if (cheque == null) return false;
        if (cheque.SellerUserId != request.SellerUserId) return false;

        #endregion

        #region Update Cheque

        cheque.OrderChequeSellerState = request.OrderChequeSellerState;
        cheque.SellerRejectDescription = request.SellerRejectDescription;

        if (request.OrderChequeSellerState == Domain.Enums.Order.OrderChequeSellerState.Accept)
            cheque.SellerRejectDescription = null;

        //Update Method
        _commandRepository.Update_OrderCheque(cheque);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
