using System;
using Microsoft.Extensions.DependencyInjection;

namespace Mailman.Extensions
{
    public static class EmailExtensions
    {
        public static IServiceCollection AddMailman(this IServiceCollection services, IEmailConfig configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddTransient<IEmailService>(provider => new EmailService(configuration));
        }

        public static IServiceCollection AddMailman(this IServiceCollection services, Func<IEmailConfig> configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IEmailConfig config = configuration();
            return services.AddTransient<IEmailService>(provider => new EmailService(config));
        }
    }
}