
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.Location;
using Window.Infra.Data.Repository.Location;

namespace Window.Application.CQRS.SiteSide.Location.Command;

public record DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
{
    #region Ctor

    private readonly ILocationCommandRepository _locationCommandRepository;
    private readonly ILocationQueryRepository _locationQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLocationCommandHandler(ILocationCommandRepository locationCommandRepository ,
                                        ILocationQueryRepository locationQueryRepository ,
                                        IUnitOfWork unitOfWork)
    {
        _locationCommandRepository = locationCommandRepository;
        _locationQueryRepository = locationQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        //Get Location By Id 
        var location = await _locationQueryRepository.GetByIdAsync(cancellationToken , request.LocationId);
        if (location == null) return false;

        location.IsDelete = true;

        //Update Location
        _locationCommandRepository.Update(location);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
