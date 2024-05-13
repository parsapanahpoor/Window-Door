
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.EditFreeConsultant;

public record EditFreeConsultantCommandHandler : IRequestHandler<EditFreeConsultantCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public EditFreeConsultantCommandHandler(ISiteSettingService siteSettingService, IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(EditFreeConsultantCommand request, CancellationToken cancellationToken)
    {
        var freeConsultant = await _siteSettingService.Get_FreeConsultant_ById(request.FreeConsultant.Id , cancellationToken);
        if (freeConsultant == null) return false;

        freeConsultant.Description = request.FreeConsultant.Description;
        freeConsultant.Title = request.FreeConsultant.Title;

        //Update Site Setting 
        _siteSettingService.Update_FreeConsultant(freeConsultant);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
