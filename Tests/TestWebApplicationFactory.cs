﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using webapi;

namespace Tests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        /// <summary>
        /// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
        /// </summary>
        /// <param name="builder"></param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IWeatherService));
                //services.Remove(descriptor);
                
            });

            builder.ConfigureTestServices(services =>
            {
                //services.AddSingleton<IWeatherService, WeatherService>();

                // add mocks, etc ...
            });

        }
    }
}