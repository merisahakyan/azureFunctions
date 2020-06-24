using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyFunctionsApp;
using MyFunctionsApp.ServiceInterfaces;
using MyFunctionsApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(Startup))]
namespace MyFunctionsApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IBlobService, BlobService>();
            builder.Services.AddSingleton<ISqlService, SqlService>();
        }
    }
}
