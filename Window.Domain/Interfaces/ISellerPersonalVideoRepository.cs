using Window.Domain.Entities.MarketInfo;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Domain.Interfaces;

public interface ISellerPersonalVideoRepository
{
    //Fill Add Or Edit Seller Personal Video DTO
    Task<AddOrEditSellerPersonalVideoDTO?> FillAddOrEditSellerPersonalVideoDTO(ulong UserId);

    //Get Seller Personal Video By Id
    Task<SelersPersonalVideos?> GetSellerPersonalVideoById(ulong id);

    //Update Seller Personal Video
    Task UpdateSellerPersonalVideo(SelersPersonalVideos videos);

    //Add Seller Personal Video
    Task AddSellerPersonalVideo(SelersPersonalVideos video);
}
