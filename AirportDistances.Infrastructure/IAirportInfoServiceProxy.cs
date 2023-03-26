using AirportDistances.Infrastructure.Contracts;

namespace AirportDistances.Infrastructure;

public interface IAirportInfoServiceProxy
{
    Task<Result<AirportInfo>> GetAirportInfo(string code);
}