using Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules;
using System.Collections.Generic;

namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Withdrawal
{
    public class JsonWithdrawalFeeRules
    {
        /// <summary>
        /// Список правил
        /// </summary>
        public Dictionary<string, JsonRule> Rules { get; set; }
    }
}