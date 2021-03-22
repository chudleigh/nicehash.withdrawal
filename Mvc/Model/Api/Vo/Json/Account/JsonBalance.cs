namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Account
{
    public class JsonBalance
    {
        /// <summary>
        /// Если <c>true</c>, то аккаунт активен, в противном случае - <c>false</c>
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Общий баланс
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// Доступный баланс
        /// </summary>
        public decimal Available { get; set; }
    }
}