using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Helpers;
using CSharpGetStarted.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context
                .Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections
                .Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var query = _context
                .Messages
                .Where(m => 
                    (
                        m.RecipientUsername == currentUsername 
                        && m.SenderUsername == recipientUsername 
                        && m.RecipientDeleted == false
                    )
                    || (
                        m.RecipientUsername == recipientUsername 
                        && m.SenderUsername == currentUsername 
                        && m.SenderDeleted == false
                    )
                )
                .OrderBy(m => m.MessageSent)
                .AsQueryable();

            var unReadMessages = query
                .Where(m => m.DateRead == null && m.RecipientUsername == currentUsername)
                .ToList();
        
            if (unReadMessages.Any())
            {
                foreach (var message in unReadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }

            return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
    }
}
