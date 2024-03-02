using System.ComponentModel.DataAnnotations;

namespace Window.Application.CQRS.SiteSide.Location.Command;

public class AddOrEditLocationCommand : IRequest<bool>
{
    #region properties

    public ulong UserId { get; set; }

    [MaxLength(50)]
    public string? PostalCode { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string State { get; set; }

    [MaxLength(100)]
    public string City { get; set; }

    public string Address { get; set; }

    [MaxLength(60)]
    public string Mobile { get; set; }

    #endregion
}
