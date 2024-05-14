
using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.DeleteLastestComponent;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditColorFullSetting;

public record DeleteLastestComponentCommandHandler : IRequestHandler<DeleteLastestComponentCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLastestComponentCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLastestComponentCommand request, CancellationToken cancellationToken)
    {
        var originLastest = await _siteSettingService.Get_LastestComponent_ById(request.LastestComponentId , cancellationToken);
        if (originLastest == null) return false;

        originLastest.IsDelete = true;

        //Update Site Setting 
        _siteSettingService.Update_LastestComponent(originLastest);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
