using System.Net;
using System.Net.Http.Json;
using AirportDistances.Infrastructure.Contracts;

namespace AirportDistances.Infrastructure;

public class AirportInfoServiceProxy : IAirportInfoServiceProxy
{
    private readonly HttpClient _httpClient;

    private const string RequestUrl = "https://places-dev.cteleport.com/airports";

    public AirportInfoServiceProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<AirportInfo> GetAirportInfo(string code)
    {
        using var response = await _httpClient.GetAsync($"{RequestUrl}/{code}");
        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
        {
            Console.WriteLine(response.StatusCode);
            return null;
        }

        var data = await response.Content.ReadFromJsonAsync<Models.AirportInfo>();
        return new AirportInfo
        {
            City = data.City,
            Country = data.City,
            Location = new AirportInfo.LocationInfo
            {
                Lat = data.Location.Lat,
                Lon = data.Location.Lon
            }
        };
    }
}