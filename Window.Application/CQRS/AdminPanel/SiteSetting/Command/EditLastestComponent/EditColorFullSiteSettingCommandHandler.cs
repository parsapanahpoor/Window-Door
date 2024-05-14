
using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditLastestComponent;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditLastestComponent;

public record EditLastestComponentCommandHandler : IRequestHandler<EditLastestComponentCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public EditLastestComponentCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(EditLastestComponentCommand request, CancellationToken cancellationToken)
    {
        var originLastest = await _siteSettingService.Get_LastestComponent_ById(request.EditLastestComponent.Id, cancellationToken);
        if (originLastest == null) return false;

        originLastest.Description = request.EditLastestComponent.Description;
        originLastest.TagClass = request.EditLastestComponent.TagClass;
        originLastest.Title = request.EditLastestComponent.Title;

        //Update Site Setting 
        _siteSettingService.Update_LastestComponent(originLastest);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
