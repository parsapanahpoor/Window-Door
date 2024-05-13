
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.CreateColorFullSetting;

public record CreateColorFullSettingCommandHandler : IRequestHandler<CreateColorFullSettingCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateColorFullSettingCommandHandler(IUnitOfWork unitOfWork, 
                                                ISiteSettingService siteSettingService)
    {
        _unitOfWork = unitOfWork;
        _siteSettingService = siteSettingService;
    }

    public async Task<bool> Handle(CreateColorFullSettingCommand request, CancellationToken cancellationToken)
    {
        var colorFullSetting = new ColorFullSiteSetting()
        {
            CircleClass = request.CircleClass,
            CreateDate = DateTime.Now,
            Description = request.Description,
            IsDelete = false,
            TagClass = request.TagClass,
            Title = request.Title
        };

        //Add To The Data Base
        await _siteSettingService.Add_ColorFullSiteSetting(colorFullSetting , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
