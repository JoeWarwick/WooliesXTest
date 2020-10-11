using APIProxy1.Models;
using APIProxy1.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(APIProxy1.Startup))]

namespace APIProxy1
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<IResourceService, ResourceService>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "JoeWarwickAPI", Version = "v1" });
            });

            builder.Services.AddOptions<Config>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Config").Bind(settings);
                });
        }
    }
}
