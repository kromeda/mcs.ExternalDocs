namespace ExternalDocs.Web.Utils
{
    internal static class Utility
    {
        internal static string CurrentEnvironment =>
            (string)Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine)["ASPNETCORE_ENVIRONMENT"]!
            ?? "Development";
    }
}
