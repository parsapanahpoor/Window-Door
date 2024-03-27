using Microsoft.EntityFrameworkCore.Storage;
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Entities.Market;
using Window.Domain.Interfaces.OrderCheque;

namespace Window.Application.CQRS.SellerPanel.SellerChequeInfo.Command.AddOrEditSellerChequeInfo;

public record AddOrEditSellerChqueInfoCommandHandler : IRequestHandler<AddOrEditSellerChqueInfoCommand, bool>
{
    #region Ctor

    private readonly IOrderChequeQueryRepository _orderChequeQueryRepository;
    private readonly IOrderChequeCommandRepository _orderChequeCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrEditSellerChqueInfoCommandHandler(IOrderChequeQueryRepository orderChequeQueryRepository,
                                                  IOrderChequeCommandRepository orderChequeCommandRepository,
                                                  IUnitOfWork unitOfWork)
    {
        _orderChequeCommandRepository = orderChequeCommandRepository;
        _orderChequeQueryRepository = orderChequeQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(AddOrEditSellerChqueInfoCommand request, CancellationToken cancellationToken)
    {
        //Get Seller Cheque Info
        var chequeInfo = await _orderChequeQueryRepository.Get_SellerChequeInfo_BySellerUserId(request.SellerUserId , cancellationToken);

        //Add New Record
        if (chequeInfo == null)
        {
            var newChequeInfo = new Domain.Entities.Market.SellerChequeInfo()
            {
                CountOfCheque = request.CountOfCheque,
                CreateDate = DateTime.Now,
                HasLimitation = request.HasLimitation,
                IsDelete = false,
                SellerMaximumDays = request.SellerMaximumDays,
                SellerUserId = request.SellerUserId
            };

            await _orderChequeCommandRepository.AddSellerChequeInfo(newChequeInfo, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        //Update Record
        else
        {
            chequeInfo.CountOfCheque = request.CountOfCheque;
            chequeInfo.SellerMaximumDays = request.SellerMaximumDays;
            chequeInfo.HasLimitation = request.HasLimitation;

            _orderChequeCommandRepository.Update_SellerChequeInfo(chequeInfo);
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
