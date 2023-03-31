# Distance-between-Airports

##Тесты
### Моковый сервис

Мок принимает массив тестовых аэропортов, затем возвращает найденный аэропорт или ошибку:

```csharp
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
```

### Сами тесты
Соответственно в классе тестов, создаем массив тестовых аэропортов с координатами. И используем их коды в TestCase'ах. Там уже моковый сервис вернет подменянный аэропорт в DistanceService. Дальше сравниваем результаты.
```csharp

private readonly MockAirportInfoServiceProxy.TestAirports[] _tests = new MockAirportInfoServiceProxy.TestAirports[]
 {
  new MockAirportInfoServiceProxy.TestAirports("AAA", -17.355648, -145.50913),
  new MockAirportInfoServiceProxy.TestAirports("BBB", 45.316667, -95.6)
 };

 private const double Delta = 50;
 
 // Тесты проводились с https://www.airportdistancecalculator.com
 [Test]
 [TestCase("AAA", "BBB", 8591)]
 [TestCase("AAA", "AAA", 0 )]
 public void Test(string firstCode, string secondCode, double expected)
 {
  var mock = new MockAirportInfoServiceProxy(_tests);
  var vm = new AirportDistanceService(mock);

  var ans = vm.GetDistance(new string[]{firstCode,secondCode}).Result.Distance;
  
  Assert.That(ans, Is.EqualTo(expected).Within(Delta));
 }
```

### Проблема

При тестах выяснилось, что он опять считает не правильно👍
<img width="1421" alt="Screenshot 2023-03-31 at 22 39 13" src="https://user-images.githubusercontent.com/83907630/229213874-67996b64-d9f2-426b-84d0-1bb6e5cc10e2.png">

Причем разные сервисы вообще дают разные цифры

 <img width="748" alt="Screenshot 2023-03-31 at 22 41 15" src="https://user-images.githubusercontent.com/83907630/229214411-37ba7750-1a39-4f50-86af-3fdd2585904a.png">
 <img width="1159" alt="Screenshot 2023-03-31 at 22 42 44" src="https://user-images.githubusercontent.com/83907630/229214539-aee7362f-6353-49c9-bf02-826a1edb988c.png">
 
 Тут я окончательно запутался, и принял решение. Что нужно юзать готовую библиотеку...
 
 ## Кеш

Для кеша я решил сделать отдельный контроллер, сервис и модель. Которые будут просто возвращать инфу о аэропорте, которую будем кешировать.
При получении объекта по Code вначале пытаемся найти этот объект в кэше, и если там не оказалось, то извлеаем его из airportInfoServiceProxy и затем добавляем в кэш.

```csharp
public async Task<InfoState> GetInfo(string code)
    {
        InfoState info;
        if (!_cache.TryGetValue(code, out info))
        {
            var data = await _airportInfoServiceProxy.GetAirportInfo(code);
            if (data.IsSuccess)
            {
                info = new InfoState
                {
                    airportInfo = new AirportInfo(),
                };
                info.airportInfo = data.Value;
                info.airportInfo.Code = code;
            }
            else
            {
                return new InfoState
                {
                    ErrorMessage = data.FaultMessage
                }; 
            }
    
            _cache.Set(info.airportInfo.Code, info,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }
        return info!;

```

Если ключ в кэше был найден, то в объект info передается извлекаемое из кэша значение, а метод TryGetValue() возвращает true. Для установки времени кэширования есть метод SetAbsoluteExpiration, 5 минут.

## Что еще я бы хотел доделать
To do list:
- Поправить наконец таки формулу(заюзать готовую либу)
- AirportInfoService возвращает не всю инфу об аэропорте, из-за этого есть поля null
- Добавить кеш в AirportDistanceService (я пока думаю как там это грамотно сделать)
