using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Sybon.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string serviceName, string serviceVersion)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(serviceVersion, new Info { Title = serviceName, Version = serviceVersion });
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("api_key", new ApiKeyScheme {In = "query", Name = "api_key"});
                c.OperationFilter<SwaggerApiKeySecurityFilter>();
            });
            return services;
        }
    }
}