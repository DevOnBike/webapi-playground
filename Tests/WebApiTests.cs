using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class WebApiTests : IClassFixture<TestWebApplicationFactory>
    {
        [Fact]
        public async Task WeatherGetTest()
        {
            // arrange

            // await using var application = new TestWebApplicationFactory();
            // using var client = application.CreateClient();

            // act
            var response = await _client.GetAsync("/weatherforecast");

            // assert
            Assert.True(response.IsSuccessStatusCode);
        }

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public WebApiTests(ITestOutputHelper output, TestWebApplicationFactory application)
        {
            _output = output;
            _client = application.CreateClient();
        }


    }
}