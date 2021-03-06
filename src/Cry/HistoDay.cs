﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Cry
{
    public static class HistoDay
    {
        private const string BaseUrl = "https://min-api.cryptocompare.com/data/histoday";
        private const int SpreadDays = 180;

        /// <summary>
        ///     a function fetches a crypto OHLC price-series for fsym/tsym and stores
        ///     it in a pandas DataFrame; uses specific Exchange as provided
        ///     src: https://www.cryptocompare.com/api/
        /// </summary>
        /// <returns>The crypto ohlcby exchange.</returns>
        /// <param name="fsym">Fsym.</param>
        /// <param name="tsym">Tsym.</param>
        /// <param name="exchange">Exchange.</param>
        /// <param name="limit">Limit n of rows</param>
        public static async Task<IDictionary<DateTimeOffset, HistoDayData>> GetExchangeData(
            string fsym, string tsym, string exchange, int limit, CultureInfo ci)
        {
            var url = $"{BaseUrl}?fsym={fsym}&tsym={tsym}&e={exchange}&limit={limit}";
            var client = new HttpClient();
            var result = await client.GetStringAsync(url);
            var parsedResult = JObject.Parse(result);
            if ((string) parsedResult["Response"] == "Error") throw new Exception((string) parsedResult["Message"]);


            return new Dictionary<DateTimeOffset, HistoDayData>(
                from i in Enumerable.Range(0, limit)
                let h = new HistoDayData
                {
                    TimeStamp = DateTimeOffset.FromUnixTimeSeconds((long) parsedResult["Data"][i]["time"]).Date,
                    OpenValue = double.Parse((string) parsedResult["Data"][i]["open"], ci.NumberFormat),
                    HighValue = double.Parse((string) parsedResult["Data"][i]["high"], ci.NumberFormat),
                    LowValue = double.Parse((string) parsedResult["Data"][i]["low"], ci.NumberFormat),
                    CloseValue = double.Parse((string) parsedResult["Data"][i]["close"], ci.NumberFormat)
                }
                where h.OpenValue + h.HighValue + h.LowValue + h.CloseValue > 0
                select new KeyValuePair<DateTimeOffset, HistoDayData>(new DateTimeOffset(h.TimeStamp.Date), h));
        }

        public static async Task<IDictionary<DateTimeOffset, double>> GetExchangeCloseTimeSeries(string fsym,
            string tsym, string market, int limit, CultureInfo ci)
        {
            var exchangeInfo = await GetExchangeData(fsym, tsym, market, limit, ci);
            if (exchangeInfo.Count <= 1) return new Dictionary<DateTimeOffset, double>();

            var dictionary = exchangeInfo
                .Where(c => c.Value.TimeStamp.Add(TimeSpan.FromDays(SpreadDays)) > DateTimeOffset.Now)
                .ToDictionary(c => c.Key, c => c.Value.CloseValue);
            return dictionary;
        }
    }
}