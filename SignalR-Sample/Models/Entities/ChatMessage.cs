using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Bugeto.Models.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public Guid ChatRoomId { get; set; }
    }
}
