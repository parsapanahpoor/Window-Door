﻿namespace Window.Application.CQRS.SiteSide.ShopOrder.Query;

public class ShopOrderQuery : IRequest<AddToShopOrderRes>
{
    public ulong productId { get; set; }

    public ulong userId { get; set; }
}

public enum AddToShopOrderRes
{
    Success,
    Faild,
    WaitingForPaymentOrderExist
}