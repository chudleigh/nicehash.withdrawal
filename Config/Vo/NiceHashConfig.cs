namespace Nicehash.Withdrawal.Config.Vo
{
    public class NiceHashConfig
    {
        /// <summary>
        /// Url для запросов
        /// </summary>
        public string UrlRoot { get; set; }

        /// <summary>
        /// Сумма вывода
        /// </summary>
        public decimal WithdrawalAmount { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Интервал проверки в минутах
        /// </summary>
        public int IntervalInMinutes { get; set; }

        public const string SectionName = "NiceHash";
    }
}