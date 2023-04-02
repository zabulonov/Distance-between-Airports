using AirportDistance.Business.States;
using AirportDistances.Infrastructure;
using AirportDistances.Infrastructure.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace AirportDistance.Business;

public class AirportInfoService
{
    private readonly IAirportInfoServiceProxy _airportInfoServiceProxy;
    private readonly IMemoryCache _cache;

    public AirportInfoService(IAirportInfoServiceProxy airportInfoServiceProxy, IMemoryCache cache)
    {
        _airportInfoServiceProxy = airportInfoServiceProxy;
        _cache = cache;
    }

    public async Task<InfoState> GetInfo(string code)
    {
        InfoState info;
        if (!_cache.TryGetValue(code, out info))
        {
            var data = await _airportInfoServiceProxy.GetAirportInfo(code);
            if (data.IsSuccess)
            {
                info = new InfoState(data.Value, code);
            }
            else
            {
                return new InfoState
                {
                    ErrorMessage = data.FaultMessage
                }; 
            }
    
            _cache.Set(info.AirportInfo.Code, info,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        return info!;
    }
    
}