namespace Window.Domain.ViewModels.Site.Shop.Location;

public class ListOfUserLocation 
{
    #region properties

    public ulong LocationId { get; set; }

    public string Username { get; set; }

    public string LocationAddress{ get; set; }

    public string Mobile { get; set; }

    public string? PostalCode { get; set; }

    public string State{ get; set; }

    public string City { get; set; }

    #endregion
}
