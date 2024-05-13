
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;

public record EditColorFullSiteSettingCommandHandler : IRequestHandler<EditColorFullSiteSettingCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public EditColorFullSiteSettingCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(EditColorFullSiteSettingCommand request, CancellationToken cancellationToken)
    {
        var originColor = await _siteSettingService.Get_ColorFullSiteSetting_ById(request.ColorFullSiteSetting.Id , cancellationToken);
        if (originColor == null) return false;

        originColor.Description = request.ColorFullSiteSetting.Description;
        originColor.CircleClass = request.ColorFullSiteSetting.CircleClass;
        originColor.TagClass = request.ColorFullSiteSetting.TagClass;
        originColor.Title = request.ColorFullSiteSetting.Title;

        //Update Site Setting 
        _siteSettingService.Update_ColorFullSiteSetting(originColor);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
