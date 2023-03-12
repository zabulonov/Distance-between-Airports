using AirportDistances.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace AirportDistances;

[ApiController]
[Route("distance")]
public class DistanceController
{
    private readonly Distance _distance;

    public DistanceController(Distance distance)
    {
        _distance = distance;
    }

    [HttpGet]
    public async Task<double> GetDistance([FromBody] AirportCodes airportCodes)
    {
        return await _distance.GetDistance(new[] { airportCodes.IataFrom, airportCodes.IataTo });
    } 
}

public class AirportCodes
{
    public string IataFrom { get; set; }
    public string IataTo { get; set; }
}