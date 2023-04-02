using Microsoft.VisualBasic.CompilerServices;
using AirportInfo = AirportDistances.Infrastructure.Contracts.AirportInfo;

namespace AirportDistances.Infrastructure;

public class MockAirportInfoServiceProxy : IAirportInfoServiceProxy
{
    private TestAirports[] _testAirports;

    public MockAirportInfoServiceProxy(TestAirports[] testAirports)
    {
        _testAirports = testAirports;
    }
    public Task<Result<Contracts.AirportInfo>> GetAirportInfo(string code)
    {
        foreach (var item in _testAirports)
        {
            if (code == item.Code)
            {
                return Task.FromResult(new Result<Contracts.AirportInfo>
                {
                    Value = new Contracts.AirportInfo
                    {
                        City = "testCity",
                        Country = "testCountry",
                        Location = new Contracts.AirportInfo.LocationInfo
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
        throw new Exception("Ты не умеешь писать тесты?");
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