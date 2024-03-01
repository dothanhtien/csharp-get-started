using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Helpers;

namespace CSharpGetStarted.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser();
        Task<IEnumerable<MessageDto>> GetMessageThread(int userId, int recipientId);
        Task<bool> SaveAllAsync();
    }
}
