using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orbit.Infra.Payments.PayPal.Interfaces;

namespace Orbit.Infra.Payments.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<PayPalService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _products;

        public PayPalService(IHostingEnvironment env, ILogger<PayPalService> logger)
        {
            _env = env;
            _logger = logger;

            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            bool.TryParse(config["PAYPAL:PROD"], out bool prod);

            if(prod)
            {
                _baseUrl = "https://api.paypal.com/";
            }
            else
            {
                _baseUrl = "https://api.sandbox.paypal.com/";
            }

            var handler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(config["PAYPAL:CLIENT_ID"],config["CLIENT_SECRET"])
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");


            _products = new Dictionary<string, string>();
            var valuesSection = config.GetSection("MySettings:MyValues");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                var key = section["PRICE"];
                var value = section["AMOUNT"];
                _products.Add(key, value);
            }

            this.Authorize().Wait();
        }

        public async Task<int> VerifyOrder(string orderId)
        {
            var response = await _httpClient.GetAsync(_baseUrl + "v2/checkout/orders/" + orderId);

            var s = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(s);

            try
            {
                var status = obj.status as string;
                if(status == "APPROVED" || status == "COMPLETED")
                {
                    var units = obj.status as dynamic[];

                    if(units.Length > 0)
                    {
                        var unit = units[0];
                        var price = unit.value as string;

                        if(_products.ContainsKey(price))
                        {
                            int.TryParse(_products[price], out int ret);
                            return ret;
                        }
                        else
                        {
                            _logger.LogError("!!! User did not donate valid amount, please check !!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went horribly wrong wile trying to verify and Order\n\n" + ex.Message);
            }
            return 0;
        }

        private async Task Authorize()
        {
            var result = await _httpClient.GetAsync(_baseUrl + "v1/oauth2/token");
            if(!result.IsSuccessStatusCode)
            {
                _logger.LogError("Could not authenticate with Paypal Credentials!");
                return;
            }
            var s = await result.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(s);

            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {(string)obj.access_token}");
            }
            catch(Exception ex)
            {
                _logger.LogError("Something went horribly wrong wile trying to authenticate\n\n" + ex.Message);
            }
        }
    }
}
