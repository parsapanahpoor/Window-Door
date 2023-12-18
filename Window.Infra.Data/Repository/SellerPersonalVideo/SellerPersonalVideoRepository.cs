using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Window.Data.Context;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Infra.Data.Repository;

public class SellerPersonalVideoRepository : ISellerPersonalVideoRepository
{
	#region Ctor

	private readonly WindowDbContext _context;

    public SellerPersonalVideoRepository(WindowDbContext context)
    {
            _context = context;
    }

    #endregion

    #region Seller Side 

    //Fill Add Or Edit Seller Personal Video DTO
    public async Task<AddOrEditSellerPersonalVideoDTO?> FillAddOrEditSellerPersonalVideoDTO(ulong UserId)
    {
        return await _context.SelersPersonalVideos
                             .Where(p => !p.IsDelete && p.UserId == UserId)
                             .Select(p => new AddOrEditSellerPersonalVideoDTO()
                             {
                                 AttachmentFileName = p.Videos,
                                 imagename = p.BanerImage,
                                 Id = p.Id,
                                 Title = p.Title
                             })
                             .FirstOrDefaultAsync();
    }

    //Get Seller Personal Video By Id
    public async Task<SelersPersonalVideos?> GetSellerPersonalVideoById(ulong id)
    {
        return await _context.SelersPersonalVideos
                             .FirstOrDefaultAsync(p => !p.IsDelete && p.Id == id);
    }

    //Update Seller Personal Video
    public async Task UpdateSellerPersonalVideo(SelersPersonalVideos videos)
    {
        _context.SelersPersonalVideos.Update(videos);
        await _context.SaveChangesAsync();
    }

    //Add Seller Personal Video
    public async Task AddSellerPersonalVideo(SelersPersonalVideos video)
    {
        await _context.SelersPersonalVideos.AddAsync(video);
        await _context.SaveChangesAsync();
    }

    #endregion
}
