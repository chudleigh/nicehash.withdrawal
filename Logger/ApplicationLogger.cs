using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;

namespace Nicehash.Withdrawal.Logger
{
    public static class ApplicationLogger
    {
        public static void Configure()
        {
            LayoutRenderer.Register<DescriptionLayoutRenderer>("description");

            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget("ConsoleTarget")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true:truncate=3} ${description} ${message} ${exception}"
            };

            // Изменим цвета логов на свои
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Trace", ForegroundColor = ConsoleOutputColor.DarkGray });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Debug", ForegroundColor = ConsoleOutputColor.DarkGray });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Info", ForegroundColor = ConsoleOutputColor.White });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Warn", ForegroundColor = ConsoleOutputColor.Yellow });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Error", ForegroundColor = ConsoleOutputColor.Red });
            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule { Condition = "level = LogLevel.Fatal", ForegroundColor = ConsoleOutputColor.Red });

            // Раскрашивание url
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = @"http(s?):\/\/.*?[\s$]+",
                ForegroundColor = ConsoleOutputColor.Cyan,
            });

            // Раскрашивание чисел
            consoleTarget.WordHighlightingRules.Add(new ConsoleWordHighlightingRule
            {
                Regex = @"\s\d*[\,]?\d+\s",
                ForegroundColor = ConsoleOutputColor.Magenta,
            });

            config.AddTarget(consoleTarget);

#if DEBUG
            config.AddRuleForAllLevels(consoleTarget);
#else
            config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
#endif
            LogManager.Configuration = config;
        }
    }
}