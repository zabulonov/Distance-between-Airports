using AirportDistance.Business;
using FluentAssertions;
using NUnit.Framework;

namespace Business.Tests;

public class GetDistanceTests
{
    private readonly MockAirportInfoServiceProxy.TestAirports[] _tests = new[]
    {
        new MockAirportInfoServiceProxy.TestAirports("AAA", -17.355648, -145.50913),
        new MockAirportInfoServiceProxy.TestAirports("BBB", 45.316667, -95.6),
        new MockAirportInfoServiceProxy.TestAirports("DME", 55.414566, 37.899494),
        new MockAirportInfoServiceProxy.TestAirports("LED", 59.799847, 30.270505)
    };

    private const double Delta = 50;

    // Тесты проводились с https://www.airportdistancecalculator.com
    [TestCase("AAA", "BBB", 8591)]
    [TestCase("AAA", "AAA", 0)]
    [TestCase("DME", "LED", 667)]
    [TestCase("LED", "DME", 667)]
    public async Task ShouldCalculateDistanceBetweenAirports(string firstCode, string secondCode, double expected)
    {
        var mock = new MockAirportInfoServiceProxy(_tests);
        var airportDistanceService = new AirportDistanceService(mock);

        var distanceResult = await airportDistanceService.GetDistance(new[] { firstCode, secondCode });

        distanceResult.ErrorMessage.Should().BeNull();
        distanceResult.Distance.Should().BeApproximately(expected, Delta);

    }
}