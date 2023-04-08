using AirportDistance.Business.States;
using AirportDistances.Infrastructure;
using AirportDistances.Infrastructure.Contracts;
using GeoCoordinatePortable;

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
        var pin1 = new GeoCoordinate(firstAirportInfo.Location.Lat, firstAirportInfo.Location.Lon);
        var pin2 = new GeoCoordinate(secondAirportInfo.Location.Lat, secondAirportInfo.Location.Lon);

        return Math.Round(pin1.GetDistanceTo(pin2) / 1000);
    }
}