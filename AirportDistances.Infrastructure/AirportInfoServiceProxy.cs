using System.Net;
using System.Net.Http.Json;
using AirportDistances.Infrastructure.Contracts;

namespace AirportDistances.Infrastructure;

public class AirportInfoServiceProxy : IAirportInfoServiceProxy
{
    private readonly HttpClient _httpClient;

    public AirportInfoServiceProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<AirportInfo>> GetAirportInfo(string code)
    {
        #warning перенести взаимодействие с кэшем сюда
        using var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{code}");
        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
        {
            Console.WriteLine(response.StatusCode);
            return new Result<AirportInfo>
            {
                IsSuccess = false,
                FaultMessage = $"Airport info for iata code {code} result:{response.StatusCode.ToString()}"
            };
        }

        var data = await response.Content.ReadFromJsonAsync<Models.AirportInfo>();
        
        return new Result<AirportInfo>
        {
            Value = new AirportInfo
            {
                Code = data.Iata,
                City = data.City,
                Country = data.City,
                Location = new AirportInfo.LocationInfo
                {
                    Lat = data.Location.Lat,
                    Lon = data.Location.Lon
                }
            },
            IsSuccess = true,
            FaultMessage = string.Empty
        };
    }
}