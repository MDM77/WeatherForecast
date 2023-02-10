using Newtonsoft.Json;
using System.Text;
using System.Web;
using Weather_Forecast;

Console.OutputEncoding = Encoding.UTF8;
var apiKey = "de652bea5fc39e262e384811c44b47a7";
var client = new HttpClient();

while (true)
{
    Console.Write("Введите название города для отображения погоды: ");
    var cityName = Console.ReadLine();
    var response = await client.GetAsync(@$"https://api.openweathermap.org/data/2.5/forecast?q={HttpUtility.UrlEncode(cityName)}&appid={apiKey}&units=metric&lang=ru");
    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<Rootobject>(result);
        Console.Clear();
        Console.WriteLine(
            $"{model.city.name}, {model.city.country}, {DateTime.Now}\n" +
            $"Погодные условия: {model.list[0].weather[0].description}\n" +
            $"Температура воздуха: {Math.Round(model.list[0].main.temp, 1)}°С, ощущается как {Math.Round(model.list[0].main.feels_like, 1)}°С\n" +
            $"Скорость ветра: {model.list[0].wind.speed}м/с, {WindDirection(model.list[0].wind.deg)}\n" +
            $"Давление: {model.list[0].main.grnd_level}\n" +
            $"Влажность: {model.list[0].main.humidity}%");
        Console.ReadKey();
        Console.Clear();
        DateTime summDay;
        int dayNo = 0;
        List forecast4Days;
        for (int i = 0; i < 4; i++)
        {
            forecast4Days = model.list[dayNo];
            summDay = DateTime.Parse(forecast4Days.dt_txt);
            Console.WriteLine($"{summDay.ToShortDateString()}");
            Console.WriteLine($"{summDay.ToString("dddd")[0].ToString() + summDay.ToString("dddd").Substring(1)}");
            Console.WriteLine($"{Math.Round(forecast4Days.main.temp_min, 1)}, {Math.Round(forecast4Days.main.temp_max, 1)} ");
            Console.WriteLine($"{(forecast4Days.weather[0].description)[0].ToString() + (forecast4Days.weather[0].description).Substring(1)}");
            Console.WriteLine(" ");
            dayNo += 8;
        }
        Console.ReadKey();
    }
    string WindDirection(int wind) =>
            wind switch
            {
                >= 0 and < 23 or >= 338 and <= 360 => "C",
                >= 23 and < 68 => "СВ",
                >= 68 and < 113 => "В",
                >= 113 and < 158 => "ЮВ",
                >= 158 and < 203 => "Ю",
                >= 203 and < 248 => "ЮЗ",
                >= 248 and < 292 => "З",
                >= 292 and < 338 => "CЗ",
                _ => "",
            };
}