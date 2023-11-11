using Microsoft.AspNetCore.Http;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.MarketInfo;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Seller.PersonalInfo;

namespace Window.Application.Services.Services;

public class SellerPersonalVideoService : ISellerPersonalVideoService
{
	#region Ctor

	private readonly ISellerPersonalVideoRepository _sellerPersonalVideoRepository;

    public SellerPersonalVideoService(ISellerPersonalVideoRepository sellerPersonalVideoRepository)
    {
        _sellerPersonalVideoRepository = sellerPersonalVideoRepository;
    }

    #endregion

    #region Seller Side 

    //Fill Add Or Edit Seller Personal Video DTO
    public async Task<AddOrEditSellerPersonalVideoDTO?> FillAddOrEditSellerPersonalVideoDTO(ulong userId)
    {
        return await _sellerPersonalVideoRepository.FillAddOrEditSellerPersonalVideoDTO(userId);
    }

    //Edit Seller Personal Video
    public async Task<bool> EditSellerPersonalVideo(ulong userId , AddOrEditSellerPersonalVideoDTO model, IFormFile? Image)
    {
        if (model.Id.HasValue)
        {
            var video = await _sellerPersonalVideoRepository.GetSellerPersonalVideoById(model.Id.Value);
            if (video != null && video.UserId == userId) 
            {
                video.Title = model.Title;

                #region  Image

                if (Image != null)
                {
                    var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                    Image.AddImageToServer(imageName, FilePaths.SellerPersonalVideoAttachmentFilesImageServer, 400, 300, FilePaths.SellerPersonalVideoAttachmentFilesImagePathThumbServer);

                    if (!string.IsNullOrEmpty(video.BanerImage))
                    {
                        video.BanerImage.DeleteImage(FilePaths.SellerPersonalVideoAttachmentFilesImageServer, FilePaths.SellerPersonalVideoAttachmentFilesImagePathThumbServer);
                    }

                    video.BanerImage = imageName;
                }

                #endregion

                #region Attachment File 

                if (!string.IsNullOrEmpty(video.Videos) &&
                      !string.IsNullOrEmpty(model.AttachmentFileName) &&
                            model.AttachmentFileName != video.Videos)
                {
                    video.Videos.DeleteFile(FilePaths.SellerPersonalVideoAttachmentFilesServerPath);
                }

                if (string.IsNullOrEmpty(video.Videos) || video.Videos != model.AttachmentFileName)
                {
                    video.Videos = model.AttachmentFileName;
                }

                #endregion

                await _sellerPersonalVideoRepository.UpdateSellerPersonalVideo(video);

                return true;
            }
        }
        else 
        {
            SelersPersonalVideos video = new SelersPersonalVideos()
            {
                Title = model.Title,
                UserId = userId,
                Videos = model.AttachmentFileName
            };

            #region Image

            if (Image != null && Image.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                Image.AddImageToServer(imageName, FilePaths.SellerPersonalVideoAttachmentFilesImageServer, 400, 300, FilePaths.SellerPersonalVideoAttachmentFilesImagePathThumbServer);
                video.BanerImage = imageName;
            }

            #endregion

            await _sellerPersonalVideoRepository.AddSellerPersonalVideo(video);

            return true;
        }

        return false;
    }

    #endregion
}
