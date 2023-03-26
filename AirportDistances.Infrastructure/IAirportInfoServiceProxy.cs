using AirportDistances.Infrastructure.Contracts;

namespace AirportDistances.Infrastructure;

public interface IAirportInfoServiceProxy
{
    Task<AirportInfo> GetAirportInfo(string code);
}