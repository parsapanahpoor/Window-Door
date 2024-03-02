
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Location;

namespace Window.Application.CQRS.SiteSide.Location.Command;

public record AddOrEditLocationCommandHandler : IRequestHandler<AddOrEditLocationCommand, bool>
{
    #region Ctor 

    private readonly ILocationCommandRepository _locationCommandRepository;
    private readonly ILocationQueryRepository _locationQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrEditLocationCommandHandler(ILocationCommandRepository locationCommandRepository ,
                                           ILocationQueryRepository locationQueryRepository , 
                                           IUnitOfWork unitOfWork)  
    {
        _locationCommandRepository = locationCommandRepository;
        _locationQueryRepository = locationQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(AddOrEditLocationCommand request, CancellationToken cancellationToken)
    {
        //Initial Entity
        var location = new Domain.Entities.Location.Location()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            City = request.City,
            PostalCode = request.PostalCode,
            State = request.State,
            Mobile = request.Mobile,
            UserId = request.UserId,
        };

        //Add Location
        await _locationCommandRepository.AddAsync(location , cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
