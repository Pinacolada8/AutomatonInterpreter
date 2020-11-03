using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace App.Logging
{
    public static class LogController
    {
#if DEBUG
        private const LogConfigEnum DEFAULT_LOGGER_CONFIG = LogConfigEnum.DEBUG_IMPROVED;
#else
        private const LogConfigEnum DEFAULT_LOGGER_CONFIG = LogConfigEnum.DEFAULT;
#endif

        public static void Setup(LogConfigEnum logConfigPreset = DEFAULT_LOGGER_CONFIG)
        {
            var presetOverride = Environment.GetEnvironmentVariable(@"AUTOMATON_LOG_PRESET");

            if (!string.IsNullOrWhiteSpace(presetOverride))
                try
                {
                    logConfigPreset =
                        (LogConfigEnum)Enum.ToObject(typeof(LogConfigEnum), Convert.ToInt32(presetOverride));
                }
                catch
                { }

            var logConfig = new LoggingConfiguration();

            if (logConfigPreset.HasFlag(LogConfigEnum.CONSOLE_DEBUG))
                logConfig.AddRule(LogLevel.Debug,
                    LogLevel.Debug,
                    new ColoredConsoleTarget("DebugConsole")
                    {
                        Layout = @"${message}",
                        RowHighlightingRules =
                        {
                            new ConsoleRowHighlightingRule()
                            {
                                ForegroundColor = ConsoleOutputColor.Cyan,
                                Condition       = "true"
                            }
                        }
                    });

            if (logConfigPreset.HasFlag(LogConfigEnum.CONSOLE_ERRORS))
                logConfig.AddRule(LogLevel.Error,
                    LogLevel.Fatal,
                    new ConsoleTarget("ErrorConsole")
                    {
                        Error = true
                    });

            if (logConfigPreset.HasFlag(LogConfigEnum.CONSOLE_DEFAULT))
                logConfig.AddRule(LogLevel.Info,
                    LogLevel.Info,
                    new ConsoleTarget("DefaultConsole")
                    {
                        Layout = @"${message}"
                    });

            if (logConfigPreset.HasFlag(LogConfigEnum.CONSOLE_COLORED))
                logConfig.AddRule(LogLevel.Info,
                    LogLevel.Info,
                    new ColoredConsoleTarget("ColoredConsole")
                    {
                        Layout = @"${message}",
                        WordHighlightingRules =
                        {
                            //TODO: Use conditions for coloring
                            new ConsoleWordHighlightingRule()
                            {
                                Text            = "Sim",
                                ForegroundColor = ConsoleOutputColor.Green,
                                BackgroundColor = ConsoleOutputColor.NoChange,
                                IgnoreCase      = true
                            },
                            new ConsoleWordHighlightingRule()
                            {
                                Text            = "Não",
                                ForegroundColor = ConsoleOutputColor.DarkRed,
                                BackgroundColor = ConsoleOutputColor.NoChange,
                                IgnoreCase      = true,
                            }
                        }
                    });

            if (logConfigPreset.HasFlag(LogConfigEnum.FILE))
                logConfig.AddRule(LogLevel.Info,
                    LogLevel.Fatal,
                    new FileTarget("FileLog")
                    {
                        FileName = @"${basedir}/logs/${shortdate}.log",
                        Layout   = @"${date:format=yyyy-MM-dd HH:mm:ss}|${level:uppercase=true}|${message}"
                    });

            LogManager.Configuration = logConfig;
        }

        public static ILogger GetLogger() => LogManager.GetLogger("Automaton");
    }
}