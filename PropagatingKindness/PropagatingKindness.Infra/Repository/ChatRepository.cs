using Microsoft.EntityFrameworkCore;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;
using PropagatingKindness.Infra.Db;

namespace PropagatingKindness.Infra.Repository;

public class ChatRepository : IChatRepository
{
    private readonly PlantsDbContext _dbContext;

    public ChatRepository(PlantsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Chat>> GetChats(int userId)
    {
        return await _dbContext.Chats
            .Include(c => c.FromUser)
            .Include(c => c.ToUser)
            .Include(c => c.Advert)
            .Where(x => x.FromUser.Id == userId || x.ToUser.Id == userId)
            .OrderByDescending(c => c.LastUpdate)
            .ToListAsync();
    }

    public async Task<Chat> GetChatWithMessages(int chatId)
    {
        return await _dbContext.Chats
            .Include(c => c.FromUser)
            .Include(c => c.ToUser)
            .Include(c => c.Messages)
            .Include(c => c.Advert)
            .ThenInclude(a => a.Photos)
            .FirstOrDefaultAsync(x => x.Id == chatId);
    }

    public async Task<Chat> GetByUserAdvert(int userId, int advertId)
    {
        return await _dbContext.Chats.FirstOrDefaultAsync(x => x.FromUser.Id == userId && x.Advert.Id == advertId);
    }

    public async Task Add(Chat chat)
    {
        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Chat chat)
    {
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }
}
