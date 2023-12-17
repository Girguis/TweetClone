namespace Core.Configs;

public class ConfigsManager
{
    private static Dictionary<AppSettingsConfig, string> dict = new();
    public static void Append(AppSettingsConfig appSettingsConfig, string value)
    {
        if (!dict.ContainsKey(appSettingsConfig))
            dict.Add(appSettingsConfig, value);
    }

    public static string TryGet(AppSettingsConfig appSettingsConfig)
    {
        dict.TryGetValue(appSettingsConfig, out string value);
        return value;
    }
    public static int TryGetNumber(AppSettingsConfig appSettingsConfig, int defaultValue = 0)
    {
        dict.TryGetValue(appSettingsConfig, out string value);
        if (!int.TryParse(value, out int parsedValue))
            return defaultValue;
        return parsedValue;
    }
}
