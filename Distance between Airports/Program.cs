using Distance_between_Airports.BusinessLogic;
using Distance_between_Airports.Infrastructure;

var getCoordinates = new GetCoordinates(new HttpClient());
var distance = new Distance(getCoordinates);
var calculatedDistance = await distance.GetDistance(new[] { "DME", "LED" });
Console.WriteLine(calculatedDistance);


var testDistance = new Distance(new MockGetCoordinates());
calculatedDistance = await testDistance.GetDistance(new[] { "DME", "DME" });
Console.WriteLine(calculatedDistance == 0);