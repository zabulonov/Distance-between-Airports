using System.Text.Json;
using AirportDistance.Business.States;
using AirportDistances.Infrastructure;
using AirportDistances.Infrastructure.Contracts;
using GeoCoordinatePortable;
using Microsoft.Extensions.Caching.Distributed;

namespace AirportDistance.Business;

public class AirportDistanceService
{
    private readonly IAirportInfoServiceProxy _airportInfoServiceProxy;
    private readonly IDistributedCache _redisCacheAirportsInfo;

    public AirportDistanceService(IAirportInfoServiceProxy airportInfoServiceProxy, IDistributedCache redisCacheAirportsInfo)
    {
        _airportInfoServiceProxy = airportInfoServiceProxy;
        _redisCacheAirportsInfo = redisCacheAirportsInfo;
    }

    public async Task<DistanceState> GetDistance(string[] airportCodes)
    {
        Result<AirportInfo> firstAirportInfo = null;
        Result<AirportInfo> secondAirportInfo = null;
        
        var firstAirportInfoString = await _redisCacheAirportsInfo.GetStringAsync(airportCodes[0]);
        var secondAirportInfoString = await _redisCacheAirportsInfo.GetStringAsync(airportCodes[1]);

        if (firstAirportInfoString != null)
            firstAirportInfo = JsonSerializer.Deserialize<Result<AirportInfo>>(firstAirportInfoString);

        if (secondAirportInfoString != null)
            secondAirportInfo = JsonSerializer.Deserialize<Result<AirportInfo>>(secondAirportInfoString);

        firstAirportInfo ??= await _airportInfoServiceProxy.GetAirportInfo(airportCodes[0]);
        secondAirportInfo ??= await _airportInfoServiceProxy.GetAirportInfo(airportCodes[1]);

        if (firstAirportInfo.IsSuccess)
        {
            // Я думаю, в теории еще нужно проверять наличие этого кеша. Или нет?
            firstAirportInfoString = JsonSerializer.Serialize(firstAirportInfo);
            await _redisCacheAirportsInfo.SetStringAsync(firstAirportInfo.Value.Code, firstAirportInfoString,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            Console.WriteLine($"Redis save code {firstAirportInfo.Value.Code}");
        }
        
        if (secondAirportInfo.IsSuccess)
        {
            secondAirportInfoString = JsonSerializer.Serialize(secondAirportInfo);
            await _redisCacheAirportsInfo.SetStringAsync(secondAirportInfo.Value.Code, secondAirportInfoString,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            Console.WriteLine($"Redis save code {secondAirportInfo.Value.Code}");
        }
        
        
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