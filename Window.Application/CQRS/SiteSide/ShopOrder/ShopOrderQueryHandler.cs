
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces;
using Window.Domain.Interfaces.Order;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.SiteSide.ShopOrder;

public record ShopOrderQueryHandler : IRequestHandler<ShopOrderQuery, AddToShopOrderRes>
{
    #region Ctor

    private readonly IOrderCommandRepository _orderCommandRepository;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ShopOrderQueryHandler(IOrderCommandRepository orderCommandRepository ,
                                 IOrderQueryRepository orderQueryRepository ,
                                 IShopProductQueryRepository shopProductQueryRepository ,
                                 IUnitOfWork unitOfWork ,
                                 IUserRepository userRepository)
    {
        _orderCommandRepository = orderCommandRepository;
        _orderQueryRepository = orderQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    #endregion

    public async Task<AddToShopOrderRes> Handle(ShopOrderQuery request, CancellationToken cancellationToken)
    {
        #region Get Product By Id

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken , request.productId);
        if (product == null) return AddToShopOrderRes.Faild;

        #endregion

        #region Get User By Id 

        var user = await _userRepository.IsUserExist(request.userId);
        if (user == false) return AddToShopOrderRes.Faild;

        #endregion

        #region Is Exist Any Order

        var order = await _orderQueryRepository.IsExistAnyWaitingOrder(request.userId , cancellationToken);

        if (order == null)
        {
            //Add Order 
            Order newOrder = new Order()
            {
                CreateDate = DateTime.Now,
                IsDelete = false,
                IsFinally = false,
                UserId = request.userId
            };

            await _orderCommandRepository.AddAsync(newOrder , cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            //Add Order Detail
            OrderDetail orderDetail = new()
            {
                Count = 1,
                CreateDate = DateTime.Now,
                IsDelete = false,
                OrderId = newOrder.Id,
                ProductId = product.Id,
            };

            await _orderCommandRepository.AddOrderDetailAsync(orderDetail , cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            //Is Exist Order Detail By This Product Id
            var orderDetail = await _orderQueryRepository.Get_OrderDetail_ByOrderIdAndProductId(order.Id , product.Id , cancellationToken);
            if (orderDetail == null)
            {
                //Add OrderDetail
                OrderDetail newOrderDetail = new()
                {
                    Count = 1,
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    OrderId = order.Id,
                    ProductId = product.Id,
                };

                await _orderCommandRepository.AddOrderDetailAsync(orderDetail, cancellationToken);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                orderDetail.Count++;

                _orderCommandRepository.UpdateOrderDetail(orderDetail);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        #endregion

        throw new NotImplementedException();
    }
}
