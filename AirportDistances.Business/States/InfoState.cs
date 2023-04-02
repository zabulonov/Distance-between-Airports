using AirportDistances.Infrastructure.Contracts;

namespace AirportDistance.Business.States;

public class InfoState
{
    public AirportInfo airportInfo { get; set; }

    public string ErrorMessage { get; set; }
}