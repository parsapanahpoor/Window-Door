using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.DeleteFreeConsultant;

public record DeleteFreeConsultantCommandHandler : IRequestHandler<DeleteFreeConsultantCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFreeConsultantCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteFreeConsultantCommand request, CancellationToken cancellationToken)
    {
        var freeConsultant = await _siteSettingService.Get_FreeConsultant_ById(request.FreeConsultantId , cancellationToken);
        if (freeConsultant == null) return false;

        freeConsultant.IsDelete = true;

        //Update Site Setting 
        _siteSettingService.Update_FreeConsultant(freeConsultant);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
