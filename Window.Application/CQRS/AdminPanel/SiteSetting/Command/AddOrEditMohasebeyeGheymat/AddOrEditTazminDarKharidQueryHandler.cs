
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditMohasebeyeGheymat;

public record AddOrEditMohasebeyeGheymatQueryHandler : IRequestHandler<AddOrEditMohasebeyeGheymatQuery, bool>
{
    #region Ctor 

    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrEditMohasebeyeGheymatQueryHandler(ISiteSettingService siteSettingService, 
                                             IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(AddOrEditMohasebeyeGheymatQuery request, CancellationToken cancellationToken)
    {
        //Get MohasebeyeGheymat 1 If Exist
        var mohasebeyeGheymat = await _siteSettingService.Show_MohasebeyeOnlineGheymat();

        //Add MohasebeyeGheymat For First Time 
        if (mohasebeyeGheymat == null)
        {
            MohasebeyeOnlineGheymat mohasebeyeGheymat1 = new()
            {
                Description = request.Description,
            };

            //Add To The Data Base
            await _siteSettingService.Add_MohasebeyeGheymat(mohasebeyeGheymat1);
            await _unitOfWork.SaveChangesAsync();
        }

        //Edit Existing MohasebeyeGheymat
        else
        {
            mohasebeyeGheymat.Description = request.Description;

            //Edit Tazmin To the Data Base
            _siteSettingService.Edit_MohasebeyeGheymat(mohasebeyeGheymat);
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
