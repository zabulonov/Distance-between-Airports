namespace AirportDistances.Infrastructure.Models;

internal class AirportInfo
{
    public string Country { get; set; }
    public string CityIata { get; set; }
    public string Iata { get; set; }
    public string City { get; set; }
    public string TimezoneRegionName { get; set; }
    public string CountryIata { get; set; }
    public int Rating { get; set; }
    public string Name { get; set; }
    public LocationInfo Location { get; set; }
    public string Type { get; set; }
    public int Hubs { get; set; }
    
    public class LocationInfo
    {
        public double Lon { get; set; }
        public double Lat { get; set; }

    }
}