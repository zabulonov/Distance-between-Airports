using AirportDistance.Business;
using AirportDistance.Business.States;
using AirportDistances.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportDistances.Controllers;

[ApiController]
[Route("distance")]
public class DistanceController
{
    private readonly AirportDistanceService _airportDistanceService;

    public DistanceController(AirportDistanceService airportDistanceService)
    {
        _airportDistanceService = airportDistanceService;
    }

    [HttpGet]
    public async Task<DistanceState> GetDistance([FromBody] AirportCodes airportCodes)
    {
        return await _airportDistanceService.GetDistance(new[] { airportCodes.IataFrom, airportCodes.IataTo });
    } 
}