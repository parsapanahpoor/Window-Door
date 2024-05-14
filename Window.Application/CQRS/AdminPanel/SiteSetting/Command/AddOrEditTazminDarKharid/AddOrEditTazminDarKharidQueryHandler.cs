
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.SiteSetting;

namespace Window.Application.CQRS.AdminPanel.SiteSetting.Command.AddOrEditTazminDarKharid;

public record AddOrEditTazminDarKharidQueryHandler : IRequestHandler<AddOrEditTazminDarKharidQuery, bool>
{
    #region Ctor 

    private readonly ISiteSettingService _siteSettingService;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrEditTazminDarKharidQueryHandler(ISiteSettingService siteSettingService, 
                                             IUnitOfWork unitOfWork)
    {
        _siteSettingService = siteSettingService;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(AddOrEditTazminDarKharidQuery request, CancellationToken cancellationToken)
    {
        //Get tazmin DarKharid 1 If Exist
        var tazminDarKharid = await _siteSettingService.Show_TazminDarKharid();

        //Add tazmin DarKharid For First Time 
        if (tazminDarKharid == null)
        {
            TazminDarKharid tazminDarKharid1 = new()
            {
                Description = request.Description,
            };

            //Add To The Data Base
            await _siteSettingService.Add_TazminDarKharid(tazminDarKharid1);
            await _unitOfWork.SaveChangesAsync();
        }

        //Edit Existing Tazmin DarKharid
        else
        {
            tazminDarKharid.Description = request.Description;

            //Edit Tazmin To the Data Base
            _siteSettingService.Edit_TazminDarKhrid(tazminDarKharid);
            await _unitOfWork.SaveChangesAsync();
        }

        return true;
    }
}
