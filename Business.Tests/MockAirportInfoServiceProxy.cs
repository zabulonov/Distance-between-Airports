using AirportDistances.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using AirportInfo = AirportDistances.Infrastructure.Contracts.AirportInfo;

namespace Business.Tests;

public class MockAirportInfoServiceProxy : IAirportInfoServiceProxy
{
    private readonly TestAirports[] _testAirports;
    public MockAirportInfoServiceProxy(TestAirports[] testAirports)
    {
        _testAirports = testAirports;
    }
    public Task<Result<AirportInfo>> GetAirportInfo(string code)
    {
        foreach (var item in _testAirports)
        {
            if (code == item.Code)
            {
                return Task.FromResult(new Result<AirportInfo>
                {
                    Value = new AirportInfo
                    {
                        City = "testCity",
                        Country = "testCountry",
                        Location = new AirportInfo.LocationInfo
                        {
                            Lat = item.Lat,
                            Lon = item.Lon
                        }
                    },
                    IsSuccess = true,
                    FaultMessage = string.Empty
                });
            }
        }
        return Task.FromResult(new Result<AirportInfo>
        {
            Value = null,
            IsSuccess = false,
            FaultMessage = "Airport not found"
        });
    }

    public class TestAirports
    {
        public string Code { get; }
        public double Lat { get; }
        public double Lon { get; }


        public TestAirports(string code, double lat, double lon)
        {
            Code = code;
            Lat = lat;
            Lon = lon;
        }
    }
}