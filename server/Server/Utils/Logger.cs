using System;
using System.IO;

namespace Server.Utils
{
    public static class Logger
    {
        private static readonly string LogFile = $"logs/server_{DateTime.Now:yyyyMMdd}.log";
        static Logger()
        {
            var dir = Path.GetDirectoryName(LogFile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        }
        public static void Info(string message)
        {
            var log = $"[INFO] [{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(log);
            File.AppendAllText(LogFile, log + Environment.NewLine);
        }
        public static void Error(string message)
        {
            var log = $"[ERROR] [{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(log);
            File.AppendAllText(LogFile, log + Environment.NewLine);
        }
    }
}
