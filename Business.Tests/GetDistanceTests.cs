using AirportDistance.Business;
using AirportDistances.Infrastructure;
using Microsoft.VisualBasic.CompilerServices;

namespace Business.Tests;

public class Tests
{
 private MockAirportInfoServiceProxy.TestAirports[] tests = new MockAirportInfoServiceProxy.TestAirports[]
 {
  new MockAirportInfoServiceProxy.TestAirports("AAA", 10.56, 23.65),
  new MockAirportInfoServiceProxy.TestAirports("BBB", 33.33, 15.10)
 };

 private const double Delta = 100;
 [Test]
 public void Test()
 {
  var mock = new MockAirportInfoServiceProxy(tests);
  var vm = new AirportDistanceService(mock);

  var ans = vm.GetDistance(new string[]{"AAA","BBB"}).Result.Distance;

  Assert.That(ans, Is.EqualTo(2566.308).Within(Delta));
 }
}
