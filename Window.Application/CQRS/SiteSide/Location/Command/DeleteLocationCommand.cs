namespace Window.Application.CQRS.SiteSide.Location.Command;

public record DeleteLocationCommand(ulong LocationId) : IRequest<bool>;
