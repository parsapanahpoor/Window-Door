
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateFreeConsultant;

public record CreateFreeConsultantCommandHandler : IRequestHandler<CreateFreeConsultantCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFreeConsultantCommandHandler(IUnitOfWork unitOfWork, 
                                              ISiteSettingService siteSettingService)
    {
        _unitOfWork = unitOfWork;
        _siteSettingService = siteSettingService;
    }

    public async Task<bool> Handle(CreateFreeConsultantCommand request, CancellationToken cancellationToken)
    {
        var freeConsultant = new FreeConsultant()
        {
            CreateDate = DateTime.Now,
            Description = request.Description,
            IsDelete = false,
            Title = request.Title
        };

        //Add To The Data Base
        await _siteSettingService.Add_FreeConsultant(freeConsultant, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
