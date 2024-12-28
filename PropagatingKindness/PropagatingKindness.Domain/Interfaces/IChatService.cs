using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Models;

namespace PropagatingKindness.Domain.Interfaces;

public interface IChatService
{
    Task<Result> CreateChat(int userId, int advertId);
    Task<Result> SendMessage(int userId, int chatId, string message);
    Task<Result<Chat>> GetChat(int chatId);
}
