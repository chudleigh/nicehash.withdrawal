namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules
{
    public class JsonRuleIntervalElement
    {
        /// <summary>
        /// Значение базовой комиссии
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Значение вторичной комиссии
        /// </summary>
        public decimal SndValue { get; set; }

        /// <summary>
        /// Тип базовой комисcи
        /// </summary>
        public JsonRuleIntervalElementType Type { get; set; }

        /// <summary>
        /// Тип вторичной комисcи
        /// </summary>
        public JsonRuleIntervalElementType SndType { get; set; }
    }
}