using Newtonsoft.Json;

namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Withdrawal
{
    public class JsonWithdrawalRequest
    {
        /// <summary>
        /// Идентификатор адреса вывода
        /// </summary>
        [JsonProperty("withdrawalAddressId")]
        public string WithdrawalAddressId { get; set; }

        /// <summary>
        /// Сумма вывода
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}