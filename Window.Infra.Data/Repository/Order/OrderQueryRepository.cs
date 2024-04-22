using Microsoft.EntityFrameworkCore;
using System.Data;
using Window.Data;
using Window.Data.Context;
using Window.Domain.Entities.ShopOrder;
using Window.Domain.Interfaces.Order;
using Window.Domain.ViewModels.Admin.Contract;
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
                                                  p.LocationId.HasValue);
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
                                                                                  ProductId = o.ProductBrandId.Value,
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

    public async Task<ManageShopOrderDetailDTO?> FillManageShopOrderDetailDTO(ulong userId,
                                                                              ulong orderId,
                                                                              CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderId &&
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

    public async Task<bool> IsExist_MoreThanOneSeller_InOrderThatWillPayByInstaller(ulong orderId,
                                                                                    CancellationToken cancellationToken)
    {
        var productIds = await _context.OrderDetails
                                      .AsNoTracking()
                                      .Where(p => !p.IsDelete &&
                                             p.OrderId == orderId)
                                      .Select(p => p.ProductId)
                                      .ToListAsync();

        var sellerIds = new List<ulong>();

        if (productIds != null && productIds.Any())
        {
            foreach (var productId in productIds)
            {
                var sellerId = await _context.ShopProducts
                                             .Where(p => !p.IsDelete &&
                                                    p.Id == productId)
                                             .Select(p => p.SellerUserId)
                                             .FirstOrDefaultAsync();

                if (sellerId != 0) sellerIds.Add(sellerId);
            }
        }

        if (sellerIds != null && sellerIds.Any())
        {
            if (sellerIds.Count() == 1) return false;

            HashSet<ulong> set = new HashSet<ulong>();
            List<ulong> uniqueNumbers = new List<ulong>();

            foreach (ulong sellerId in sellerIds)
            {
                if (set.Add(sellerId))
                {
                    // If the number is added to the set, it means it's unique
                    uniqueNumbers.Add(sellerId);
                }
            }

            if (uniqueNumbers.Count() > 1) return true;
        }
        else
        {
            //This show that there is a problem 
            return true;
        }

        return false;
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

    #region Seller Side 

    public async Task<FilterShopOrdersSellerSideDTO> Fill_FilterShopOrdersSeller(FilterShopOrdersSellerSideDTO filter,
                                                                                 CancellationToken cancellationToken)
    {
        var orders = _context.Orders
                             .Where(p => !p.IsDelete)
                             .OrderByDescending(p => p.CreateDate)
                             .AsQueryable();

        var orderDetails = _context.OrderDetails
                                 .Where(p => !p.IsDelete)
                                 .OrderByDescending(p => p.CreateDate)
                                 .AsQueryable();

        var sellerProductsIds = _context.ShopProducts
                                        .Where(p => !p.IsDelete &&
                                               p.SellerUserId == filter.SellerUserId)
                                        .Select(p => p.Id)
                                        .AsQueryable();

        orderDetails = from o in orderDetails
                       join p in sellerProductsIds
                       on o.ProductId equals p
                       select o;

        var query = from o in orders
                    join orderDetail in orderDetails
                    on o.Id equals orderDetail.OrderId
                    select new ListOfShopOrdersSellerSideDTO()
                    {
                        CreateDate = o.CreateDate,
                        IsFinally = o.IsFinally,
                        PaymentWay = o.PaymentWay,
                        OrderId = o.Id,
                        CustomerUserInfo = _context.Users
                                               .AsNoTracking()
                                               .Where(p => !p.IsDelete &&
                                                      p.Id == o.UserId)
                                               .Select(p => new ListOfShopOrdersSellerSideDTO_CustomerUserInfo()
                                               {
                                                   CustomerUserId = p.Id,
                                                   Mobile = p.Mobile,
                                                   Username = p.Username
                                               }).FirstOrDefault()

                    };

        if (!string.IsNullOrEmpty(filter.CustomerName))
        {
            query = query.Where(p => p.CustomerUserInfo.Username.Contains(filter.CustomerName));
        }

        await filter.Paging(query);

        filter.Entities = filter.Entities.DistinctBy(p => p.OrderId).ToList();

        return filter;
    }

    public async Task<ShowOrderFactorDTO?> ShowOrderFactorDTO_ByOrderId(ulong orderId , 
                                                                        CancellationToken cancellationToken)
    {
        return await _context.Orders
                             .AsNoTracking()
                             .Where(p => !p.IsDelete &&
                                    p.Id == orderId)
                             .Select(p => new ShowOrderFactorDTO()
                             {
                                 OrdeId = orderId,
                                 IsFinally = p.IsFinally,
                                 PaymentWay = p.PaymentWay,
                                 Price = p.Price,
                                 CreateDate = p.CreateDate,
                                 Location = _context.Locations
                                                    .AsNoTracking()
                                                    .Where(l => l.UserId == p.UserId)
                                                    .FirstOrDefault(),
                                 CustomerInfo = _context.Users
                                                        .AsNoTracking()
                                                        .Where(u => u.Id == p.UserId &&
                                                               !u.IsDelete)
                                                        .Select(u => new ShowOrderFactor_CustmoerInfoDTO()
                                                        {
                                                            CustoemrMobile = u.Mobile,
                                                            CustomerUsername = u.Username , 
                                                            SellerUserId = u.Id
                                                        })
                                                        .FirstOrDefault(),
                                 OrderDetails = _context.OrderDetails
                                                        .AsNoTracking()
                                                        .Where(o => !o.IsDelete && 
                                                               o.OrderId == p.Id)
                                                        .Select(o => new ShowOrderFactor_OrderDetailDTO()
                                                        {
                                                            Count = o.Count,
                                                            OrderDetailId = o.Id , 
                                                            Product = _context.ShopProducts
                                                                              .AsNoTracking()
                                                                              .Where(pr=> !pr.IsDelete && 
                                                                                     pr.Id == o.ProductId)
                                                                              .Select(pr => new ShowOrderFactor_ProductDetailsDTO()
                                                                              {
                                                                                  Price = pr.Price,
                                                                                  ProductId = pr.Id,
                                                                                  ProductName = pr.ProductName,
                                                                                  ScaleTitle = _context.SalesScales
                                                                                                       .AsNoTracking() 
                                                                                                       .Where(scale => !scale.IsDelete && 
                                                                                                              scale.Id == pr.SaleScaleId)
                                                                                                       .Select(scale => scale.ScaleTitle)
                                                                                                       .FirstOrDefault(),

                                                                                  ShopBrand = _context.MainBrands
                                                                                                      .AsNoTracking()
                                                                                                      .Where(brand => !brand.IsDelete && 
                                                                                                             brand.Id == pr.ProductBrandId)
                                                                                                      .FirstOrDefault(),

                                                                                  ShopColor = _context.ShopColors
                                                                                                      .AsNoTracking()
                                                                                                      .Where(color => !color.IsDelete &&
                                                                                                             color.Id == pr.ProductColorId)
                                                                                                      .FirstOrDefault(),

                                                                                  SellerInfo = _context.Users
                                                                                                       .AsNoTracking()
                                                                                                       .Where(user => !user.IsDelete && 
                                                                                                              user.Id == pr.SellerUserId)
                                                                                                       .Select(user => new ShowOrderFactor_ProductSellerInfoDTO()
                                                                                                       {
                                                                                                           SellerMobile = user.Mobile,
                                                                                                           SellerUserId = user.Id,
                                                                                                           SellerUsername = user.Username
                                                                                                       })
                                                                                                       .FirstOrDefault()
                                                                              })
                                                                              .FirstOrDefault()
                                                        })
                                                        .ToList()
                             })
                             .FirstOrDefaultAsync();
    }

    #endregion
}
