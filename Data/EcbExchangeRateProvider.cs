using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Xml;
using CoreApp.Models;
using System.Globalization;

namespace CoreApp.Data
{
    public class EcbExchangeRateProvider
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode)
        {
            if (exchangeRateCurrencyCode == null)
                throw new ArgumentNullException(nameof(exchangeRateCurrencyCode));

            //add euro with rate 1
            var ratesToEuro = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    CurrencyCode = "EUR",
                    Rate = 1,
                    UpdatedOn = DateTime.UtcNow
                }
            };

                var stream = await client.GetStreamAsync("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
                var document = new XmlDocument();
                document.Load(stream);

                var namespaces = new XmlNamespaceManager(document.NameTable);
                namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                //get daily rates
                var dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out var updateDate))
                    updateDate = DateTime.UtcNow;

                foreach (XmlNode currency in dailyRates.ChildNodes)
                {
                    //get rate
                    if (!decimal.TryParse(currency.Attributes["rate"].Value, NumberStyles.Currency, CultureInfo.InvariantCulture, out var currencyRate))
                        continue;

                    ratesToEuro.Add(new ExchangeRate()
                    {
                        CurrencyCode = currency.Attributes["currency"].Value,
                        Rate = currencyRate,
                        UpdatedOn = updateDate
                    });
                }

            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
                return ratesToEuro;

            //use only currencies that are supported by ECB
            var exchangeRateCurrency = ratesToEuro.FirstOrDefault(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
                throw new Exception("EcbExchange.Error");

            //return result for the selected (not euro) currency
            return ratesToEuro.Select(rate => new ExchangeRate
            {
                CurrencyCode = rate.CurrencyCode,
                Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
                UpdatedOn = rate.UpdatedOn
            }).ToList();
        }
    }
}
