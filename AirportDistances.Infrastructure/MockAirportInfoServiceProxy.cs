namespace AirportDistances.Infrastructure.Models;

public class MockAirportInfoServiceProxy : IAirportInfoServiceProxy
{
    public async Task<Result<AirportInfo>> GetAirportInfo(string code)
    {
        return new Result<AirportInfo>
        {
            Value = new AirportInfo
            {
                City = "testCity",
                Country = "testCountry",
                Location = new AirportInfo.LocationInfo
                {
                    Lat = 1,
                    Lon = 1
                }
            },
            IsSuccess = true,
            FaultMessage = string.Empty
        };
    }
}