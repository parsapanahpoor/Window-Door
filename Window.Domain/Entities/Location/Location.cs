using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.Location;

public sealed class Location : BaseEntity
{
    #region properties

    public ulong UserId { get; set; }

    [MaxLength(50)]
    public string PostalCode { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string State { get; set; }

    [MaxLength(100)]
    public string City { get; set; }

    public string Address { get; set; }

    #endregion
}
