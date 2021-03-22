using System;

namespace Nicehash.Withdrawal.Mvc.Model.App.Msg
{
    public class MsgAppProxyStartup : BaseMessage
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Прокси
        /// </summary>
        public BaseProxy Proxy { get; }

        /// <summary>
        /// Результат первоначального запуска
        /// </summary>
        public bool StartupResult { get; }

        public MsgAppProxyStartup(BaseProxy proxy, bool result, Exception exception = default)
        {
            Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            StartupResult = result;
            Exception = exception;
        }
    }
}