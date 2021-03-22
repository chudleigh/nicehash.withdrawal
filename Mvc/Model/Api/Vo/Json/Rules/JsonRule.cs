namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules
{
    public class JsonRule
    {
        /// <summary>
        /// Монета
        /// </summary>
        public string Coin { get; set; }

        /// <summary>
        /// Интервалы
        /// </summary>
        public JsonRuleInterval[] Intervals { get; set; }
    }
}