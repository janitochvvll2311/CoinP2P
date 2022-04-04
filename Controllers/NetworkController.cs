using System.Net.WebSockets;
using CoinP2P.Models;
using CoinP2P.Models.Network;
using Microsoft.AspNetCore.Mvc;

namespace CoinP2P.Controllers;

public class NetworkController : Controller
{

    public P2PNode Node { get; }

    public NetworkController(P2PNode node)
    {
        Node = node;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task ConnectMe()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var host = new P2PHost
            {
                Remote = $"{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}",
                Socket = socket
            };
            await Node.Poll<ChatMessage>(host, (m) =>
            {
                return true;
            });
        }
    }

    [HttpGet]
    public async Task ConnectTo(string target)
    {
        var socket = new ClientWebSocket();
        var uri = new Uri($"wss://{target}/ConnectMe");
        await socket.ConnectAsync(uri, CancellationToken.None);
        var host = new P2PHost
        {
            Remote = target,
            Socket = socket
        };
        await Node.Poll<ChatMessage>(host, (m) =>
        {
            return true;
        });
    }

}