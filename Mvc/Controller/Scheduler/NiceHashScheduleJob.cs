using Nicehash.Withdrawal.Config;
using Nicehash.Withdrawal.Mvc.Model.Api.Vo;
using Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules;
using NLog;
using Quartz;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Nicehash.Withdrawal.Mvc.Controller.Scheduler
{
    [Description("Scheduler")]
    public class NiceHashScheduleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            #region Проверка баланса

            Logger.Info("Проверка баланса аккаунта...");
            var balance = await NiceHashApi.GetBtcBalanceAsync(Configuration.NiceHash.Currency);

            if (balance is null)
            {
                Logger.Warn("Не удалось получить баланс аккаунта");
                return;
            }

            if (balance.Active == false)
            {
                Logger.Warn("Аккаунт помечен как неактивный");
                return;
            }

            Logger.Info($"Баланс аккаунта получен. Всего: {balance.TotalBalance} {balance.Currency}. Доступно: {balance.Available} {balance.Currency}");

            #endregion Проверка баланса

            #region Расчёт комиссий

            Logger.Info("Получаем текущие правила расчёта комисcии...");

            var rule = await NiceHashApi.GetWithdrawalFeeRulesAsync(Configuration.NiceHash.Currency);
            if (rule is null)
            {
                Logger.Warn("Не удалось получить правила расчёта комисcии");
                return;
            }

            Logger.Info("Правила расчёта комисcии получены");

            // Получаем интервал
            Logger.Info("Вычисление интервала...");
            var interval = GetRuleInterval(rule, Configuration.NiceHash.WithdrawalAmount);
            if (interval is null)
            {
                Logger.Warn("Не удалось получить интервал");
                return;
            }
            Logger.Info("Текущий интервал получен");

            // Получаем общую сумму вывода с учётом комиссии
            var amount = GetWithdrawalAmount(interval, Configuration.NiceHash.WithdrawalAmount);

            // Комиссия вывода
            var fee = amount - Configuration.NiceHash.WithdrawalAmount;

            // Процент комиссии
            //var percentage = (fee) / Configuration.NiceHash.WithdrawalAmount * 100;

            #endregion Расчёт комиссий

            // Если доступных средств достаточно и отсутствует дополнительная комиссия
            if (balance.Available >= amount && interval.Element.SndValue == 0)
            {
                Logger.Info($"Запрос вывода средств. Сумма вывода: {Configuration.NiceHash.WithdrawalAmount} {Configuration.NiceHash.Currency}. Комиссия: {fee} {Configuration.NiceHash.Currency}");

                // Оформим вывод
                var withdrawal = await NiceHashApi.PostWithdrawalAsync(amount, Configuration.Api.WalletId);

                if (withdrawal?.Id != null)
                {
                    Logger.Info($"Идентификатор транзации: {withdrawal.Id}");
                }
                else
                {
                    Logger.Warn("Не удалось вывести средства");
                }
            }
            else
            {
                Logger.Info("Средств для вывода не достаточно");
            }
        }

        /// <summary>
        /// Получение комиссии вывода
        /// </summary>
        private static decimal GetWithdrawalAmount(JsonRuleInterval interval, decimal value)
        {
            var amount = interval.Element.SndType == JsonRuleIntervalElementType.ABSOLUTE ?
                        (value + interval.Element.SndValue) / (1 - value) :
                        value / (1 - value - interval.Element.SndValue);

            return Math.Round(amount, 8);
        }

        /// <summary>
        /// Получение интервала для переданного значения
        /// </summary>
        private static JsonRuleInterval GetRuleInterval(JsonRule rule, decimal value)
        {
            foreach (var item in rule.Intervals)
            {
                // Если начальный интервал не указан, то пусть он будет нулевым
                if (item.Start.HasValue == false) item.Start = 0;

                // Если конечный интервал не указан, то будем считать его бесконечным
                if (item.End.HasValue == false) item.End = decimal.MaxValue;
            }

            return rule.Intervals.Where(x => value >= x.Start).Where(x => value <= x.End).FirstOrDefault();
        }

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly static NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Апи
        /// </summary>
        private readonly NiceHashApi NiceHashApi = new(Configuration.Api.ApiKey, Configuration.Api.ApiSecret, Configuration.Api.OrganizationId);
    }
}