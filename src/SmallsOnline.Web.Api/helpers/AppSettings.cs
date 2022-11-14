namespace SmallsOnline.Web.Api.Helpers;

/// <summary>
/// Hosts methods to get Azure Function app settings.
/// </summary>
public static class AppSettings
{
    /// <summary>
    /// Get the value of a setting configured in the Azure Function app settings.
    /// </summary>
    /// <param name="settingName">The name of the setting.</param>
    /// <returns>A string value of the setting.</returns>
    public static string? GetSetting(string settingName)
    {
        string? settingValue = Environment.GetEnvironmentVariable(
            settingName,
            EnvironmentVariableTarget.Process
        );

        return settingValue;
    }
}