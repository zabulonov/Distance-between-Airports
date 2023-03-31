using AirportDistance.Business.States;
using AirportDistances.Infrastructure;
using AirportDistances.Infrastructure.Contracts;

namespace AirportDistance.Business;

public class AirportDistanceService
{
    private readonly IAirportInfoServiceProxy _airportInfoServiceProxy;
    private const int EarthRadius = 6371;

    public AirportDistanceService(IAirportInfoServiceProxy airportInfoServiceProxy)
    {
        _airportInfoServiceProxy = airportInfoServiceProxy;
    }

    public async Task<DistanceState> GetDistance(string[] airportCodes)
    {
        var firstAirportInfo = await _airportInfoServiceProxy.GetAirportInfo(airportCodes[0]);
        var secondAirportInfo = await _airportInfoServiceProxy.GetAirportInfo(airportCodes[1]);
        if (firstAirportInfo.IsSuccess && secondAirportInfo.IsSuccess)
        {
            var distance = CalculateDistance(firstAirportInfo.Value, secondAirportInfo.Value);
            return new DistanceState
            {
                Distance = distance
            };    
        }

        return new DistanceState
        {
            Distance = 0,
            ErrorMessage = string.Join(",", firstAirportInfo.FaultMessage, secondAirportInfo.FaultMessage)
        };
    }

    private static double CalculateDistance(AirportInfo firstAirportInfo, AirportInfo secondAirportInfo)
    {
        var firstAirportLatInRadians = ToRadians(firstAirportInfo.Location.Lat);
        var firstAirportLonInRadians = ToRadians(firstAirportInfo.Location.Lon);

        var secondAirportLatInRadians = ToRadians(secondAirportInfo.Location.Lat);
        var secondAirportLonInRadians = ToRadians(secondAirportInfo.Location.Lon);
        
        var sinLat = Math.Pow(Math.Sin((secondAirportLatInRadians - firstAirportLatInRadians) / 2), 2);
        var sinLon = Math.Pow(Math.Sin((secondAirportLonInRadians - firstAirportLonInRadians) / 2), 2);

        return EarthRadius * 2 * Math.Asin(Math.Sqrt(sinLat + Math.Cos(secondAirportLatInRadians) *
            Math.Cos(secondAirportLatInRadians) * sinLon));
    }


    private static double ToRadians(double value)
    {
        return value * Math.PI / 180.0;
    }
}