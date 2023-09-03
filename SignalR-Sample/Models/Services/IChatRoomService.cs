using Microsoft.EntityFrameworkCore;
using SignalR.Bugeto.Contexts;
using SignalR.Bugeto.Models.Entities;

namespace SignalR.Bugeto.Models.Services
{
    public interface IChatRoomService
    {
        Task<Guid> CreateChatRoom(string ConnectionId);
        Task<Guid> GetChatRoomForConnection(string CoonectionId);
        Task<List<Guid>> GetAllrooms();
    }

    public class ChatRoomService : IChatRoomService
    {
        private readonly DataBaseContext _context;
        public ChatRoomService(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<Guid> CreateChatRoom(string ConnectionId)
        {
            var existChatRoom = _context.ChatRooms.SingleOrDefault(p => p.ConnectionId == ConnectionId);
            if(existChatRoom != null)
            {
                return await Task.FromResult(existChatRoom.Id);
            }

            ChatRoom chatRoom = new ChatRoom()
            {
                ConnectionId = ConnectionId,
                Id = Guid.NewGuid(),
            };
            _context.ChatRooms.Add(chatRoom);
            _context.SaveChanges();
            return await Task.FromResult(chatRoom.Id);
        }

        public async Task<List<Guid>> GetAllrooms()
        {
            var rooms = _context.ChatRooms
                .Include(p=> p.ChatMessages)
                .Where(p=> p.ChatMessages.Any())
                .Select(p =>
              p.Id).ToList();
            return await Task.FromResult(rooms);
        }

        public async Task<Guid> GetChatRoomForConnection(string CoonectionId)
        {
            var chatRoom = _context.ChatRooms.SingleOrDefault(p => p.ConnectionId == CoonectionId);
            return await Task.FromResult(chatRoom.Id);
        }
    }
}
