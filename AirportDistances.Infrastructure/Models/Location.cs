namespace AirportDistances.Infrastructure.Models;

public class Location
{
    public double Lon1 { get; set; }
    public double Lat { get; set; }

    public void ToRadians()
    {
        Lat = (Lat * Math.PI) / 180;
        Lon1 = (Lon1 * Math.PI) / 180;
    }
}