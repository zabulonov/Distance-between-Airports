using static Distance_between_Airports.BusinessLogic.Distance;

var httpClient = new HttpClient();
await SetCoordinates(new string[]{"DME","LED"}, httpClient);
Console.WriteLine(GetDistance());
Console.WriteLine(GetDistanceTest());



