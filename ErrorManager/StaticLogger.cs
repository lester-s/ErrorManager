using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ErrorManager.Properties;

namespace ErrorManager
{
    public class StaticLogger
    {
        #region Synchrone logging
        public static void Log(Exception ex)
        {
            ExecuteWrite(ex);
        }

        public static void Log<T>(string message) where T : Exception
        {
            var innerEx = Activator.CreateInstance<T>();
            var ex = new Exception(message, innerEx);
            ExecuteWrite(ex);
        }

        public static void Log(string message)
        {
            var ex = new Exception(message);
            ExecuteWrite(ex);
        }
        #endregion

        #region asynchrone logging
        public static async Task LogAsync(Exception ex)
        {
            var errorMessage = CreateMessage(ex);
            await ExecuteWrite(ex, true);
        }

        public static async Task LogAsync<T>(string message) where T : Exception
        {
            var innerEx = Activator.CreateInstance<T>();
            var ex = new Exception(message, innerEx);
            await ExecuteWrite(ex, true);
        }

        public static async Task LogAsync(string message)
        {
            var ex = new Exception(message);
            await ExecuteWrite(ex, true);
        }
        #endregion

        private static async Task ExecuteWrite(Exception ex, bool isAsync = false)
        {
            var fileStream = GetLogFilePath();
            fileStream.Position = fileStream.Length <= 0 ? 0 : fileStream.Length;
            using (StreamWriter stream = new StreamWriter(fileStream))
            {
                var errorMessage = CreateMessage(ex);
                try
                {
                    if (isAsync)
                    {
                        await stream.WriteLineAsync(errorMessage);
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
            var t = ex.GetType(); ;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------");
            sb.AppendLine("Error" + ex.GetType() + ": " + DateTime.Now);
            sb.AppendLine("   --> Message: " + ex.Message);
            sb.AppendLine("   --> from: " + ex.TargetSite);
            if (ex.InnerException != null)
            {
                sb.AppendLine("   --> InnerException: " + ex.InnerException.Message);
            }
            sb.AppendLine("   --> StackTrace: " + ex.StackTrace);

            return sb.ToString();
        }

        private static FileStream GetLogFilePath()
        {
            var path = Settings.Default.LogFilePath + "\\";
            path += Assembly.GetEntryAssembly().GetName().Name ?? "defaultAppName";
            path += "Log.txt";
            return File.OpenWrite(path);
        }
    }
}
