using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.Bugeto.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Bugeto.Hubs
{
    [Authorize]
    public class SupportHub:Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessageService _messageService;

        private readonly IHubContext<SiteChatHub> _siteChathub;
        public SupportHub(IChatRoomService chatRoomService,
            IMessageService messageService
            , IHubContext<SiteChatHub> hubContext)
        {
            _chatRoomService = chatRoomService;
            _messageService = messageService;
            _siteChathub = hubContext;
        }
        public async override Task OnConnectedAsync()
        {
            var rooms = await _chatRoomService.GetAllrooms();
            await Clients.Caller.SendAsync("GetRooms", rooms);
            await base.OnConnectedAsync(); 
        }


        public async Task LoadMessage(Guid roomId)
        {
            var message = await _messageService.GetChatMessage(roomId);
            await Clients.Caller.SendAsync("getNewMessage", message);
        }

        public async Task SendMessage(Guid roomId,string text)
        {
            var message = new MessageDto
            {
                Sender = Context.User.Identity.Name,
                Message = text,
                Time = DateTime.Now,
            };

            await _messageService.SaveChatMessage(roomId, message);

            await _siteChathub.Clients.Group(roomId.ToString())
                .SendAsync("getNewMessage", message.Sender, message.Message, message.Time.ToShortTimeString());
                
        }
    }
}
