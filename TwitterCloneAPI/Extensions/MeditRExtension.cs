namespace TwitterCloneAPI.Extensions;

public static class MeditRExtension
{
    public static IServiceCollection LoadMeditR(this IServiceCollection services)
    {
        services.AddMediatR(r =>
        {
            r.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load(nameof(Application)));
        });

        return services;
    }
}
