using Microsoft.AspNetCore.Http;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Application.Services.Interfaces;

public interface ISellerPersonalVideoService
{
    //Fill Add Or Edit Seller Personal Video DTO
    Task<AddOrEditSellerPersonalVideoDTO?> FillAddOrEditSellerPersonalVideoDTO(ulong userId);

    //Edit Seller Personal Video
    Task<bool> EditSellerPersonalVideo(ulong userId, AddOrEditSellerPersonalVideoDTO model, IFormFile? Image);
}
