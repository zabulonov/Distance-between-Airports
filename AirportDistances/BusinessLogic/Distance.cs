using AirportDistances.Infrastructure;

namespace AirportDistances.BusinessLogic;

public class Distance
{
    private readonly IGetCoordinates _getCoordinates;

    public Distance(IGetCoordinates getCoordinates)
    {
        _getCoordinates = getCoordinates;
    }
    //1. разделить инфраструктурные модели и модели бизнес-логики
    //2. поправить формулу / заюзать готовую библиотеку GeoCoordinates
    //3. почитать про mock/stub
    //4. написать тесты на GetDistance
    //5. вынести бизнес-логику в отдельную сборку
    //6. places-dev.cteleport.com вынести в конфиг
    //7. сделать чтобы SetCoordinates (с изменённым названием) возвращал инфу по одному аэропорту
    //8*. попробовать прикрутить кэш, чтобы кэшировать инфу по аэропортам
    public async Task<double> GetDistance(string[] airportCodes)
    {
        await _getCoordinates.SetCoordinates(airportCodes);
        // Вот тут нифига не правильно
        var locations = _getCoordinates.GetLocations();
        int earthRadius = 6371;
        var cosd = Math.Sin(locations[0]!.Lat) * Math.Sin(locations[1]!.Lat) + Math.Cos(locations[0]!.Lat) *
            Math.Cos(locations[1]!.Lat) * Math.Cos(locations[0]!.Lon1 + locations[1]!.Lon1);

        return Math.Acos(cosd) * earthRadius;
    }
}