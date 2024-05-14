
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.LastestComponent;

public record CreateLastestComponentCommandHandler : IRequestHandler<CreateLastestComponentCommand, bool>
{
    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLastestComponentCommandHandler(IUnitOfWork unitOfWork, 
                                                ISiteSettingService siteSettingService)
    {
        _unitOfWork = unitOfWork;
        _siteSettingService = siteSettingService;
    }

    public async Task<bool> Handle(CreateLastestComponentCommand request, CancellationToken cancellationToken)
    {
        var lastestComponent = new Domain.Entities.SiteSetting.LastestComponent()
        {
            CreateDate = DateTime.Now,
            Description = request.Description,
            IsDelete = false,
            TagClass = request.TagClass,
            Title = request.Title
        };

        //Add To The Data Base
        await _siteSettingService.Add_LastestComponent(lastestComponent , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
