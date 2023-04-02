using AirportDistances.Infrastructure.Contracts;

namespace AirportDistance.Business.States;

public class InfoState
{
    public AirportInfo AirportInfo { get; set; }

    public string ErrorMessage { get; set; }

    public InfoState()
    {
        
    }

    public InfoState(AirportInfo airportInfo, string code)
    {
        AirportInfo = airportInfo;
        AirportInfo.Code = code;
    }
}