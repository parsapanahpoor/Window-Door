using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Data;
using System.Net.NetworkInformation;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Admin.OrderCheque;
using Window.Domain.ViewModels.Seller.ShopOrder;
using Window.Domain.ViewModels.Site.Shop.Order;
namespace Window.Infra.Data.Repository.Order;

public class OrderQueryRepository : QueryGenericRepository<Domain.Entities.ShopOrder.Order>, IOrderQueryRepository
{
    #region Ctor

    private readonly WindowDbContext _context;

    public OrderQueryRepository(WindowDbContext context) : base(context)
    {
        _context = context;
    }

    #endregion

    #region Site Side 

    public async Task<Domain.Entities.ShopOrder.Order?> IsExistAnyWaitingOrder(ulong userId,
                                                                              CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.UserId == userId &&
                                   !p.LocationId.HasValue &&
                                   !p.IsFinally)
                             .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.ShopOrder.Order?> IsExistAnyNotFinallyOrder(ulong userId,
                                                                             CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.UserId == userId &&
                                   !p.IsFinally)
                             .FirstOrDefaultAsync();
    }

    public async Task<OrderDetail?> Get_OrderDetail_ByOrderIdAndProductId(ulong orderId,
                                                                         ulong productId,
                                                                         CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId &&
                                    p.ProductId == productId)
                             .FirstOrDefaultAsync();
    }

    public async Task<List<ulong>> GetProductIds_InOrderDetails_ByOrderId(ulong orderId,
                                                                          CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId)
                             .Select(p => p.ProductId)
                             .ToListAsync();
    }

    public async Task<List<ulong>> GetOrderDetailIds_OrderDetails_ByOrderId(ulong orderId, CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId)
                             .Select(p => p.Id)
                             .ToListAsync();
    }

    public async Task<List<OrderDetail>> GetOrderDetails_ByOrderId(ulong orderId, CancellationToken cancellation)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId)
                             .ToListAsync();
    }


    public async Task<ShopCartOrderDetailItems?> FillShopCartOrderDetailItems(ulong orderDetailId, CancellationToken cancellationToken)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderDetailId)
                             .Select(p => new ShopCartOrderDetailItems()
                             {
                                 Products = _context.ShopProducts
                                                    .AsNoTracking()
                                                    .Where(s => !s.IsDelete &&
                                                           s.Id == p.ProductId)
                                                    .Select(s => new Product()
                                                    {
                                                        ProductEntity = p.Count,
                                                        ProductId = s.Id,
                                                        ProductTitle = s.ProductName,
                                                        ProductImage = s.ProductImage,
                                                        ProductPrice = s.Price,
                                                        SellerInfo = _context.Users
                                                                             .AsNoTracking()
                                                                             .Where(u => !u.IsDelete &&
                                                                                    u.Id == s.SellerUserId)
                                                                             .Select(u => new ShopCartOrderProductSellerInfo()
                                                                             {
                                                                                 SellerUserId = s.SellerUserId,
                                                                                 SellerUsername = u.Username
                                                                             })
                                                                             .FirstOrDefault(),
                                                        Color = _context.ShopColors
                                                                        .AsNoTracking()
                                                                        .Where(c => !c.IsDelete &&
                                                                               c.Id == s.ProductColorId)
                                                                        .Select(c => new Color()
                                                                        {
                                                                            ColorCode = c.ColorCode,
                                                                            ColorId = c.Id,
                                                                            ColorTitle = c.ColorTitle
                                                                        })
                                                                        .FirstOrDefault()
                                                    })
                                                    .FirstOrDefault()
                             })
                             .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.ShopOrder.Order?> GetLastestWaitingForInformationOrderByUserId(ulong UserId,
                                                                                                    CancellationToken cancellation)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.UserId == UserId &&
                                                 !p.LocationId.HasValue);
    }

    public async Task<Domain.Entities.ShopOrder.Order?> GetLastest_WaitingForPaymentOrder_ByUserId(ulong UserId,
                                                                                                   CancellationToken cancellation)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => !p.IsDelete &&
                                                  p.UserId == UserId &&
                                                 !p.IsFinally &&
                                                  p.LocationId.HasValue &&
                                                 !p.PaymentWay.HasValue);
    }

    public async Task<ulong> GetLastestProductId_InOrderDetail_ByOrderId(ulong orderId)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => p.OrderId == orderId)
                             .Select(p => p.ProductId)
                             .FirstOrDefaultAsync();
    }

    public async Task<bool> IsExistAnyOrderInWaitingForPaymentStateByUserId(ulong userId,
                                                                            CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .AnyAsync(p => !p.IsDelete &&
                                       p.UserId == userId &&
                                       p.LocationId.HasValue &&
                                      !p.IsFinally);
    }

    public async Task<List<OrderDetail>> GetOrderDetails_InOrderDetails_ByOrderId(ulong orderId,
                                                                                  CancellationToken cancellationToken)
    {
        return await _context.OrderDetails
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.OrderId == orderId)
                             .ToListAsync();

    }

    public async Task<InvoiceDTO?> FillInvoiceDTO(ulong userId,
                                                  ulong orderId,
                                                  CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderId)
                             .Select(p => new InvoiceDTO()
                             {
                                 OrderId = orderId,
                                 OrderDetails = _context.OrderDetails
                                                        .AsNoTracking()
                                                        .Where(s => !s.IsDelete &&
                                                               s.OrderId == orderId)
                                                        .Select(s => new OrderDetailForInvoice()
                                                        {
                                                            Count = s.Count,
                                                            Product = _context.ShopProducts
                                                                              .AsNoTracking()
                                                                              .Where(o => !o.IsDelete &&
                                                                                     o.Id == s.ProductId)
                                                                              .Select(o => new ProductForInvoice()
                                                                              {
                                                                                  Price = o.Price,
                                                                                  ProductId = o.ProductBrandId,
                                                                                  ProductImage = o.ProductImage,
                                                                                  ProductTitle = o.ProductName,
                                                                                  SellerName = _context.Users
                                                                                 .AsNoTracking()
                                                                                 .Where(u => !u.IsDelete &&
                                                                                        u.Id == o.SellerUserId)
                                                                                 .Select(u => u.Username)
                                                                                 .FirstOrDefault(),
                                                                                  ColorName = _context.ShopColors
                                                                                                      .AsNoTracking()
                                                                                                      .Where(c => !c.IsDelete &&
                                                                                                             c.Id == o.ProductColorId)
                                                                                                      .Select(c => c.ColorTitle)
                                                                                                      .FirstOrDefault(),
                                                                              }).FirstOrDefault()

                                                        }).ToList(),
                                 Location = _context.Locations
                                                    .AsNoTracking()
                                                    .Where(l => !l.IsDelete &&
                                                           l.UserId == userId &&
                                                           l.Id == p.LocationId)
                                                    .FirstOrDefault(),

                             })
                             .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.ShopOrder.Order?> GetLastest_NotFinally_Order(ulong userId)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.UserId == userId &&
                                   !p.IsFinally)
                             .FirstOrDefaultAsync();
    }

    public async Task<ManageShopOrderDetailDTO?> FillManageShopOrderDetailDTO(ulong userId,
                                                                             CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.UserId == userId &&
                                   !p.IsFinally &&
                                    p.PaymentWay.HasValue &&
                                    p.PaymentWay.Value == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment)
                             .Select(p=> new ManageShopOrderDetailDTO()
                             {
                                 Order = p,
                                 OrderDetails = _context.OrderDetails
                                                        .AsNoTracking()
                                                        .Where(d => !d.IsDelete &&
                                                               d.OrderId == p.Id)
                                                        .Select(d => new ManageShopOrderDetailAdminDTO()
                                                        {
                                                            CountOfChoice = d.Count,
                                                            Product = _context.ShopProducts
                                                                              .AsNoTracking()
                                                                              .Where(pr => !pr.IsDelete &&
                                                                                     pr.Id == d.ProductId)
                                                                              .Select(pr => new ProductAdminSideDTO()
                                                                              {
                                                                                  ProductId = pr.Id,
                                                                                  ProducPrice = pr.Price,
                                                                                  ProductImage = pr.ProductImage,
                                                                                  ProductTitle = pr.ProductName,
                                                                                  ShortDescription = pr.ShortDescription
                                                                              })
                                                                              .FirstOrDefault(),
                                                        })
                                                        .ToList(),
                                 Location = _context.Locations
                                                    .AsNoTracking()
                                                    .Where(w=> !w.IsDelete && 
                                                           w.Id == p.LocationId)
                                                    .FirstOrDefault(),
                                 OrderCheques = _context.orderCheques
                                                        .AsNoTracking()
                                                        .Where(c=> !c.IsDelete && 
                                                               c.OrderId == p.Id)
                                                        .ToList(),
                                 CustomerChequeInformation = null,
                                 SellerInformations = null,
                                 sellerChequeInfo = null,
                                 CustomerUserInformations = _context.Users
                                                                    .AsNoTracking()
                                                                    .Where(u => !u.IsDelete &&
                                                                           u.Id == p.UserId)
                                                                    .FirstOrDefault()
                             })
                             .FirstOrDefaultAsync();
    }

    public async Task<ManageShopOrderDetailDTO?> FillManageShopOrderDetailDTO(ulong userId,
                                                                              ulong orderId ,
                                                                              CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.UserId == userId &&
                                    p.Id == orderId && 
                                   !p.IsFinally &&
                                    p.PaymentWay.HasValue &&
                                    p.PaymentWay.Value == Domain.Enums.Order.OrderPaymentWay.InstallmentPayment)
                             .Select(p => new ManageShopOrderDetailDTO()
                             {
                                 Order = p,
                                 OrderDetails = _context.OrderDetails
                                                        .AsNoTracking()
                                                        .Where(d => !d.IsDelete &&
                                                               d.OrderId == p.Id)
                                                        .Select(d => new ManageShopOrderDetailAdminDTO()
                                                        {
                                                            CountOfChoice = d.Count,
                                                            Product = _context.ShopProducts
                                                                              .AsNoTracking()
                                                                              .Where(pr => !pr.IsDelete &&
                                                                                     pr.Id == d.ProductId)
                                                                              .Select(pr => new ProductAdminSideDTO()
                                                                              {
                                                                                  ProductId = pr.Id,
                                                                                  ProducPrice = pr.Price,
                                                                                  ProductImage = pr.ProductImage,
                                                                                  ProductTitle = pr.ProductName,
                                                                                  ShortDescription = pr.ShortDescription
                                                                              })
                                                                              .FirstOrDefault(),
                                                        })
                                                        .ToList(),
                                 Location = _context.Locations
                                                    .AsNoTracking()
                                                    .Where(w => !w.IsDelete &&
                                                           w.Id == p.LocationId)
                                                    .FirstOrDefault(),
                                 OrderCheques = _context.orderCheques
                                                        .AsNoTracking()
                                                        .Where(c => !c.IsDelete &&
                                                               c.OrderId == p.Id)
                                                        .ToList(),
                                 CustomerChequeInformation = null,
                                 SellerInformations = null,
                                 sellerChequeInfo = null,
                                 CustomerUserInformations = _context.Users
                                                                    .AsNoTracking()
                                                                    .Where(u => !u.IsDelete &&
                                                                           u.Id == p.UserId)
                                                                    .FirstOrDefault()
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion

    #region Admin Side 

    public async Task<ShowOrderChequeDetailAdminDTO?> FillShowOrderChequeDetailAdminDTO(ulong orderId,
                                                                                        CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderId)
                             .Select(p => new ShowOrderChequeDetailAdminDTO()
                             {
                                 Order = p,
                                 OrderDetails = _context.OrderDetails
                                                        .AsNoTracking()
                                                        .Where(d => !d.IsDelete &&
                                                               d.OrderId == p.Id)
                                                        .Select(d=> new ManageShopOrderDetailAdminDTO()
                                                        {
                                                            CountOfChoice = d.Count,
                                                            Product = _context.ShopProducts
                                                                              .AsNoTracking()
                                                                              .Where(pr=> !pr.IsDelete &&
                                                                                     pr.Id == d.ProductId)
                                                                              .Select(pr=> new ProductAdminSideDTO()
                                                                              {
                                                                                  ProductId = pr.Id,
                                                                                  ProducPrice = pr.Price,
                                                                                  ProductImage = pr.ProductImage,
                                                                                  ProductTitle = pr.ProductName,
                                                                                  ShortDescription = pr.ShortDescription
                                                                              })
                                                                              .FirstOrDefault(),
                                                        })
                                                        .ToList(),
                                 Location = _context.Locations
                                                    .AsNoTracking()
                                                    .Where(w => !w.IsDelete &&
                                                           w.Id == p.LocationId)
                                                    .FirstOrDefault(),
                                 OrderCheques = _context.orderCheques
                                                        .AsNoTracking()
                                                        .Where(c => !c.IsDelete &&
                                                               c.OrderId == p.Id)
                                                        .ToList(),
                                 CustomerChequeInformation = null,
                                 SellerInformations = null,
                                 sellerChequeInfo = null,
                                 CustomerUserInformations = _context.Users
                                                                    .AsNoTracking()
                                                                    .Where(u=> !u.IsDelete && 
                                                                           u.Id == p.UserId)
                                                                    .FirstOrDefault()
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion
}
