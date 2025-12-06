using System;
using System.IO;

namespace PlantCare.Utils
{
    public static class Logger
    {
        private static readonly object _lock = new object();

        private static string LogFilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error-log.txt");

        public static void Log(Exception ex)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(
                        LogFilePath,
                        $"[{DateTime.Now}] {ex}\n-----------------\n"
                    );
                }
            }
            catch
            {
                // If logging fails, ignore
            }
        }
    }
}
