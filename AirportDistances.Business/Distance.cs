using AirportDistances.Infrastructure;

namespace AirportDistance.Business;

public class Distance
{
    private readonly IGetCoordinates _getCoordinates;

    public Distance(IGetCoordinates getCoordinates)
    {
        _getCoordinates = getCoordinates;
    }

    public async Task<double> GetDistance(string[] airportCodes)
    {
        await _getCoordinates.SetCoordinates(airportCodes);
        var points = _getCoordinates.GetLocations();
        points[0]?.ToRadians();
        points[1]?.ToRadians();

        const int earthRadius = 6371;

        var sinLat = Math.Pow(Math.Sin((points[1]!.Lat - points[0]!.Lat) / 2), 2);

        var sinLon = Math.Pow(Math.Sin((points[1]!.Lon - points[0]!.Lon) / 2), 2);
        
        return earthRadius * 2 * Math.Asin(Math.Sqrt(sinLat + Math.Cos(points[0]!.Lat) * Math.Cos(points[1]!.Lat)*sinLon));
    }
    
}