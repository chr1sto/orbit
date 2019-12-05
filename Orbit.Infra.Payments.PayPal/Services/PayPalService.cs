using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly IConfiguration _config;

        public PayPalService(IHostingEnvironment env, ILogger<PayPalService> logger)
        {
            _env = env;
            _logger = logger;

            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
            _config = config;

            bool.TryParse(config["PAYPAL:PROD"], out bool prod);

            if(prod)
            {
                _baseUrl = "https://api.paypal.com/";
            }
            else
            {
                _baseUrl = "https://api.sandbox.paypal.com/";
            }

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));           

            _products = new Dictionary<string, string>();
            var valuesSection = config.GetSection("PAYPAL:PRODUCTS");
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                var key = section["PRICE"];
                var value = section["AMOUNT"];
                _products.Add(key, value);
            }
        }

        public async Task<int> VerifyOrder(string orderId)
        {
            await this.Authorize();
            var response = await _httpClient.GetAsync("/v2/checkout/orders/" + orderId);

            var s = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(s);

            try
            {
                string status = obj.status;
                if(status == "COMPLETED")
                {
                    if(obj.purchase_units[0].payments.captures[0].status == "COMPLETED")
                    {
                        string price = obj.purchase_units[0].amount.value;

                        if (_products.ContainsKey(price))
                        {
                            int.TryParse(_products[price], out int ret);
                            return ret;
                        }
                        else
                        {
                            _logger.LogError("!!! User did not donate valid amount, please check !!!");
                        }
                    }
                    else
                    {
                        _logger.LogError("Payment declined");
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
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_config["PAYPAL:CLIENT_ID"]}:{_config["PAYPAL:CLIENT_SECRET"]}"))}");

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token")
            {
                Content = new FormUrlEncodedContent(nvc)
            };

            var result = await _httpClient.SendAsync(request);


            if(!result.IsSuccessStatusCode)
            {
                _logger.LogError("Could not authenticate with Paypal Credentials!");
                return;
            }
            var s = await result.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(s);

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {(string)obj.access_token}");
            }
            catch(Exception ex)
            {
                _logger.LogError("Something went horribly wrong wile trying to authenticate\n\n" + ex.Message);
            }
        }
    }
}
