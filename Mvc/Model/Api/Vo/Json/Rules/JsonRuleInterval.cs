namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules
{
    public class JsonRuleInterval
    {
        /// <summary>
        /// Начало интервала
        /// </summary>
        public decimal? Start { get; set; }

        /// <summary>
        /// Конец интервала
        /// </summary>
        public decimal? End { get; set; }

        /// <summary>
        /// Правило, по которому считается комиссия
        /// </summary>
        public JsonRuleIntervalElement Element { get; set; }
    }
}