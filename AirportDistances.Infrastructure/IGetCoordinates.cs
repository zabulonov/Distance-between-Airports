using AirportDistances.Infrastructure.Models;

namespace AirportDistances.Infrastructure;

public interface IGetCoordinates
{ 
    Task SetCoordinates(string[] codes);

    Location?[] GetLocations();
}