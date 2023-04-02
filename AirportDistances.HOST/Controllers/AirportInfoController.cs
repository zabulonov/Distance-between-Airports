using AirportDistance.Business;
using AirportDistance.Business.States;
using AirportDistances.Infrastructure;
using AirportDistances.Infrastructure.Contracts;
using AirportDistances.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportDistances.Controllers;

[ApiController]
[Route("info")]
public class AirportInfoController
{
    private readonly AirportInfoService _airportInfoService;

    public AirportInfoController(AirportInfoService airportInfoService)
    {
        _airportInfoService = airportInfoService;
    }

    [HttpGet]
    public async Task<InfoState> GetInfo(AirportCode code)
    {
        return await _airportInfoService.GetInfo(code.code);
    }
    
}