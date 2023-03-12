using AirportDistances.Infrastructure.Models;

namespace AirportDistances.Infrastructure;

public class MockGetCoordinates : IGetCoordinates
{
    private static readonly Location?[] Locations = new Location[2];
    
    public Task SetCoordinates(string[] codes)
    {
        var location = new Location
        {
            Lat = 1,
            Lon1 = 1
        };

        Locations[0] = location;
        Locations[1] = location;
        return Task.CompletedTask;
    }

    public Location?[] GetLocations()
    {
        return Locations;
    }
}