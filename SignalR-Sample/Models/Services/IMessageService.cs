using SignalR.Bugeto.Contexts;
using SignalR.Bugeto.Models.Entities;

namespace SignalR.Bugeto.Models.Services
{
    public interface IMessageService
    {
        Task SaveChatMessage(Guid RoomId, MessageDto message);
        Task<List<MessageDto>> GetChatMessage(Guid RoomId);
    }

    public class MessageService : IMessageService
    {
        private readonly DataBaseContext _context;
        public MessageService(DataBaseContext context)
        {
            _context = context;
        }

        public Task<List<MessageDto>> GetChatMessage(Guid RoomId)
        {
            var messages = _context.ChatMessages.Where(p => p.ChatRoomId == RoomId)
                .Select(p => new MessageDto
                {
                    Message = p.Message,
                    Sender = p.Sender,
                    Time = p.Time
                }).OrderBy(p => p.Time).ToList();
            return Task.FromResult(messages);
        }

        public Task SaveChatMessage(Guid RoomId, MessageDto message)
        {
            var room = _context.ChatRooms.SingleOrDefault(p => p.Id == RoomId);
            ChatMessage chatMessage = new ChatMessage()
            {
                ChatRoom = room,
                Message = message.Message,
                Sender = message.Sender,
                Time = message.Time,
            };
            _context.ChatMessages.Add(chatMessage);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }


    public class MessageDto
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
