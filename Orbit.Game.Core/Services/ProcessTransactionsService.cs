using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Services
{
    public class ProcessTransactionsService : IProcessTransactionsService
    {
        private readonly TransactionsClient _client;
        private readonly ILogger<ProcessTransactionsService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProcessTransactionsService(ILogger<ProcessTransactionsService> logger, HttpClient httpClient, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _client = new TransactionsClient(httpClient);
            _client.BaseUrl = configuration["BASE_API_PATH"];
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Process()
        {
            var transactions = await _client.TransactionsGetAsync();
            if(transactions.Success ?? false)
            {
                if(transactions.Data != null)
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
                        foreach (var t in transactions.Data)
                        {
                            var itemId = getItemIdFromCurrency(t.Currency);
                            if (itemId != 0)
                            {
                                var player = context.Characters.Where(e => e.Name == t.TargetInfo).FirstOrDefault();

                                if (player != null)
                                {
                                    try
                                    {
                                        using (var client = new TcpClient())
                                        {
                                            client.Connect(IPAddress.Parse("127.0.0.1"), 29000);
                                            byte[] bytes = new byte[36];

                                            //ServerIndex
                                            this.ToLittleEndianByteArray(1)
                                                .CopyTo(bytes, 0);
                                            //PlayerId
                                            this.ToLittleEndianByteArray(int.Parse(player.IdPlayer))
                                                .CopyTo(bytes, 4);
                                            //TargetId
                                            this.ToLittleEndianByteArray(int.Parse(player.IdPlayer))
                                                .CopyTo(bytes, 8);
                                            //Command
                                            this.ToLittleEndianByteArray(101)
                                                .CopyTo(bytes, 12);
                                            //ItemId
                                            this.ToLittleEndianByteArray(itemId)
                                                .CopyTo(bytes, 16);
                                            //Amount
                                            this.ToLittleEndianByteArray(Math.Abs(t.Amount ?? 0))
                                                .CopyTo(bytes, 20);
                                            //param3 is empty;
                                            //Password 1
                                            this.ToLittleEndianByteArray(3851872)
                                                .CopyTo(bytes, 28);
                                            //Password 2
                                            this.ToLittleEndianByteArray(6381597)
                                                .CopyTo(bytes, 32);


                                            using (NetworkStream stream = client.GetStream())
                                            {
                                                stream.Write(bytes, 0, bytes.Length);
                                                stream.Close();
                                            }

                                            client.Close();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        t.Status = "FAILED";
                                        await _client.TransactionsPatchAsync(t);
                                    }

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

        private int getItemIdFromCurrency(string currency)
        {
            switch (currency.ToUpper())
            {
                case "VP": return 31439;
                default: return 0;
            }
        }

        private byte[] ToLittleEndianByteArray(int data)
        {
            byte[] b = new byte[4];
            b[0] = (byte)data;
            b[1] = (byte)(((uint)data >> 8) & 0xFF);
            b[2] = (byte)(((uint)data >> 16) & 0xFF);
            b[3] = (byte)(((uint)data >> 24) & 0xFF);
            return b;
        }
    }
}
