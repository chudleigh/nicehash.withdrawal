namespace Nicehash.Withdrawal.Mvc.Model.App.Msg
{
    public class MsgAppStartup : BaseMessage
    {
        /// <summary>
        /// Результат первоначального запуска
        /// </summary>
        public bool StartupResult { get; }

        public MsgAppStartup(bool result)
        {
            StartupResult = result;
        }
    }
}