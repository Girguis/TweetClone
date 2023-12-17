using Core.Configs;

namespace TwitterCloneAPI.Extension;

public static class ConfigureServiceExtension
{
    public static IConfiguration LoadConfiguration(this IConfiguration configuration)
    {
        ConfigsManager.Append(AppSettingsConfig.ConnectionString, configuration["ConnectionStrings:DefaultDb"]);
        ConfigsManager.Append(AppSettingsConfig.JwtAudience, configuration["Jwt:Audience"]);
        ConfigsManager.Append(AppSettingsConfig.JwtIssuer, configuration["Jwt:Issuer"]);
        ConfigsManager.Append(AppSettingsConfig.JwtKey, configuration["Jwt:Key"]);
        ConfigsManager.Append(AppSettingsConfig.JwtExpireTime, configuration["Jwt:ExpireTime"]);

        return configuration;
    }
}