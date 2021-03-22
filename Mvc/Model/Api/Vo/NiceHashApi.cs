using Newtonsoft.Json;
using Nicehash.Withdrawal.Config;
using Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Account;
using Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Rules;
using Nicehash.Withdrawal.Mvc.Model.Api.Vo.Json.Withdrawal;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nicehash.Withdrawal.Mvc.Model.Api.Vo
{
    public class NiceHashApi
    {
        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string ApiKey { get; }

        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string ApiSecret { get; }

        /// <remarks>
        /// Подробнее на <see href="https://www.nicehash.com/my/settings/keys" />
        /// </remarks>
        public string OrganizationId { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public NiceHashApi(string apiKey, string apiSecret, string organizationId)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            ApiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
            OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        }

        /// <summary>
        /// Получение текущего баланса в BTC
        /// </summary>
        public async Task<JsonBalance> GetBtcBalanceAsync(string coin)
        {
            var responce = await GetAsync($"/main/api/v2/accounting/account2/{coin}", true);
            return JsonConvert.DeserializeObject<JsonBalance>(responce);
        }

        /// <summary>
        /// Получение правила вычисления комисии
        /// </summary>
        public async Task<JsonRule> GetWithdrawalFeeRulesAsync(string coin)
        {
            var responce = await GetAsync("/main/api/v2/accounting/fees/WITHDRAWAL/BITGO", true);
            var rules = JsonConvert.DeserializeObject<JsonWithdrawalFeeRules>(responce);
            return rules.Rules.TryGetValue(coin, out var rule) ? rule : default;
        }

        /// <summary>
        /// Запрос вывода средств
        /// </summary>
        public async Task<JsonWithdrawalResponce> PostWithdrawalAsync(decimal amount, string walletId)
        {
            var request = new JsonWithdrawalRequest { Amount = amount.ToString().Replace(",", "."), Currency = "BTC", WithdrawalAddressId = walletId };
            var responce = await PostAsync("/main/api/v2/accounting/withdrawal/", JsonConvert.SerializeObject(request), true);
            return JsonConvert.DeserializeObject<JsonWithdrawalResponce>(responce);
        }

        /// <summary>
        /// Отправка Post запроса
        /// </summary>
        private async Task<string> PostAsync(string url, string payload, bool requestId)
        {
            var time = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var client = new RestClient(Configuration.NiceHash.UrlRoot);
            var request = new RestRequest(url);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");

            var nonce = Guid.NewGuid().ToString();
            var digest = HashBySegments(ApiSecret, ApiKey, time, nonce, OrganizationId, "POST", GetPath(url), GetQuery(url), payload);

            if (payload != null)
            {
                request.AddJsonBody(payload);
            }

            request.AddHeader("X-Time", time);
            request.AddHeader("X-Nonce", nonce);
            request.AddHeader("X-Auth", ApiKey + ":" + digest);
            request.AddHeader("X-Organization-Id", OrganizationId);

            if (requestId)
            {
                request.AddHeader("X-Request-Id", Guid.NewGuid().ToString());
            }

            var stopwatch = Stopwatch.StartNew();
            var response = await client.ExecuteAsync(request, Method.POST);
            stopwatch.Stop();
            Logger.Trace($"{response.ResponseUri.Scheme.ToLower()} {Method.POST} {Configuration.NiceHash.UrlRoot}{url} responded {(int)response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms.");

            return response.Content;
        }

        /// <summary>
        /// Отправка Get запроса
        /// </summary>
        private async Task<string> GetAsync(string url, bool auth)
        {
            var time = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var client = new RestClient(Configuration.NiceHash.UrlRoot);
            var request = new RestRequest(url);

            if (auth)
            {
                var nonce = Guid.NewGuid().ToString();
                var digest = HashBySegments(ApiSecret, ApiKey, time, nonce, OrganizationId, "GET", GetPath(url), GetQuery(url), null);

                request.AddHeader("X-Time", time);
                request.AddHeader("X-Nonce", nonce);
                request.AddHeader("X-Auth", ApiKey + ":" + digest);
                request.AddHeader("X-Organization-Id", OrganizationId);
            }

            var stopwatch = Stopwatch.StartNew();
            var response = await client.ExecuteAsync(request, Method.GET);
            stopwatch.Stop();
            Logger.Trace($"{response.ResponseUri.Scheme.ToLower()} {Method.GET} {Configuration.NiceHash.UrlRoot}{url} responded {(int)response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms.");

            return response.Content;
        }

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        #region Код из примера для осуществления запросов к API

        private static string GetPath(string url)
        {
            var arrSplit = url.Split('?');
            return arrSplit[0];
        }

        private static string GetQuery(string url)
        {
            var arrSplit = url.Split('?');
            return arrSplit.Length == 1 ? null : arrSplit[1];
        }

        private static string JoinSegments(List<string> segments)
        {
            var sb = new StringBuilder();

            bool first = true;
            foreach (var segment in segments)
            {
                if (!first)
                {
                    sb.Append('\0');
                }
                else
                {
                    first = false;
                }

                if (segment != null)
                {
                    sb.Append(segment);
                }
            }
            return sb.ToString();
        }

        private static string CalcHMACSHA256Hash(string plaintext, string salt)
        {
            var baText2BeHashed = Encoding.Default.GetBytes(plaintext);
            var baSalt = Encoding.Default.GetBytes(salt);
            var baHashedText = new HMACSHA256(baSalt).ComputeHash(baText2BeHashed);
            return string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
        }

        private static string HashBySegments(string key, string apiKey, string time, string nonce, string orgId, string method, string encodedPath, string query, string bodyStr)
        {
            var segments = new List<string>
            {
                apiKey,
                time,
                nonce,
                null,
                orgId,
                null,
                method,
                encodedPath ?? null,
                query ?? null
            };

            if (bodyStr != null && bodyStr.Length > 0)
            {
                segments.Add(bodyStr);
            }

            return CalcHMACSHA256Hash(JoinSegments(segments), key);
        }

        #endregion Код из примера для осуществления запросов к API
    }
}