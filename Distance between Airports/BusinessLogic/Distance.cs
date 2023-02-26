using System.Net;
using System.Net.Http.Json;
using Distance_between_Airports.Models;

namespace Distance_between_Airports.BusinessLogic;

public static class Distance
{
    private static readonly Location?[] Locations = new Location[2];
    

    public static async Task SetCoordinates(string[] codes, HttpClient httpClient)
    {
        
        for (int i = 0; i < 2; i++)
        {
            using var response = await httpClient.GetAsync("https://places-dev.cteleport.com/airports/" + codes[i]);
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var data = await response.Content.ReadFromJsonAsync<Root>();
                Locations[i] = data?.Location;
                Locations[i]?.ToRadians();
            }
        }
    }


public static double GetDistance()
    {
        // Вот тут нифига не правильно
        int earthRadius = 6371;
        var cosd = Math.Sin(Locations[0]!.Lat) * Math.Sin(Locations[1]!.Lat) + Math.Cos(Locations[0]!.Lat) *
            Math.Cos(Locations[1]!.Lat) * Math.Cos(Locations[0]!.Lon + Locations[1]!.Lon);
        
        return Math.Acos(cosd) * earthRadius;
    }

    public static double GetDistanceTest()
        {
            // Вот тут все правильно
            // см пример в конце http://osiktakan.ru/geo_koor.htm
            Locations[0]!.Lat = 64.28;
            Locations[0]!.Lon = 100.22;
            Locations[1]!.Lat = 40.71;
            Locations[1]!.Lon = 74.01;
            Locations[0]?.ToRadians();
            Locations[1]?.ToRadians();
            int earthRadius = 6371;
            var cosd = Math.Sin(Locations[0]!.Lat) * Math.Sin(Locations[1]!.Lat) + Math.Cos(Locations[0]!.Lat) *
                Math.Cos(Locations[1]!.Lat) * Math.Cos(Locations[0]!.Lon + Locations[1]!.Lon);
  
            return Math.Acos(cosd) * earthRadius;
        }
}