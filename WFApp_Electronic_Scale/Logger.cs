using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFApp_Electronic_Scale
{
    public class Logger : IDisposable
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private readonly ConcurrentQueue<LogEntry> _logQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _logTask;
        private readonly string _logFilePath;
        private bool _disposed = false;
        private readonly int _maxLogEntries = 1000;

        private Logger()
        {
            _logFilePath = Path.Combine(Application.StartupPath, "log.txt");
            _logQueue = new ConcurrentQueue<LogEntry>();
            _cancellationTokenSource = new CancellationTokenSource();

            _logTask = Task.Run(ProcessLogQueue, _cancellationTokenSource.Token);
        }

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Log(string message, LogLevel level = LogLevel.Info, Exception exception = null)
        {
            if (_disposed) return;

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Level = level,
                Exception = exception
            };

            _logQueue.Enqueue(logEntry);

            // Prevent queue from growing too large
            if (_logQueue.Count > _maxLogEntries)
            {
                while (_logQueue.Count > _maxLogEntries / 2)
                {
                    _logQueue.TryDequeue(out _);
                }
            }
        }

        private async Task ProcessLogQueue()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    while (_logQueue.TryDequeue(out var logEntry))
                    {
                        await WriteLogEntry(logEntry);
                    }

                    await Task.Delay(100, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // Fallback logging to prevent infinite loops
                    try
                    {
                        File.AppendAllText(_logFilePath, $"[ERROR] Logger exception: {ex.Message}\n");
                    }
                    catch { }
                }
            }
        }

        private async Task WriteLogEntry(LogEntry logEntry)
        {
            try
            {
                var logLine = $"[{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss}] [{logEntry.Level}] {logEntry.Message}";

                if (logEntry.Exception != null)
                {
                    logLine += $"\nException: {logEntry.Exception}";
                }

                logLine += Environment.NewLine;

                await Task.Run(() => File.AppendAllText(_logFilePath, logLine));
            }
            catch (Exception ex)
            {
                // Silent fail to prevent infinite loops
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource?.Cancel();
                    _logTask?.Wait(5000); // Wait up to 5 seconds for completion
                    _cancellationTokenSource?.Dispose();
                }
                _disposed = true;
            }
        }

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
            public LogLevel Level { get; set; }
            public Exception Exception { get; set; }
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }
}
