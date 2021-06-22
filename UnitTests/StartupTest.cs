using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace UnitTests
{
    using EonetApp;
    
    public class StartupTest
    {
        private readonly IServiceCollection _services;
        private readonly Startup _startup;

        private StartupTest()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Urls:", "http://localhost:5000"},
                {"Redis:ConnectionString", "localhost:6379,password=password"},
            }).Build();

            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            webHostEnvironment.SetupGet(e => e.EnvironmentName)
                  .Returns("Development");
            _startup = new Startup(configuration);

            _services = new ServiceCollection();
            _startup.ConfigureServices(_services);
        }

        [Theory]
        [MemberData(nameof(AllControllers))]
        public void AllControllersCanBeInstantiated(Type controller)
        {
            _services.AddTransient(controller);

            var serviceProvider = _services.BuildServiceProvider();
            var controllerInstance = serviceProvider.GetService(controller);
            controllerInstance.Should().NotBeNull();
        }

        public static IEnumerable<object[]> AllControllers()
        {
            var assembly = typeof(Startup).Assembly;
            var controllers = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ControllerBase)) && !type.IsAbstract);
            return controllers.Select(x => new object[] {x});
        }
    }
}
