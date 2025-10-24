using System;
using System.IO;
using System.Text;

namespace KPP_Alpha1.HelperClasses
{
    /// <summary>
    /// Simple file-based logging for error tracking and debugging.
    /// Logs are written to /Logs directory with daily rotation.
    /// </summary>
    public static class LogHelper
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Logs");
        
        static LogHelper()
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }
        
        /// <summary>
        /// Logs an error with exception details.
        /// </summary>
        /// <param name="source">Source of the error (e.g., "OdjelController.Insert")</param>
        /// <param name="message">Error message or context</param>
        /// <param name="ex">Exception object (optional)</param>
        public static void LogError(string source, string message, Exception ex = null)
        {
            Log("ERROR", source, message, ex);
        }
        
        /// <summary>
        /// Logs an informational message (for debugging).
        /// </summary>
        /// <param name="source">Source of the log entry</param>
        /// <param name="message">Informational message</param>
        public static void LogInfo(string source, string message)
        {
            Log("INFO", source, message, null);
        }
        
        /// <summary>
        /// Deletes log files older than specified days.
        /// Call this on application startup to prevent log accumulation.
        /// </summary>
        /// <param name="daysToKeep">Number of days to retain logs (default: 365)</param>
        public static void CleanOldLogs(int daysToKeep = 365)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var files = Directory.GetFiles(_logDirectory, "KPP_*.log");
                
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch
            {
                // Swallow exceptions during cleanup to prevent blocking application startup
            }
        }
        
        private static void Log(string level, string source, string message, Exception ex)
        {
            lock (_lock)
            {
                try
                {
                    string logFile = Path.Combine(_logDirectory, 
                        $"KPP_{DateTime.Now:yyyy-MM-dd}.log");
                    
                    var sb = new StringBuilder();
                    sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level} | {source}");
                    sb.AppendLine($"  Message: {message}");
                    
                    if (ex != null)
                    {
                        sb.AppendLine($"  Exception: {ex.GetType().Name}");
                        sb.AppendLine($"  Details: {ex.Message}");
                        if (!string.IsNullOrEmpty(ex.StackTrace))
                        {
                            sb.AppendLine($"  Stack: {ex.StackTrace}");
                        }
                    }
                    
                    sb.AppendLine(new string('-', 80));
                    
                    File.AppendAllText(logFile, sb.ToString());
                }
                catch
                {
                    // Swallow logging errors to prevent cascading failures
                }
            }
        }
    }
}
