using Microsoft.AspNetCore.SignalR;
using SignalR.Shared.Contracts;

namespace SignalR.HubServer.Hubs;

public sealed class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private static readonly string[] ValidRooms = ["Desarrollo", "Diseño"];

    public async Task JoinRoom(string room)
    {
        ValidateRoom(room);
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task LeaveRoom(string room)
    {
        ValidateRoom(room);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
    }

    public async Task SendMessage(string user, string message, string room)
    {
        if (string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(message))
        {
            throw new HubException("El usuario y el mensaje son obligatorios.");
        }

        user = user.Trim();
        message = message.Trim();

        if (user.Length > 40 || message.Length > 1000)
        {
            throw new HubException("El mensaje supera el límite permitido.");
        }

        ValidateRoom(room);

        var chatMessage = new ChatMessage(user, message, DateTimeOffset.UtcNow);

        await Clients.Group(room).SendAsync("ReceiveMessage", chatMessage);
    }

    private static void ValidateRoom(string room)
    {
        if (!ValidRooms.Contains(room))
        {
            throw new HubException("Sala no válida.");
        }
    }
}