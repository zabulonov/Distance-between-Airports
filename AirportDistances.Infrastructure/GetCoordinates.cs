using System.Net;
using System.Net.Http.Json;
using AirportDistances.Infrastructure.Models;

namespace AirportDistances.Infrastructure;

public class GetCoordinates : IGetCoordinates
{
    private readonly HttpClient _httpClient;
    private static readonly Location?[] Locations = new Location[2];

    private const string RequestUrl = "https://places-dev.cteleport.com/airports/";

    public GetCoordinates(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task SetCoordinates(string[] codes)
    {
        for (int i = 0; i < 2; i++)
        {
            using var response = await _httpClient.GetAsync( RequestUrl + codes[i]);
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var data = await response.Content.ReadFromJsonAsync<Root>();
                Locations[i] = data?.Location;
            }
        }
    }

    public async Task<Root?> GetAirportInfo(string code)
    {
        using var response = await _httpClient.GetAsync( RequestUrl + code);
        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine(response.StatusCode);
        }
        else
        {
            var data = await response.Content.ReadFromJsonAsync<Root>();
            return data;
        }

        throw new InvalidOperationException();
    }

    public Location?[] GetLocations()
    {
        return Locations;
    }
}