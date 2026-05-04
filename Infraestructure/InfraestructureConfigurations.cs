using Domain.Models.Base;
using Infraestructure.Models.Options;
using Infraestructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure
{
    public static class InfraestructureConfigurations
    {
        public static IServiceCollection AddAInfraestructureLayer(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            // Options Configurations
            var tokenSectionName = configuration.GetSection(TokenManageOptions.SectionName);
            services.Configure<TokenManageOptions>(tokenSectionName);

            // Repositories and Services Injections
            //services.AddScoped<IAuthCachedRepository, AuthCachedRepository>();

            // Database Context Configuration
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // For AutoMapper Configuration

            var appplicationAssembly = typeof(BaseModel).Assembly;

            services.AddAutoMapper(mc =>
            {
                mc.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;

                mc.ShouldMapMethod = m =>
                    !m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false);
            }, appplicationAssembly);

            return services;
        }
    }
}
