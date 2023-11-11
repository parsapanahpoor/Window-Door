using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Application.StticTools
{
    public static class FilePaths
    {

        #region Site

        public static string SiteFarsiName = "Windows";
        public static string SiteAddress = "https://panjerehyab.com";
        public static string merchant = "8f5510c9-10bd-4eb6-a3e2-2795a2f36edf";

        public static readonly string SiteLogo = "/content/images/site/logo/main/";
        public static readonly string SiteLogoServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/logo/main/");

        public static readonly string DefaultSiteLogo = "/content/images/site/logo/default/logo.png";
        public static readonly string SiteLogoThumb = "/content/images/site/logo/thumb/";
        public static readonly string SiteLogoThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/logo/thumb/");

        public static readonly string EmailBanner = "/content/images/site/emailBanner/main/";
        public static readonly string EmailBannerServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/emailBanner/main/");

        public static readonly string EmailBannerThumb = "/content/images/site/emailBanner/thumb/";
        public static readonly string EmailBannerThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/site/emailBanner/thumb/");

        #endregion

        #region UserAvatar

        public static readonly string UserAvatarPath = "/content/images/user/main/";
        public static readonly string UserAvatarPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/user/main/");

        public static readonly string UserAvatarPathThumb = "/content/images/user/thumb/";
        public static readonly string UserAvatarPathThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/user/thumb/");

        public static readonly string DefaultUserAvatar = "/content/images/user/DefaultAvatar.png";

        #endregion

        #region Seller

        public static readonly string DefaultSellerInfoImage = "/content/images/SellerInfo/ImageNotFound.png";
        public static string SellerInfoOriginimage = "/content/images/SellerInfo/main/";
        public static readonly string SellerInfoPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerInfo/main/");
        public static readonly string SellerInfoPathThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerInfo/thumb/");

        public static string SellerInfoPath = "/content/images/SellerInfo/main/";
        public static string SellerInfoThumbimage = "/content/images/SellerInfo/thumb/";

        #endregion

        #region Article

        public static readonly string DefaultArticleImage = "/content/images/Article/default.jpg";
        public static string ArticleOriginimage = "/content/images/Article/origin/";
        public static string ArticleThumbimage = "/content/images/Article/thumb/";
        public static string DefaultArticleThumbimage = "/content/images/Article/thumb/default.jpg";
        public static readonly string ArticleImageOriginPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Article/origin/");
        public static readonly string ArticleImageThumbPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Article/thumb/");

        #endregion

        #region Brand

        public static readonly string DefaultBrandImage = "/content/images/Brand/default.jpg";
        public static string BrandOriginimage = "/content/images/Brand/origin/";
        public static string BrandThumbimage = "/content/images/Brand/thumb/";
        public static string DefaultBrandThumbimage = "/content/images/Brand/thumb/default.jpg";
        public static readonly string BrandImageOriginPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Brand/origin/");
        public static readonly string BrandImageThumbPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Brand/thumb/");

        #endregion

        #region Sample

        public static readonly string DefaultSampleImage = "/content/images/Sample/default.jpg";
        public static string SampleOriginimage = "/content/images/Sample/origin/";
        public static string SampleThumbimage = "/content/images/Sample/thumb/";
        public static string DefaultSampleThumbimage = "/content/images/Sample/thumb/default.jpg";
        public static readonly string SampleImageOriginPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Sample/origin/");
        public static readonly string SampleImageThumbPathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/Sample/thumb/");

        #endregion

        #region Ticket File

        public static readonly string TicketFilePath = "/content/images/OnlineVisitRequestFile/main/";
        public static readonly string TicketFilePathServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/OnlineVisitRequestFile/main/");

        #endregion

        #region Seller Personal Video  Attachment Files

        public static readonly string SellerPersonalVideoAttachmentFilesServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerPersonalVideoAttachmentFiles/Files/");
        public static readonly string SellerPersonalVideoAttachmentFilesChunkServerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerPersonalVideoAttachmentFiles/Chunks/");

        public static readonly string SellerPersonalVideoAttachmentFilesPath = "/content/images/SellerPersonalVideoAttachmentFiles/Files/";

        public static readonly string SellerPersonalVideoAttachmentFilesImagePath = "/content/images/SellerPersonalVideoAttachmentFiles/main/";
        public static readonly string SellerPersonalVideoAttachmentFilesImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerPersonalVideoAttachmentFiles/main/");

        public static readonly string SellerPersonalVideoAttachmentFilesImagePathThumb = "/content/images/SellerPersonalVideoAttachmentFiles/thumb/";
        public static readonly string SellerPersonalVideoAttachmentFilesImagePathThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/images/SellerPersonalVideoAttachmentFiles/thumb/");

        #endregion

    }
}
