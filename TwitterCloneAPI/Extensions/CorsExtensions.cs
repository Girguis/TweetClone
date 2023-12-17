namespace TwitterCloneAPI.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection LoadCorsServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(o => o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        });
        return services;
    }

}
