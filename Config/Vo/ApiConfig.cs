namespace Nicehash.Withdrawal.Config.Vo
{
    public class ApiConfig
    {
        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string ApiKey { get; set; }

        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string ApiSecret { get; set; }

        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Идентификатор кошелька
        /// </summary>
        public string WalletId { get; set; }

        public const string SectionName = "Api";
    }
}