using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Services
{
    public class ProcessTransactionsService : IProcessTransactionsService
    {
        private readonly CharacterDbContext _context;
        private readonly TransactionsClient _client;
        private readonly ILogger<ProcessTransactionsService> _logger;

        public ProcessTransactionsService(ILogger<ProcessTransactionsService> logger, CharacterDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _client = new TransactionsClient(httpClient);
            _client.BaseUrl = configuration["BASE_API_PATH"];
            _logger = logger;
        }

        public async Task Process()
        {
            var transactions = await _client.TransactionsGetAsync();
            if(transactions.Success ?? false)
            {
                if(transactions.Data != null)
                {
                    foreach(var t in transactions.Data)
                    {
                        var itemName = getItemNameFromCurrency(t.Currency);
                        if(itemName != null)
                        {
                            var player = _context.Characters.Where(e => e.Name == t.TargetInfo).FirstOrDefault();

                            if(player != null)
                            {
                                var sendItem = new SendItem()
                                {
                                    ItemName = itemName,
                                    PlayerId = player.IdPlayer,
                                    ItemCount = Math.Abs(t.Amount ?? 0),
                                    SenderId = "0000000",
                                    ServerIndex = "01"
                                };

                                _context.Add(sendItem);
                                _context.SaveChanges();

                                t.Status = "FINISHED";

                                await _client.TransactionsPatchAsync(t);
                                continue;
                            }
                        }

                        t.Status = "FAILED";
                        await _client.TransactionsPatchAsync(t);
                    }
                }
            }
            else
            {
                if(transactions.Errors != null)
                {
                    string message = "";
                    foreach(var error in transactions.Errors)
                    {
                        message += error + "\n";
                    }
                    _logger.LogError($"Could not retrieve pending transactions: \n{message}");
                }
            }
        }

        private string getItemNameFromCurrency(string currency)
        {
            switch(currency.ToUpper())
            {
                case "VP": return "Vote Points";
                default: return null;
            }
        }
    }
}
