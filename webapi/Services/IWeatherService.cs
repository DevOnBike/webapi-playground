namespace webapi.Services
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GetWeather();
    }
}
