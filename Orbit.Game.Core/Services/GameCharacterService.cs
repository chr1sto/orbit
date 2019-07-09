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

namespace Orbit.Game.Core.Services
{
    public class GameCharacterService : IGameCharacterService
    {
        private readonly CharacterDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly GameCharacterClient _client;
        private readonly ILogger<GameCharacterService> _logger;

        public GameCharacterService(ILogger<GameCharacterService> logger,CharacterDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _client = new GameCharacterClient(configuration["BASE_API_PATH"], _httpClient);
            _logger = logger;
        }

        public IEnumerable<Character> GetAll()
        {
            var characters = _context.Characters.ToList();

            return characters;
        }

        public async void UpdateAll()
        {
            var characters = _context.Characters.ToList();
            Guid updateId = Guid.NewGuid();
            DateTime updatedOn = DateTime.Now;

            var list = new List<CharacterAdminViewModel>();

            foreach(var @char in characters)
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

        private string GetClass(int i)
        {
            return i.ToString();
        }
    }
}
