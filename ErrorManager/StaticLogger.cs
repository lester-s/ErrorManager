using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorManager.Properties;

namespace ErrorManager
{
    public class StaticLogger
    {
        public static void Log(Exception ex)
        {
            ExecuteWrite(ex);
        }

        public static void Log<T>(string message, T exception) where T: Exception
        {
            var ex = Activator.CreateInstance<T>();
            ExecuteWrite(ex);
        }

        public static void Log(string message)
        {
            var ex = new Exception(message);
            ExecuteWrite(ex);
        }

        public static async Task LogAsync(Exception ex)
        {
            using (StreamWriter stream = new StreamWriter(Settings.Default.LogFilePath))
            {
                var errorMessage = CreateMessage(ex);
                ExecuteWrite(ex);
            }
        }

        public static void LogAsync<T>(string message, T exception) where T : Exception
        {
            var ex = Activator.CreateInstance<T>();
            ExecuteWrite(ex);
        }

        public static void Log(string message)
        {
            var ex = new Exception(message);
            ExecuteWrite(ex);
        }

        private static void ExecuteWrite(Exception ex, bool isAsync = false)
        {
            using (StreamWriter stream = new StreamWriter(GetLogFilePath(ex.Source)))
            {
                var errorMessage = CreateMessage(ex);
                try
                {
                    if (isAsync)
                    {
                        stream.WriteLineAsync(errorMessage);
                    }
                    else
                    {
                        stream.WriteLine(errorMessage);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static string CreateMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Error" + ex.GetType() + ": " + DateTime.Now);
            sb.AppendLine("   --> Mesage: " + ex.Message);
            sb.AppendLine("   --> from: " + ex.TargetSite);
            sb.AppendLine("   --> InnerException: " + ex.Message);
            sb.AppendLine("   --> StackTrace: " + ex.StackTrace);

            return sb.ToString();
        }

        private static FileStream GetLogFilePath(string appName)
        {
            var path = Settings.Default.LogFilePath;
            path += appName ?? "defaultAppName";
            path += ".txt";
            return File.OpenWrite(path);
        }
    }
}
