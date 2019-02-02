using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Mailman.Net.Smtp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMailman(this IServiceCollection services, IEmailConfig configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddTransient<IMailman>(provider => new Mailman(configuration));
        }

        public static IServiceCollection AddMailman(this IServiceCollection services, Func<IEmailConfig> configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IEmailConfig config = configuration();
            return services.AddTransient<IMailman>(provider => new Mailman(config));
        }

        public static IServiceCollection AddMailman(this IServiceCollection services, IConfiguration configuration)
        {
           if (services == null) throw new ArgumentNullException(nameof(services));
           if (configuration == null) throw new ArgumentNullException(nameof(configuration)); 

           var config = new EmailConfig();
           configuration.Bind(config);
           return services.AddTransient<IMailman>(provider => new Mailman(config));
        }
    }
}