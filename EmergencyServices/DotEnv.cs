namespace EmergencyServices.Group8
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    [ExcludeFromCodeCoverage]
    public static class DotEnv // Backup class to load environment variables in case DotNetEnv does not work
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
