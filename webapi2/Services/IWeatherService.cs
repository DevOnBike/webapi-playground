namespace webapi2.Services
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> GetWeather();
    }
}
