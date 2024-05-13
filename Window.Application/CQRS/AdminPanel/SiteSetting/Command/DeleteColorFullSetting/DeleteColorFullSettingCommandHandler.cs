
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;

public record DeleteColorFullSettingCommandHandler : IRequestHandler<DeleteColorFullSettingCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteColorFullSettingCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteColorFullSettingCommand request, CancellationToken cancellationToken)
    {
        var originColor = await _siteSettingService.Get_ColorFullSiteSetting_ById(request.ColorFullId , cancellationToken);
        if (originColor == null) return false;

        originColor.IsDelete = true;

        //Update Site Setting 
        _siteSettingService.Update_ColorFullSiteSetting(originColor);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
