
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditSiteSetting1;

public record AddOrEditSiteSetting1QueryHandler : IRequestHandler<AddOrEditSiteSetting1Query, bool>
{
    #region Ctor 

    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrEditSiteSetting1QueryHandler(ISiteSettingService siteSettingService, 
                                             IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(AddOrEditSiteSetting1Query request, CancellationToken cancellationToken)
    {
        //Get Site Setting 1 If Exist
        var siteSetting = await _siteSettingService.Show_SiteSetting1();

        //Add Site Setting For First Time 
        if (siteSetting == null)
        {
            SiteSetting1 siteSetting1 = new()
            {
                Description = request.Description,
                Title = request.Title,
            };

            //Add To The Data Base
            await _siteSettingService.Add_SiteSetting1(siteSetting1);
            await _unitOfWork.SaveChangesAsync();
        }

        //Edit Existing Site Setting
        else
        {
            siteSetting.Title = request.Title;
            siteSetting.Description = request.Description;

            //Edit Site Setting To the Data Base
            _siteSettingService.Edit_SiteSetting(siteSetting);
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
