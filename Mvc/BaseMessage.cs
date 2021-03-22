namespace Nicehash.Withdrawal.Mvc
{
    public abstract class BaseMessage
    {
        /// <summary>
        /// Ключ сообщения
        /// </summary>
        public string Key => GetType().GUID.ToString();
    }
}