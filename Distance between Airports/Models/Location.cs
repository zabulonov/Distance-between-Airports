namespace Distance_between_Airports.Models;

public class Location
{
    public double Lon { get; set; }
    public double Lat { get; set; }

    public void ToRadians()
    {
        Lat = (Lat * Math.PI) / 180;
        Lon = (Lon * Math.PI) / 180;
    }
}