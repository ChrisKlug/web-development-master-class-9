using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.Extensions.DependencyInjection;

namespace WebDevMasterClass.Testing;
internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTestAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
                {
                    options.DefaultScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = BasicAuthenticationDefaults.AuthenticationScheme;
                })
                .AddBasicAuthentication(creds => Task.FromResult(creds.username.Equals("Test", StringComparison.InvariantCultureIgnoreCase)
                                                                && creds.password == "test"));

        return services;
    }
}
