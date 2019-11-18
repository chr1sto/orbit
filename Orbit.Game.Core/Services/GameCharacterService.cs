using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Models.CharacterDb;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Orbit.Game.Core.Services
{
    public class GameCharacterService : IGameCharacterService
    {
        private readonly HttpClient _httpClient;
        private readonly GameCharacterClient _client;
        private readonly ILogger<GameCharacterService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GameCharacterService(ILogger<GameCharacterService> logger, HttpClient httpClient, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _httpClient = httpClient;
            _client = new GameCharacterClient( _httpClient);
            _client.BaseUrl = configuration["BASE_API_PATH"];
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<Character> GetAll()
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
                var characters = context.Characters.AsNoTracking().ToList();

                return characters;
            }

        }

        public async void UpdateAll()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
                var characters = context.Characters.AsNoTracking().ToList();
                Guid updateId = Guid.NewGuid();
                DateTime updatedOn = DateTime.Now;

                var list = new List<CharacterAdminViewModel>();

                foreach (var @char in characters)
                {
                    var charViewModel = new CharacterAdminViewModel()
                    {
                        Account = @char.Account,
                        BossKills = @char.BossKills ?? 0,
                        Class = GetClass(@char.Job),
                        Dexterity = @char.Dex,
                        DonateCoins = @char.Donate,
                        EuphresiaCoins = @char.Farm,
                        GearScore = @char.GearScore,
                        Id = Guid.Empty,
                        Intelligence = @char.Int,
                        IsStaff = @char.Authority != 'F',
                        Level = @char.Level,
                        Name = @char.Name,
                        Penya = @char.Gold,
                        Perin = @char.Perin,
                        PlayerId = @char.IdPlayer,
                        PlayTime = @char.TotalPlayTime,
                        RedChips = @char.Chips,
                        Stamina = @char.Sta,
                        Strength = @char.Str,
                        UpdateId = updateId,
                        UpdatedOn = updatedOn,
                        VotePoints = @char.Vote,
                        CreatedOn = DateTime.MinValue,
                        IsDeleted = @char.IsBlock == 'D'
                    };

                    list.Add(charViewModel);
                }

                _logger.LogDebug("UPDATING " + list.Count + " CHARACTERS");
                await _client.GameCharacterPostAsync(list.AsEnumerable());
            }
        }

        private string GetClass(int i)
        {
            return i.ToString();
        }
    }
}
