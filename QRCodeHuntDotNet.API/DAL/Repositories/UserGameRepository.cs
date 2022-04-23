using Microsoft.EntityFrameworkCore;
using QRCodeHuntDotNet.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.DAL.Repositories
{
    public interface IUserGameRepository
    {
        Task<UserGame> GetUserGame(long userId, string gameName);
        Task CreateUserGame(UserGame userGame);
        Task UpdateUserGame(UserGame userGame);
        bool ExistsInCurrentContext(long userId, string gameName);
    }

    public class UserGameRepository : IUserGameRepository
    {
        private readonly ApplicationDbContext _context;

        public UserGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserGame> GetUserGame(long userId, string gameName)
        {
            string gameNameLower = gameName.ToLower();
            return await _context.UserGames
                .SingleOrDefaultAsync(userGame => userGame.UserId == userId && userGame.GameName.ToLower() == gameNameLower);
        }

        public async Task CreateUserGame(UserGame userGame)
        {
            _context.UserGames.Add(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserGame(UserGame userGame)
        {
            _context.Entry(userGame).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool ExistsInCurrentContext(long userId, string gameName)
        {
            string gameNameLower = gameName.ToLower();
            return _context.UserGames.Any(userGame => userGame.UserId == userId && userGame.GameName.ToLower() == gameNameLower);
        }
    }
}
