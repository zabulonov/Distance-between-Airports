using System.Text;
using AirportDistance.Business;
using AirportDistances.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace Business.Tests;
[TestFixture]
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
        // Я не понимаю как это работает, мне страшно трогать
        var cacheMock = new Mock<IDistributedCache>();
        cacheMock.Setup(c => c.GetAsync(firstCode, default)).ReturnsAsync((byte[])null);
        cacheMock.Setup(cc => cc.GetAsync(secondCode, default)).ReturnsAsync((byte[])null);
        byte[] newData = new byte[] { 4, 5, 6 };
        var options = new DistributedCacheEntryOptions();
        options.SetSlidingExpiration(TimeSpan.FromMinutes(5));
        cacheMock.Setup(c => c.SetAsync("mykey", newData, options, default)).Returns(Task.CompletedTask);


        var mock = new MockAirportInfoServiceProxy(_tests);

        var airportDistanceService = new AirportDistanceService(mock, cacheMock.Object);

        var distanceResult = await airportDistanceService.GetDistance(new[] { firstCode, secondCode });

        distanceResult.ErrorMessage.Should().BeNull();
        distanceResult.Distance.Should().BeApproximately(expected, Delta);

    }
}